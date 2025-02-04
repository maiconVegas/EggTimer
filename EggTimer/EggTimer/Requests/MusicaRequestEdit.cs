namespace ScreenSound.API.Requests
{
    public record MusicaRequestEdit(int id, string nome, int anoLancamento) : MusicaRequest(nome, anoLancamento);
}
