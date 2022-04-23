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
        
        public string LoggingPath {get; set; }
        
        private List<DebugLogEntitySpawn> _loggedEntitySpawns   = new List<DebugLogEntitySpawn>();
        private List<DebugLogServerData>  _loggedServerMessages = new List<DebugLogServerData>();

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
            LoggingPath = Path.Combine(documentDirPath, "My Games", "TrafficSimulation");
        }
        
        public void LogServerMessage(DebugLogServerData data)
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
                    if (File.Exists(Path.Combine(LoggingPath, "LatestEntityLog.Json")))
                    {
//                      delete the log which predates the previous log
                        File.Delete(Path.Combine(LoggingPath, "PrevEntityLog.Json"));
//                      and put the previous log into the Prev position
                        File.Move(Path.Combine(LoggingPath, "LatestEntityLog.Json"), Path.Combine(LoggingPath, "PrevEntityLog.Json"));
                    }
//                  same as above but for the serverLogs
                    if (File.Exists(Path.Combine(LoggingPath, "LatestServerLog.Json")))
                    {
                        File.Delete(Path.Combine(LoggingPath, "PrevServerLog.Json"));
                        File.Move(Path.Combine(LoggingPath, "LatestServerLog.Json"), Path.Combine(LoggingPath, "PrevServerLog.Json"));
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
            File.Create(Path.Combine(LoggingPath, serverFileName)).Close();
            File.Create(Path.Combine(LoggingPath, entityFileName)).Close();
            
            using (StreamWriter sw = new StreamWriter(Path.Combine(LoggingPath, serverFileName)))
            {
                sw.WriteLine(JsonSerializer.Serialize(_loggedServerMessages));
            }
            using (StreamWriter sw = new StreamWriter(Path.Combine(LoggingPath, entityFileName)))
            {
                sw.WriteLine(JsonSerializer.Serialize(_loggedEntitySpawns));
            }
        }

        public bool DoesALogExist()
        {
            if (File.Exists(Path.Combine(LoggingPath, "LatestEntityLog.Json")) && File.Exists(Path.Combine(LoggingPath, "LatestServerLog.Json")))
            {
                return true;
            }
            return false;
        }
    }
}