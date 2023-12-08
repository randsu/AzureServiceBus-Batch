using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace AzureFunction.QueueBatchProcessing.Models
{

    public class Order
    {
        public Order()
        {

        }
        public Order(Guid id, decimal amount, DateTime date)
        {
            Id = id;
            Amount = amount;
            Date = date;
        }

        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
        }
    }
}
