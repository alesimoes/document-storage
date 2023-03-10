SET search_path TO app_documents;
DROP FUNCTION IF EXISTS insert_document;
DROP FUNCTION IF EXISTS get_document_by_id;

DROP TYPE IF EXISTS "document_info";

CREATE TYPE "document_info" AS (
    id uuid,
    name text,
    description text,
    category text,
	filename text,	
    posted_date timestamp with time zone	
);

CREATE OR REPLACE FUNCTION insert_document(
    document_info app_documents."document_info",
	current_user_id uuid
) RETURNS app_documents."document_info"
AS $$
BEGIN
    CALL app_users.authorize(current_user_id, ARRAY['admin'::public.user_role,'manager'::public.user_role]);
    INSERT INTO "document" (document_id, name, description, category, filename, posted_date)
    VALUES  (document_info.id,
			document_info.name, 
			document_info.description,
			document_info.category,
			document_info.filename,
			(SELECT CURRENT_TIMESTAMP AT TIME ZONE 'UTC')
		   )
	RETURNING document_id, name, description, category, filename, posted_date INTO document_info;	
	INSERT INTO "document_users" (document_access_id,document_id,user_id)
    VALUES (gen_random_uuid(), document_info.id, current_user_id);
	RETURN document_info;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION get_document_by_id(
    document_info app_documents."document_info",
	current_user_id uuid
) RETURNS app_documents."document_info"
AS $$
BEGIN
    CALL app_users.authorize(current_user_id, ARRAY['admin'::public.user_role,'manager'::public.user_role,'regular'::public.user_role]);
    SELECT DISTINCT d.document_id as id,d.name,d.description,d.category,d.filename,d.posted_date
	INTO document_info 
	FROM document d
	LEFT JOIN document_users du on d.document_id = du.document_id
	LEFT JOIN document_groups dg on d.document_id = dg.document_id	
	LEFT JOIN user_group gg on (gg.group_id = dg.group_id)
	WHERE
	d.document_id = document_info.id
	AND (du.user_id = current_user_id OR gg.user_id = current_user_id);
	
    IF NOT FOUND THEN       
		 RAISE EXCEPTION 'Document not found';
    END IF;
	RETURN document_info;
END;
$$ LANGUAGE plpgsql;

