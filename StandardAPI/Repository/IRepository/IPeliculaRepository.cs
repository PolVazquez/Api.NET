using ApiCurso.Model;

namespace ApiCurso.Repository.IRepository
{
    public interface IPeliculaRepository
    {
        //v1
        ICollection<Pelicula> GetPeliculas();
        //v2
        ICollection<Pelicula> GetPeliculas(int pageNumber, int pageSize);
        ICollection<Pelicula> GetPeliculasByCategoria(int categoriaId);
        IEnumerable<Pelicula> SearchPelicula(string nombre);
        Pelicula GetPelicula(int id);
        int GetTotalPeliculas();
        bool ExistsPelicula(int id);
        bool ExistsPelicula(string nombre);
        bool CreatePelicula(Pelicula pelicula);
        bool UpdatePelicula(Pelicula pelicula);
        bool DeletePelicula(Pelicula pelicula);
        bool Save();
    }
}