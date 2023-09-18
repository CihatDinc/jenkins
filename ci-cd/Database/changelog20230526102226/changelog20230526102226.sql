CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `migration_id` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
    `product_version` varchar(32) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `pk___ef_migrations_history` PRIMARY KEY (`migration_id`)
) CHARACTER SET=utf8mb4;

ALTER DATABASE CHARACTER SET utf8mb4;

CREATE TABLE `customers` (
    `id` varchar(128) CHARACTER SET utf8mb4 NOT NULL,
    `code` varchar(50) CHARACTER SET utf8mb4 NOT NULL,
    `name` varchar(50) CHARACTER SET utf8mb4 NULL,
    `surname` varchar(50) CHARACTER SET utf8mb4 NULL,
    `birth_date` datetime(3) NULL,
    `gender_code` int NULL,
    `phone_number` varchar(25) CHARACTER SET utf8mb4 NULL,
    `email` varchar(50) CHARACTER SET utf8mb4 NULL,
    `communication_preferences` json NOT NULL,
    `is_deleted` tinyint(1) NOT NULL,
    `deleted_by` varchar(128) CHARACTER SET utf8mb4 NULL,
    `deleted_at` datetime(3) NULL,
    `updated_by` varchar(128) CHARACTER SET utf8mb4 NULL,
    `updated_at` datetime(3) NULL,
    `created_by` varchar(128) CHARACTER SET utf8mb4 NOT NULL,
    `created_at` datetime(3) NOT NULL,
    `tenant_id` varchar(128) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `pk_customers` PRIMARY KEY (`id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `customer_addresses` (
    `id` varchar(128) CHARACTER SET utf8mb4 NOT NULL,
    `address_name` varchar(50) CHARACTER SET utf8mb4 NOT NULL,
    `first_name` varchar(50) CHARACTER SET utf8mb4 NOT NULL,
    `last_name` varchar(50) CHARACTER SET utf8mb4 NOT NULL,
    `phone_number` varchar(25) CHARACTER SET utf8mb4 NOT NULL,
    `email` varchar(50) CHARACTER SET utf8mb4 NOT NULL,
    `identity_number` varchar(25) CHARACTER SET utf8mb4 NOT NULL,
    `country_id` int NULL,
    `country_name` varchar(25) CHARACTER SET utf8mb4 NOT NULL,
    `city_id` int NULL,
    `city_name` varchar(25) CHARACTER SET utf8mb4 NOT NULL,
    `district_id` int NULL,
    `district_name` varchar(25) CHARACTER SET utf8mb4 NOT NULL,
    `address_line` varchar(100) CHARACTER SET utf8mb4 NULL,
    `postal_code` varchar(10) CHARACTER SET utf8mb4 NULL,
    `company_name` varchar(50) CHARACTER SET utf8mb4 NULL,
    `tax_office` varchar(50) CHARACTER SET utf8mb4 NULL,
    `tax_number` varchar(10) CHARACTER SET utf8mb4 NULL,
    `is_default` tinyint NOT NULL,
    `invoice_type` int NOT NULL,
    `customer_id` varchar(128) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `pk_customer_addresses` PRIMARY KEY (`id`),
    CONSTRAINT `fk_customer_addresses_customers_customer_id` FOREIGN KEY (`customer_id`) REFERENCES `customers` (`id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE INDEX `ix_customer_addresses_customer_id` ON `customer_addresses` (`customer_id`);

INSERT INTO `__EFMigrationsHistory` (`migration_id`, `product_version`)
VALUES ('20230526102226_initial', '7.0.5');

