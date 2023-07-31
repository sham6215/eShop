using Azure.Messaging.ServiceBus;
using eShop.Services.EmailAPI.Models.Dto;
using eShop.Services.EmailAPI.Services;
using eShop.Services.EmailAPI.Services.IService;
using Newtonsoft.Json;
using System.Text;

namespace eShop.Services.EmailAPI.Messageing
{
    public class AzureServiceBusConsumer : IAzureServiceBusConsumer
    {
        private readonly string _serviceBusConnectionString;
        private readonly string _emailCartQueue;
        private readonly string _emailRegisterUserQueue;
        private readonly IConfiguration _configuration;

        private readonly ServiceBusProcessor _emailCartProcessor;
        private readonly ServiceBusProcessor _emailRegisterUserProcessor;
        private readonly EmailService _emailService;


        public AzureServiceBusConsumer(IConfiguration configuration, EmailService emailService)
        {
            _configuration = configuration;
            _serviceBusConnectionString = _configuration.GetValue<string>("ServiceBusConnectionString");
            
            _emailCartQueue = _configuration.GetValue<string>("TopicAndQueueNames:EmailShoppingCartQueue");
            _emailRegisterUserQueue = _configuration.GetValue<string>("TopicAndQueueNames:EmailRegisterUserQueue");

            var client = new ServiceBusClient(_serviceBusConnectionString);
            _emailCartProcessor = client.CreateProcessor(_emailCartQueue);
            _emailRegisterUserProcessor = client.CreateProcessor(_emailRegisterUserQueue);

            _emailService = emailService;
        }

        public async Task Start()
        {
            _emailCartProcessor.ProcessMessageAsync += _emailCartProcessor_ProcessMessageAsync;
            _emailCartProcessor.ProcessErrorAsync += _emailCartProcessor_ProcessErrorAsync;

            _emailRegisterUserProcessor.ProcessMessageAsync += _emailRegisterUserProcessor_ProcessMessageAsync;
            _emailRegisterUserProcessor.ProcessErrorAsync += _emailRegisterUserProcessor_ProcessErrorAsync;

            await _emailCartProcessor.StartProcessingAsync();
            await _emailRegisterUserProcessor.StartProcessingAsync();
        }

        private Task _emailRegisterUserProcessor_ProcessErrorAsync(ProcessErrorEventArgs arg)
        {
            Console.WriteLine(arg.Exception.ToString());
            return Task.CompletedTask;
        }

        private async Task _emailRegisterUserProcessor_ProcessMessageAsync(ProcessMessageEventArgs arg)
        {
            var message = arg.Message;
            var user = JsonConvert.DeserializeObject<UserDto>(Encoding.UTF8.GetString(message.Body));

            try
            {
                await _emailService.EmailRegisterUserAndLog(user);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private Task _emailCartProcessor_ProcessErrorAsync(ProcessErrorEventArgs arg)
        {
            
            Console.WriteLine(arg.Exception.ToString());
            return Task.CompletedTask;
        }

        private async Task _emailCartProcessor_ProcessMessageAsync(ProcessMessageEventArgs arg)
        {
            var message = arg.Message;
            string messageBody = Encoding.UTF8.GetString(message.Body);
            CartDto cartDto = JsonConvert.DeserializeObject<CartDto>(messageBody);

            try
            {
                /// All actions with email. Send, log etc
                /// 
                await _emailService.EmailCartAndLog(cartDto);
                await arg.CompleteMessageAsync(message);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task Stop()
        {
            await _emailCartProcessor.StopProcessingAsync();
            await _emailCartProcessor.DisposeAsync();
        }
    }
}
