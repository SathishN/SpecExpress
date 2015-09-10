using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpecExpress.Test.Entities
{
    public class Person
    {
        public string Name { get; set; }

        public Address2 MailingAddress { get; set; }

        public Address2 ShippingAddress { get; set; }
    }
    
    public class Address2
    {
        public const string MailingType = "MAIL";

        public const string ShippingType = "SHIP";

        public string Type { get; set; }
        public string AddressLine { get; set; }
        public string City { get; set; }
        public string State { get; set; }
    }

    public class PersonSpecification : Validates<Person>
    {
        public PersonSpecification()
        {
            Check(_ => _.Name).Required();

            Check(_ => _.MailingAddress).Optional()
                .Specification(v => v.Check(x => x.Type == Address2.MailingType))
                .And
                .Specification<Address2Specification>();

            Check(_ => _.ShippingAddress).Optional()
                .Specification(v => v.Check(x => x.Type == Address2.ShippingType))
                .And
                .Specification<Address2Specification>();
        }
    }

    public class Address2Specification : Validates<Address2>
    {
        public Address2Specification()
        {
            Check(_ => _.AddressLine).Required();
            Check(_ => _.City).Required();
            Check(_ => _.State).Required();
        }
    }
}
