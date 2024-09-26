//using productservice.Model;
//using Microsoft.EntityFrameworkCore;
//using productservice.Service; // อย่าลืมเพิ่ม namespace ของ RabbitMQService

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

//// เรียกใช้ RabbitMQService เพื่อเริ่มรับคำสั่งจากคิว updateCart
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
using productservice.Service; // อย่าลืมเพิ่ม namespace ของ RabbitMQService

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddHttpClient();

// Register RabbitMQ Service
builder.Services.AddScoped<IRabbitMQService, RabbitMQService>(); // ลงทะเบียนให้ตรงกับ interface

// Add DbContext
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

var app = builder.Build();

// เรียกใช้ RabbitMQService เพื่อเริ่มรับคำสั่งจากคิว updateCart
using (var scope = app.Services.CreateScope())
{
    var rabbitMQService = scope.ServiceProvider.GetRequiredService<IRabbitMQService>(); // แก้ให้เรียกใช้ interface IRabbitMQService
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




