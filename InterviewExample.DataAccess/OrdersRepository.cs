using System.Collections.Generic;
using InterviewExample.ApplicationLayer.Interfaces;
using InterviewExample.Domain;

namespace InterviewExample.DataAccess
{
    public class OrdersRepository : IOrdersRepository
    {
        public List<Order> GetCustomerOrders(long customerId)
        {
            // Assume this has been implemented correctly
            return new List<Order>();
        }

        public long SaveOrder(Order order, long customerId)
        {
            // Assume this has been implemented correctly
            return 12456;
        }
    }
}
