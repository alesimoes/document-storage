DROP FUNCTION IF EXISTS "fn_add_document_access_group";
DROP FUNCTION IF EXISTS "fn_add_document_access_user";
DROP FUNCTION IF EXISTS "fn_remove_document_access_group";
DROP FUNCTION IF EXISTS "fn_remove_document_access_user";

DROP TYPE IF EXISTS "tp_document_access_info";

CREATE TYPE "tp_document_access_info" AS (
    id uuid,
    document_id uuid, 
	group_id uuid,
    user_id uuid,
	message tp_error_types
);

CREATE OR REPLACE FUNCTION fn_add_document_access_group(
   document_access_info "tp_document_access_info",
   current_user_id uuid
) RETURNS tp_document_access_info
AS $$
DECLARE
    entity_exists "tp_document_access_info";
BEGIN
    CALL pe_authorize(current_user_id, ARRAY['admin'::tp_user_role]);
    CALL pe_document_exists(document_access_info.document_id);
    CALL pe_group_exists(document_access_info.group_id);
    SELECT document_access_id,document_id,group_id,user_id FROM "tb_document_access" 
	WHERE "tb_document_access".group_Id = document_access_info.group_id 
	and tb_document_access.document_id = document_access_info.document_id
	INTO entity_exists;
	
    IF FOUND THEN	     
        entity_exists.message := 'group_already_exists_in_document_access';	
		document_access_info := entity_exists;
	ELSE
        INSERT INTO "tb_document_access" (document_access_id,document_id,group_id)
    	VALUES (gen_random_uuid(), document_access_info.document_id, document_access_info.group_id)
		RETURNING document_access_id,document_id,group_id,user_id into document_access_info;	
    END IF;
	RETURN document_access_info;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION fn_add_document_access_user(
   document_access_info "tp_document_access_info",
   current_user_id uuid
) RETURNS tp_document_access_info
AS $$
DECLARE
    entity_exists "tp_document_access_info";
BEGIN	
    CALL pe_authorize(current_user_id, ARRAY['admin'::tp_user_role]);
    CALL pe_document_exists(document_access_info.document_id);
    CALL pe_user_exists(document_access_info.user_id);
    SELECT document_access_id,document_id,group_id,user_id FROM "tb_document_access" 
	WHERE "tb_document_access".user_Id = document_access_info.user_id 
    and tb_document_access.document_id = document_access_info.document_id
	INTO entity_exists;
	
    IF FOUND THEN	     
        entity_exists.message := 'user_already_exists_in_document_access';	
		document_access_info := entity_exists;
	ELSE
        INSERT INTO "tb_document_access" (document_access_id,document_id,user_id)
    	VALUES (gen_random_uuid(), document_access_info.document_id, document_access_info.user_id)
		RETURNING document_access_id,document_id,group_id,user_id into document_access_info;	
    END IF;
	RETURN document_access_info;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION fn_remove_document_access_group(
    document_access_info "tp_document_access_info",
	 current_user_id uuid
) RETURNS tp_document_access_info
AS $$
BEGIN
    CALL pe_authorize(current_user_id, ARRAY['admin'::tp_user_role]);
    DELETE FROM "tb_document_access"
	WHERE tb_document_access.group_id = document_access_info.group_id 
	and tb_document_access.document_id = document_access_info.document_id;  
    
    IF NOT FOUND THEN
        document_access_info.message := 'group_not_found_in_document_access';	
	END IF;  
	RETURN document_access_info;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION fn_remove_document_access_user(
    document_access_info "tp_document_access_info",
	 current_user_id uuid
) RETURNS tp_document_access_info
AS $$
BEGIN
    CALL pe_authorize(current_user_id, ARRAY['admin'::tp_user_role]);
    DELETE FROM "tb_document_access"
	WHERE tb_document_access.user_id = document_access_info.user_id 
	and tb_document_access.document_id = document_access_info.document_id;  
    
    IF NOT FOUND THEN
        document_access_info.message := 'user_not_found_in_document_access';	
	END IF;  
	RETURN document_access_info;
END;
$$ LANGUAGE plpgsql;
