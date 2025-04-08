using ApiCurso.Data;
using ApiCurso.Model;
using ApiCurso.Repository.IRepository;

namespace ApiCurso.Repository
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly ApplicationDbContext _db;

        public CategoriaRepository(ApplicationDbContext db)
            => _db = db;

        public bool CreateCategoria(Categoria categoria)
        {
            categoria.FechaCreacion = DateTime.UtcNow;
            _db.Categorias.Add(categoria);

            return Save();
        }

        public bool DeleteCategoria(Categoria categoria)
        {
            _db.Categorias.Remove(categoria);

            return Save();
        }

        public bool ExistsCategoria(int id)
            => _db.Categorias.Any(c => c.Id == id);

        public bool ExistsCategoria(string nombre)
            => _db.Categorias.Any(c => c.Nombre.ToLower().Trim() == nombre.ToLower().Trim());

        public Categoria GetCategoria(int id)
            => _db.Categorias.FirstOrDefault(c => c.Id == id);

        public ICollection<Categoria> GetCategorias()
            => _db.Categorias.OrderBy(c => c.Nombre).ToList();

        public bool Save()
            => _db.SaveChanges() >= 0;

        public bool UpdateCategoria(Categoria categoria)
        {
            categoria.FechaActualizacion = DateTime.UtcNow;
            var categoriaExistente = _db.Categorias.Find(categoria.Id);
            if (categoriaExistente != null)
                _db.Entry(categoriaExistente).CurrentValues.SetValues(categoria);
            else
                _db.Categorias.Update(categoria);

            return Save();
        }
    }
}