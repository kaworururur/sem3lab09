using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
class Program
{
    static object filelock = new object();
    static string output = "results.txt";
    static string acces_token = "c1c0cnRUeG1Vb2hyVVpaVENzZEhGdGgxbl9JWEVFVTJTYmxuc05iNDdEWT0";
    static async Task Main()
    {
        var tickers = System.IO.File.ReadAllLines("C:/Users/stepa/Documents/code/lab9/lab9_1/bin/Debug/net8.0/tickers.txt");
        var tasks = new List<Task>();
        foreach (var ticker in tickers)
        {
            tasks.Add(Perform(ticker));
        }
        await Task.WhenAll(tasks);
    }

    static async Task Perform(string ticker)
    {
        var startDate = new DateTimeOffset(DateTime.Now.AddDays(-90)).ToUnixTimeSeconds();
        var endDate = new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds();
        var url = $"https://api.marketdata.app/v1/stocks/candles/D/{ticker}/?from={startDate}&to={endDate}";

        using (var client = new HttpClient())
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", acces_token);
            var response = await client.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            JObject json = JObject.Parse(content);
            if (response.ReasonPhrase == "OK")
            {
                double totalPrice = 0;
                int days = 0;
                double[] h = json["h"].ToObject<double[]>();
                double[] l = json["l"].ToObject<double[]>();
                for (int i = 0; i < h.Length; i++)
                {
                    totalPrice += (h[i] + l[i]) / 2;
                    days++;
                }
                var AveragePrice = totalPrice / days;
                lock (filelock)
                {
                    File.AppendAllText(output, $"{ticker} : {AveragePrice:F2} \n");
                }
                Console.WriteLine($"Средняя цена акции {ticker} за год : {AveragePrice:F2}");
            }
            else
            {
                Console.WriteLine($"No data for ticker - {ticker}");
            }
        }
    }
}