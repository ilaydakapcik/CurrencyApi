using Business.Services.Abstract;
using DataAccess.Concrete.EntityFramework.Contexts;
using Entities.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business.Services.Concrete
{
    public class ExchangeRate : IExchangeRate
    {
        private readonly DailycurrencyContext _context;
        private DbSet<tbl_exchangeRates> _entities;
        public ExchangeRate(DailycurrencyContext context)
        {
            _context = context;
            _entities = context.Set<tbl_exchangeRates>();
        }


        public IQueryable<tbl_exchangeRates> GetAllRates()
        {
            return _entities;
        }

        public IQueryable<tbl_exchangeRates> GetByCode(int id)
        {
            return _entities.Where(x => x.currencyId == id);
        }

        public void Insert(tbl_exchangeRates data)
        {
            _context.tbl_exchangeRates.Add(data);

            _context.SaveChanges();
        }
    }
}
