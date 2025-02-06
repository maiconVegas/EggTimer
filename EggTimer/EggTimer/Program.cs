//using EggTimer.API.Services;
using EggTimer.Dados.Banco;
using EggTimer.Modelos;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<EggTimerContext>();
builder.Services.AddTransient<DAL<TimerTask>>();

//builder.Services.AddSingleton<TimerTaskService>();

// Adicionando o baguiu do CORS para que eu possa utilizar pelo ajax do javascript, não sabia disso e to aprendendo
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
        });
});


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//aqui em baixo é usar o CORS antes dos controladores, usar de acordo que foi construido no build
app.UseCors("AllowAll");

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
