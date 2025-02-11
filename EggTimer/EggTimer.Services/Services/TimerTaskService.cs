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

    //private void AtualizarStatusTarefas()
    //{
    //    var tarefas = _dal.Listar();

    //    foreach (var task in tarefas)
    //    {
    //        var statusAtualizado = AtualizarStatus(task);
    //        if (task.Status != statusAtualizado)
    //        {
    //            task.Status = statusAtualizado;
    //            _dal.Atualizar(task);
    //        }
    //    }
    //}

    private void AtualizarStatusTarefas()
    {
        /*
         O Entity Framework Core (EF Core) tem um mecanismo interno chamado Change Tracker. Esse mecanismo rastreia todas as entidades que foram carregadas do banco de dados dentro de um mesmo contexto (DbContext).

Se uma entidade já foi carregada antes dentro desse contexto, o EF não a busca novamente no banco porque ele já tem essa entidade na memória.

Isso pode causar um problema quando você tenta atualizar uma entidade que já foi carregada antes.
         */
        using (var contexto = new EggTimerContext())
        {
            var dal = new DAL<TimerTask>(contexto);
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


    private string AtualizarStatus(TimerTask task)
    {
        var horarioAtual = TimeSpan.Parse(DateTime.Now.ToString("HH:mm:ss"));
        return DateOnly.FromDateTime(DateTime.Now) <= task.Data && horarioAtual < task.HorarioFim ? "Em Andamento" : "Concluído";
    }

    /* melhor explicação sobre a memoria do EF
     
    🔴 O comportamento antes da correção
1️⃣ Ao iniciar, o TimerTaskService buscava todas as tarefas do banco e armazenava no contexto dele.
2️⃣ A partir desse momento, ele só trabalhava com as tarefas que carregou naquele instante.
3️⃣ Se uma nova tarefa fosse adicionada pelo Controller, quando o TimerTaskService chamava .Listar(), ele buscava do banco, então ele via essa nova tarefa.
4️⃣ Mas se uma tarefa já existente fosse atualizada pelo Controller, o TimerTaskService não pegava essa atualização, porque ele já tinha carregado aquela tarefa antes e o EF não reconsultava o banco para ela.

💡 Por que ele via as novas tarefas, mas não via as atualizações feitas por outras partes do sistema?

➡️ Porque quando ele chamava .Listar(), o EF ia no banco para trazer as novas tarefas.
➡️ Mas, para tarefas que ele já tinha carregado, o EF não consultava o banco de novo, pois achava que a versão que já estava na memória era suficiente.
➡️ Então, novas tarefas eram carregadas, mas tarefas já existentes não eram atualizadas.
     
     */
}
