using System.Collections.Generic;
using InterviewExample.Domain;

namespace InterviewExample.ApplicationLayer.Interfaces
{
    public interface IOrdersRepository
    {
        List<Order> GetCustomerOrders(long customerId);
        long SaveOrder(Order order, long customerId);
    }
}
