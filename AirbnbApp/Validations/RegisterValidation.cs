using AirbnbApp.ViewModels;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirbnbApp.Validations
{
    class RegisterValidation : AbstractValidator<RegisterVM>
    {
        public RegisterValidation()
        {
            RuleFor(x => x.FirstName).MinimumLength(3);
            RuleFor(x => x.LastName).MinimumLength(3);
            RuleFor(x => x.Email).MinimumLength(5).EmailAddress();
            RuleFor(x => x.Password).MinimumLength(3);
        }
    }

    
}
