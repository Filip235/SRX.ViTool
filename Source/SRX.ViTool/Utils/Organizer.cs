//srgjanx

using System;
using System.IO;

namespace SRX.ViTool.Utils
{
    public class Organizer : IOrganizer
    {
        private string dir;
        private IOrganizerArgs args;

        public Organizer(string dir, IOrganizerArgs args)
        {
            this.dir = dir;
            this.args = args;
        }

        public void Organize(out int? filesProcessedCount)
        {
            CheckIfDirectoryExists();

            string[] files = Directory.GetFiles(dir);

            ProcessFiles(files);

            filesProcessedCount = files.Length;

            DeleteTempifEmpty();
        }

        private void ProcessFiles(string[] files)
        {
            if (files != null && files.Length > 0)
            {
                int length = files.Length;
                Console.WriteLine($"{length} files found!");
                for (int i = 0; i < length; i++)
                {
                    DateTime modDate = File.GetLastWriteTime(files[i]);
                    string folder = dir + "\\" + PrepareFolderName(modDate);
                    if (!Directory.Exists(folder))
                        Directory.CreateDirectory(folder);
                    FileInfo fi = new FileInfo(files[i]);
                    string newFile =
                        fi.Directory
                        + "\\"
                        + PrepareFolderName(modDate)
                        + "\\"
                        + GetFileNamePrefix(modDate)
                        + fi.Name;
                    File.Move(files[i], newFile);
                    Console.WriteLine($"[{i + 1}/{length}]   {fi.Name}");
                }
            }
        }

        private void CheckIfDirectoryExists()
        {
            if (!Directory.Exists(dir))
            {
                throw new DirectoryNotFoundException($"Directory \"{dir}\" not found.");
            }
        }

        /// <summary>
        /// Returns formatted folder name by given date time.
        /// </summary>
        private string PrepareFolderName(DateTime dt)
        {
            return dt.ToString("MM_yyyy");
        }

        private string GetFileNamePrefix(DateTime dt)
        {
            return args.DatePrefix
                ? dt.ToString("dd_MM_yyyy") + "_"
                : "";
        }

        private void DeleteTempifEmpty()
        {
            string tempDir = dir + "\\Temporary";
            if (Directory.Exists(tempDir))
            {
               string [] folder = Directory.GetFiles(tempDir);
                if(folder.Length == 0)
                {
                    Directory.Delete(tempDir);
                }
            }

        }

        

    }
}