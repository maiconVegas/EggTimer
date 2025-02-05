namespace EggTimer.API.Responses;
public record TimerTaskResponse(string Nome, TimeSpan HorarioCronometrado, string Status);
