--liquibase formatted sql
--changeset author:20230719093752 context:Passport_Init
CREATE TABLE `passports` (
    `id` varchar(128)  NOT NULL,
    `customer_id` varchar(128)  NOT NULL,
    `number` varchar(25)  NOT NULL,
    `issuing_state_code` varchar(25)  NOT NULL,
    `nationality` varchar(25)  NOT NULL,
    `issue_date` datetime(3) NOT NULL,
    `tenant_id` varchar(128)  NOT NULL,
    `is_deleted` tinyint(1) NOT NULL,
    `deleted_by` varchar(128)  NULL,
    `deleted_at` datetime(3) NULL,
    `updated_by` varchar(128)  NULL,
    `updated_at` datetime(3) NULL,
    `created_by` varchar(128)  NOT NULL,
    `created_at` datetime(3) NOT NULL,
    CONSTRAINT `pk_passports` PRIMARY KEY (`id`)
);

INSERT INTO `__EFMigrationsHistory` (`migration_id`, `product_version`)
VALUES ('20230719093752_Passport_Init', '7.0.9');

--rollback DROP TABLE `passports`;

--rollback DELETE FROM `__EFMigrationsHistory`
--rollback WHERE `migration_id` = '20230719093752_Passport_Init';

