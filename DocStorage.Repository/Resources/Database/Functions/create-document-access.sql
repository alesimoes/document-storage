SET search_path TO app_documents;
DROP FUNCTION IF EXISTS "add_document_group";
DROP FUNCTION IF EXISTS "add_document_user";
DROP FUNCTION IF EXISTS "remove_document_group";
DROP FUNCTION IF EXISTS "remove_document_user";

DROP TYPE IF EXISTS "document_access_info";

CREATE TYPE "document_access_info" AS (
    id uuid,
    document_id uuid, 
	entity_id uuid 	
);

CREATE OR REPLACE FUNCTION add_document_group(
   document_access_info app_documents."document_access_info",
   current_user_id uuid
) RETURNS document_access_info
AS $$
DECLARE
    entity_exists app_documents."document_access_info";
BEGIN
    CALL app_users.authorize(current_user_id, ARRAY['admin'::public.user_role]);
    CALL app_documents.document_exists(document_access_info.document_id);
    CALL app_users.group_exists(document_access_info.entity_id);
    SELECT document_access_id,document_id,group_id FROM "document_groups" 
	WHERE "document_groups".group_Id = document_access_info.entity_id 
	and document_groups.document_id = document_access_info.document_id
	INTO entity_exists;
	
    IF FOUND THEN	     
        RAISE EXCEPTION 'Group already has permission to access this document.';
	ELSE
        INSERT INTO "document_groups" (document_access_id,document_id,group_id)
    	VALUES (gen_random_uuid(), document_access_info.document_id, document_access_info.entity_id)
		RETURNING document_access_id,document_id,group_id into document_access_info;	
    END IF;
	RETURN document_access_info;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION add_document_user(
   document_access_info app_documents."document_access_info",
   current_user_id uuid
) RETURNS app_documents.document_access_info
AS $$
DECLARE
    entity_exists app_documents."document_access_info";
BEGIN	
    CALL app_users.authorize(current_user_id, ARRAY['admin'::public.user_role]);
    CALL app_documents.document_exists(document_access_info.document_id);
    CALL app_users.user_exists(document_access_info.entity_id);
    SELECT document_access_id,document_id,user_id FROM "document_users" 
	WHERE "document_users".user_Id = document_access_info.entity_id 
    and "document_users".document_id = document_access_info.document_id
	INTO entity_exists;
	
    IF FOUND THEN	     
       RAISE EXCEPTION 'Group already has permission to access this document.';
	ELSE
        INSERT INTO "document_users" (document_access_id,document_id,user_id)
    	VALUES (gen_random_uuid(), document_access_info.document_id, document_access_info.entity_id)
		RETURNING document_access_id,document_id,user_id into document_access_info;	
    END IF;
	RETURN document_access_info;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION remove_document_group(
    document_access_info app_documents."document_access_info",
	current_user_id uuid
) RETURNS app_documents.document_access_info
AS $$
BEGIN
    CALL app_users.authorize(current_user_id, ARRAY['admin'::public.user_role]);
    DELETE FROM "document_groups"
	WHERE group_id = document_access_info.entity_id 
	and document_id = document_access_info.document_id;  
    
    IF NOT FOUND THEN
         RAISE EXCEPTION 'Selected group doesn t have permission for this document.';
	END IF;  
	RETURN document_access_info;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION remove_document_user(
    document_access_info app_documents."document_access_info",
	current_user_id uuid
) RETURNS app_documents.document_access_info
AS $$
BEGIN
    CALL app_users.authorize(current_user_id, ARRAY['admin'::public.user_role]);
    DELETE FROM "document_users"
	WHERE user_id = document_access_info.entity_id 
	and document_id = document_access_info.document_id;  
    
    IF NOT FOUND THEN
        RAISE EXCEPTION 'Selected user doesn t have permission for this document.';
	END IF;  
	RETURN document_access_info;
END;
$$ LANGUAGE plpgsql;
