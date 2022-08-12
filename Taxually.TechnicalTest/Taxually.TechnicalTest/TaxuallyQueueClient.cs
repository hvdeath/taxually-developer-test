namespace Taxually.TechnicalTest
{
    public class TaxuallyQueueClient : ITaxuallyQueueClient
    {
        public Task EnqueueAsync<TPayload>(string queueName, TPayload payload)
        {
            // Code to send to message queue removed for brevity
            return Task.CompletedTask;
        }
    }
}
