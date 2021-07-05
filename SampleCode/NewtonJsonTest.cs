using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace SampleCode
{
    [TestClass]
    public class NewtonJsonTest
    {
        public TestContext TestContext { get; set; }
        public Person Person { get; set; }
        public string PersonJson { get; set; }
        [TestCleanup]
        public void Cleanup()
        {

        }

        [TestInitialize]
        public void Initialize()
        {
            Person = new ()
            {
                Age = 28,
                Name = "kook, choi"
            };
            PersonJson = JsonConvert.SerializeObject(Person);
        }

        [TestMethod]
        public void SerializeDeserializeNormal()
        {
            //
            // save file path
            //
            string personJsonFile = Path.Combine(TestContext.TestRunResultsDirectory, $"person.txt");

            //
            // serialize
            //
            File.WriteAllText(personJsonFile, PersonJson);

            // 
            // deserialize to person
            //
            var readPerson = JsonConvert.DeserializeObject<Person>(File.ReadAllText(personJsonFile));

            Assert.IsTrue(Person.Name == readPerson.Name && Person.Age == readPerson.Age);
            //
            // not opening...
            //
            TestContext.AddResultFile(personJsonFile);
        }

        [TestMethod]
        public void ToDictionary()
        {
            //
            // deserialize to dict
            //
            var personDict = JsonConvert.DeserializeObject<Dictionary<object, object>>(PersonJson);


            foreach (var person in personDict)
            {
                TestContext.WriteLine($"{person.Key}: {person.Value}");
            }
        }

        [TestMethod]
        public void ToJToken()
        {
            //
            // convert jtoken
            //
            var personJtoken = JToken.Parse(PersonJson);

            //
            // children to properties
            //
            foreach (var property in personJtoken.Children<JProperty>())
            {
                TestContext.WriteLine($"{property.Name}: {property.Value}");
            }
            
            
        }
    }
}
