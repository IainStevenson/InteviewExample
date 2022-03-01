using System.Collections.Generic;
using InterviewExample.ApplicationLayer;
using InterviewExample.ApplicationLayer.Interfaces;
using InterviewExample.Domain;
using Moq;
using NUnit.Framework;

namespace InterviewExample.Tests.ApplicationLayer
{
    [TestFixture]
    public class DeleteCustomerTests
    {
        [Test]
        public void TheCustomerIsLoadedFromTheDatabase()
        {
            var customerRepository = new Mock<ICustomerRepository>();
            var ordersRepository = new Mock<IOrdersRepository>();
            ordersRepository.Setup(x => x.GetCustomerOrders(It.IsAny<long>())).Returns(new List<Order>());
            var appService = new CustomerApplicationService(customerRepository.Object, ordersRepository.Object);

            appService.DeleteCustomer(1234);

            customerRepository.Verify(x => x.GetCustomer(1234));
        }

        [Test]
        public void TheCustomerIsDeletedFromTheDatabase()
        {
            var customerRepository = new Mock<ICustomerRepository>();
            var ordersRepository = new Mock<IOrdersRepository>();
            ordersRepository.Setup(x => x.GetCustomerOrders(It.IsAny<long>())).Returns(new List<Order>());
            var appService = new CustomerApplicationService(customerRepository.Object, ordersRepository.Object);

            appService.DeleteCustomer(1234);

            customerRepository.Verify(x => x.DeleteCustomer(1234));
        }

        [Test]
        public void IfTheTheCustomerHasOrdersThenTheyAreNotDeleted()
        {
            var customerRepository = new Mock<ICustomerRepository>();
            var ordersRepository = new Mock<IOrdersRepository>();
            ordersRepository.Setup(x => x.GetCustomerOrders(It.IsAny<long>())).Returns(new List<Order>());
            var appService = new CustomerApplicationService(customerRepository.Object, ordersRepository.Object);

            ordersRepository.Setup(x => x.GetCustomerOrders(1234)).Returns(new List<Order> { new Order() });

            appService.DeleteCustomer(1234);

            customerRepository.Verify(x => x.DeleteCustomer(1234), Times.Never);
        }
    }
}
