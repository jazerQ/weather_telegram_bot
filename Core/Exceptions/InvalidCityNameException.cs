using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Exceptions
{
    public class InvalidCityNameException : Exception
    {
        public InvalidCityNameException(string message) : base(message)
        {
           
        }
    }
}
