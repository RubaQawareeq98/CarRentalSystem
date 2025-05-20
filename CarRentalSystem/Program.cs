using CarRentalSystem.Configurations;
using Microsoft.Extensions.Hosting;

namespace CarRentalSystem;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
        var host = Host.CreateDefaultBuilder();
        host.Configure();
    }
}