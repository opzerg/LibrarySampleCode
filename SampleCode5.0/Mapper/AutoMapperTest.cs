using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleCode5._0
{
    [TestClass]
    public class AutoMapperTest
    {
        public TestContext TestContext { get; set; }

        Person Person { get; set; }
        OtherPerson OtherPerson { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            Person = new Person()
            {
                Age = 29,
                Gender = null,
                Name = "최용국"
            };

            OtherPerson = new OtherPerson()
            {
                Email = "opzerg9378@gmail.com"
            };
        }

        [TestMethod]
        public void PersonNormalMapperTest()
        {
            Mapper mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Person, OtherPerson>();
            }));

            TestContext.WriteLine($"{mapper.Map(Person, OtherPerson)}");
        }

        [TestMethod]
        public void PersonForMemberMapperTest()
        {
            Mapper mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Person, OtherPerson>().ForMember(op => op.Sex, mo => mo.MapFrom(p => p.Gender ?? false));
            }));


            TestContext.WriteLine($"{Person}");
            TestContext.WriteLine($"{mapper.Map(Person, OtherPerson)}");

            TestContext.WriteLine("-");
            Person.Gender = true;

            TestContext.WriteLine($"{Person}");
            TestContext.WriteLine($"{mapper.Map(Person, OtherPerson)}");
        }
    }
}
