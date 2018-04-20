using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleDate_Test
{
    public class Interval
    {
        public int Id { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime DueDate { get; set; }
    }
    class Program
    {
        static HttpClient client = new HttpClient();

        //вывод в строку элемента таблицы
        static void ShowInterval (Interval interval)
        {
            Console.WriteLine($"Begin Date: {interval.BeginDate}\tDue Date: " + 
                $"{interval.DueDate}");
        }

        //добавление нового элемента в таблицу
        static async Task<Uri> CreateIntervalAsync (Interval interval)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync(
                "api/date", interval);
            response.EnsureSuccessStatusCode();
            return response.Headers.Location;
        }

        //получение списка элементов таблицы (применяется как для полного списка, так и для выборки)
        static async Task<List<Interval>> GetAllIntervalAsync(string path)
        {
            List<Interval> interval = null;
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                interval = await response.Content.ReadAsAsync<List<Interval>>();
            }
            return interval;
        }

        //метод для формирования нового элемента
        static async Task AddInterval(DateTime beginDate, DateTime dueDate)
        {
            try
            {
                Interval interval = new Interval
                {
                    BeginDate = beginDate,
                    DueDate = dueDate
                };

                var url = await CreateIntervalAsync(interval);
                Console.WriteLine($"Created at {url}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        //метод для вызова выборки
        static async Task CheckInterval(DateTime beginDate, DateTime dueDate)
        {

            try
            {
                var url = "/api/date?begin=" + beginDate.ToString("yyyy-MM-dd") + "&due=" + dueDate.ToString("yyyy-MM-dd");
                var intervals = await GetAllIntervalAsync(url);
                foreach (var item in intervals)
                {
                    ShowInterval(item);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        static void Main(string[] args)
        {
            RunAsync().GetAwaiter().GetResult();
            TypeStart:
            Console.WriteLine("Type begin date in format YYYY-MM-DD:");
            string beginDate = Console.ReadLine();
            Console.WriteLine("Type due date in format YYYY-MM-DD:");
            string dueDate = Console.ReadLine();
            Console.WriteLine("You type interval: from {0} to {1}", 
                Convert.ToDateTime(beginDate), Convert.ToDateTime(dueDate));
            Console.WriteLine("Type 1 for add interval to DB; Type 2 for check interval");
            string userAction = Console.ReadLine();
            if (userAction == "1")
            {
                AddInterval(Convert.ToDateTime(beginDate), Convert.ToDateTime(dueDate))
                    .GetAwaiter().GetResult();
            }
            else if (userAction == "2")
            {
                CheckInterval(Convert.ToDateTime(beginDate), Convert.ToDateTime(dueDate))
                    .GetAwaiter().GetResult();
            }
            else Console.WriteLine("Wrong data!");

            Console.ReadLine();
            goto TypeStart;
        }

        static async Task RunAsync()
        {
            client.BaseAddress = new Uri("http://localhost:50099/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                var url = "/api/date";
                var intervals = await GetAllIntervalAsync(url);
                foreach (var item in intervals)
                {
                    ShowInterval(item);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
