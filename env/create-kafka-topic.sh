#! /bin/bash
 
TOPIC_NAMES=("Nebim.Era.Plt.Comm.Customer.Cdc")
echo "Waiting for kafka to be ready... "
sleep 10             
for topicName in "${TOPIC_NAMES[@]}"
do
  echo "creating $topicName"
  docker exec Comm.Customer_kafka bash -c "kafka-topics.sh --create --bootstrap-server localhost:9044 --replication-factor 1 --partitions 1 --if-not-exists --topic $topicName;"
done