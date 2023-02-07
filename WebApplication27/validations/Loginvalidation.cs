using FluentValidation;
using WebApplication27.Twilio;
using WebApplication27.Twilio.Login;

namespace WebApplication27.validations
{
    public class Loginvalidation:AbstractValidator<Login>
    {
        public Loginvalidation() 
        {

            RuleFor(x => x.Email).NotEmpty();
            RuleFor(x => x.Password).NotEmpty();
        }

    }
}
