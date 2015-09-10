using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SpecExpress.Test.Entities;

namespace SpecExpress.Test
{
    [TestFixture]
    public class PersonSpecificationTests
    {
        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            ValidationCatalog.Reset();
            ValidationCatalog.Scan(x => x.AddAssembly(typeof(Person).Assembly));
            ValidationCatalog.AssertConfigurationIsValid();
        }

        [TestFixtureTearDown]
        public void FixtureTeardown()
        {
            ValidationCatalog.Reset();
        }

        [Test]
        public void Valid_Person()
        {
            var person = new Person()
            {
                Name = "Someone",
                MailingAddress =
                    new Address2()
                    {
                        AddressLine = "Dallas Pkwy",
                        City = "Dallas",
                        State = "TX",
                        Type = Address2.MailingType
                    },
                ShippingAddress = 
                    new Address2()
                    {
                        AddressLine = "Dallas Pkwy",
                        City = "Dallas",
                        State = "TX",
                        Type = Address2.ShippingType
                    },
            };

            var results = ValidationCatalog.Validate(person);
            
            Assert.That(results.IsValid, Is.True);

            CollectionAssert.IsEmpty(results.All());
        }

        [Test]

        public void Invalid_Person()
        {
            var person = new Person()
            {
                Name = "Someone",
                MailingAddress =
                    new Address2()
                    {
                        AddressLine = "Dallas Pkwy",
                        City = "Dallas",
                        State = "TX",
                    },
                ShippingAddress =
                    new Address2()
                    {
                        //AddressLine = "Dallas Pkwy",
                        //City = "Dallas",
                        //State = "TX",
                        Type = Address2.ShippingType
                    },
            };

            var results = ValidationCatalog.Validate(person);
            
            foreach (var result in results.All())
            {
                Console.WriteLine(result.Message);
            }

            Assert.That(results.IsValid, Is.False);

            CollectionAssert.IsNotEmpty(results.All());

            
             
        }
    }
}
