using CRUDAPI.DOMAIN;
using CRUDAPI.EFCORE;
using CRUDAPI.Services.Services.CustomerServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRUDAPI.Controllers
{

    [ApiController]
    [Authorize(policy: "ApiScope")]
    [Route("api/[controller]")]
    public class CrudOperationController : ControllerBase
    {
        private readonly CrudApiDbContext _dbContext;
        private readonly DbSet<Customer> _dbSet;
        private readonly ICustomerService _customerService;

        public CrudOperationController(CrudApiDbContext dbContext, ICustomerService customerService)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<Customer>(); // Burada Tipine göre veritabanımızdan tablomuzu Değişkende tutuyoruz.
            _customerService = customerService;
        }

        [HttpGet("GetCustomerModel")]
        public List<Customer> GetCustomerModel(long Id)
        {
            try
            {
                var entity = _customerService.GetCustomerModel(Id);

                return entity;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                throw;
            }
        }

        [HttpPost("CustomerSave")]
        public void CustomerSave(Customer entity)
        {
            try
            {
                _customerService.CustomerSave(entity);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                throw;
            }
        }

        [HttpPost("CustomerDelete")]
        public bool CustomerDelete(long Id)
        {
            try
            {
                bool result = _customerService.CustomerDelete(Id);

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                throw;
            }
        }

        [HttpPut("CustomerPassive")]
        public bool Passive(long Id)
        {
            try
            {
                bool result = _customerService.Passive(Id);

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                throw;
            }
        }

    }
}
