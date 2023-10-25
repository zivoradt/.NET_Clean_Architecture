using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exceptions
{
    public class NotFoundException : ApplicationException
    {
        public NotFoundException(string message, object key) : base($"{message} ({key}) is not found")
        {
        }
    }
}