using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShop.MessageBus
{
    public class MessageBus : IMessageBus
    {
        public async Task PublishMessage(object message, string queueTopicName)
        {
            /// Connection string must be defined in a project config.
            /// It is hardcoded to keep things simple.
            var connectionString = "Endpoint=sb://eshop-service-bus-sham6215.servicebus.windows.net/;SharedAccessKeyName=queue-client;SharedAccessKey=g637RLa2eSAryqGZfOGVo/Q8JzPyf26d0+ASbBgU09M=;EntityPath=emailshopingcart";
            var client = new ServiceBusClient(connectionString);
            try
            {
                var sender = client.CreateSender(queueTopicName);
                var jsonMessage = JsonConvert.SerializeObject(message);
                var busMessage = new ServiceBusMessage(Encoding.UTF8.GetBytes(jsonMessage)) {
                    CorrelationId = Guid.NewGuid().ToString()
                };

                await sender.SendMessageAsync(busMessage);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (client != null)
                {
                    await client.DisposeAsync();
                }
            }
        }
    }
}
