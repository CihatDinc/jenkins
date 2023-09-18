--liquibase formatted sql
--changeset author:20230526102226 context:initial
CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `migration_id` varchar(150)  NOT NULL,
    `product_version` varchar(32)  NOT NULL,
    CONSTRAINT `pk___ef_migrations_history` PRIMARY KEY (`migration_id`)
) ;



CREATE TABLE `customers` (
    `id` varchar(128)  NOT NULL,
    `code` varchar(50)  NOT NULL,
    `name` varchar(50)  NULL,
    `surname` varchar(50)  NULL,
    `birth_date` datetime(3) NULL,
    `gender_code` int NULL,
    `phone_number` varchar(25)  NULL,
    `email` varchar(50)  NULL,
    `communication_preferences` json NOT NULL,
    `is_deleted` tinyint(1) NOT NULL,
    `deleted_by` varchar(128)  NULL,
    `deleted_at` datetime(3) NULL,
    `updated_by` varchar(128)  NULL,
    `updated_at` datetime(3) NULL,
    `created_by` varchar(128)  NOT NULL,
    `created_at` datetime(3) NOT NULL,
    `tenant_id` varchar(128)  NOT NULL,
    CONSTRAINT `pk_customers` PRIMARY KEY (`id`)
) ;
 
CREATE TABLE `customer_addresses` (
    `id` varchar(128)  NOT NULL,
    `address_name` varchar(50)  NOT NULL,
    `first_name` varchar(50)  NOT NULL,
    `last_name` varchar(50)  NOT NULL,
    `phone_number` varchar(25)  NOT NULL,
    `email` varchar(50)  NOT NULL,
    `identity_number` varchar(25)  NOT NULL,
    `country_id` int NULL,
    `country_name` varchar(25)  NOT NULL,
    `city_id` int NULL,
    `city_name` varchar(25)  NOT NULL,
    `district_id` int NULL,
    `district_name` varchar(25)  NOT NULL,
    `address_line` varchar(100)  NULL,
    `postal_code` varchar(10)  NULL,
    `company_name` varchar(50)  NULL,
    `tax_office` varchar(50)  NULL,
    `tax_number` varchar(10)  NULL,
    `is_default` tinyint NOT NULL,
    `invoice_type` int NOT NULL,
    `customer_id` varchar(128)  NOT NULL,
    CONSTRAINT `pk_customer_addresses` PRIMARY KEY (`id`),
    CONSTRAINT `fk_customer_addresses_customers_customer_id` FOREIGN KEY (`customer_id`) REFERENCES `customers` (`id`) ON DELETE CASCADE
) ;

CREATE INDEX `ix_customer_addresses_customer_id` ON `customer_addresses` (`customer_id`);

INSERT INTO `__EFMigrationsHistory` (`migration_id`, `product_version`)
VALUES ('20230526102226_initial', '7.0.5');

--rollback DROP TABLE `customer_addresses`;

--rollback DROP TABLE `customers`;

--rollback DELETE FROM `__EFMigrationsHistory`
--rollback WHERE `migration_id` = '20230526102226_initial';

