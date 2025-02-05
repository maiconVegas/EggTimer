using EggTimer.Dados.Banco;
using EggTimer.Services.Services;


using (var contexto = new EggTimerContext())
{
    var service = new TimerTaskService(contexto);
    service.Executar();
}