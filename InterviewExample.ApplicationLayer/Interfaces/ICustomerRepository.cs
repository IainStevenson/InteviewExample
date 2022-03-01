using InterviewExample.Domain;

namespace InterviewExample.ApplicationLayer.Interfaces
{
    public interface ICustomerRepository
    {
        long SaveCustomer(Customer customer);
        void DeleteCustomer(long customerId);
        Customer GetCustomer(long customerId);
        long SaveAddress(Address address, long customerId);
    }
}
