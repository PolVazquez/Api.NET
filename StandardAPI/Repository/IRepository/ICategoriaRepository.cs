using ApiCurso.Model;

namespace ApiCurso.Repository.IRepository
{
    public interface ICategoriaRepository
    {
        ICollection<Categoria> GetCategorias();
        Categoria GetCategoria(int id);
        bool ExistsCategoria(int id);
        bool ExistsCategoria(string nombre);
        bool CreateCategoria(Categoria categoria);
        bool UpdateCategoria(Categoria categoria);
        bool DeleteCategoria(Categoria categoria);
        bool Save();
    }
}