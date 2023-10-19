using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Commons.Exceptions
{
    public class BaseBussinessLogicException : Exception
    {
        public List<string> ErrorMessages { get; set; }

        public int StatusCode { get; set; }
        public BaseBussinessLogicException(int statusCode,params string[] messages)
        {
            ErrorMessages = messages.ToList();
            StatusCode = statusCode;
        }
    }
}
