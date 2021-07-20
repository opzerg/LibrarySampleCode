using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyModel
{
    [AttributeUsage(AttributeTargets.Field)]
    public class StatusAttribute : Attribute
    {
        public string Message { get; }

        public StatusAttribute(string message)
        {
            this.Message = message;
        }
    }
}
