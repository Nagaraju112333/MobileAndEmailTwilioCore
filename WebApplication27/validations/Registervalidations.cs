using WebApplication27.Twilio;
using FluentValidation;
namespace WebApplication27.validations
{
    public class Registervalidations: AbstractValidator<UserRegister>
    {
        public Registervalidations()
        {
            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.LastName).NotEmpty();
            RuleFor(x => x.Email).NotEmpty();
           
            RuleFor(x => x.Password).Length(5, 10);
            RuleFor(x => x.PhoneNumber).NotEmpty();
            RuleFor(x => x.Password).Length(1, 10);
        }
    }
}
