using System.Collections.Generic;
namespace WarehouseManagement_Shirinkin.Data.Services
{
    public interface IRepository<T>
    {
        List<T> GetAll();
        T? GetById(object id);
        void Add(T entity);
        void Update(T entity);
        void Delete(object id);
    }
}