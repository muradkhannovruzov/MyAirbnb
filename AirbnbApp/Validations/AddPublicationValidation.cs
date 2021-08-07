using AirbnbApp.ViewModels;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirbnbApp.Validations
{
    class AddPublicationValidation : AbstractValidator<AddPublicationVM>
    {
        public AddPublicationValidation()
        {
            RuleFor(x => x.SelectedCity).NotEmpty();
            RuleFor(x => x.SelectedCountry).NotEmpty();
            RuleFor(x => x.Address).MinimumLength(5);
            RuleFor(x => x.Number).MinimumLength(5);
            RuleFor(x => x.HomeTypeComboBox).NotEmpty();
            RuleFor(x => x.PlaceType).NotEmpty();
            RuleFor(x => x.Price).NotEmpty();
            RuleFor(x => x.Collection).NotEmpty();
        }
    }
}
