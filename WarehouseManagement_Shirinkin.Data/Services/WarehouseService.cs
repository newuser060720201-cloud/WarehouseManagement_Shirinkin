using System.Collections.Generic;
using System.Linq;
using WarehouseManagement_Shirinkin.Data.Models;

namespace WarehouseManagement_Shirinkin.Data.Services
{
    public class WarehouseService : IRepository<Warehouse>
    {
        private List<Warehouse> _warehouses;
        public WarehouseService(OrganizationService orgService)
        {
            _warehouses = DataGenerator.GenerateWarehouses(orgService.GetAll());
        }
        public List<Warehouse> GetAll() => _warehouses.ToList();
        public List<Warehouse> GetByOrganizationId(long organizationId) => _warehouses.Where(w => w.OrganizationId == organizationId).ToList();
        public Warehouse? GetById(object id) => id is long longId ? _warehouses.FirstOrDefault(w => w.Id == longId) : null;
        public void Add(Warehouse entity) => _warehouses.Add(entity);
        public void Update(Warehouse entity)
        {
            var index = _warehouses.FindIndex(w => w.Id == entity.Id);
            if (index != -1) _warehouses[index] = entity;
        }
        public void Delete(object id)
        {
            var wh = GetById(id);
            if (wh != null) _warehouses.Remove(wh);
        }
    }
}