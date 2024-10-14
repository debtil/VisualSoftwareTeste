public interface IIBGERepository
{
    bool NoticiaJaExiste(string noticiaId);
    void InserirNoticia(string noticiaId, string titulo, string editorias, string introducao, string dataPublicacao, string link);
}
