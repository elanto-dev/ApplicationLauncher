using System;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Threading;
using FileTransformerService;
using Logging;

namespace LauncherService
{
    public partial class FileSystemWatcher : ServiceBase
    {
        private readonly string _fileReadingPath = ConfigurationManager.AppSettings["MonitoringPath"];
        private readonly string _monitoringFileName = ConfigurationManager.AppSettings["MonitoringFileName"];
        private readonly string _logsPath = ConfigurationManager.AppSettings["LogsPath"];
        private readonly string _targetFilePath = ConfigurationManager.AppSettings["ExecutingPath"];
        private LogFilesCreation _logFilesCreationMethod; 

        public FileSystemWatcher()
        {
            _logFilesCreationMethod = new LogFilesCreation(_logsPath);
            InitializeComponent();
            fileSystemWatcher1.Created += fileSystemWatcher1_Created;
        }

        protected override void OnStart(string[] args)
        {
            fileSystemWatcher1.Path = _fileReadingPath;
        }

        protected override void OnStop()
        {
            _logFilesCreationMethod.CreateServiceStoppedLog();
        }

        public void fileSystemWatcher1_Created(object sender, FileSystemEventArgs e)
        {
            //Then we need to check file is exist or not which is created.
            if (!CheckFileExistence(_fileReadingPath, e.Name)) return;
            //Then write code for log detail of file in text file.
            _logFilesCreationMethod.CreateServiceStatusLog(_fileReadingPath, e.Name);
            var fullPath = Path.Combine(_fileReadingPath, e.Name);
            var fileTransformerService = new FileTransformer(fullPath);
            var studyInformation = fileTransformerService.FileReader();
            if (studyInformation == null)
            {
                _logFilesCreationMethod.CreateErrorLog(Path.Combine(_fileReadingPath, _monitoringFileName));
                return;
            }

            _logFilesCreationMethod.CreateParameterLog(studyInformation.ToParametersString);
        }

        private static bool CheckFileExistence(string fullPath, string fileName)
        {
            // Get the subdirectories for the specified directory.'
            var dir = new DirectoryInfo(fullPath);
            if (!dir.Exists) { return false; }
            var fileFullPath = Path.Combine(fullPath, fileName);
            return File.Exists(fileFullPath);
        }

        private void FileSystemWatcher1_Changed(object sender, FileSystemEventArgs e)
        {
        }

       /* private void CreateErrorTextFile()
        {
            if (!File.Exists(_errorFilePath + "\\errors_" +
                             DateTime.Now.ToString("yyyyMMdd") + ".txt"))
            {
                var sw = File.CreateText(_errorFilePath + "errors_" +
                                         DateTime.Now.ToString("yyyyMMdd") + ".txt");
                sw.Close();
            }

            var errorCounter = File.ReadLines(_errorFilePath).Count();
            MoveFilesWithErrors(errorCounter);

            using (var sw = File.AppendText(_errorFilePath + "\\errors_" +
                                            DateTime.Now.ToString("yyyyMMdd") + ".txt"))
            {
                sw.WriteLine("Error occured while processing the file at " + DateTime.Now
                                                                           + ". File with error added to \"Error\" directory in current location with name: " + "FileWithError" + errorCounter + ".ini");
                sw.Close();
            }
        }*/
    }
}
