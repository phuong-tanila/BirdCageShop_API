using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Commons.Exceptions
{
    public class InvalidCageComponentException : BaseBussinessLogicException
    {
        public InvalidCageComponentException(params string[] messages)
            : base(400,messages)
        {
            
        }
    }
}
