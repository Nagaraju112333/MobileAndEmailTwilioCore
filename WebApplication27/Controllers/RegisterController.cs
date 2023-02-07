using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Twilio.Types;
using Twilio;
using WebApplication27.Twilio;
using Twilio.Rest.Api.V2010.Account;
using WebApplication27.Twilio.Login;
using Microsoft.Identity.Client;
using WebApplication27.Twilio.Forgot;
using Microsoft.AspNetCore.Identity;
using Twilio.TwiML.Messaging;
using WebApplication27.Messages;
using Twilio.Jwt.AccessToken;
using Newtonsoft.Json.Linq;
using WebApplication27.EmailService;
using WebApplication27.Twilio.Otpverify;

namespace WebApplication27.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;

        private readonly TwilloContext _context;
        public RegisterController(TwilloContext context,IEmailService service,IConfiguration configuration)
        {
            _context = context;
            _emailService= service;
            _configuration= configuration;
        }
        [Route("newUser")]
        [HttpPost]
        public IActionResult NewRegister(UserRegister user)
        {
            if (user != null)
            {
                var isExsit = IsEmailExsit(user.PhoneNumber);
                if(isExsit)
                {
                    return new ObjectResult(" Phone Number is already Exsit") { StatusCode = 403 };
                }
                const string accountSid = "ACaafc26dcf5eb4310e57794ae223c5ae1";
                const string authToken = "d893195b0f51901c8edcf0f8dd382a5b";


                TwilioClient.Init(accountSid, authToken);
                var to = new PhoneNumber("+91" + user.PhoneNumber);

                var message = MessageResource.Create(
                    to,

                    from: new PhoneNumber("+15092848144"),
                    body: $"Your Account Successfully creted " + "UserName:" + user.Email + "Password:" + user.Password + "");
                _context.UserRegisters.Add(user);
                _context.SaveChanges();
                return new OkObjectResult("Success"){ StatusCode=200} ;
            }
            else
            {
               return BadRequest(); 
            }
        }
        [NonAction]
        public bool IsEmailExsit(string Number)
        {
            var result = _context.UserRegisters.Where(x => x.PhoneNumber == Number).FirstOrDefault();
            return result != null;
        }
        [HttpPost]
        [Route("UserLogin")]
        public IActionResult UserLogin(Login User)
        {
           
          var result=CheckEmailAndPassword(User.Email, User.Password);
            if (result)
            {
                var Usre=_context.UserRegisters.ToList();
                return Ok(Usre);
            }
            else
            {
                return new OkObjectResult("invalid Username and password") { StatusCode = 400 };
            }
                    
        }
       public bool CheckEmailAndPassword(string Email, string Password)
        {
           var result= _context.UserRegisters.Where(x=>x.Email==Email && x.Password==Password).FirstOrDefault();
            return result != null;
        }
        [Route("Userforgotpassword")]
        [HttpPost]
        public IActionResult ForgotPassword(ForgotPassword Password)
        {
            var result = _context.UserRegisters.ToList();
            var resltu=_context.UserRegisters.FirstOrDefault(x=>x.RegisterId==result.Select(x=>x.RegisterId).FirstOrDefault()); 
            if (Password != null)
            {
               
                var number = GetNumber(Password.GetemailandPassword);
                if (number)
                {
                    int otpValue = new Random().Next(100000, 999999);
                    const string accountSid = "AC48f2191188263b7f3b4eb5d1c9c0bd95";
                    const string authToken = "67524477cbae24c982e59b2dbd36ee53";
                    TwilioClient.Init(accountSid, authToken);
                    var to = new PhoneNumber("+91" + Password.GetemailandPassword);
                    var message = MessageResource.Create(
                        to,
                        
                        from: new PhoneNumber("+13602275378"),
                        body: $"Verification OTP " + otpValue + "");
                     Password.otp = otpValue;
                    resltu.Otp = otpValue;
                    _context.SaveChanges();
                    return new OkObjectResult("otp sent successfully+" + Password.GetemailandPassword) { StatusCode = 200 };
                }
                else
                {
                  
                    int EmailOtp = new Random().Next(100000, 999999);
                    var Email = Getemail(Password.GetemailandPassword.ToString());
                    //var user =_context.UserRegisters.FindByNameAsync(Password.GetemailandPassword);
                    if (Email)
                    {
                        // var result=new Message(Password.GetemailandPassword);
                      var message = new Message111(new string[] { Password.GetemailandPassword! }, "OTP Confrimation","Your OTP is"+ " "+EmailOtp.ToString());
                     //   var message = new Message(Password.GetemailandPassword! , "OTP Confrimation", EmailOtp.ToString());
                      //  var message1 = EmailOtp;
                       //  _emailService.SendEmail();
                        _emailService.SendEmail(message);
                        return new OkObjectResult("Otp send Successfully In Email") { StatusCode=200};
                        /*return StatusCode(StatusCodes.Status200OK,
                         new Response { Status = "Success", Message = $"We have sent an OTP to your Email {Password.GetemailandPassword}" });*/


                    }
                }
            }
                return BadRequest();
        }
        public bool Getemail(string email)
        {
            var result=_context.UserRegisters.Where(x=>x.Email== email).FirstOrDefault();   
            return result != null;
        }
        public bool GetNumber(string Number)
        {
            var getnumber=_context.UserRegisters.Where(x=>x.PhoneNumber==Number).FirstOrDefault();
            return getnumber != null;
        }
        [HttpPost]
        [Route("OtpVerification")]
        public IActionResult VerifyOtp(OtpVerification Verification)
        {
            if (Verification != null)
            {
                var success = otp(Verification.Otp);
                if (success)
                {
                    return new OkObjectResult("Verification SuccessFull") { StatusCode = 200 };
                }
                else
                {
                    return new OkObjectResult("") { StatusCode = 404 };
                }
            }
            return BadRequest();
           

        }
        public bool otp( int otp)
        {
            var result=_context.UserRegisters.Where(x=>x.Otp== otp).FirstOrDefault();
            return result != null;
        }
       
      /*  public IActionResult ResetPassword()
        {

        }*/

    }
   
}
