using System;
using System.IO;
namespace FileFragmentationMVC.Models
{
    public class FileModel
    {
        private readonly string _dataFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "Data");
        public string InputFile => Path.Combine(_dataFolder, "input.txt");
        public string OutputFile => Path.Combine(_dataFolder, "output.txt");
        public string FragmentFolder => Path.Combine(_dataFolder, "Fragments");
        public FileModel()
        {
            if (!Directory.Exists(_dataFolder))
                Directory.CreateDirectory(_dataFolder);

            ClearDataFolder();
        }
        private void ClearDataFolder()
        {
            DirectoryInfo di = new DirectoryInfo(_dataFolder);

            foreach (FileInfo file in di.GetFiles())
                file.Delete();

            foreach (DirectoryInfo dir in di.GetDirectories())
                dir.Delete(true);
        }
        public void SaveParagraph(string paragraph)
        {
            File.WriteAllText(InputFile, paragraph);
        }
        public string[] FragmentFile(int fragmentSize)
        {
            if (!Directory.Exists(FragmentFolder))
                Directory.CreateDirectory(FragmentFolder);
            string content = File.ReadAllText(InputFile);
            int totalFragments = (int)Math.Ceiling((double)content.Length / fragmentSize);
            int digits = totalFragments.ToString().Length;
            string[] fragmentFiles = new string[totalFragments];
            for (int i = 0; i < totalFragments; i++)
            {
                int start = i * fragmentSize;
                int length = Math.Min(fragmentSize, content.Length - start);
                string fragmentContent = content.Substring(start, length);
                string fragmentFile = Path.Combine(FragmentFolder, (i + 1).ToString().PadLeft(digits, '0') + ".txt");
                File.WriteAllText(fragmentFile, fragmentContent);
                fragmentFiles[i] = fragmentFile;
            }

            return fragmentFiles;
        }
        public void Reassemble(string[] fragmentFiles)
        {
            string reassembledData = "";
            foreach (var file in fragmentFiles)
            {
                reassembledData += File.ReadAllText(file);
            }
            File.WriteAllText(OutputFile, reassembledData);
        }
        public bool CompareFiles()
        {
            string originalData = File.ReadAllText(InputFile);
            string finalData = File.ReadAllText(OutputFile);
            return originalData == finalData;
        }
        public string ReadFragment(string fileName)
        {
            if (File.Exists(fileName))
                return File.ReadAllText(fileName);
            return null;
        }
    }
}
