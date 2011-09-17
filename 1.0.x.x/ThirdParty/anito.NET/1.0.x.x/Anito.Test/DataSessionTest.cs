using Anito.Data.Schema;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using System.Linq.Expressions;
using Anito.Test.Mocks;
using Anito.Test.Entities;
using DataSession = Anito.Data.DataSession;
using TestSession = Anito.Test.Mocks.DataSession;

namespace Anito.Test
{

    [TestClass]
    public class DataSessionTest
    {

        [TestMethod]
        public void GetTByExpression()
        {
            
            Expression<Func<Customer, bool>> expression = e => e.ID == 1;

            var customer = new Customer { 
                ID = 1, Balance = 0, 
                BalanceRate = 0, DefaultContactID = 1, 
                Name = "Customer1", ProfileID = 1};

            var reader = new DbDataReader();
            reader.Setup(r => r.Read()).Returns(true);

            var provider = new IProvider();

            var command = new ICommand();

            var schema = new TypeTable(typeof(Customer));

            provider.Setup(p => p.GetSchema(typeof(Customer))).Returns(schema);

            provider.CommandBuilder.Setup(c => c.CreateGetTCommand<Customer>(expression)).Returns(command.Object);
            provider.CommandExecutor.Setup(c => c.ExecuteReader(command.Object)).Returns(reader.Object);

            provider.Mapper.Setup(m => m.GetTMappingMethod<Customer>()).Returns(d => customer);

            var target = new DataSession(provider.Object);
            
            var actual = target.GetT(expression);
 
            Assert.AreEqual(actual, customer);
        }

        [TestMethod]
        public void GetTByKey()
        {

            var customer = new Customer
            {
                ID = 1,
                Balance = 0,
                BalanceRate = 0,
                DefaultContactID = 1,
                Name = "Customer1",
                ProfileID = 1
            };

            var reader = new DbDataReader();
            reader.Setup(r => r.Read()).Returns(true);

            var provider = new IProvider();

            var command = new ICommand();

            var schema = new TypeTable(typeof(Customer));

            provider.Setup(p => p.GetSchema(typeof(Customer))).Returns(schema);

            provider.CommandBuilder.Setup(c => c.CreateGetObjectByKeyCommand<Customer>()).Returns(command.Object);
            provider.CommandExecutor.Setup(c => c.ExecuteReader(command.Object)).Returns(reader.Object);

            provider.Mapper.Setup(m => m.GetTMappingMethod<Customer>()).Returns(d => customer);

            var target = new DataSession(provider.Object);

            var actual = target.GetT<Customer>("CUS0000001");

            Assert.AreEqual(actual, customer);
        }


        [TestMethod]
        public void GetListByExpression()
        {
            Expression<Func<Customer, bool>> expression = e => e.ID < 4;

            var customer1 = new Customer
            {
                ID = 1,
                Balance = 0,
                BalanceRate = 0,
                DefaultContactID = 1,
                Name = "Customer1",
                ProfileID = 1
            };
            var customer2 = new Customer
            {
                ID = 2,
                Balance = 0,
                BalanceRate = 0,
                DefaultContactID = 2,
                Name = "Customer2",
                ProfileID = 2
            };
            var customer3 = new Customer
            {
                ID = 3,
                Balance = 0,
                BalanceRate = 0,
                DefaultContactID = 3,
                Name = "Customer3",
                ProfileID = 3
            };

            var customerQueue = new Queue<Customer>(new[] {customer1, customer2, customer3});
            var readerResults = new Queue<bool>(new []{true, true, true, false});

            var reader = new DbDataReader();
            reader.Setup(r => r.Read()).Returns(readerResults.Dequeue);

            var provider = new IProvider();

            var command = new ICommand();

            var schema = new TypeTable(typeof(Customer));

            provider.Setup(p => p.GetSchema(typeof(Customer))).Returns(schema);

            provider.CommandBuilder.Setup(c => c.CreateGetListCommand<Customer>(expression)).Returns(command.Object);
            provider.CommandExecutor.Setup(c => c.ExecuteReader(command.Object)).Returns(reader.Object);

            provider.Mapper.Setup(m => m.GetTMappingMethod<Customer>()).Returns(d => customerQueue.Dequeue());

            var target = new DataSession(provider.Object);

            var actual = target.GetList<List<Customer>, Customer>(expression);

            Assert.IsTrue(actual[0].ID == 1 &&
                actual[1].ID == 2 && actual[2].ID == 3);
        }
    }
}
