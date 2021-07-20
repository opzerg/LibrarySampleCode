using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyModel
{
    [Flags]
    public enum MyStatus
    {
        [Status("이미지 상태")]
        Image = 1,
        [Status("텍스트 상태")]
        Text = 2,
        [Status("암호화 상태")]
        Encrypt = 1 << 10,
    }

}
