SET search_path TO app_users;
DROP FUNCTION IF EXISTS insert_group;
DROP FUNCTION IF EXISTS get_group_by_id;
DROP FUNCTION IF EXISTS delete_group;
DROP FUNCTION IF EXISTS update_group;
DROP FUNCTION IF EXISTS add_user_group;
DROP FUNCTION IF EXISTS delete_user_group;

DROP TYPE IF EXISTS group_info;
DROP TYPE IF EXISTS user_group_info;

CREATE TYPE group_info AS (
   id uuid,
   name text   
);

CREATE TYPE user_group_info AS (
   id uuid,
   user_id uuid,
   group_id uuid
);

CREATE OR REPLACE FUNCTION insert_group(
    group_info app_users.group_info,
	current_user_id uuid
)  RETURNS app_users.group_info
AS $$
DECLARE
     entity_exists app_users.group_info;
BEGIN
    CALL app_users.authorize(current_user_id, ARRAY['admin'::public.user_role]);
    SELECT group_id, name FROM "group" 
	WHERE name = group_info.name INTO entity_exists;
   
    IF FOUND THEN	     
       RAISE EXCEPTION 'Group already exist';	
    ELSE
        INSERT INTO "group" (group_id, name)
    VALUES (gen_random_uuid(), group_info.name)
	RETURNING group_id, name INTO group_info;	
    END IF;
	RETURN group_info;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION get_group_by_id(
    group_info app_users.group_info,
	current_user_id uuid
)  RETURNS app_users.group_info
AS $$
BEGIN
    CALL app_users.authorize(current_user_id, ARRAY['admin'::public.user_role]);
    SELECT group_id as id, name
	INTO group_info 
	FROM "group" WHERE group_id = group_info.id;
	
    IF NOT FOUND THEN       
		RAISE EXCEPTION 'Group not found';	
    END IF;
	RETURN group_info;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION delete_group(
   group_info app_users.group_info,
   current_user_id uuid
)  RETURNS app_users.group_info
AS $$
BEGIN
    CALL app_users.authorize(current_user_id, ARRAY['admin'::public.user_role]);
    DELETE FROM "document_groups" WHERE group_id = group_info.id;  
    DELETE FROM "user_group" WHERE group_id = group_info.id;
    DELETE FROM "group" WHERE group_id = group_info.id;
    
    IF NOT FOUND THEN
        RAISE EXCEPTION 'Group not found';	
    END IF;  
	
	RETURN group_info;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION update_group(
    group_info app_users.group_info,
	current_user_id uuid
) RETURNS app_users.group_info
AS $$

BEGIN
    CALL app_users.authorize(current_user_id, ARRAY['admin'::public.user_role]);
    UPDATE "group"
    SET name = group_info.name
    WHERE group_id = group_info.id
    RETURNING group_id, name INTO group_info;
	
   IF NOT FOUND THEN
       RAISE EXCEPTION 'Group not found';	
    END IF;  
	
   RETURN group_info;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION add_user_group(
   user_group_info app_users.user_group_info,
   current_user_id uuid
) RETURNS app_users.user_group_info
AS $$
DECLARE
     entity_exists app_users.user_group_info;
BEGIN
    CALL app_users.authorize(current_user_id, ARRAY['admin'::public.user_role]);
    CALL app_users.user_exists(user_group_info.user_id);
    CALL app_users.group_exists(user_group_info.group_id);
    SELECT user_group_id,group_id, user_id FROM "user_group" 
	WHERE group_id = user_group_info.group_id 
	and user_id= user_group_info.user_id 
	INTO entity_exists;
    
	IF FOUND THEN
        RAISE EXCEPTION 'User is already added to the selected group';	
    ELSE
        INSERT INTO "user_group" (user_group_id, group_id, user_id)
   		VALUES (gen_random_uuid(), user_group_info.group_id, user_group_info.user_id)
		RETURNING user_group_id, group_id,user_id INTO user_group_info;	
    END IF;
	RETURN user_group_info;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION delete_user_group(
   user_group_info app_users.user_group_info,
   current_user_id uuid
)  RETURNS app_users.user_group_info
AS $$
BEGIN  
    CALL app_users.authorize(current_user_id, ARRAY['admin'::public.user_role]);
    DELETE FROM "user_group"
	WHERE "user_group".group_id = user_group_info.group_id 
	and "user_group".user_id= user_group_info.user_id;
	
    IF NOT FOUND THEN
       RAISE EXCEPTION 'User is already added to the selected group';		
    END IF;  
	
	RETURN user_group_info;
END;
$$ LANGUAGE plpgsql;
