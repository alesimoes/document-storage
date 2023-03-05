DROP FUNCTION IF EXISTS "fn_insert_user";
DROP FUNCTION IF EXISTS "fn_authorize_user";
DROP FUNCTION IF EXISTS "fn_get_user_by_id";
DROP FUNCTION IF EXISTS "fn_delete_user";
DROP FUNCTION IF EXISTS "fn_update_user";

DROP FUNCTION IF EXISTS "fn_insert_document";
DROP FUNCTION IF EXISTS "fn_get_document_by_id";

DROP FUNCTION IF EXISTS "fn_insert_group";
DROP FUNCTION IF EXISTS "fn_get_group_by_id";
DROP FUNCTION IF EXISTS "fn_delete_group";
DROP FUNCTION IF EXISTS "fn_update_group";
DROP FUNCTION IF EXISTS "fn_add_user_group";
DROP FUNCTION IF EXISTS "fn_delete_user_group";

DROP FUNCTION IF EXISTS "fn_add_user_group";
DROP FUNCTION IF EXISTS "fn_delete_user_group";

DROP FUNCTION IF EXISTS "fn_insert_document";
DROP FUNCTION IF EXISTS "fn_add_document_access_group";

DROP FUNCTION IF EXISTS "fn_add_document_access_group";
DROP FUNCTION IF EXISTS "fn_add_document_access_user";
DROP FUNCTION IF EXISTS "fn_remove_document_access_group";
DROP FUNCTION IF EXISTS "fn_remove_document_access_user";

DROP PROCEDURE IF EXISTS "pe_authorize";
DROP PROCEDURE IF EXISTS "pe_user_exists";
DROP PROCEDURE IF EXISTS "pe_document_exists";
DROP PROCEDURE IF EXISTS "pe_group_exists";

DROP TYPE IF EXISTS "tp_group_info";
DROP TYPE IF EXISTS "tp_user_group_info";
DROP TYPE IF EXISTS "tp_document_access_info";
DROP TYPE IF EXISTS "tp_document_info";
DROP TYPE IF EXISTS "tp_user_info";
DROP TYPE IF EXISTS "tp_error_types";

DROP TABLE IF EXISTS "tb_user_group";
DROP TABLE IF EXISTS "tb_document_access";
DROP TABLE IF EXISTS "tb_group";
DROP TABLE IF EXISTS "tb_document";
DROP TABLE IF EXISTS "tb_user";

DROP TYPE IF EXISTS "tp_user_role";


CREATE TABLE tb_document (
    "document_id" uuid NOT NULL,
    "name" text NOT NULL,
    "description" text NOT NULL,
    "category" text NOT NULL,
    "filename" text NOT NULL,   
    "posted_date" timestamp with time zone NOT NULL,    
    CONSTRAINT "PK_document" PRIMARY KEY ("document_id")
);


CREATE TABLE "tb_group" (
    "group_id" uuid NOT NULL,
    "name" text NOT NULL,
    CONSTRAINT "PK_group" PRIMARY KEY ("group_id")
);

CREATE TYPE tp_user_role AS ENUM ('admin', 'manager', 'regular');

CREATE TABLE "tb_user" (
    "user_id" uuid NOT NULL,
    "username" text NOT NULL,
    "password_hash" text NOT NULL,
    "role" tp_user_role,
    CONSTRAINT "PK_user" PRIMARY KEY ("user_id")
);

CREATE TABLE tb_document_access (
    "document_access_id" uuid NOT NULL,
    "document_id" uuid NOT NULL,
    "group_id" uuid NULL,
    "user_id" uuid NULL,
    CONSTRAINT "PK_documentaccess" PRIMARY KEY ("document_access_id"),
    CONSTRAINT "FK_documentaccess_document_DocumentId" FOREIGN KEY ("document_id") REFERENCES tb_document ("document_id") ON DELETE CASCADE,
    CONSTRAINT "FK_documentaccess_group_GroupId" FOREIGN KEY ("group_id") REFERENCES "tb_group" ("group_id"),
    CONSTRAINT "FK_documentaccess_user_UserId" FOREIGN KEY ("user_id") REFERENCES "tb_user" ("user_id")
);


CREATE TABLE tb_user_group (
    "user_group_id" uuid NOT NULL,
    "user_id" uuid NOT NULL,
    "group_id" uuid NOT NULL,
    CONSTRAINT "PK_usergroup" PRIMARY KEY ("user_group_id"),
    CONSTRAINT "FK_usergroup_group_GroupId" FOREIGN KEY ("group_id") REFERENCES "tb_group" ("group_id") ON DELETE CASCADE,
    CONSTRAINT "FK_usergroup_user_UserId" FOREIGN KEY ("user_id") REFERENCES "tb_user" ("user_id") ON DELETE CASCADE
);


CREATE INDEX "IX_documentaccess_DocumentId" ON tb_document_access ("document_id");


CREATE INDEX "IX_documentaccess_GroupId" ON tb_document_access ("group_id");


CREATE INDEX "IX_documentaccess_UserId" ON tb_document_access ("user_id");


CREATE INDEX "IX_usergroup_GroupId" ON tb_user_group ("group_id");


CREATE INDEX "IX_usergroup_UserId" ON tb_user_group ("user_id");

-----

CREATE TYPE tp_error_types AS ENUM 
('',
 'user_already_exists',
 'user_not_found',
 'document_not_found',
 'group_not_found',
 'group_already_exists',
 'user_already_in_group',
 'user_group_not_found',
 'group_already_exists_in_document_access',
 'group_not_found_in_document_access',
 'user_already_exists_in_document_access',
 'user_not_found_in_document_access'
 );


CREATE TYPE "tp_user_info" AS (
   Id uuid,
   Username text,
   Password text,
   Role tp_user_role
);

CREATE OR REPLACE PROCEDURE pe_authorize(userid uuid, roles tp_user_role[])
AS $$
BEGIN
    IF NOT EXISTS(
        SELECT 1 FROM "tb_user" WHERE user_id = userid AND role = ANY(roles)
    ) THEN
        RAISE EXCEPTION 'Unauthorized';
    END IF;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE PROCEDURE pe_user_exists(id uuid)
AS $$
BEGIN
    IF NOT EXISTS(
        SELECT 1 FROM "tb_user" WHERE user_id = id
    ) THEN
        RAISE EXCEPTION 'User not found';
    END IF;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE PROCEDURE pe_document_exists(id uuid)
AS $$
BEGIN
    IF NOT EXISTS(
        SELECT 1 FROM "tb_document" WHERE document_id = id
    ) THEN
        RAISE EXCEPTION 'Document not found';
    END IF;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE PROCEDURE pe_group_exists(id uuid)
AS $$
BEGIN
    IF NOT EXISTS(
        SELECT 1 FROM "tb_group" WHERE group_id = id
    ) THEN
        RAISE EXCEPTION 'Group not found';
    END IF;
END;
$$ LANGUAGE plpgsql;

------ Create a initial ADM USER for testing 
INSERT INTO "tb_user" (user_id, username, password_hash, role) Values ('2E5AD477-6412-4B8C-9035-D9F296FEC160','admin','$2a$11$Fp1asDWq2.gKvkidTuyZRO6H/C0SUJlXX4ndHl/6M/fduqJQFgn8i','admin');
