using StealCatsModels;

namespace StealCatsServices.Interfaces
{
    public interface ICatService
    {
        Task FetchCatsAsync();

        Task<IEnumerable<CatEntity>> GetCatsAsync(int page, int pageSize, string tag);

        Task<CatEntity> GetCatByIdAsync(int id);
    }
}