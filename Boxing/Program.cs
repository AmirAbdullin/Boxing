using System;
using System.IO;

namespace Boxing
{
    class Program
    {
        static void Main(string[] args)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "Data\\0\\100_cl.json");
            var json = File.ReadAllText(path);

            var data = Newtonsoft.Json.JsonConvert.DeserializeObject <Models.Input_JSON>(json);

            Console.WriteLine(data.Cargo_groups[0].Id);
        }
    }
}
