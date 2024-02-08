// See https://aka.ms/new-console-template for more information
using Azure.Core;
using Azure.Identity;
using Azure.Messaging.ServiceBus;
using AzureFunction.QueueBatchProcessing.Models;

var experimentalQueueConnectionString = "Endpoint=sb://experimental-rsa.servicebus.windows.net/;SharedAccessKeyName=manage;SharedAccessKey=kDyuHPy4W0FRB/FMLicuQSff/hb6lQ2gi+ASbBZ+uf4=;EntityPath=experimental-queue";

Console.WriteLine("ServiceBus connectionstring or enter for default:");
var connString = Console.ReadLine();
if (string.IsNullOrEmpty(connString))
    connString = experimentalQueueConnectionString;

var client = new ServiceBusClient(connString);

var entityPath = connString.Split(';')?.Last()?.Split('=')?.Last();
if(string.IsNullOrEmpty(entityPath))
{
    Console.WriteLine("EntityPath or enter for default:");
    entityPath = Console.ReadLine();
}

var sender = client.CreateSender(entityPath);
using ServiceBusMessageBatch messageBatch = await sender.CreateMessageBatchAsync();

const int numOfMessages = 15;
for (int i = 1; i <= numOfMessages; i++)
{
    var payload =
        new Order(
            Guid.NewGuid(),
            (int)Math.Floor(numOfMessages * i * 0.3d),
            DateTime.Now
        ).ToString();

    if (!messageBatch.TryAddMessage(new ServiceBusMessage(payload)))
        throw new Exception($"The message {i} is too large to fit in the batch.");
}
try
{
    await sender.SendMessagesAsync(messageBatch);
    Console.WriteLine($"A batch of {numOfMessages} messages has been published to the servicebus namespace.");
}
finally
{
    await sender.DisposeAsync();
    await client.DisposeAsync();
}

Console.WriteLine("Press any key to end the application");
Console.ReadKey();
