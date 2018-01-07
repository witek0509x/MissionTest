using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestMission
{
    class Program
    {
        static void Main(string[] args)
        {
            string Name;
            string ip;
            Mision mission = new Mision();
            Name = Console.ReadLine();
            mission.Init(Name, "127.0.0.1");
            mission.ReciveUpdate();
            mission.LastImage();
            
        }
    }
}
