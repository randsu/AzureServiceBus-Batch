using System;
using AzureFunction.QueueBatchProcessing.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace AzureFunction.QueueBatchProcessing
{
    public class OrderFunction
    {
        [FunctionName("OrderFunction")]
        public void Run([ServiceBusTrigger("lab-queue", Connection = "SbConString")] Order[] orders, ILogger log)
        {
            foreach (var order in orders)            
                Console.WriteLine($"Order '{order.Id}' with the amount of {order.Amount:C} created at {order.Date}");
            
        }
    }
}
