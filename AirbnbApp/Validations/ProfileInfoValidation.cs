using AirbnbApp.ViewModels;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirbnbApp.Validations
{
    class ProfileInfoValidation : AbstractValidator<ProfileInfoVM>
    {
        public ProfileInfoValidation()
        {
            RuleFor(x => x.Firstname).MinimumLength(3);
            RuleFor(x => x.Lastname).MinimumLength(3);
        }
    }
}
