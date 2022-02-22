using System;
using System.Net;
using Xunit;

namespace CurrencyUnitTest
{
    public class UnitTest1
    {
        [Theory]
        [InlineData("Rate",false)]
        public async void GetServiceTest(string orderField,bool? isAscending)
        {
            var client = new TestClientProvider().Client;

            var okResult = await client.GetAsync(string.Format("https://localhost:44371/api/currency/getall/orderField={0}&isAscending={1}", orderField, isAscending));

            Assert.Equal(HttpStatusCode.OK, okResult.StatusCode);

        }

        //[Fact]
        //public async void GetServiceTestInvalidValue()
        //{
        //    var client = new TestClientProvider().Client;

        //    var okResult = await client.GetAsync("values/0");
        //    okResult.EnsureSuccessStatusCode();

        //    Assert.Equal(HttpStatusCode.OK, okResult.StatusCode);

        //}
    }
}
