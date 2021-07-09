using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyModel.MyNewtonJson
{
    internal class PersonReadConverter : JsonConverter
    {
        //
        // serialize
        //
        public override bool CanRead => base.CanRead;

        //
        // deserialize
        //
        public override bool CanWrite => false;

        public override bool CanConvert(Type objectType) => typeof(Person) == objectType;

        //
        // serialize
        //
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var personToken = JToken.ReadFrom(reader);
            var person = personToken.ToObject<Person>();

            //
            // 1. 성별로 group
            var genderGroups = person.Family.GroupBy(family => family.Gender);
            // 2. 나이순으로 정렬
            var oldestMale = genderGroups.Where(genderGroup => genderGroup.Key)
                .SelectMany(genderGroup => genderGroup)
                .Max(genderGroup => genderGroup.Age);

            var oldestFemale = genderGroups.Where(genderGroup => !genderGroup.Key)
                .SelectMany(genderGroup => genderGroup)
                .Max(genderGroup => genderGroup);

            if(person.Age > oldestMale)
            {

            }
            // 3. person과 비교해서 구성원 Immediate 결정
            //
        }

        
        //
        // deserialize
        //
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
