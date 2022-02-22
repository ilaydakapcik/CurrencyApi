using Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business.Services.Abstract
{
    public interface IExchangeRate
    {
        IQueryable<tbl_exchangeRates> GetAllRates();
        IQueryable<tbl_exchangeRates> GetByCode(int id);
        void Insert(tbl_exchangeRates data);
    }
}
