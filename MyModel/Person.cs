using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MyModel
{
    public partial class Person
    {
        public enum ImmediateEnum
        { 
            Mother,
            Father,
            Brother,
            Sister,
            Twin,
            Son,
            Daughter
        }

        public string Name { get; set; }
        public int Age { get; set; }
        /// <summary>
        /// true: male, false female
        /// </summary>
        public bool? Gender { get; set; }
        public List<Person> Family { get; set; }

        //
        // PersonReadConverter에서 정의
        //
        [JsonIgnore]
        public ImmediateEnum Immediate { get; set; }

        public override string ToString()
        {
            return $"이름: {Name} 나이: {Age} 성별: {Gender}";
        }
    }
}
