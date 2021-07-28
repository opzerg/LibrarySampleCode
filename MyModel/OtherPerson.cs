using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyModel
{
    public class OtherPerson
    {
        public string Name { get; set; }
        public int Age { get; set; }

        public string Email { get; set; }

        public bool Sex { get; set; }
        public override string ToString()
        {
            return $"이름: {Name} 나이: {Age} 이메일: {Email} 성별: {Sex}";
        }
    }
}
