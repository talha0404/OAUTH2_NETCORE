using CRUDAPI.DOMAIN;
using CRUDAPI.EFCORE;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDAPI.Services.Services.CustomerServices
{
    public class CustomerService : ICustomerService
    {
        private readonly CrudApiDbContext _dbContext;
        private readonly DbSet<Customer> _dbSet;

        public CustomerService(CrudApiDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<Customer>();
        }

        public List<Customer> GetCustomerModel(long Id)
        {
            try
            {
                var entity = _dbSet.AsNoTracking().Where(x => !x.IsDeleted && x.Id == Id).ToList();
                return entity;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                throw;
            }
        }

        public void CustomerSave(Customer entity)
        {
            try
            {
                if (entity.Id == 0)
                    _dbSet.Add(entity);
                else
                    _dbSet.Update(entity);

                _dbContext.SaveChanges();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                throw;
            }
        }

        public bool CustomerDelete(long Id)
        {
            try
            {
                var entity = _dbSet.Where(x => !x.IsDeleted && x.Id == Id).FirstOrDefault();

                if (object.Equals(entity, null)) return false;

                _dbSet.Remove(entity);

                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                throw;
            }
        }

        public bool Passive(long Id)
        {
            var entity = _dbSet.Where(x => !x.IsDeleted && x.Id == Id).FirstOrDefault();

            if (object.Equals(entity, null)) return false;

            entity.IsDeleted = true;

            _dbContext.SaveChanges();
            return true;
        }

    }
}
