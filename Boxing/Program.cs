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
           
            Models.Cargo_Groups[] Cargo_Groups = data.Cargo_groups;
            Models.Cargo_Space Cargo_Space = data.Cargo_space;
            Console.WriteLine("Cargo_Space:\n");
            Cargo_Space.Write_ALL();
            Console.WriteLine("Cargo_Groups\n");
            foreach (var Cargo_Group in Cargo_Groups)
            {
                Cargo_Group.Write_ALL();
            }
        }
    }
}
