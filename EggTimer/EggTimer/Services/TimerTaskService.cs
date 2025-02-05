//using EggTimer.Dados.Banco;
//using EggTimer.Modelos;

//namespace EggTimer.API.Services;
//public class TimerTaskService
//{
//    private readonly DAL<TimerTask> _dal;
//    private readonly Timer _timer;

//    public TimerTaskService(DAL<TimerTask> dal)
//    {
//        _dal = dal;
//        _timer = new Timer(AtualizarStatusTarefas, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));
//    }

//    private void AtualizarStatusTarefas(object state)
//    {
//        var tarefas = _dal.Listar();
//        foreach (var task in tarefas)
//        {
//            var statusAtualizado = AtualizarStatus(task);
//            if (task.Status != statusAtualizado)
//            {
//                task.Status = statusAtualizado;
//                _dal.Atualizar(task);
//            }
//        }
//    }

//    private string AtualizarStatus(TimerTask task)
//    {
//        var horarioAtual = TimeSpan.Parse(DateTime.Now.ToString("HH:mm:ss"));
//        return horarioAtual < task.HorarioFim ? "Em Andamento" : "Concluído";
//    }
//}
