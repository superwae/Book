using Lafatkotob.ViewModels;

namespace Lafatkotob.Services.GenreService
{
    public interface IGenreService
    {
        Task<GenreModel> Post(GenreModel model);
        Task<GenreModel> GetById(int id);
        Task<List<GenreModel>> GetAll();
        Task<GenreModel> Update(GenreModel model);
        Task<GenreModel> Delete(int id);
    }
}
