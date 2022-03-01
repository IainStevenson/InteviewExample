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
    public class CreateCustomerTests
    {
        [Test]
        public void ThenTheRepositoryIsCalledForTheCustomer()
        {
            var customerRepository = new Mock<ICustomerRepository>();
            var ordersRepository = new Mock<IOrdersRepository>();
            var appService = new CustomerApplicationService(customerRepository.Object, ordersRepository.Object);

            var customerJson = "{ firstName: 'Joe', lastName: 'Bloggs', homeAddress: { addressLine1: 'Line 1', postcode: 'postcode' }, homeTelephoneNumber: '0123456789', emailAddress: 'test@email.com' }";

            appService.CreateCustomer(customerJson);

            customerRepository.Verify(x => x.SaveCustomer(It.IsAny<Customer>()));
        }

        [Test]
        public void ThenTheRepositoryIsCalledForTheAddresses()
        {
            var customerRepository = new Mock<ICustomerRepository>();
            var ordersRepository = new Mock<IOrdersRepository>();
            var appService = new CustomerApplicationService(customerRepository.Object, ordersRepository.Object);

            var customerJson = "{ firstName: 'Joe', lastName: 'Bloggs', homeAddress: { addressLine1: 'Line 1', postcode: 'postcode' }, correspondenceAddress: { addressLine1: 'Line 1', postcode: 'postcode' }, homeTelephoneNumber: '0123456789', emailAddress: 'test@email.com' }";

            customerRepository.Setup(x => x.SaveCustomer(It.IsAny<Customer>())).Returns(1235);

            appService.CreateCustomer(customerJson);

            customerRepository.Verify(x => x.SaveAddress(It.IsAny<Address>(), 1235), Times.Exactly(2));
        }
    }
}
