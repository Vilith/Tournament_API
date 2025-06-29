using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tournament.Tests.TestHelpers
{
    public class FakeObjectModelValidator : IObjectModelValidator
    {
        public void Validate(
            ActionContext actionContext,
            ValidationStateDictionary validationState,
            string prefix,
            object model)
        {
            // Do nothing, effectively bypassing validation
        }
    }
}
