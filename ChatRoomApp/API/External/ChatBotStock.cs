using ChatRoomApp.API.External.Entities;
using Newtonsoft.Json;
using System.Reflection;

namespace ChatRoomApp.API.External
{
    public class ChatBotStock
    {
        private readonly HttpClient _httpClient;

        public ChatBotStock(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetBotStock(string stockCode)
        {
            string stockStr = ReplaceStockKeyWord(stockCode);
            var response = await _httpClient.GetAsync($"https://stooq.com/q/l/?s={stockStr}&f=sd2t2ohlcv&h&e=csv");

            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                bool hasValueQuote = false;
                string result = "";
                Stock ? stock = new Stock();

                if (!responseContent.Contains("Ticker missing"))
                {
                    var responseArray = responseContent.Split('\n');
                    var responseDatas = responseArray[1].Split(',');
                    hasValueQuote = HasValueQuote(responseDatas[6]);
                    stock = MapStock(responseDatas);
                }

                result = hasValueQuote ? $"{stockStr} quote is ${stock.Close} per share." : $"This stock does not have quote.";
                return result;
            }

            return "No data";
        }

        public bool IsStockCodeInvoked(string stockCode)
        {
            bool isStock = (string.Compare(stockCode, 0, "/stock=", 0, 7) == 0);
            return isStock;
        }

        //Note: This removes the "/stock=" 
        private string ReplaceStockKeyWord(string stockCode)
        {
            return IsStockCodeInvoked(stockCode) ? stockCode.Substring(7) : stockCode;
        }

        private Stock? MapStock(string[]? responseDatas)
        {
            Stock? stock = new Stock();

            stock = new Stock()
            {
                Symbol = responseDatas[0],
                Date = !responseDatas[1].Contains("N/D") ? DateTime.Parse(responseDatas[1]) : default,
                Time = !responseDatas[2].Contains("N/D") ? TimeSpan.Parse(responseDatas[2]) : default,
                Open = !responseDatas[3].Contains("N/D") ? double.Parse(responseDatas[3]) : default,
                High = !responseDatas[4].Contains("N/D") ? double.Parse(responseDatas[4]) : default,
                Low = !responseDatas[5].Contains("N/D") ? double.Parse(responseDatas[5]) : default,
                Close = !responseDatas[6].Contains("N/D") ? double.Parse(responseDatas[6]) : default,
                Volume = !responseDatas[7].Contains("N/D") ? int.Parse(responseDatas[7]) : default
            };

            return stock;
        }

        private bool HasValueQuote(string quote)
        {
            return !quote.Contains("N/D");
        }
    }
}
