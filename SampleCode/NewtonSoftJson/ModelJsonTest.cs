using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyModel;
using MyModel.MyNewtonJson;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

namespace SampleCode.NewtonSoftJson
{
    [TestClass]
    public class ModelJsonTest
    {
        public TestContext TestContext { get; set; }
        public Person FirstPerson { get; set; }
        public string FirstPersonJson { get; set; }
        public Person SecondPerson { get; set; }
        public string SecondPersonJson { get; set; }
        public List<Person> People { get; set; }
        public string PeopleJson { get; set; }

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

            FirstPersonJson = JsonConvert.SerializeObject(FirstPerson);
            SecondPersonJson = JsonConvert.SerializeObject(SecondPerson);

            People = new List<Person>()
            {
                FirstPerson,
                SecondPerson
            };
            PeopleJson = JsonConvert.SerializeObject(People);
        }
        #endregion

        #region Simple Using
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
            File.WriteAllText(personJsonFile, FirstPersonJson);

            // 
            // deserialize to person
            //
            var readPerson = JsonConvert.DeserializeObject<Person>(File.ReadAllText(personJsonFile));

            Assert.IsTrue(FirstPerson.Name == readPerson.Name && FirstPerson.Age == readPerson.Age);
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
            var personDict = JsonConvert.DeserializeObject<Dictionary<object, object>>(FirstPersonJson);


            foreach (var person in personDict)
            {
                TestContext.WriteLine($"{person.Key}: {person.Value}");
            }
        }

        [TestMethod]
        public void ToJToken()
        {
            //
            // convert jToken
            //
            var jToken = JToken.Parse(FirstPersonJson);

            //
            // children to properties
            //
            foreach (var jProperty in jToken.Children<JProperty>())
            {
                TestContext.WriteLine($"{jProperty.Name}: {jProperty.Value}");
            }
        }

        [TestMethod]
        public void ToJArray()
        {
            //
            // convert jArray
            //
            var jArray = JArray.Parse(PeopleJson);

            //
            // IEnumerable<JToken>
            //
            foreach (var jToken in jArray)
            {
                //
                // children to properties
                //
                foreach (var jProperty in jToken.Children<JProperty>())
                {
                    TestContext.WriteLine($"{jProperty.Name}: {jProperty.Value}");
                }
            }
        }
        #endregion

        #region Settings Using

        [TestMethod]
        public void DefaultIgnore()
        {
            //
            // default value ignore settings
            //
            JsonSerializerSettings serializeSettings = new () 
            { 
                DefaultValueHandling = DefaultValueHandling.Ignore 
            };

            //
            // set default
            //
            FirstPerson.Name = default;

            //
            // no ignore
            //
            TestContext.WriteLine(JsonConvert.SerializeObject(FirstPerson));

            //
            // ignore
            //
            TestContext.WriteLine(JsonConvert.SerializeObject(FirstPerson, serializeSettings));

        }

        [TestMethod]
        public void NullIgnore()
        {
            //
            // default value ignore settings
            //
            JsonSerializerSettings serializeSettings = new()
            {
                NullValueHandling = NullValueHandling.Ignore,
            };

            //
            // set default
            // string default is null
            //
            FirstPerson.Name = default;
            FirstPerson.Age = default;

            //
            // no ignore
            //
            TestContext.WriteLine(JsonConvert.SerializeObject(FirstPerson));

            //
            // ignore
            //
            TestContext.WriteLine(JsonConvert.SerializeObject(FirstPerson, serializeSettings));
        }

        [TestMethod]
        public void StringIndent()
        {
            //
            // indent
            //
            TestContext.WriteLine(JsonConvert.SerializeObject(FirstPerson, Newtonsoft.Json.Formatting.Indented));
            //
            // not indent
            //
            TestContext.WriteLine(JsonConvert.SerializeObject(FirstPerson));
        }

        [TestMethod]
        public void ObjectName()
        {
            //
            // default value ignore settings
            //
            JsonSerializerSettings serializeSettings = new()
            {
                TypeNameHandling = TypeNameHandling.Objects
            };

            //
            // object name ignore
            //
            TestContext.WriteLine(JsonConvert.SerializeObject(FirstPerson));

            //
            // object name exclude
            //
            TestContext.WriteLine(JsonConvert.SerializeObject(FirstPerson, serializeSettings));
        }

        #endregion


        #region Converter

        [TestMethod]
        public void PersonReadConverterTest()
        {
            //
            // test 1.
            // 1. first is father (age 29)
            // 2. second is mother (age 32)
            // 3. add 2 son, 2 dauther
            //
            FirstPerson.Gender = true;
            SecondPerson.Gender = false;

            FirstPerson.Family = new List<Person>();
            FirstPerson.Family.Add(SecondPerson);
            // dauther
            FirstPerson.Family.Add(new() { Age = 1, Gender = false, Name = "second dauther"});
            FirstPerson.Family.Add(new() { Age = 4, Gender = false, Name = "first dauther"});
            // son
            FirstPerson.Family.Add(new() { Age = 7, Gender = true, Name = "second son"});
            FirstPerson.Family.Add(new() { Age = 10, Gender = true, Name = "first son"});

            var person = JsonConvert.DeserializeObject<Person>(JsonConvert.SerializeObject(FirstPerson), new PersonReadConverter());
            var people = new List<Person>(person.Family);
            people.Insert(0, person);

            TestContext.WriteLine(string.Join("\n", people.Select(person => $"[{person.Name}_{person.Immediate}]")));


            TestContext.WriteLine("=================================");
            //
            // test 2.
            // 1. first is son (age 29)
            // 2. second is mother (age 32)
            // 3. add 1 father, 1 dauther
            //
            FirstPerson.Gender = true;
            SecondPerson.Gender = false;

            FirstPerson.Family = new List<Person>();
            FirstPerson.Family.Add(SecondPerson);

            // father
            FirstPerson.Family.Add(new() { Age = 55, Gender = true, Name = "first father" });
            // son
            FirstPerson.Family.Add(new() { Age = 10, Gender = false, Name = "first son" });

            person = JsonConvert.DeserializeObject<Person>(JsonConvert.SerializeObject(FirstPerson), new PersonReadConverter());
            people = new List<Person>(person.Family);
            people.Insert(0, person);

            TestContext.WriteLine(string.Join("\n", people.Select(person => $"[{person.Name}_{person.Immediate}]")));

        }

        #endregion
    }
}
