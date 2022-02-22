using Business.Services.Abstract;
using DataAccess.Concrete.EntityFramework.Contexts;
using Entities.Entities;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business.Services.Concrete
{
    public class Currency : ICurrency
    {
        //public bool GetRate()
        //{
        //    throw new NotImplementedException();
        //}
        private readonly DailycurrencyContext _context;
        private DbSet<tbl_currency> _entities;
        public Currency(DailycurrencyContext context)
        {
            _context = context;
            _entities = context.Set<tbl_currency>();
        }

        public IQueryable<tbl_currency> GetAllCurrencies()
        {
            return _entities;
        }

        public object GetRatesByCode(string code)
        {
            var currency = GetByCode(code);
            var currencies = GetAllCurrencies();
            DateTime firstDate = _context.tbl_exchangeRates.ToList().Where(x => x.currencyId == currency.id).First().date;
            var rates = _context.tbl_exchangeRates.ToList().Where(x => x.currencyId == currency.id).OrderBy(x=>x.date).ToList();


            var result = from o in rates
                         join x in currencies on o.currencyId equals x.id
            select new
            {
                Currency = x.currencyCode + "-" + Enum.GetName(typeof(CurrencyConstants), 0),
                Date = o.date.ToShortDateString(),
                Rate = Math.Round(o.forexBuying, 3)
            };

            var query = result.Select((x, i) => new
            {
                Currency = x.Currency,
                Date = x.Date,
                Rate = x.Rate,
                DateDiff = i == 0 ? 0 : (Math.Round((x.Rate - result.ToList()[i - 1].Rate) % 10,3))
            });

            return query;

        }

        public object GetLastRates(string columnName, bool? isAscending)
        {
            var currencies = GetAllCurrencies();
            var exchangeRates = _context.tbl_exchangeRates.ToList();

            DateTime lastUpdate = exchangeRates.OrderByDescending(x=>x.date).First().date;
            var rates = exchangeRates.ToList();
            var result = from o in rates
                         join x in currencies on o.currencyId equals x.id
                         where o.date == lastUpdate
                         select new
                         {
                             Currency = x.currencyCode + "-" + Enum.GetName(typeof(CurrencyConstants), 0),
                             LastUpdated = o.date.ToShortDateString(),
                             CurrentRate = Math.Round(o.forexBuying, 2)
                         };
            if (columnName != null && isAscending != null)
            {
                if (columnName == "Rate")
                {
                    switch (isAscending)
                    {
                        case true:
                            result = result.OrderBy(x => x.CurrentRate);
                            break;
                        case false:
                            result = result.OrderByDescending(x => x.CurrentRate);
                            break;
                        default:
                            break;
                    }
                }
                else if (columnName == "Code")
                {
                    switch (isAscending)
                    {
                        case true:
                            result = from o in rates
                                     join x in currencies on o.currencyId equals x.id
                                     where o.date == lastUpdate
                                     orderby x.currencyCode
                                     select new
                                     {
                                         Currency = x.currencyCode + "-" + Enum.GetName(typeof(CurrencyConstants), 0),
                                         LastUpdated = o.date.ToShortDateString(),
                                         CurrentRate = Math.Round(o.forexBuying, 2)
                                     };
                            break;
                        case false:
                            result = from o in rates
                                     join x in currencies on o.currencyId equals x.id
                                     where o.date == lastUpdate
                                     orderby x.currencyCode descending
                                     select new
                                     {
                                         Currency = x.currencyCode + "-" + Enum.GetName(typeof(CurrencyConstants), 0),
                                         LastUpdated = o.date.ToShortDateString(),
                                         CurrentRate = Math.Round(o.forexBuying, 2)
                                     };
                            break;
                        default:
                            break;
                    }
                }
            }
            return result;
        }

        public tbl_currency GetByCode(string code)
        {
            var currentCurrency = _entities.Where(x => x.currencyCode == code).FirstOrDefault();

            return currentCurrency;
        }

        public void Insert(tbl_currency data)
        {
            _context.tbl_currency.Add(data);

            _context.SaveChanges();
        }
    }
}
