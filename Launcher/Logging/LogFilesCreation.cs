using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Logging
{
    public class LogFilesCreation
    {
        private readonly string _logFilePath;

        public LogFilesCreation(string logFilePath)
        {
            _logFilePath = logFilePath;
        }

        public void CreateLogDirectoryIfNotExist()
        {
            if (!Directory.Exists(_logFilePath))
            {
                Directory.CreateDirectory(_logFilePath);
            }
        }

        private static void CreateFileIfNotExist(string filePath, out StreamWriter sw)
        {
            if (!File.Exists(filePath))
            {
                sw = File.CreateText(filePath);
                sw.Close();
            }
            sw = File.AppendText(filePath);
        }

        public void CreateServiceStatusLog(string fullPath, string fileName)
        {
            CreateLogDirectoryIfNotExist();

            var filePath = Path.Combine(_logFilePath, "launcherServiceStatus_" +
                                                          DateTime.Now.ToString("yyyyMMdd") + ".txt");
            CreateFileIfNotExist(filePath, out var sw);

            using (sw)
            {
                sw.WriteLine("File Created with Name: " + fileName + " at this location: " + fullPath + " at " + DateTime.Now);
                sw.Close();
            }
        }

        public void CreateParameterLog(string parameters)
        {
            CreateLogDirectoryIfNotExist();

            var filePath = Path.Combine(_logFilePath, "createdFileParameters_" +
                                                      DateTime.Now.ToString("yyyyMMdd") + ".txt");
            CreateFileIfNotExist(filePath, out var sw);

            using (sw)
            {
                sw.WriteLine("All file (created at " + DateTime.Now + ") parameters are: " + parameters);
                sw.Close();
            }
        }

        public void CreateErrorLog(string pathToFileWithErrors)
        {
            CreateLogDirectoryIfNotExist();

            var filePath = Path.Combine(_logFilePath, "fileWithErrors_" +
                                        DateTime.Now.ToString("yyyyMMdd") + ".txt");
            CreateFileIfNotExist(filePath, out var sw);
            
            using (sw)
            {
                sw.WriteLine("Error occured while processing file. START.ini file added to directory for files with errors at: " + DateTime.Now);
                sw.Close();
            }

            var errorsPerDay = File.ReadLines(filePath).Count();

            MoveFilesWithErrors(errorsPerDay, pathToFileWithErrors);
        }

        public void CreateServiceStoppedLog()
        {
            CreateLogDirectoryIfNotExist();

            var filePath = Path.Combine(_logFilePath, "serviceStopped_" + DateTime.Now.ToString("yyyyMMdd") + ".txt");

            CreateFileIfNotExist(filePath, out var sw);
           
            using (sw)
            {
                sw.Write("\r\n\n");
                sw.WriteLine("Service Stopped at: " + DateTime.Now.ToString("dd-MM-yyyy H:mm:ss"));
                sw.Close();
            }
        }

        private void MoveFilesWithErrors(int errorFileCounter, string pathToFile)
        {
            var filesWithError = Path.Combine(_logFilePath, "FilesWithErrors");
            if (!Directory.Exists(filesWithError))
            {
                Directory.CreateDirectory(filesWithError);
            }

            var parser = new IniFileParser.IniFileParser();
            var oldData = parser.ReadFile(pathToFile);
            var newFilePath = Path.Combine(filesWithError, "fileWithError" + errorFileCounter + "_at" + DateTime.Now.ToString("yyyyMMdd") + ".txt");
            if (!File.Exists(newFilePath))
            {
                File.Create(newFilePath);
            }

            //TODO! add an appropriate method to write INI file ----------> parser.WriteFile(newFilePath, oldData);
                
            File.Delete(pathToFile);
        }
    }
}
