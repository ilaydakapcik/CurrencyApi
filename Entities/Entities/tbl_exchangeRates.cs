using System;
using System.Collections.Generic;

namespace Entities.Entities
{
    public partial class tbl_exchangeRates
    {
        public int id { get; set; }
        public int currencyId { get; set; }
        public double forexBuying { get; set; }
        public double forexSelling { get; set; }
        public double? banknoteBuying { get; set; }
        public double? banknoteSelling { get; set; }
        public DateTime date { get; set; }

        public virtual tbl_currency currency { get; set; }
    }
}