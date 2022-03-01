using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterviewExample.ApplicationLayer;
using InterviewExample.ApplicationLayer.Interfaces;
using InterviewExample.Domain;
using Moq;
using NUnit.Framework;

namespace InterviewExample.Tests.ApplicationLayer
{
    [TestFixture]
    public class UpdateCustomerTests
    {
        [Test]
        public void ThenTheRepositoryIsCalledForTheCustomer()
        {
            var customerRepository = new Mock<ICustomerRepository>();
            var ordersRepository = new Mock<IOrdersRepository>();
            var appService = new CustomerApplicationService(customerRepository.Object, ordersRepository.Object);

            var customerJson = "{ id: 1234, firstName: 'Joe', lastName: 'Bloggs', homeAddress: { addressLine1: 'Line 1', postcode: 'postcode' }, homeTelephoneNumber: '0123456789', emailAddress: 'test@email.com', preferredCustomer: false }";

            ordersRepository.Setup(x => x.GetCustomerOrders(1234)).Returns(new List<Order>() { new Order { Value = 15000 } });

            appService.UpdateCustomer(customerJson);

            customerRepository.Verify(x => x.SaveCustomer(It.IsAny<Customer>()));
        }
    }
}
