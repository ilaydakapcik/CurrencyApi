using Business.Services.Abstract;
using DataAccess.Concrete.EntityFramework.Contexts;
using Entities.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CurrencyApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {

        private ICurrency _dataService;
        public CurrencyController(ICurrency dataService)
        {
            _dataService = dataService;
        }

        // GET: api/<Currency>
        [HttpGet("GetAll")]
        public ActionResult Get(string orderField, bool? isAscending)
        {
            var list =  _dataService.GetLastRates(orderField, isAscending);
          
            return Ok(list);
        }

        // GET api/Currency/Service/5
        [HttpGet("GetByCode/{code}")]
        public ActionResult Get(string code)
        {
           var list =  _dataService.GetRatesByCode(code.ToUpper());

            return Ok(list);
        }

      
    }
}
