using EggTimer.Dados.Banco;
using EggTimer.Modelos;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<EggTimerContext>();
builder.Services.AddTransient<DAL<TimerTask>>();

// Add services to the container.

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
