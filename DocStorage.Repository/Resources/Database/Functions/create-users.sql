DROP FUNCTION IF EXISTS "fn_insert_user";
DROP FUNCTION IF EXISTS "fn_authorize_user";
DROP FUNCTION IF EXISTS "fn_get_user_by_id";
DROP FUNCTION IF EXISTS "fn_delete_user";
DROP FUNCTION IF EXISTS "fn_update_user";

DROP TYPE IF EXISTS "tp_user_info";

CREATE TYPE "tp_user_info" AS (
   id uuid,
   username text,
   password text,
   role tp_user_role,
   message "tp_error_types"
);

CREATE OR REPLACE FUNCTION fn_insert_user(
    user_info "tp_user_info",
	current_user_id uuid
) RETURNS tp_user_info
AS $$
DECLARE
    user_exists "tp_user_info";
BEGIN
    CALL pe_authorize(current_user_id, ARRAY['admin'::tp_user_role]);
    SELECT user_id as Id, username, null, role FROM "tb_user" WHERE username = user_info.Username INTO user_exists; -- We don't want show the password in any cases.
    IF FOUND THEN
        user_exists.message := 'user_already_exists';
		user_info := user_exists;
    ELSE
        INSERT INTO "tb_user" (user_id, username, password_hash, role)
    	VALUES (gen_random_uuid(), user_info.username, user_info.password, user_info.role)
		RETURNING user_id, username, null, role INTO user_info;	
    END IF;
	RETURN user_info;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION fn_authorize_user(
    user_info "tp_user_info"	
) RETURNS "tp_user_info"
AS $$
BEGIN
    SELECT user_id as Id, username as Username, password_hash as Password, role as Role 
	INTO user_info 
	FROM "tb_user" WHERE username = user_info.Username;
	
    IF NOT FOUND THEN       
		user_info.message := 'user_not_found';
    END IF;
	RETURN user_info;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION fn_get_user_by_id(
    id "uuid",
	current_user_id "uuid"
) RETURNS tp_user_info
AS $$
DECLARE
    user_info "tp_user_info";
BEGIN
    CALL pe_authorize(current_user_id, ARRAY['admin'::tp_user_role]);
    SELECT user_id as Id, username as Username, null as Password, role as Role -- We don't want show the password in any cases.
	INTO user_info 
	FROM "tb_user" WHERE user_id = id;
	
    IF NOT FOUND THEN       
		user_info.message := 'user_not_found';
    END IF;
	RETURN user_info;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION fn_delete_user(
    id "uuid",
	current_user_id "uuid"
) RETURNS tp_user_info
AS $$
DECLARE
     user_info "tp_user_info";
BEGIN
    CALL pe_authorize(current_user_id, ARRAY['admin'::tp_user_role]);
    DELETE FROM "tb_document_access" WHERE user_id = id;  
    DELETE FROM "tb_user_group" WHERE user_id = id;
    DELETE FROM "tb_user" WHERE user_id = id;   
    
    IF NOT FOUND THEN
        user_info.message := 'user_not_found';		
    END IF;  
	
   RETURN user_info;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION fn_update_user(
    user_info "tp_user_info",
	current_user_id "uuid"
) RETURNS tp_user_info
AS $$
BEGIN
    CALL pe_authorize(current_user_id, ARRAY['admin'::tp_user_role]);
    UPDATE "tb_user"
    SET role = user_info.Role
    WHERE user_id = user_info.Id
    RETURNING user_id, username, null, role INTO user_info;
	
   IF NOT FOUND THEN
        user_info.message := 'user_not_found';		
    END IF;  	
    RETURN user_info;
END;
$$ LANGUAGE plpgsql;
