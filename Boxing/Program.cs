using System;
using System.IO;
using System.Collections.Generic;

namespace Boxing
{
    
    class Cargo_Groups_Comparer : IComparer<Models.Cargo_Groups>
    {
        public int Compare(Models.Cargo_Groups? x, Models.Cargo_Groups? y)
        {
            if (x.Size[0] * x.Size[1] * x.Size[2] > y.Size[0] * y.Size[1] * y.Size[2])
            {
                return -1;
            }
            else
                return 1;
        }
    } 
    public class Package : Models.Cargo_Groups
    {        
        public Package(Models.Cargo_Groups Object) { Id = Object.Id; Size = Object.Size; Mass = Object.Mass; Group_id = Object.Group_id; Count = Object.Count; }
        //                       L W H       
        public int[] Position = {0,0,0};
    }
    public class FillingSpace : Models.Cargo_Space          // Я наследовал классы, возможно, лучше сделать их с нуля
    {

        public FillingSpace(Models.Cargo_Space Container)
        {
            Grand = Container.Size;
            Ontop = new FillingSpace();
            Ontop.Size = new int[3];
            Onright = new FillingSpace();
            Onright.Size = new int[3];
            Down = new FillingSpace();
            Down.Size = new int[3];
            Size = Container.Size;
        }
        public FillingSpace() { }
        public int[] Size = new int[3];
        bool setbox(Package sub)
        {
            if (sub.Size[0] <= Size[0] && sub.Size[1] <= Size[1] && sub.Size[2] <= Size[2])                              //  | - l             
            {                                                                                                            //  -- - w

                sub.Position = Position;  // ВЫДЕЛЕНИЕ МЕСТА ПОД ПОДЗОНЫ
                Ontop.Size[0] = 0; // у области сверху площадь устанавливаемой коробки
                Ontop.Size[1] = sub.Size[1]; // и оставшаяся высота до потолка
                Ontop.Size[2] = Grand[2] - (sub.Size[2] + Position[2]);
                Ontop.Position = Position;
                Ontop.Position[2] += sub.Size[2];

                Onright.Size[0] = sub.Size[0]; // у области справа длина устанавливаемой коробки и ширина остаточной области
                Onright.Size[1] = Grand[1] - (Position[1] + sub.Size[1]);
                Onright.Size[2] = Grand[2] - Position[2];// и оставшаяся высота до потолка
                Onright.Position = Position;
                Onright.Position[1] += sub.Size[1];

                Down.Size[0] = Grand[0] - (sub.Size[0] + Position[0]); // у области справа длина устанавливаемой коробки и ширина остаточной области
                Down.Size[1] = Grand[1] - (Position[1] + sub.Size[1]);
                Down.Size[2] = Grand[2] - Position[2];// и оставшаяся высота до потолка
                Down.Position = Position;
                Down.Position[0] += sub.Size[0];
                return true;
            }
            else return false;
        }
        public void fillbox(List<Package> sub)
        {
            for (int i = 0; i < sub.Count - 1; i++)
            {
                if (setbox(sub[i]) == true)
                {
                    setbox(sub[i]);
                    sub.Remove(sub[i]);
                }
                if (Ontop.setbox(sub[i]) == true) ////
                {
                    Ontop.setbox(sub[i]);
                    sub.Remove(sub[i]);
                }
                if (Ontop.setbox(sub[i]) == true)
                {
                    Onright.setbox(sub[i]);
                    sub.Remove(sub[i]);
                }
                if (Down.setbox(sub[i]) == true)
                {
                    Down.setbox(sub[i]);
                    sub.Remove(sub[i]);
                }
                // else if (sub.Size[0])
            }
        }

        //                       L W H       
        public int[] Position = { 0, 0, 0 };
        public int[] Grand;
        public FillingSpace Ontop;
        public FillingSpace Onright;
        public FillingSpace Down;

    }
    class Program
    {
        void Output()
        {

        }
        static void Main(string[] args)
        {
            string path = Directory.GetCurrentDirectory();
            path = path.Remove(path.Length - 10, 10);
            path = Path.Combine(path, "Data\\0\\100_cl.json");
            var json = File.ReadAllText(path);

            var data = Newtonsoft.Json.JsonConvert.DeserializeObject <Models.Input_JSON>(json);
           
            Models.Cargo_Groups[] Cargo_Groups = data.Cargo_groups;
            Models.Cargo_Space Cargo_Space = data.Cargo_space;
            
            Array.Sort(Cargo_Groups, new Cargo_Groups_Comparer());

            Console.WriteLine("Cargo_Space:\n");
            Cargo_Space.Write_ALL();
            Console.WriteLine("Cargo_Groups\n");

            List <Package> Package = new List<Package> (130);


            for (int i = 0,j = 0;i < Cargo_Groups.Length; i++)
            {
                for (int n = 0; n < Cargo_Groups[i].Count; n++)         //ВЫВОД ЯЩИКОВ В МАССИВ
                {
                    //Добавить изменения ID !!!
                    if (j >= Package.Count)
                        Package.Add(new Package(Cargo_Groups[i]));
                    Package[j] = new Package(Cargo_Groups[i]);
                    j++;
                }
            }
            FillingSpace Hangar = new FillingSpace (Cargo_Space);
            
            Hangar.fillbox(Package);
            for(int i = 0; i < Package.Count; i++)
            {

                Package[i].Write_ALL();
                Console.WriteLine(Package[i].Position[0]);
                Console.WriteLine(Package[i].Position[1]);
                Console.WriteLine(Package[i].Position[2]);
            }

        }
    }
}
