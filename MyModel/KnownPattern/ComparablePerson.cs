using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyModel
{
    public partial class Person : IComparable<Person>
    {
        public int CompareTo(Person other)
        {
            if (this.Age > other.Age)
                return 1;
            else if (this.Age < other.Age)
                return -1;
            else
                return 0;
        }
    }
}
