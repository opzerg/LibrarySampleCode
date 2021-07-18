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
        Image = 1,
        Text = 2,
        Encrypt = 1 << 10,
    }

}
