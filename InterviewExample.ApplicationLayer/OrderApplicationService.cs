using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterviewExample.ApplicationLayer.Interfaces;
using InterviewExample.Domain;
using Newtonsoft.Json;

namespace InterviewExample.ApplicationLayer
{
    public class OrderApplicationService
    {
        private IOrdersRepository _ordersRepository;
        private readonly ICustomerRepository _customerRepository;

        public OrderApplicationService(IOrdersRepository ordersRepository, ICustomerRepository customerRepository)
        {
            _ordersRepository = ordersRepository;
            _customerRepository = customerRepository;
        }

        public Order CreateOrder(string order, long customerId)
        {
            var orderObject = JsonConvert.DeserializeObject<Order>(order);

            // Validate the order
            Validate(orderObject);

            // Get customer
            var customer = _customerRepository.GetCustomer(customerId);

            // Check if preferred customer and apply discount
            if (customer.PreferredCustomer)
            {
                orderObject.Value = orderObject.Value * .9m;
            }

            // Save order
            orderObject.Id = _ordersRepository.SaveOrder(orderObject, customerId);

            return orderObject;
        }

        private void Validate(Order order)
        {
            if (string.IsNullOrEmpty(order.Description))
            {
                throw new ValidationException("Order description must be entered.");
            }

            if (order.Value <= 0)
            {
                throw new ValidationException("The order must have a value.");
            }

            Validate(order.DeliveryAddress);
        }

        private void Validate(Address address)
        {
            if (string.IsNullOrEmpty(address.AddressLine1))
            {
                throw new ValidationException("Address line 1 must be entered.");
            }

            if (string.IsNullOrEmpty(address.Postcode))
            {
                throw new ValidationException("Postcode must be entered.");
            }
        }
    }
}
