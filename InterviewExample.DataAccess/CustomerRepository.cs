using InterviewExample.ApplicationLayer.Interfaces;
using InterviewExample.Domain;

namespace InterviewExample.DataAccess
{
    public class CustomerRepository : ICustomerRepository
    {
        public long SaveCustomer(Customer customer)
        {
            if (customer.Id == 0)
            {
                return CreateCustomer(customer);
            }
            UpdateCustomer(customer);
            return customer.Id;
        }

        public Customer GetCustomer(long customerId)
        {
            // Assume this has been implemented correctly
            return new Customer();
        }

        public void DeleteCustomer(long customerId)
        {
            // Assume this has been implemented correctly
        }

        public long SaveAddress(Address address, long customerId)
        {
            if (address.Id == 0)
            {
                return CreateAddress(address, customerId);
            }
            UpdateAddress(address, customerId);
            return address.Id;
        }

        private void UpdateCustomer(Customer customer)
        {
            // Assume this has been implemented correctly
        }

        private long CreateCustomer(Customer customer)
        {
            // Assume this has been implemented correctly
            return 12345;
        }

        private void UpdateAddress(Address address, long customerId)
        {
            // Assume this has been implemented correctly
        }

        private long CreateAddress(Address address, long customerId)
        {
            // Assume this has been implemented correctly
            return 12345;
        }
    }
}
