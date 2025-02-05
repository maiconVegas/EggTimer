using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EggTimer.Modelos;
using EggTimer.Dados.Banco;
using Microsoft.Extensions.DependencyInjection;

namespace EggTimer.Services.Services;
public static class TimerTaskService
{
    private static Timer _timer;
    private static IServiceProvider _serviceProvider;

    public static void Executar()
    {
        var services = new ServiceCollection();
        services.AddDbContext<EggTimerContext>(options =>
            options.UseSqlServer("Server=localhost;Database=EggTimerDB;Trusted_Connection=True;TrustServerCertificate=True;")); // Configure sua string de conexão aqui

        services.AddScoped<DAL<TimerTask>>();

        _serviceProvider = services.BuildServiceProvider();
        _timer = new Timer(AtualizarStatusTarefas, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));

        Console.WriteLine("Serviço rodando... Pressione Enter para sair.");
        Console.ReadLine();
    }

    private static void AtualizarStatusTarefas(object state)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var dal = scope.ServiceProvider.GetRequiredService<DAL<TimerTask>>();
            var tarefas = dal.Listar();

            foreach (var task in tarefas)
            {
                var statusAtualizado = AtualizarStatus(task);
                if (task.Status != statusAtualizado)
                {
                    task.Status = statusAtualizado;
                    dal.Atualizar(task);
                }
            }
        }
    }

    private static string AtualizarStatus(TimerTask task)
    {
        var horarioAtual = TimeSpan.Parse(DateTime.Now.ToString("HH:mm:ss"));
        return horarioAtual < task.HorarioFim ? "Em Andamento" : "Concluído";
    }
}
