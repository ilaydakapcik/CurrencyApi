using Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business.Services.Abstract
{
    public interface ICurrency
    {
        IQueryable<tbl_currency> GetAllCurrencies();
        tbl_currency GetByCode(string code);
        void Insert(tbl_currency data);

        object GetRatesByCode(string code);
        object GetLastRates(string orderField, bool? isAscending);

    }
}
