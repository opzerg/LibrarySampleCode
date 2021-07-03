using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyModel;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace SampleCode
{
    [TestClass]
    public class NewtonJsonTest
    {
        public TestContext TestContext { get; set; }


        [TestMethod]
        public void SerializeDeserialize_Normal()
        {
            Person writePerson = new()
            {
                Age = 28,
                Name = "kook, choi"
            };

            var serializePerson = JsonConvert.SerializeObject(writePerson);

            string personJsonFile = Path.Combine(TestContext.TestRunResultsDirectory, $"person.txt");
            File.WriteAllText(personJsonFile, serializePerson);

            var readPerson = JsonConvert.DeserializeObject<Person>(File.ReadAllText(personJsonFile));

            Assert.IsTrue(writePerson.Name == readPerson.Name && writePerson.Age == readPerson.Age);
            //
            // not opening...
            //
            TestContext.AddResultFile(personJsonFile);
            

        }
    }
}
