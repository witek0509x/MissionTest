using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestMission
{
    class Mision
    {
        bool admin;
        string ServerIP;
        List<float> MissionTime;
        int AtributsNumber;
        string name;
        string IP;
        Command command;
        DB db;
        List<string> Atributs = new List<string>();
        public Mision()
        {

        }
        public int Init(string Name, string ServerIP, bool Admin = false, List<string> atributs = null)
        {
            MissionTime = new List<float>();
            name = Name;
            admin = Admin;
            IP = ServerIP;
            command = new Command(ServerIP, 3456);
            /*if (DBExist(name))
            {
                return 1;
            }*/
            if (1 == 0) ;
            else
            {
                if (admin)
                {
                    Atributs = atributs;
                    AtributsNumber = atributs.Count;
                    string sql;
                    db = new DB(name + ".db");
                    foreach (string atribut in Atributs) MissionTime.Add(0);
                    foreach (string element in Atributs)
                    {
                        sql = "CREATE TABLE " + element + " (time real, value real)";
                        db.Query(sql);
                    }
                    command.Create(name, Atributs);
                }
                else
                {

                    db = new DB(name + ".db");
                    Atributs = command.GetColumns(name);
                    string sql;
                    foreach (string element in Atributs)
                    {
                        sql = "CREATE TABLE " + element + " (time real, value real)";
                        db.Query(sql);
                    }
                    foreach (string atribut in Atributs) MissionTime.Add(0);
                    ReciveUpdate();
                    UpdateMissionTime();
                }
                
                return 0;
            }
        }
        void save(string value, string atribut)
        {
            string[] splited = value.Split(' ');
            List<string> values = new List<string>(splited);
            for(int i = 0; i < values.Count; i += 2)
            {
                string sql = "insert into " + atribut + " values (" + values[i] + ", " + values[i+1] + ")";
                db.Query(sql);
            }
        }
        public void ReciveUpdate()
        {
            for (int i = 0; i < Atributs.Count; i++)
            {
                if(command.CheckTopicality(name, Atributs[i], MissionTime[i]) == 0) save(command.ReciveUpdate(name, Atributs[i], MissionTime[i]), Atributs[i]);
            }
            UpdateMissionTime();
        }
        bool DBExist(string Name)
        {
            return !(db.Query("select * from dbstructure where Name = '" + Name + "'") == "");
        }
        public bool SendUpdate()
        {
            int i = 0;
            List<string> times = new List<string>();
            List<string> values = new List<string>();
            if (TXT.ReadOneLine(0, "data.txt") == "FileIsEmpty") return false;
            if (TXT.ReadOneLine(0, "data.txt") == "SomeExeption") return false;
            if (TXT.ReadOneLine(0, "picture.txt") == "FileIsEmpty") return false;
            if (TXT.ReadOneLine(0, "picture.txt") == "SomeExeption") return false;
            else
            {
                foreach(string atribut in Atributs)
                {
                    while (TXT.ReadOneLine(i, atribut + ".txt") != null)
                    {
                        times.Add(TXT.ReadOneLine(i, atribut + ".txt"));
                        values.Add(TXT.ReadOneLine(i + 1, atribut + ".txt"));
                        i += 2;
                    }
                    command.SendUpdate(name, atribut, times, values);
                    TXT.clear();
                }
                times.Clear();
                values.Clear();
                while (TXT.ReadOneLine(i, "picture.txt") != null)
                {
                    times.Add(TXT.ReadOneLine(i, "picture.txt"));
                    values.Add(TXT.ReadOneLine(i + 1, "picture.txt"));
                    i += 2;
                }
                command.SendUpdate(name, "picture", times, values);
                return true;
            }
        }
        public bool LastImage()
        {
            string respond = command.ReciveLastImage(name);
            TXT.Overwrite(respond, "picture.txt");
            return true;
        }
        public bool UpdateMissionTime()
        {
            for(int i = 0; i < Atributs.Count; i++) MissionTime[i] = db.QueryFloat("SELECT MAX(time) FROM " + Atributs[i])[0];
            return true;
        }

    }
}
