using Entities.Models;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Collections;
using DataAccess.Concrete.EntityFramework.Contexts;
using Entities.Entities;
using Microsoft.Extensions.DependencyInjection;
using Business.Services.Abstract;
using Business.Services.Concrete;

namespace Business.BackgroundServices
{
    public class DailyCurrencyUpdate : IHostedService
    {
        private IObservable<int> _timer = null;
        private IDisposable disp = null;
        private static readonly object locked = new object();

        private readonly IServiceScopeFactory _scopeFactory;
        public DailyCurrencyUpdate(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _timer = Observable.Interval(TimeSpan.FromDays(1)).Select(x => 0);
            disp = _timer.Subscribe(x =>
           {
               lock (locked)
               {
                   DoWork();
               }

           });

            return Task.CompletedTask;
        }

        private void DoWork()
        {
            const string URL = "https://www.tcmb.gov.tr/kurlar/today.xml";


            XDocument doc = XDocument.Load(URL);
            var tarih = Convert.ToDateTime(((System.Xml.Linq.XElement)doc.LastNode).FirstAttribute.Value);
            var tarihDate = doc.Descendants("Currency").Where(x => Enum.GetNames(typeof(CurrencyConstants)).ToList().Contains(x.LastAttribute.Value)).Select(x => new CurrencyModel()
            {
                Unit = (int)x.Element("Unit"),
                Kod = x.LastAttribute.Value,
                Isim = (string)x.Element("Isim"),
                CurrencyName = (string)x.Element("CurrencyName"),
                ForexBuying = (float)x.Element("ForexBuying"),
                ForexSelling = (float)x.Element("ForexSelling")
            }).ToList();

           
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<DailycurrencyContext>();
                Currency currency = new Currency(dbContext);
                ExchangeRate dailyExchangeRate = new ExchangeRate(dbContext);
                foreach (var item in tarihDate)
                {
                    tbl_currency existsCurrent = currency.GetByCode(item.Kod);
                    tbl_exchangeRates tbl_ExchangeRates = new tbl_exchangeRates();
                    tbl_currency tbl_Currency = new tbl_currency();
                    tbl_ExchangeRates.forexBuying = item.ForexBuying;
                    tbl_ExchangeRates.forexSelling = item.ForexSelling;
                    tbl_ExchangeRates.date = tarih;
                    if (existsCurrent == null)
                    {
                        tbl_Currency.currencyCode = item.Kod;
                        tbl_Currency.currencyNameTr = item.Isim;
                        tbl_Currency.currencyNameEn = item.CurrencyName;
                        currency.Insert(tbl_Currency);
                    }
                    else
                    {
                        tbl_Currency = existsCurrent;
                    }

                    int test = tbl_Currency.id;
                    tbl_ExchangeRates.currencyId = tbl_Currency.id;


                    dailyExchangeRate.Insert(tbl_ExchangeRates);

                }

            }





        }
        IEnumerable<XElement> XmlReadTest(string uri)
        {
            using (XmlReader reader = XmlReader.Create(uri))
            {
                reader.MoveToContent();

                // Parse the file and return each of the nodes.
                while (!reader.EOF)
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "Currency")
                    {
                        XElement el = XElement.ReadFrom(reader) as XElement;
                        if (el != null)
                            yield return el;
                    }
                    else
                    {
                        reader.Read();
                    }
                }
            }
        }


        public Task StopAsync(CancellationToken stoppingToken)
        {
            disp.Dispose();
            disp = null;

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            disp.Dispose();
            disp = null;
        }
    }
}
