using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Money
{
    public class CoinbaseResponse
    {
        public CoinbaseData Data { get; set; }
    }

    public class CoinbaseData
    {
        public double Amount { get; set; }
    }

    public class CurrencyData
    {
        [JsonProperty("rate")]
        public double Rate { get; set; }
    }

    public class Currency
    {
        private HttpClient client = new HttpClient();

        public async Task<double> GetCurrencyRateAsync(string url)
        {
            var response = await client.GetStringAsync(url);
            var currencyData = JsonConvert.DeserializeObject<List<CurrencyData>>(response);
            return currencyData[0].Rate;
        }

        public async Task<double> GetEUR_CurrencyAsync()
        {
            return await GetCurrencyRateAsync("https://bank.gov.ua/NBUStatService/v1/statdirectory/exchange?valcode=EUR&date=20240104&json");
        }

        public async Task<double> GetUSD_CurrencyAsync()
        {
            return await GetCurrencyRateAsync("https://bank.gov.ua/NBUStatService/v1/statdirectory/exchange?valcode=USD&date=20240104&json");
        }

        public async Task<double> GetBTC_CurrencyAsync()
        {
            var response = await client.GetStringAsync("https://api.coinbase.com/v2/prices/BTC-USD/spot");
            var btcResponse = JsonConvert.DeserializeObject<CoinbaseResponse>(response);
            return btcResponse.Data.Amount;
        }
    }

    internal class Program
    {
        static async Task Main(string[] args)
        {
            Currency currency = new Currency();

            Console.WriteLine($"USD = {await currency.GetUSD_CurrencyAsync()}");
            Console.WriteLine($"EUR = {await currency.GetEUR_CurrencyAsync()}");
            Console.WriteLine($"BTC = {await currency.GetBTC_CurrencyAsync()}");

            Console.ReadLine();
        }
    }
}
