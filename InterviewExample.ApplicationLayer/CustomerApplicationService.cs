using System;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using InterviewExample.ApplicationLayer.Interfaces;
using InterviewExample.Domain;
using Newtonsoft.Json;

namespace InterviewExample.ApplicationLayer
{
    public class CustomerApplicationService
    {
        private readonly ICustomerRepository _customerRepository;
        private IOrdersRepository _ordersRepository;

        public CustomerApplicationService(ICustomerRepository customerRepository, IOrdersRepository ordersRepository)
        {
            _customerRepository = customerRepository;
            _ordersRepository = ordersRepository;
        }

        public Customer CreateCustomer(string customer)
        {
            var customerObject = JsonConvert.DeserializeObject<Customer>(customer);

            // Validate the customer
            Validate(customerObject);
            
            // Set preferred address
            if (customerObject.CorrespondenceAddress != null)
            {
                customerObject.CorrespondenceAddress.IsPreferredAddress = true;
            }
            else
            {
                customerObject.HomeAddress.IsPreferredAddress = true;
            }

            // Set preferred customer to false
            customerObject.PreferredCustomer = false;

            // Save customer
            var customerId = _customerRepository.SaveCustomer(customerObject);
            customerObject.Id = customerId;

            // Save addresses
            customerObject.HomeAddress.Id = _customerRepository.SaveAddress(customerObject.HomeAddress, customerId);
            if (customerObject.CorrespondenceAddress != null)
            {
                customerObject.CorrespondenceAddress.Id = _customerRepository.SaveAddress(customerObject.CorrespondenceAddress, customerId);
            }

            return customerObject;
        }

        public Customer UpdateCustomer(string customer)
        {
            var customerObject = JsonConvert.DeserializeObject<Customer>(customer);

            // Validate the customer
            Validate(customerObject);

            // Set preferred address
            if (customerObject.CorrespondenceAddress != null)
            {
                customerObject.CorrespondenceAddress.IsPreferredAddress = true;
            }
            else
            {
                customerObject.HomeAddress.IsPreferredAddress = true;
            }

            // Set preferred customer based on orders
            var orders = _ordersRepository.GetCustomerOrders(customerObject.Id);
            var orderTotal = orders.Sum(x => x.Value);
            var originalPreferredCustomerFlag = customerObject.PreferredCustomer;
            customerObject.PreferredCustomer = orderTotal > 10000;
            if (originalPreferredCustomerFlag != customerObject.PreferredCustomer)
            {
                // Let the customer know their preferred status has changed
                SendMessage(customerObject.EmailAddress, customerObject.PreferredCustomer);
            }

            // Save customer
            _customerRepository.SaveCustomer(customerObject);

            // Save addresses
            customerObject.HomeAddress.Id = _customerRepository.SaveAddress(customerObject.HomeAddress, customerObject.Id);
            if (customerObject.CorrespondenceAddress != null)
            {
                customerObject.CorrespondenceAddress.Id = _customerRepository.SaveAddress(customerObject.CorrespondenceAddress, customerObject.Id);
            }

            return customerObject;
        }

        public Customer DeleteCustomer(long customerId)
        {
            var customer = _customerRepository.GetCustomer(customerId);
            var orders = _ordersRepository.GetCustomerOrders(customerId);
            if (orders.Any())
            {
                return customer;
            }
            _customerRepository.DeleteCustomer(customerId);
            return customer;
        }

        public Customer GetCustomer(long customerId)
        {
            var customer = _customerRepository.GetCustomer(customerId);
            return customer;
        }

        private void Validate(Customer customer)
        {
            if (string.IsNullOrEmpty(customer.FirstName))
            {
                throw new ValidationException("Customer first name must be entered.");
            }

            if (string.IsNullOrEmpty(customer.LastName))
            {
                throw new ValidationException("Customer last name must be entered.");
            }

            Validate(customer.HomeAddress);
            if (customer.CorrespondenceAddress != null)
            {
                Validate(customer.CorrespondenceAddress);
            }

            if (string.IsNullOrEmpty(customer.HomeTelephoneNumber) && string.IsNullOrEmpty(customer.MobileTelephoneNumber))
            {
                throw new ValidationException("A contact number must be provided.");
            }

            var contactNumberRegex = new Regex("^(?:(?:\\(?(?:0(?:0|11)\\)?[\\s-]?\\(?|\\+)44\\)?[\\s-]?(?:\\(?0\\)?[\\s-]?)?)|(?:\\(?0))(?:(?:\\d{5}\\)?[\\s-]?\\d{4,5})|(?:\\d{4}\\)?[\\s-]?(?:\\d{5}|\\d{3}[\\s-]?\\d{3}))|(?:\\d{3}\\)?[\\s-]?\\d{3}[\\s-]?\\d{3,4})|(?:\\d{2}\\)?[\\s-]?\\d{4}[\\s-]?\\d{4}))(?:[\\s-]?(?:x|ext\\.?|\\#)\\d{3,4})?$");
            if (!string.IsNullOrEmpty(customer.HomeTelephoneNumber) && !contactNumberRegex.IsMatch(customer.HomeTelephoneNumber))
            {
                throw new ValidationException("Home telephone number invalid.");
            }

            if (!string.IsNullOrEmpty(customer.MobileTelephoneNumber) && !contactNumberRegex.IsMatch(customer.MobileTelephoneNumber))
            {
                throw new ValidationException("Mobile telephone number invalid.");
            }

            var emailRegex = new Regex("^\\w+@[a-zA-Z_]+?\\.[a-zA-Z]{2,3}$");
            if (string.IsNullOrEmpty(customer.EmailAddress) || !emailRegex.IsMatch(customer.EmailAddress))
            {
                throw new ValidationException("Email address invalid.");
            }
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

        private void SendMessage(string emailAddress, bool isPreferredCustomer)
        {
            var title = "Preferred Customer Status";
            string body;
            if (isPreferredCustomer)
            {
                body = "You are now a preferred customer";
            }
            else
            {
                body = "You are no longer a preferred customer";
            }
            
            MailHelper.Send(emailAddress, title, body);
        }
    }
}
