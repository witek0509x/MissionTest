using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace TestMission
{
    
    class Command
    {
        string IP;
        int port;
        string command = "";
        public Command(string ip, int Port)
        {
            IP = ip;
            port = Port;
        }
        public void Create(string name, List<string> columns)
        {
            command = "create " + name;
            foreach (string atribut in columns) command += " " + atribut;
            CommandExecute();
        }
        public void SendUpdate(string name, string atribut, List<string> times, List<string> values)
        {
            command = "SendUpdate " + name + " " + atribut;
            for(int i = 0; i < values.Count; i++)
            {
                command += " " + times[i];
                command += " " + values[i];
            }
            CommandExecute();
        }
        public string ReciveUpdate(string name, string Atribut, float MissionTime)
        {
            command = "ReciveUpdate " + name + " " + Atribut + " " + MissionTime.ToString();
            return CommandExecute();
        }
        public List<string> GetColumns(string name)
        {
            command = "GetColumns " + name;
            string response = CommandExecute();
            List<string> Atributs = new List<string>();
            string[] splited = response.Split(' ');
            foreach (string element in splited) Atributs.Add(element);
            return Atributs;
        }
        public int CheckTopicality(string name, string atribut, float time)
        {
            command = "CheckTopicality " + name + " " + atribut + " " + time.ToString();
            string respond = CommandExecute();
            if (respond == "No") return 0;
            else return 1;
        }
        string CommandExecute()
        {
            string respond;
            if (command == "") return "NoCommandError";
            else respond = TCP.Connect(IP, command, 3456);
            clear();
            return respond;
        }
        public string ReciveLastImage(string name)
        {
            command = "GetLastImage " + name;
            CommandExecute();
            return "";
        }
        void clear()
        {
            command = "";
        }
    }
}
