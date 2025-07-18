using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Exceptions
{
    public class BusinessRuleViolationException : Exception
    {

        public BusinessRuleViolationException(string message) : base(message)
        {
        }
        public BusinessRuleViolationException(string message, Exception innerException) : base(message, innerException)
        {
        }
        public BusinessRuleViolationException(string message, string ruleName) : base($"{message} (Rule: {ruleName})")
        {
        }       

    }
}
