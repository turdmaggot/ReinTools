using System;
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

        private static void GenerateLogFile()
        {
            string path = System.Reflection.Assembly.GetExecutingAssembly().Location;
            var directory = System.IO.Path.GetDirectoryName(path);
            var fullpath = directory + "/log.txt";

            string message = "The quick brown fox jumps over the lazy dog.";
            var logEvent = new LogEvent(-100, LogSeverity.Info, message);


            for (int i = 0; i <= 48000; i++)
            {
                LogEventRecord record = new LogEventRecord
                {
                    Index = i,
                    Log = logEvent
                };

                string jsonLine = JsonConvert.SerializeObject(record) + ", ";

                if (!string.IsNullOrEmpty(jsonLine))
                    WriteToFile(jsonLine, fullpath, i);
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
    }
}
