using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyModel;

//
// Person partial 때문에 namespace를 맞춰줘야 한다.
//
namespace MyModel
{
    public partial class Person : IEquatable<Person>
    {
        public static bool operator ==(Person left, Person right)
        {
            //
            // 연산자에서는 왼쪽이 null이 올 수 있기 때문에 null만 검사하고 Equals로 같음을 검사
            //
            if(left is null)
            {
                if (right is null)
                    return true;
                return false;
            }

            return left.Equals(right);
        }
        //
        // left != rigth x !(left == right) o => 순환참조
        //
        public static bool operator !=(Person left, Person right) => !(left == right);

        public override bool Equals(object obj) => this.Equals(obj as Person);

        //
        // 들어오는 매개변수 null 검사, Reference 검사 후 사용자 정의 같음을 검사
        //
        public bool Equals(Person other)
        {
            if (other is null)
                return false;

            if (ReferenceEquals(this, other))
                return true;


            return this.Name == other.Name && this.Age == other.Age;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Age);
        }
    }
}
