using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyModel.MyNewtonJson
{
    public class PersonReadConverter : JsonConverter
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
            var oldestMale = genderGroups.Where(genderGroup => genderGroup.Key.Value)
                .SelectMany(genderGroup => genderGroup)
                .Max(genderGroup => genderGroup);

            var oldestFemale = genderGroups.Where(genderGroup => !genderGroup.Key.Value)
                .SelectMany(genderGroup => genderGroup)
                .Max(genderGroup => genderGroup);

            // 3. person과 비교해서 구성원 Immediate 결정
            // 3-1. person이 남자면
            if (person.Gender.Value)
            {
                // 아무도 없으면 아빠
                if(oldestMale is null)
                {
                    person.Immediate = Person.ImmediateEnum.Father;
                }
                // 3-1-1. person의 나이가 가장 많으면 person은 아빠
                else if (person.Age > oldestMale.Age)
                {
                    person.Immediate = Person.ImmediateEnum.Father;
                    // 가장 나이가 많은 Female은 엄마
                    oldestFemale.Immediate = Person.ImmediateEnum.Mother;
                    // 전부 아들
                    SetImmediate(genderGroups, genderGroup => genderGroup.Key.Value, Person.ImmediateEnum.Son);
                    // 전부 딸
                    // 엄마 제외
                    SetImmediate(genderGroups, genderGroup => !genderGroup.Key.Value, Person.ImmediateEnum.Daughter, p => p == oldestFemale);
                }
                else if (person.Age == oldestMale.Age) //3-1-2. 나이가 같으면 쌍둥이
                {
                    person.Immediate = Person.ImmediateEnum.Father;
                    // 쌍둥이
                    oldestMale.Immediate = Person.ImmediateEnum.Twin;
                    // 엄마
                    oldestFemale.Immediate = Person.ImmediateEnum.Mother;

                    // 전부 아들
                    // 쌍둥이 제외
                    SetImmediate(genderGroups, genderGroup => genderGroup.Key.Value, Person.ImmediateEnum.Son, p => p == oldestMale);
                    // 전부 딸
                    // 엄마 제외
                    SetImmediate(genderGroups, genderGroup => !genderGroup.Key.Value, Person.ImmediateEnum.Daughter, p => p == oldestFemale);
                }
                else
                {
                    // 아빠
                    oldestMale.Immediate = Person.ImmediateEnum.Father;
                    // 엄마
                    oldestFemale.Immediate = Person.ImmediateEnum.Mother;
                    // 아들
                    person.Immediate = Person.ImmediateEnum.Son;
                    // 쌍둥이 혹은 형, 동생
                    TwinOr(person, genderGroups, oldestMale, Person.ImmediateEnum.Brother);
                    // 쌍둥이 혹은 누나, 동생
                    TwinOr(person, genderGroups, oldestFemale, Person.ImmediateEnum.Sister);
                }
            }
            else  // 3-2. person이 여자면
            {
                // 아무도 없으면 엄마
                if(oldestFemale is null)
                {
                    person.Immediate = Person.ImmediateEnum.Mother;
                }
                else if (person.Age > oldestFemale.Age)
                {
                    person.Immediate = Person.ImmediateEnum.Mother;
                    // 가장 나이가 많은 Male은 아빠
                    oldestMale.Immediate = Person.ImmediateEnum.Father;
                    // 전부 아들
                    // 아빠 제외
                    SetImmediate(genderGroups, genderGroup => genderGroup.Key.Value, Person.ImmediateEnum.Son, p => p == oldestMale);
                    // 전부 딸
                    SetImmediate(genderGroups, genderGroup => !genderGroup.Key.Value, Person.ImmediateEnum.Daughter);
                }
                else if (person.Age == oldestFemale.Age) //3-1-2. 나이가 같으면 쌍둥이
                {
                    person.Immediate = Person.ImmediateEnum.Mother;
                    // 아빠
                    oldestMale.Immediate = Person.ImmediateEnum.Father;
                    // 쌍둥이
                    oldestFemale.Immediate = Person.ImmediateEnum.Twin;

                    // 전부 아들
                    // 아빠 제외
                    SetImmediate(genderGroups, genderGroup => genderGroup.Key.Value, Person.ImmediateEnum.Son, p => p == oldestMale);
                    // 전부 딸
                    // 쌍둥이 제외
                    SetImmediate(genderGroups, genderGroup => !genderGroup.Key.Value, Person.ImmediateEnum.Daughter, p => p == oldestFemale);
                }
                else
                {
                    // 아빠
                    oldestMale.Immediate = Person.ImmediateEnum.Father;
                    // 엄마
                    oldestFemale.Immediate = Person.ImmediateEnum.Mother;
                    // 딸
                    person.Immediate = Person.ImmediateEnum.Daughter;
                    // 쌍둥이 혹은 형, 동생
                    TwinOr(person, genderGroups, oldestMale, Person.ImmediateEnum.Brother);
                    // 쌍둥이 혹은 누나, 동생
                    TwinOr(person, genderGroups, oldestFemale, Person.ImmediateEnum.Sister);
                }
            }

            return person;
        }

        private static void TwinOr(Person person, IEnumerable<IGrouping<bool?, Person>> genderGroups, Person oldest, Person.ImmediateEnum immediate)
        {
            foreach (var femalePerson in genderGroups.Where(genderGroup => !genderGroup.Key.Value).SelectMany(genderGroup => genderGroup))
            {
                if (femalePerson == oldest)
                    continue;

                var result = person.CompareTo(femalePerson);

                if (result == 0)
                    femalePerson.Immediate = Person.ImmediateEnum.Twin;
                else
                    femalePerson.Immediate = immediate;
            }
        }

        private static void SetImmediate(IEnumerable<IGrouping<bool?, Person>> genderGroups,
            Func<IGrouping<bool?, Person>, bool> gender,
            Person.ImmediateEnum immediate,
            Func<Person, bool> equals = null)
        {
            foreach (var person in genderGroups.Where(genderGroup => gender.Invoke(genderGroup))
                                    .SelectMany(genderGroup => genderGroup))
            {
                if (equals != null && equals.Invoke(person))
                    continue;

                person.Immediate = immediate;
            }
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
