using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleCode
{
    [TestClass]
    public class KnownPatternTest
    {
        public Person FirstPerson { get; set; }
        public Person SecondPerson { get; set; }
        public TestContext TestContext { get; set; }
        #region InitVariable
        [TestCleanup]
        public void Cleanup()
        {

        }

        [TestInitialize]
        public void Initialize()
        {
            FirstPerson = new()
            {
                Age = 29,
                Name = "kook, choi"
            };
            SecondPerson = new()
            {
                Age = 32,
                Name = "grace, lee"
            };
        }
        #endregion
        [TestMethod]
        public void Equatable()
        {
            //
            // new reference
            //
            Person thirdPerson = new()
            {
                Age = 29,
                Name = "kook, choi"
            };

            Assert.AreEqual(thirdPerson, FirstPerson);

            TestContext.WriteLine($"{ReferenceEquals(thirdPerson, FirstPerson)}");
        }

        [TestMethod]
        public void ComparableMAX()
        {
            //
            // new reference
            //
            Person thirdPerson = new()
            {
                Age = 2,
                Name = "jr, choi"
            };

            Person[] people = new[] { thirdPerson, SecondPerson, FirstPerson };

            //
            // MAX Linq가 Comparable Interface를 사용
            //
            Assert.AreEqual(people.Max(person => person), SecondPerson);
        }
    }
}
