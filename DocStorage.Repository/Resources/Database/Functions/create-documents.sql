DROP FUNCTION IF EXISTS "fn_insert_document";
DROP FUNCTION IF EXISTS "fn_get_document_by_id";

DROP TYPE IF EXISTS "tp_document_info";

CREATE TYPE "tp_document_info" AS (
    id uuid,
    name text,
    description text,
    category text,
	filename text,	
    posted_Date timestamp with time zone,
	message "tp_error_types"
);

CREATE OR REPLACE FUNCTION fn_insert_document(
    document_info "tp_document_info",
	current_user_id uuid
) RETURNS "tp_document_info"
AS $$
BEGIN
    CALL pe_authorize(current_user_id, ARRAY['admin'::tp_user_role,'manager'::tp_user_role]);
    INSERT INTO "tb_document" (document_id, name, description, category, filename, posted_date)
    VALUES  (document_info.Id,
			document_info.Name, 
			document_info.Description,
			document_info.Category,
			document_info.Filename,
			(SELECT CURRENT_TIMESTAMP AT TIME ZONE 'UTC')
		   )
	RETURNING document_id, name, description, category,filename,posted_date INTO document_info;	
	INSERT INTO "tb_document_access" (document_access_id,document_id,user_id)
    VALUES (gen_random_uuid(), document_info.id, current_user_id);
	RETURN document_info;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION fn_get_document_by_id(
    id "uuid",
	current_user_id uuid
) RETURNS "tp_document_info"
AS $$
DECLARE
     document_info "tp_document_info";
BEGIN
    CALL pe_authorize(current_user_id, ARRAY['admin'::tp_user_role,'manager'::tp_user_role,'regular'::tp_user_role]);
    SELECT d.document_id as Id,d.name,d.description,d.category,d.filename,d.posted_date
	INTO document_info 
	FROM tb_document d
	INNER JOIN tb_document_access a on d.document_id = a.document_id
	LEFT JOIN tb_user_group g on (a.group_id = g.group_id)
	WHERE
	d.document_id = id
	AND (a.user_id = current_user_id OR g.user_id = current_user_id);
	
    IF NOT FOUND THEN       
		document_info.message := 'document_not_found';
    END IF;
	RETURN document_info;
END;
$$ LANGUAGE plpgsql;

