namespace EggTimer.API.Responses;
public record TimerTaskResponse(int Id, string Nome, TimeSpan HorarioFim, TimeSpan HorarioCronometrado, string Status);
