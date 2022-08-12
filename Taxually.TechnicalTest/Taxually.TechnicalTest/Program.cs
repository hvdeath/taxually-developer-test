using MediatR;
using System.Reflection;
using Taxually.TechnicalTest;
using Taxually.TechnicalTest.VatRegistration.VatRegistrators;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
builder.Services.AddTransient<ITaxuallyHttpClient, TaxuallyHttpClient>();
builder.Services.AddTransient<ITaxuallyQueueClient, TaxuallyQueueClient>();
builder.Services.Scan(scan => scan
  .FromAssemblyOf<IVatRegistrator>()
    .AddClasses(classes => classes.AssignableTo<IVatRegistrator>())
        .AsImplementedInterfaces()
        .WithTransientLifetime());

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
