SET search_path TO app_users;
DROP FUNCTION IF EXISTS "insert_user";
DROP FUNCTION IF EXISTS "authorize_user";
DROP FUNCTION IF EXISTS "get_user_by_id";
DROP FUNCTION IF EXISTS "delete_user";
DROP FUNCTION IF EXISTS "update_user";

DROP FUNCTION IF EXISTS "insert_group";
DROP FUNCTION IF EXISTS "get_group_by_id";
DROP FUNCTION IF EXISTS "delete_group";
DROP FUNCTION IF EXISTS "update_group";
DROP FUNCTION IF EXISTS "add_user_group";
DROP FUNCTION IF EXISTS "delete_user_group";

DROP FUNCTION IF EXISTS "add_user_group";
DROP FUNCTION IF EXISTS "delete_user_group";

DROP PROCEDURE IF EXISTS "authorize";
DROP PROCEDURE IF EXISTS "user_exists";
DROP PROCEDURE IF EXISTS "document_exists";
DROP PROCEDURE IF EXISTS "group_exists";

DROP TYPE IF EXISTS "group_info";
DROP TYPE IF EXISTS "user_group_info";
DROP TYPE IF EXISTS "user_info";

SET search_path TO app_documents;
DROP FUNCTION IF EXISTS "insert_document";
DROP FUNCTION IF EXISTS "get_document_by_id";
DROP FUNCTION IF EXISTS "insert_document";
DROP FUNCTION IF EXISTS "add_document_group";
DROP FUNCTION IF EXISTS "add_document_group";
DROP FUNCTION IF EXISTS "add_document_user";
DROP FUNCTION IF EXISTS "remove_document_group";
DROP FUNCTION IF EXISTS "remove_document_user";

DROP TYPE IF EXISTS "document_access_info";
DROP TYPE IF EXISTS "document_info";

SET search_path TO public;
DROP TABLE IF EXISTS "user_group";
DROP TABLE IF EXISTS "document_groups";
DROP TABLE IF EXISTS "document_users";
DROP TABLE IF EXISTS "group";
DROP TABLE IF EXISTS "document";
DROP TABLE IF EXISTS "user";

DROP TYPE IF EXISTS "user_role";

CREATE TABLE document (
    "document_id" uuid NOT NULL,
    "name" text NOT NULL,
    "description" text NOT NULL,
    "category" text NOT NULL,
    "filename" text NOT NULL,   
    "posted_date" timestamp with time zone NOT NULL,    
    CONSTRAINT "pk_document" PRIMARY KEY ("document_id")
);

CREATE TABLE "group" (
    "group_id" uuid NOT NULL,
    "name" text NOT NULL,
    CONSTRAINT "pk_group" PRIMARY KEY ("group_id")
);

CREATE TYPE user_role AS ENUM 
('admin', 
'manager',
'regular');

CREATE TABLE "user" (
    "user_id" uuid NOT NULL,
    "username" text NOT NULL,
    "password_hash" text NOT NULL,   
    "role" public.user_role,
    CONSTRAINT "pk_user" PRIMARY KEY ("user_id")
);

CREATE TABLE document_groups (
    "document_access_id" uuid NOT NULL,
    "document_id" uuid NOT NULL,
    "group_id" uuid NULL,
    CONSTRAINT "pk_document_groups" PRIMARY KEY ("document_access_id"),
    CONSTRAINT "pk_document_groups_document_document_id" FOREIGN KEY ("document_id") REFERENCES document ("document_id") ON DELETE CASCADE,
    CONSTRAINT "pk_document_groups_group_group_id" FOREIGN KEY ("group_id") REFERENCES "group" ("group_id")   
);

CREATE TABLE document_users (
    "document_access_id" uuid NOT NULL,
    "document_id" uuid NOT NULL,
    "group_id" uuid NULL,
    "user_id" uuid NULL,
    CONSTRAINT "pk_document_users" PRIMARY KEY ("document_access_id"),
    CONSTRAINT "pk_document_users_user_document_document_id" FOREIGN KEY ("document_id") REFERENCES document ("document_id") ON DELETE CASCADE,  
    CONSTRAINT "pk_document_users_user_user_user_id" FOREIGN KEY ("user_id") REFERENCES "user" ("user_id")
);

CREATE TABLE user_group (
    "user_group_id" uuid NOT NULL,
    "user_id" uuid NOT NULL,
    "group_id" uuid NOT NULL,
    CONSTRAINT "pk_user_group" PRIMARY KEY ("user_group_id"),
    CONSTRAINT "pk_user_group_group_group_id" FOREIGN KEY ("group_id") REFERENCES "group" ("group_id") ON DELETE CASCADE,
    CONSTRAINT "pk_user_group_user_group_id" FOREIGN KEY ("user_id") REFERENCES "user" ("user_id") ON DELETE CASCADE
);

CREATE INDEX "ix_document_users_document_id" ON document_users ("document_id");
CREATE INDEX "ix_document_groups_document_id" ON document_groups ("document_id");

CREATE INDEX "ix_document_groups_group_id" ON document_groups ("group_id");
CREATE INDEX "ix_document_users_user_id" ON document_users ("user_id");

CREATE INDEX "IX_usergroup_group_id" ON user_group ("group_id");
CREATE INDEX "IX_usergroup_user_id" ON user_group ("user_id");

-----

CREATE OR REPLACE PROCEDURE app_users.authorize(userid uuid, roles public.user_role[])
AS $$
BEGIN
    IF NOT EXISTS(
        SELECT 1 FROM "user" WHERE user_id = userid AND role = ANY(roles)
    ) THEN
        RAISE EXCEPTION 'Unauthorized';
    END IF;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE PROCEDURE app_users.user_exists(id uuid)
AS $$
BEGIN
    IF NOT EXISTS(
        SELECT 1 FROM "user" WHERE user_id = id
    ) THEN
        RAISE EXCEPTION 'User not found';
    END IF;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE PROCEDURE app_documents.document_exists(id uuid)
AS $$
BEGIN
    IF NOT EXISTS(
        SELECT 1 FROM "document" WHERE document_id = id
    ) THEN
        RAISE EXCEPTION 'Document not found';
    END IF;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE PROCEDURE app_users.group_exists(id uuid)
AS $$
BEGIN
    IF NOT EXISTS(
        SELECT 1 FROM "group" WHERE group_id = id
    ) THEN
        RAISE EXCEPTION 'Group not found';
    END IF;
END;
$$ LANGUAGE plpgsql;

------ Create a initial ADM USER for testing 
INSERT INTO "user" (user_id, username, password_hash, role) Values ('2E5AD477-6412-4B8C-9035-D9F296FEC160','admin','fPHfVG67uTfMuOE3FU8siL6aSYC7tWjUZY5A/jKFItToG76ffJcVNGStwmK8I133','admin');
