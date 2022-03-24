using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MiscTools.Objects;
using Newtonsoft.Json;

namespace MiscTools
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello! Welcome to Reiner's tools! Please select a function:");
            Console.WriteLine("Enter 1  - Generate log with json file.");
            Console.WriteLine("Enter 2  - Read from log file and generate formatted log file.");

            string choice = Console.ReadLine();

            if (!string.IsNullOrEmpty(choice))
            {
                if (int.TryParse(choice.Trim(), out int choiceNum))
                {
                    switch (choiceNum)
                    {
                        case 1:
                            GenerateLogFile();
                            break;
                        case 2:
                            ReadAndGenerate();
                            break;
                        default:
                            InvalidChoice();
                            break;
                    }
                }
                else
                    InvalidChoice();
            }
            else
                InvalidChoice();
        }

        private static void InvalidChoice()
        {
            Console.WriteLine("You did not pick a valid choice. Please re-run the app.");
            Console.ReadLine();
        }

        private static string GetFilePath(string fileToOpen)
        {
            string path = System.Reflection.Assembly.GetExecutingAssembly().Location;
            var directory = Path.GetDirectoryName(path);
            var fullpath = directory + "/" + fileToOpen;

            return fullpath;
        }

        private static void GenerateLogFile()
        {
            string message = "The quick brown fox jumps over the lazy dog.";
            var logEvent = new LogEvent(-100, LogSeverity.Info, message);

            for (int i = 0; i <= 48000; i++)
            {
                LogEventRecord record = new LogEventRecord
                {
                    Index = i,
                    TimeStamp = DateTime.Now,
                    Log = logEvent
                };

                string jsonLine = JsonConvert.SerializeObject(record) + ", ";

                string fullPath = GetFilePath("log.txt");

                if (!string.IsNullOrEmpty(jsonLine))
                    WriteToFile(jsonLine, fullPath, i);
            }

            Console.WriteLine("Done writing to file.");
            Console.ReadLine();
        }

        private static void WriteToFile(string content, string path, int index)
        {
            Console.WriteLine("Writing object with index: " + index.ToString() + " to file.");

            if (index == 0 && File.Exists(path))
                File.Delete(path);

            using (var fileStream = File.Open(path, FileMode.Append, FileAccess.Write))
            {
                using (var binaryWriter = new BinaryWriter(fileStream))
                {
                    binaryWriter.Write(content.ToArray());
                    binaryWriter.Flush();
                }
            }
        }

        private static void ReadAndGenerate()
        {
            string fullPath = GetFilePath("log.txt");

            if (File.Exists(fullPath))
            {
                string result;

                using (var fileStream = File.OpenRead(fullPath))
                {
                    using (var streamReader = new StreamReader(fileStream))
                    {
                        result = streamReader.ReadToEnd();
                    }
                }

                if (!string.IsNullOrEmpty(result))
                {
                    result = "[" + result + "]";

                    DateTime deserializeStart = DateTime.Now;
                    Console.WriteLine("Log file started deserializing at " + deserializeStart.ToString());
                    List<LogEventRecord> logs = JsonConvert.DeserializeObject<List<LogEventRecord>>(result);

                    if (logs != null)
                    {

                        DateTime deserializeEnd = DateTime.Now;
                        Console.WriteLine("Log file finished deserializing at " + deserializeEnd.ToString());

                        TimeSpan span = deserializeEnd - deserializeStart;

                        Console.WriteLine("Deserialization took " + span.TotalMilliseconds + "ms to complete for " + logs.Count.ToString() + " log items.");

                        //TODO: Write formatted log file

                        Console.WriteLine("Formatted log file generated.");

                    }
                    else
                        Console.WriteLine("Log file is invalid.");
                }
                else
                    Console.WriteLine("Log file is empty.");
            }
            else
                Console.WriteLine("Log file not found. Please generate one using option 1.");
            
            Console.ReadLine();
        }
    }
}
