// See https://aka.ms/new-console-template for more information
using Azure.Core;
using Azure.Identity;
using Azure.Messaging.ServiceBus;
using AzureFunction.QueueBatchProcessing.Models;

var client = new ServiceBusClient("Paste the SB con string here");
var sender = client.CreateSender("lab-queue");
using ServiceBusMessageBatch messageBatch = await sender.CreateMessageBatchAsync();

const int numOfMessages = 15;
for (int i = 1; i <= numOfMessages; i++)
{
    if (!messageBatch.TryAddMessage(new ServiceBusMessage(new Order(Guid.NewGuid(), numOfMessages * 10, DateTime.Now).ToString())))
        throw new Exception($"The message {i} is too large to fit in the batch.");
}
try
{
    await sender.SendMessagesAsync(messageBatch);
    Console.WriteLine($"A batch of {numOfMessages} messages has been published to the queue.");
}
finally
{
    await sender.DisposeAsync();
    await client.DisposeAsync();
}

Console.WriteLine("Press any key to end the application");
Console.ReadKey();

