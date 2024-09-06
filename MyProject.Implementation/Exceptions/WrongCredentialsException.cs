using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Implementation.Exceptions
{
    public class WrongCredentialsException : Exception
    {
        public WrongCredentialsException()
            : base("Wrong credentials")
        {
        }
    }
}
