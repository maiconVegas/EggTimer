namespace EggTimer.API.Requests;
public record TimerTaskRequest(string Nome, TimeSpan HorarioCronometrado, string Status);
