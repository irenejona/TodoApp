using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

namespace SignalRCient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Starting client subscription");
            var subscriptionList = args.FirstOrDefault();
            var connection = new HubConnectionBuilder()
                .WithUrl($"http://localhost:5005/subscribe?todoListId={subscriptionList}")
                .Build();
            
            connection.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(0,5) * 1000);
                await connection.StartAsync();
            };

            connection.On<string>("notify", o => { Console.WriteLine(o); });
            
            await connection.StartAsync();
            Console.ReadLine();
        }
    }
}