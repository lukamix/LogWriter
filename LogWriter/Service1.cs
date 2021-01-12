using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace LogWriter
{
    public partial class Service1 : ServiceBase
    {
        FileSystemWatcher fileSystemWatcher = new FileSystemWatcher();
        Timer timer = new Timer();
        string pathtowatch = @"C:\Users\PV\Desktop\Test_Log";
        string fnpath = @"C:\Users\PV\Desktop\Test_Log" + "\\FinalStatus\\FinalStatus_" + DateTime.Now.Date.ToShortDateString().Replace('/', '_') + ".txt";
        public Service1()
        {
            InitializeComponent();
        }
        
        protected override void OnStart(string[] args)
        {
            WriteAllFileName(pathtowatch,fnpath);
            timer.Elapsed += new ElapsedEventHandler(OnElapsedTime);
            fileSystemWatcher.IncludeSubdirectories=true;
            fileSystemWatcher.Created += FileSystemWatcher_Created;
            //fileSystemWatcher.Changed += FileSystemWatcher_Changed;
            fileSystemWatcher.Deleted += FileSystemWatcher_Deleted;
            fileSystemWatcher.Renamed += FileSystemWatcher_Renamed;

            fileSystemWatcher.Path = @"C:\Users\PV\Desktop\Test_Log";

            fileSystemWatcher.EnableRaisingEvents = true;

            timer.Interval = 5000;  
            timer.Enabled = true;
        }

        protected override void OnStop()
        {

        }
        private void OnElapsedTime(object source, ElapsedEventArgs e)
        {

        }
        protected override void OnShutdown() { 
        }
        public void WriteToFile(string Message)
        {
            string path = @"C:\Users\PV\Desktop\Test_Log" + "\\Logs";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filepath = @"C:\Users\PV\Desktop\Test_Log" + "\\Logs\\ServiceLog_" + DateTime.Now.Date.ToShortDateString().Replace('/', '_') + ".txt";
            string filepathByte = @"C:\Users\PV\Desktop\Test_Log" + "\\Logs\\ServiceLogBytes_" + DateTime.Now.Date.ToShortDateString().Replace('/', '_') + ".txt";
            if (!File.Exists(filepath))
            {
                using (StreamWriter sw = File.CreateText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
            if (!File.Exists(filepathByte))
            {
                using (StreamWriter sw = File.CreateText(filepathByte))
                {
                    sw.Write(TextToBytes(Message));
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(filepathByte))
                {
                    sw.Write(TextToBytes(Message));
                }
            }
        }
       
        private void FileSystemWatcher_Renamed(object sender, RenamedEventArgs e)
        {           
            WriteToFile($"A file has been renamed from {e.OldName} to {e.Name} at: " + DateTime.Now);
            WriteAllFileName(pathtowatch, fnpath);
        }

        private void FileSystemWatcher_Deleted(object sender, FileSystemEventArgs e)
        {
            WriteToFile($"A file has been deleted - {e.Name} at: " + DateTime.Now);
            WriteAllFileName(pathtowatch, fnpath);
        }

        //private void FileSystemWatcher_Changed(object sender, FileSystemEventArgs e)
        //{
        //    WriteToFile($"A file has been changed - {e.Name} at: " + DateTime.Now);
        //}

        
        private void FileSystemWatcher_Created(object sender, FileSystemEventArgs e)
        {
            WriteToFile($"A new file has been created - {e.Name} at: " + DateTime.Now);
            WriteAllFileName(pathtowatch, fnpath);

            if (e.ChangeType == WatcherChangeTypes.Created)
            {
                if (Directory.Exists(e.FullPath))
                {
                    foreach (string file in Directory.GetFiles(e.FullPath))
                    {
                        var eventArgs = new FileSystemEventArgs(
                            WatcherChangeTypes.Created,
                            Path.GetDirectoryName(file),
                            Path.GetFileName(file));
                        FileSystemWatcher_Created(sender, eventArgs);
                    }
                    foreach (string directory in Directory.GetDirectories(e.FullPath)){
                        var eventArgs = new FileSystemEventArgs(
                            WatcherChangeTypes.Created,
                            Path.GetDirectoryName(directory),
                            Path.GetFileName(directory));
                        FileSystemWatcher_Created(sender, eventArgs);
                    };
                }
                else
                {
                    //WriteToFile($"A new file has been created - {e.Name} at: " + DateTime.Now);
                }
            }
            
        }
        
        private string TextToBytes(string message)
        {
            string s="";
            for(int i = 0; i < message.Length; i++)
            {
                int temp;
                temp = message[i];
                s = s+NumbertoBytes(temp);
            }
            return s;
        }
        private string NumbertoBytes(int number)
        {
            string s = "";
            int temp;
            while (number > 0)
            {
                temp = number % 2;
                s = temp.ToString() + s;
                number /= 2;
            }
            if (s.Length < 8)
            {
                int i = s.Length;
                while (i < 8)
                {
                    s = "0" + s;
                    i++;
                }
            }
            return s;
        }      
        public void WriteAllFileName(string despath,string fnpath)
        {
            string path = @"C:\Users\PV\Desktop\Test_Log" + "\\FinalStatus";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            if (File.Exists(fnpath))
            {
                File.Delete(fnpath);
            }

            string[] files = Directory.GetFiles(despath, "*", SearchOption.AllDirectories);
            foreach ( var file in files)
            {
                if (!File.Exists(fnpath))
                {
                    using (StreamWriter sw = File.CreateText(fnpath))
                    {
                        sw.WriteLine(file);
                    }
                }
                else
                {
                    using (StreamWriter sw = File.AppendText(fnpath))
                    {
                        sw.WriteLine(file);
                    }
                }
            }
        }
    }
}
