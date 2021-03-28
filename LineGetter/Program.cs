using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Diagnostics;

namespace LineGetter
{
    class Program
    {
        static List<int> lineList = new List<int>();
        static List<string> filterOutExtensions = new List<string>();
        static List<string> filterInExtensions = new List<string>();
        

        static void Main(string[] args)
        {
            filterOutExtensions.Add(".jpg");
            filterOutExtensions.Add(".png");
            filterOutExtensions.Add(".jpeg");
            filterOutExtensions.Add(".packages");
            filterOutExtensions.Add(".flutter-plugins-dependencies");
            filterOutExtensions.Add("");

            ReadConsoleAndRedirect();
        }

        public static void ReadConsoleAndRedirect()
        {

            Console.Clear();
            Console.WriteLine("Starting new:");
            lineList = new List<int>();
            filterInExtensions = new List<string>();
            StartNew();
        }

        private static void StartNew()
        {
            Console.WriteLine("Specify file extensions : seperate with ',' .");
            Console.WriteLine("To use all extensions, just hit Enter.");
            string inp = Console.ReadLine();

            if (!String.IsNullOrEmpty(inp))
            {
                string[] arr = inp.Split(',');
                foreach (string ext in arr)
                {
                    filterInExtensions.Add(ext);
                }
            }

            Console.WriteLine("Input path to Directory or File to be read. . .");
            string path = Console.ReadLine();
            if (String.IsNullOrEmpty(path))
            {
                ReadConsoleAndRedirect();
                return;
            }

            string curr = Directory.GetCurrentDirectory();
            string final = Path.GetRelativePath(curr, path);

            bool isFile = File.Exists(final);
            bool isDirectory = Directory.Exists(final);


                lineList = new List<int>();
                Console.Clear();
                Console.WriteLine(" ");

                if (isFile)
                {
                    Console.WriteLine("**************************");
                    Console.WriteLine("***** Path is a File *****");
                    Console.WriteLine("**************************");
                    int num = ReadFile(final);
                    Console.WriteLine(num.ToString());
                }
                else if (isDirectory)
                {
                    Console.WriteLine("*******************************");
                    Console.WriteLine("***** Path is a Directory *****");
                    Console.WriteLine("*******************************");
                    ReadFilesInDirectory(final);
                }
                else
                {
                    Console.WriteLine("*****************************");
                    Console.WriteLine("***** Path is not valid *****");
                    Console.WriteLine("*****************************");
                    ReadConsoleAndRedirect();
                    return;
                }


                Console.ReadLine();
            ReadConsoleAndRedirect();




        }

        public static int ReadFile(string path)
        {
            FileInfo info = new FileInfo(path);

            if (filterOutExtensions.Contains(info.Extension))
            {
                return 0;
            }

            if(filterInExtensions.Count > 0)
            {
                if (!filterInExtensions.Contains(info.Extension))
                    return 0;
            }

            string[] lines = File.ReadAllLines(path);
            return lines.Count();
        }

        

        public static void ReadFilesInDirectory(string path)
        {
            string[] filePaths = Directory.GetFiles(path);
            List<int> linesInThis = new List<int>();
            foreach (var file in filePaths)
            {
                int lines = ReadFile(file);
                lineList.Add(lines);
                linesInThis.Add(lines);
            }

            Console.WriteLine(" ");
            Console.WriteLine("Reading Directory:");
            Console.WriteLine(path);   
            Console.WriteLine("THIS DIRECTORY:" + linesInThis.Sum());
            Console.WriteLine("TOTAL:" + lineList.Sum());
            Console.WriteLine("___________________");
            string[] directoryPaths = Directory.GetDirectories(path);

            foreach (string dir in directoryPaths)
            {
                ReadFilesInDirectory(dir);
            }
        }

    }

    
}
