using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Models
{
    //public class Currency
    //{
    //    public int Unit { get; set; }
    //    public string Isim { get; set; }
    //    public string CurrencyName { get; set; }
    //    public float ForexBuying { get; set; }
    //    public float ForexSelling { get; set; }
    //    public float BanknoteBuying { get; set; }
    //    public float BanknoteSelling { get; set; }
    //    public float CrossRateUSD { get; set; }
    //    public float CrossRateOther { get; set; }
    //}

    public class CurrencyModel
    {
        public int Unit { get; set; }
        public string Isim { get; set; }
        public string Kod { get; set; }
        public string CurrencyName { get; set; }
        public float ForexBuying { get; set; }
        public float ForexSelling { get; set; }
    }
    public class Tarih_Date
    {
        public List<CurrencyModel> Currencies { get; set; }
    }
}
