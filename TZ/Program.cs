using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Json;

namespace TZ
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            using (HttpClient client = new HttpClient())
            {
                //Получаем первую страницу
                string url = "https://catfact.ninja/facts";
                string response = await client.GetStringAsync(url);

                var firstPage = JsonSerializer.Deserialize<CatFactsResponse>(response);

                // Получаем total и per_page
                int total = firstPage.total;
                int perPage = firstPage.per_page;

                //Вычисляем последнюю страницу
                int lastPage = (int)Math.Ceiling((double)total / perPage);

                Console.WriteLine($"Всего фактов: {total}");
                Console.WriteLine($"Фактов на странице: {perPage}");
                Console.WriteLine($"Последняя страница: {lastPage}");

                //Запрашиваем последнюю страницу
                string lastPageUrl = $"https://catfact.ninja/facts?page={lastPage}";
                string lastPageResponse = await client.GetStringAsync(lastPageUrl);
                var lastPageData = JsonSerializer.Deserialize<CatFactsResponse>(lastPageResponse);

                // Самый короткий факт по символам
                string shortestFact = lastPageData.data.OrderBy(f => f.fact.Length).First().fact;

                Console.WriteLine("\nСамый короткий факт с последней страницы:");
                Console.WriteLine(shortestFact);
            }
        }
    }

    internal class async
    {
    }
}

public class CatFactsResponse
{
    public int current_page { get; set; }
    public Fact[] data { get; set; }
    public string first_page_url { get; set; }
    public int from { get; set; }
    public int last_page { get; set; }
    public string last_page_url { get; set; }
    public string next_page_url { get; set; }
    public string path { get; set; }
    public int per_page { get; set; }
    public string prev_page_url { get; set; }
    public int to { get; set; }
    public int total { get; set; }
}

public class Fact
{
    public string fact { get; set; }
    public int length { get; set; }
}
