using DataAccess.Concrete.EntityFramework;
using Entities.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Reflection;



namespace DataAccess.Concrete.EntityFramework.Contexts
{
    public partial class DailycurrencyContext : DbContext
    {
        public DailycurrencyContext()
        {
        }

        public DailycurrencyContext(DbContextOptions<DailycurrencyContext> options)
            : base(options)
        {
        }

        public virtual DbSet<tbl_currency> tbl_currency { get; set; }
        public virtual DbSet<tbl_exchangeRates> tbl_exchangeRates { get; set; }
    }
}