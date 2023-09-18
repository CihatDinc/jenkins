echo "Waiting for debezium to be ready... "
sleep 1
# shellcheck disable=SC2016
curl -H 'Content-Type: application/json' localhost:8041/connectors --data '
{
  "name": "era-plt-domain-customer-outbox",
  "config": {
    "connector.class": "io.debezium.connector.mysql.MySqlConnector",
    "tasks.max": "1",
    "database.hostname": "mysql",
    "database.port": "3341",
    "database.user": "root",
    "database.password": "P@ssword123",
    "database.server.id": "184054",
    "database.server.name": "customer",
    "tombstones.on.delete": "false",
    "database.include.list": "nebim.era.plt.comm.domain.customer.db",
    "key.converter": "org.apache.kafka.connect.storage.StringConverter",
    "key.converter.schemas.enable": "false",
    "binary.handling.mode": "bytes",
    "value.converter": "org.apache.kafka.connect.json.JsonConverter",
    "value.converter.schemas.enable": "false",
    "database.history.kafka.bootstrap.servers": "kafka:29041",
    "database.history.kafka.topic": "customer.schema-changes",
    "include.schema.changes": "false",
    "transforms": "outbox",
    "transforms.outbox.type": "io.debezium.transforms.outbox.EventRouter",
    "table.include.list": "nebim.era.plt.comm.domain.customer.db._MessageOutbox",
    "transforms.outbox.table.field.event.id": "MessageId",
    "transforms.outbox.table.field.event.key": "GroupId",
    "transforms.outbox.table.field.event.type": "MessageName",
    "transforms.outbox.table.field.event.payload": "Message",
    "transforms.outbox.table.expand.json.payload": "false",
    "transforms.outbox.route.by.field": "Destination",
    "transforms.outbox.route.topic.replacement": "${routedByValue}",
    "transforms.outbox.table.fields.additional.placement": "MessageContentType:header,CorrelationId:header,TimeToLive:header,RequestId:header,Source:header,ReplyTo:header,ExtendedProperties:header"
  }
}'