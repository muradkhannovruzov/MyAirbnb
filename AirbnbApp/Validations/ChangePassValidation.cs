using AirbnbApp.ViewModels;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirbnbApp.Validations
{
    class ChangePassValidation : AbstractValidator<ChangePassVM>
    {
        public ChangePassValidation()
        {
            RuleFor(x => x.NewPass).MinimumLength(3);
            RuleFor(x => x.ConfirmPass).MinimumLength(3);
        }
    }
}
