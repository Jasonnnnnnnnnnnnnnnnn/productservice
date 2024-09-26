namespace productservice.Service
{
    public interface IRabbitMQService
    {
        void Publish(string message);
    }

}
