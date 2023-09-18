--liquibase formatted sql
--changeset author:20230906144653 context:Passport_ChangeDataTypes_Nationality_IssuingStateCode
ALTER TABLE `passports` MODIFY COLUMN `nationality` int NOT NULL;

ALTER TABLE `passports` MODIFY COLUMN `issuing_state_code` int NOT NULL;

INSERT INTO `__EFMigrationsHistory` (`migration_id`, `product_version`)
VALUES ('20230906144653_Passport_ChangeDataTypes_Nationality_IssuingStateCode', '7.0.9');

--rollback ALTER TABLE `passports` MODIFY COLUMN `nationality` varchar(25)  NOT NULL;

--rollback ALTER TABLE `passports` MODIFY COLUMN `issuing_state_code` varchar(25)  NOT NULL;

--rollback DELETE FROM `__EFMigrationsHistory`
--rollback WHERE `migration_id` = '20230906144653_Passport_ChangeDataTypes_Nationality_IssuingStateCode';

