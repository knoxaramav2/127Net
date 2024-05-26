using HomeNet.Migrations;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddRazorPages();

var cstr = builder.Configuration["ConnectionStrings:127NetDb"] ?? "Server=.\\SQLEXPRESS;Database=NetHomeDb;Trusted_Connection=True;MultipleActiveResultSets=true;Initial Catalog=NetHomeDb;";
builder.Services.AddDbContextPool<NetHomeDbCtx>(opt => opt.UseMySQL(cstr));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapRazorPages();
app.UseStaticFiles();
app.MapControllers();

app.Run();
