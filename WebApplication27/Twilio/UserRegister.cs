using System;
using System.Collections.Generic;

namespace WebApplication27.Twilio;

public partial class UserRegister
{
    public int RegisterId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public string? PhoneNumber { get; set; }

    public int? Otp { get; set; }
}
