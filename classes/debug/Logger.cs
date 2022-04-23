using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.Json;
using traffic_light_simulation.classes.dataClasses;


namespace traffic_light_simulation.classes.debug
{
    public class Logger
    {
        private static Logger _instance;
        private static readonly object Padlock = new object();
        private Logger() {}
        public static Logger Instance
        {
            get
            {
                lock (Padlock)
                {
                    if (_instance == null)
                    {
                        _instance = new Logger();
                    }
                    return _instance;
                }
            }
        }
        
        private string _loggingPath;
        
        private List<DebugLogEntitySpawn> _loggedEntitySpawns   = new List<DebugLogEntitySpawn>();
        private List<DebugServerData>     _loggedServerMessages = new List<DebugServerData>();

        public void SetUp()
        {
//          creates debug storage dirs (My games and My games/TrafficSimulation)
            string documentDirPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (!File.Exists(Path.Combine(documentDirPath, "My Games")))
            {
                Directory.CreateDirectory(Path.Combine(documentDirPath, "My Games"));
            }

            if (!File.Exists(Path.Combine(documentDirPath, "My Games", "TrafficSimulation")))
            {
                Directory.CreateDirectory(Path.Combine(documentDirPath, "My Games", "TrafficSimulation"));
            }
            _loggingPath = Path.Combine(documentDirPath, "My Games", "TrafficSimulation");
        }
        
        public void LogServerMessage(DebugServerData data)
        {
            _loggedServerMessages.Add(data);
        }

        public void LogEntitySpawn(DebugLogEntitySpawn data)
        {
            _loggedEntitySpawns.Add(data);
        }

        public void CreateLogs()
        {
            if (DebugManager.Instance.Logging)
            {
                try
                {
//                  if we already have a log
                    if (File.Exists(Path.Combine(_loggingPath, "LatestEntityLog.Json")))
                    {
//                      delete the log which predates the previous log
                        File.Delete(Path.Combine(_loggingPath, "PrevEntityLog.Json"));
//                      and put the previous log into the Prev position
                        File.Move(Path.Combine(_loggingPath, "LatestEntityLog.Json"), Path.Combine(_loggingPath, "PrevEntityLog.Json"));
                    }
//                  same as above but for the serverLogs
                    if (File.Exists(Path.Combine(_loggingPath, "LatestServerLog.Json")))
                    {
                        File.Delete(Path.Combine(_loggingPath, "PrevServerLog.Json"));
                        File.Move(Path.Combine(_loggingPath, "LatestServerLog.Json"), Path.Combine(_loggingPath, "PrevServerLog.Json"));
                    }

                    _createLoggingFiles("LatestServerLog.Json","LatestEntityLog.Json");
                }
//              the code will end up here if one of the files was either opened/being used by another program
//              we create a panic log from here these logs cannot be directly used by the replay system
                catch (Exception e)
                {
                    _createLoggingFiles(DateTime.Now.ToString(CultureInfo.InvariantCulture) + "PanicLatestServerLog.Json", 
                        DateTime.Now.ToString(CultureInfo.InvariantCulture) + "PanicLatestEntityLog.Json");
                
                }   
            }
        }

        private void _createLoggingFiles(string serverFileName, string entityFileName)
        {
            File.Create(Path.Combine(_loggingPath, serverFileName)).Close();
            File.Create(Path.Combine(_loggingPath, entityFileName)).Close();
            
            using (StreamWriter sw = new StreamWriter(Path.Combine(_loggingPath, serverFileName)))
            {
                sw.WriteLine(JsonSerializer.Serialize(_loggedServerMessages));
            }
            using (StreamWriter sw = new StreamWriter(Path.Combine(_loggingPath, entityFileName)))
            {
                sw.WriteLine(JsonSerializer.Serialize(_loggedEntitySpawns));
            }
        }
    }
}