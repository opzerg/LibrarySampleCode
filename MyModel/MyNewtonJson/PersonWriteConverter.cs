using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyModel.MyNewtonJson
{
    public class PersonWriteConverter : JsonConverter
    {
        public override bool CanRead => false;

        public override bool CanConvert(Type objectType) => objectType == typeof(Person);

        
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            // serialize

            // person.gedner는 nullable boolean이지만
            // json형태로 변환할 때 null이면 제외한다.

            if(value is Person person)
            {
                if (person.Gender is null)
                    return;
            }

            serializer.Serialize(writer, value);
        }
    }
}
