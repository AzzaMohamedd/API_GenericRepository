using APIGenericRepo.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace APIGenericRepo.Repositories
{
    public class GenericRepository<TEntity> where TEntity : class  //o force it accessed by class only
    {
        ITIContext db;
        public GenericRepository(ITIContext db)
        {
            this.db = db;
        }

        public List<TEntity> GetAll()
        {
            return db.Set<TEntity>().ToList();
        }

        public TEntity GetByID(int id)
        {
            return db.Set<TEntity>().Find(id);
        }

        public void Add(TEntity entity) { 
            db.Set<TEntity>().Add(entity);
        }

        public void Update(TEntity entity)
        {
            db.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(TEntity obj)
        {
            db.Set<TEntity>().Remove(obj);
        }

        public void Save()
        {
            db.SaveChanges();
        }
    }
}
