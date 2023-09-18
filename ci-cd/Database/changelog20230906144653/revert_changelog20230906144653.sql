ALTER TABLE `passports` MODIFY COLUMN `nationality` varchar(25) CHARACTER SET utf8mb4 NOT NULL;

ALTER TABLE `passports` MODIFY COLUMN `issuing_state_code` varchar(25) CHARACTER SET utf8mb4 NOT NULL;

DELETE FROM `__EFMigrationsHistory`
WHERE `migration_id` = '20230906144653_Passport_ChangeDataTypes_Nationality_IssuingStateCode';

