using System.Runtime.CompilerServices;

namespace ScreenSound.API.Requests
{
    public record MusicaRequest(string nome, int anoLancamento, ICollection<GeneroRequest> Generos=null);
}
