namespace MyPortfolio.Models.Repositories.Contracts
{
    public interface IBaseRepository<T>
    {
        Task<List<T>> GetAll();
        Task<T> GetOne(string id);
        Task Add(T entity);
        Task Update(T entity, string id);
        Task Delete(string id);
    }
}
