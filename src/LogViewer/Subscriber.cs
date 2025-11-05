using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading.Channels;

namespace LogViewer;

public class Subscriber
{
    private readonly string _hostname = "localhost";
    private readonly string _queueName = "subscriber";
    private readonly string _userName = "1234";
    private readonly string _password = "1234";
    private readonly int _port = 5672;
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly IBasicProperties _properties;

    public void ProcessOrder(Subscriber subscriber) {
            Console.WriteLine("Repartidor esperando...");
            Console.WriteLine("Pedido completado.");
    }

    public Subscriber() {
        var factory = new ConnectionFactory()
        {
            HostName = _hostname,
            UserName = _userName,
            Password = _password,
            Port = _port
        };
        _connection = factory.CreateConnection();

        _channel = _connection.CreateModel();
        _channel.ExchangeDeclare(_exchangeName, ExchangeType.fanout);
        _properties = _channel.CreateBasicProperties();
        _properties.Persistent = true;

        var queueName = tempQueue.QueueName;

        _channel.QueueBind(queue: queueName, exchange: _exchangeName, routingKey: "");

        var consumer = new EventingBasicConsumer(_channel);

        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine($"Pedido recibido: {message}");
            ProcessOrder(message);
        };

        _channel.BasicConsume(
            queue: _queueName,
            autoAck: true,
            consumer: consumer
);

    }
}