--liquibase formatted sql
--changeset carlapolat:20230712102100 context:unique_indexes
create unique index ix_customers_code_tenant_id_is_deleted
            on plt_comm_customer.customers(code,tenant_id,(case when is_deleted=0 then is_deleted end));

--rollback drop index ix_customers_code_tenant_id_is_deleted on plt_comm_customer.customers;

