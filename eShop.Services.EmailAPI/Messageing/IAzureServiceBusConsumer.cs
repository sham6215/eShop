namespace eShop.Services.EmailAPI.Messageing
{
    public interface IAzureServiceBusConsumer
    {
        Task Start();
        Task Stop();
    }
}
