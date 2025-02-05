using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EggTimer.Modelos;
using EggTimer.Dados.Banco;

namespace EggTimer.Services.Services;
public class TimerTaskService
{
    private Timer _timer;
    private readonly DAL<TimerTask> _dal;

    public TimerTaskService(EggTimerContext contexto)
    {
        _dal = new DAL<TimerTask>(contexto);
    }

    public void Executar()
    {
        _timer = new Timer(state => AtualizarStatusTarefas(), null, TimeSpan.Zero, TimeSpan.FromMinutes(1));

        Console.WriteLine("Serviço rodando... Pressione Enter para sair.");
        Console.ReadLine();
    }

    private void AtualizarStatusTarefas()
    {
        var tarefas = _dal.Listar();

        foreach (var task in tarefas)
        {
            var statusAtualizado = AtualizarStatus(task);
            if (task.Status != statusAtualizado)
            {
                task.Status = statusAtualizado;
                _dal.Atualizar(task);
            }
        }
    }

    private string AtualizarStatus(TimerTask task)
    {
        var horarioAtual = TimeSpan.Parse(DateTime.Now.ToString("HH:mm:ss"));
        return horarioAtual < task.HorarioFim ? "Em Andamento" : "Concluído";
    }
}
