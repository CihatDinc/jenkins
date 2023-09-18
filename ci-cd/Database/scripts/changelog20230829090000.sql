--liquibase formatted sql
--changeset carlapolat:20230829090000 context:deleted_changelogs
create table deleted_changelogs(id varchar(255),filename varchar(255),deletedate datetime,deletedby  varchar(255) );

--rollback drop table deleted_changelogs;   

--changeset carlapolat:20230829090100 context:trigger
CREATE TRIGGER trigger_delete_changelog
AFTER DELETE
   ON databasechangelog FOR EACH ROW

BEGIN
-- declare variables
  DECLARE var_User varchar(50);

   -- Get user name who is performing delete
   SELECT USER() INTO var_User;

   -- Insert record  
   INSERT INTO deleted_changelogs VALUES(old.id, old.filename, SYSDATE(),var_User);

END; 

--rollback drop trigger trigger_delete_changelog;  
