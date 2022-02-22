using System;
using System.Collections.Generic;

namespace Entities.Entities
{
    public partial class tbl_currency
    {
        public tbl_currency()
        {
            tbl_exchangeRates = new HashSet<tbl_exchangeRates>();
        }

        public int id { get; set; }
        public string currencyCode { get; set; }
        public string currencyNameTr { get; set; }
        public string currencyNameEn { get; set; }

        public virtual ICollection<tbl_exchangeRates> tbl_exchangeRates { get; set; }
    }
}