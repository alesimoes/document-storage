DROP FUNCTION IF EXISTS "fn_insert_group";
DROP FUNCTION IF EXISTS "fn_get_group_by_id";
DROP FUNCTION IF EXISTS "fn_delete_group";
DROP FUNCTION IF EXISTS "fn_update_group";
DROP FUNCTION IF EXISTS "fn_add_user_group";
DROP FUNCTION IF EXISTS "fn_delete_user_group";

DROP TYPE IF EXISTS "tp_group_info";
DROP TYPE IF EXISTS "tp_user_group_info";

CREATE TYPE "tp_group_info" AS (
   Id uuid,
   Name text,
   message tp_error_types
);

CREATE TYPE "tp_user_group_info" AS (
   Id uuid,
   user_id uuid,
   group_id uuid,
   message tp_error_types
);

CREATE OR REPLACE FUNCTION fn_insert_group(
    group_info "tp_group_info",
	current_user_id uuid
)  RETURNS tp_group_info
AS $$
DECLARE
     entity_exists "tp_group_info";
BEGIN
    CALL pe_authorize(current_user_id, ARRAY['admin'::tp_user_role]);
    SELECT group_id, name FROM "tb_group" 
	WHERE name = group_info.name INTO entity_exists;
   
    IF FOUND THEN	     
        entity_exists.message := 'group_already_exists';	
		group_info := entity_exists;
    ELSE
        INSERT INTO "tb_group" (group_id, name)
    VALUES (gen_random_uuid(), group_info.name)
	RETURNING group_id, name INTO group_info;	
    END IF;
	RETURN group_info;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION fn_get_group_by_id(
    id "uuid",
	current_user_id uuid
)  RETURNS tp_group_info
AS $$
DECLARE
     group_info "tp_group_info";
BEGIN
    CALL pe_authorize(current_user_id, ARRAY['admin'::tp_user_role]);
    SELECT group_id as Id, name
	INTO group_info 
	FROM "tb_group" WHERE group_id = id;
	
    IF NOT FOUND THEN       
		group_info.message := 'group_not_found';
    END IF;
	RETURN group_info;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION fn_delete_group(
   id uuid,
	current_user_id uuid
)  RETURNS tp_group_info
AS $$
DECLARE
     group_info "tp_group_info";
BEGIN
    CALL pe_authorize(current_user_id, ARRAY['admin'::tp_user_role]);
    DELETE FROM "tb_document_access" WHERE group_id = id;  
    DELETE FROM "tb_user_group" WHERE group_id = id;
    DELETE FROM "tb_group" WHERE group_id = id;
    
    IF NOT FOUND THEN
        group_info.message := 'group_not_found';		
    END IF;  
	
	RETURN group_info;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION fn_update_group(
    group_info "tp_group_info",
	current_user_id uuid
) RETURNS tp_group_info
AS $$

BEGIN
    CALL pe_authorize(current_user_id, ARRAY['admin'::tp_user_role]);
    UPDATE "tb_group"
    SET name = group_info.Name
    WHERE group_id = group_info.Id
    RETURNING group_id, name INTO group_info;
	
   IF NOT FOUND THEN
        group_info.message := 'group_not_found';
    END IF;  
	
   RETURN group_info;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION fn_add_user_group(
   user_group_info "tp_user_group_info",
	current_user_id uuid
) RETURNS "tp_user_group_info"
AS $$
DECLARE
     entity_exists "tp_user_group_info";
BEGIN
    CALL pe_authorize(current_user_id, ARRAY['admin'::tp_user_role]);
    CALL pe_user_exists(user_group_info.user_id);
    CALL pe_group_exists(user_group_info.group_id);
    SELECT user_group_id,group_id, user_id FROM "tb_user_group" 
	WHERE group_id = user_group_info.group_id 
	and user_id= user_group_info.user_id 
	INTO entity_exists;
    
	IF FOUND THEN
         entity_exists.message := 'user_already_in_group';
		 user_group_info := entity_exists;
    ELSE
        INSERT INTO "tb_user_group" (user_group_id, group_id, user_id)
   		VALUES (gen_random_uuid(), user_group_info.group_id, user_group_info.user_id)
		RETURNING user_group_id, group_id,user_id INTO user_group_info;	
    END IF;
	RETURN user_group_info;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION fn_delete_user_group(
   user_group_info "tp_user_group_info",
   current_user_id uuid
)  RETURNS "tp_user_group_info"
AS $$
BEGIN  
    CALL pe_authorize(current_user_id, ARRAY['admin'::tp_user_role]);
    DELETE FROM "tb_user_group"
	WHERE "tb_user_group".group_id = user_group_info.group_id 
	and "tb_user_group".user_id= user_group_info.user_id;
	
    IF NOT FOUND THEN
        user_group_info.message := 'user_group_not_found';		
    END IF;  
	
	RETURN user_group_info;
END;
$$ LANGUAGE plpgsql;
