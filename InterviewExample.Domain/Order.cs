namespace InterviewExample.Domain
{
    public class Order
    {
        public long Id { get; set; }
        public string Description { get; set; }
        public decimal Value { get; set; }
        public Address DeliveryAddress { get; set; }
    }
}
