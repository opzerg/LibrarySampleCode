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
        [TestMethod]
        public void PersonMapperTest()
        {
            Person person = new Person()
            {
                Age = 29,
                Gender = true,
                Name = "최용국"
            };

            OtherPerson otherPerson = new OtherPerson()
            {
                Email = "opzerg9378@gmail.com"
            };

            Mapper mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Person, OtherPerson>();
            }));

            TestContext.WriteLine($"{mapper.Map(person, otherPerson)}");
        }
    }
}
