using CRUDAPI.DOMAIN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDAPI.Services.Services.CustomerServices
{
    public interface ICustomerService
    {
        List<Customer> GetCustomerModel(long Id);
        void CustomerSave(Customer entity);
        bool CustomerDelete(long Id);
        bool Passive(long Id);
    }
}
