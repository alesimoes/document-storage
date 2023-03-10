SET search_path TO app_users;
DROP FUNCTION IF EXISTS insert_user;
DROP FUNCTION IF EXISTS authorize_user;
DROP FUNCTION IF EXISTS get_user_by_id;
DROP FUNCTION IF EXISTS delete_user;
DROP FUNCTION IF EXISTS update_user;

DROP TYPE IF EXISTS user_info;

CREATE TYPE user_info AS (
   id uuid,
   username text,
   password text,
   salt text,
   role public.user_role
);

CREATE OR REPLACE FUNCTION insert_user(
    user_info app_users."user_info",
	current_user_id uuid
) RETURNS app_users."user_info"
AS $$
DECLARE
    user_exists app_users."user_info";
BEGIN
    CALL app_users.authorize(current_user_id, ARRAY['admin'::public.user_role]);
    SELECT user_id as id, username, null, null, role FROM "user" WHERE username = user_info.Username INTO user_exists;
    IF FOUND THEN
       RAISE EXCEPTION 'User already exists';
    ELSE
        INSERT INTO "user" (user_id, username, password_hash,salt, role)
    	VALUES (gen_random_uuid(), user_info.username, user_info.password, user_info.role)
		RETURNING user_id, username, null, role INTO user_info;	
    END IF;
	RETURN user_info;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION authorize_user(
    user_info app_users."user_info"	
) RETURNS app_users."user_info"
AS $$
BEGIN
    SELECT user_id as id, username as username, password_hash as password, salt, role as role 
	INTO user_info 
	FROM "user" WHERE username = user_info.username;
	
    IF NOT FOUND THEN       
		 RAISE EXCEPTION 'User not found';
    END IF;
	RETURN user_info;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION get_user_by_id(
    user_info app_users.user_info,
	current_user_id "uuid"
) RETURNS app_users.user_info
AS $$
DECLARE
    exists_user_info app_users.user_info;
BEGIN
    CALL app_users.authorize(current_user_id, ARRAY['admin'::public.user_role]);
    SELECT user_id as id, username as username, null as Password, null, role as role 
	INTO exists_user_info 
	FROM "user" WHERE user_id = user_info.id;
	
    IF NOT FOUND THEN       
		 RAISE EXCEPTION 'User not found';
    END IF;
	RETURN exists_user_info;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION delete_user(
     user_info app_users."user_info",
	current_user_id "uuid"
) RETURNS app_users."user_info"
AS $$
BEGIN
    CALL app_users.authorize(current_user_id, ARRAY['admin'::public.user_role]);
    DELETE FROM "document_users" WHERE user_id = user_info.id;  
    DELETE FROM "user_group" WHERE user_id = user_info.id;
    DELETE FROM "user" WHERE user_id = user_info.id;   
    
    IF NOT FOUND THEN
        RAISE EXCEPTION 'User not found';
    END IF;  
	
   RETURN user_info;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION update_user(
    user_info app_users."user_info",
	current_user_id "uuid"
) RETURNS app_users."user_info"
AS $$
BEGIN
    CALL app_users.authorize(current_user_id, ARRAY['admin'::public.user_role]);
    UPDATE "user"
    SET role = user_info.role
    WHERE user_id = user_info.id
    RETURNING user_id, username, null, role INTO user_info;
	
   IF NOT FOUND THEN
        RAISE EXCEPTION 'User not found';
    END IF;  	
    RETURN user_info;
END;
$$ LANGUAGE plpgsql;
