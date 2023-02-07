using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApplication27.Twilio.Login;
using WebApplication27.Twilio;
using WebApplication27.Messages;
using WebApplication27.validations;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.Configuration;
using WebApplication27.EmailService;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddFluentValidation(x =>
{
    x.AutomaticValidationEnabled
    = true;
});
var configuration = builder.Configuration;
builder.Services.AddTransient<IValidator<UserRegister>, Registervalidations>();
builder.Services.AddTransient<IValidator<Login>, Loginvalidation>();
builder.Services.AddControllers();
builder.Services.AddDbContext<TwilloContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("Myconnection")));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var emailConfig = configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
builder.Services.AddSingleton(emailConfig);

builder.Services.AddScoped<IEmailService,EmailService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
