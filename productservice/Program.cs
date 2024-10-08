//using productservice.Model;
//using Microsoft.EntityFrameworkCore;
//using productservice.Service; // ����������� namespace �ͧ RabbitMQService

//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
//builder.Services.AddControllers();
//builder.Services.AddHttpClient();

//// Register RabbitMQ Service
//builder.Services.AddScoped<RabbitMQService>();

//// Add DbContext
//var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//{
//    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
//});

//var app = builder.Build();

//// ���¡�� RabbitMQService ����������Ѻ����觨ҡ��� updateCart
//using (var scope = app.Services.CreateScope())
//{
//    var rabbitMQService = scope.ServiceProvider.GetRequiredService<RabbitMQService>();
//}

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//    app.UseDeveloperExceptionPage();
//}

//app.UseHttpsRedirection();
//app.UseAuthorization();
//app.MapControllers();
//app.Run();

using productservice.Model;
using Microsoft.EntityFrameworkCore;
using productservice.Service; // ����������� namespace �ͧ RabbitMQService

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddHttpClient();

// Register RabbitMQ Service
builder.Services.AddScoped<IRabbitMQService, RabbitMQService>(); // ŧ����¹���ç�Ѻ interface

// Add DbContext
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

var app = builder.Build();

// ���¡�� RabbitMQService ����������Ѻ����觨ҡ��� updateCart
using (var scope = app.Services.CreateScope())
{
    var rabbitMQService = scope.ServiceProvider.GetRequiredService<IRabbitMQService>(); // ��������¡�� interface IRabbitMQService
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();




