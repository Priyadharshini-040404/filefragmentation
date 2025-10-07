using System;
using System.IO;
using FileFragmentationMVC.Models;
using FileFragmentationMVC.Views;
namespace FileFragmentationMVC.Controllers
{
    public class FileController
    {
        private readonly FileModel _model;
        private readonly ConsoleView _view;

        public FileController(FileModel model, ConsoleView view)
        {
            _model = model;
            _view = view;
        }
        public void Run()
        {
            _view.ShowMessage("Enter your paragraph (type 'end' on a new line to finish):");
            string paragraph = "";
            string line;
            while ((line = Console.ReadLine()) != null && line.ToUpper() != "END")
            {
                paragraph += line + "\n";
            }
            _model.SaveParagraph(paragraph.TrimEnd('\n'));
            _view.ShowMessage($"\nParagraph saved to {_model.InputFile}");
            int fragmentSize;
            while (true)
            {
                string input = _view.GetInput("Enter the number of characters per fragment:");
                if (int.TryParse(input, out fragmentSize) && fragmentSize > 0) break;
                _view.ShowMessage("Please enter a valid positive number.");
            }

            string[] fragments = _model.FragmentFile(fragmentSize);
            string[] fragmentNames = new string[fragments.Length];
            for (int i = 0; i < fragments.Length; i++)
                fragmentNames[i] = Path.GetFileName(fragments[i]);

            _view.ShowMessage("\nCreated Fragment Files in folder: " + _model.FragmentFolder);
            foreach (var name in fragmentNames)
                _view.ShowMessage(name);
            while (true)
            {
                string fileName = _view.GetInput("\nEnter the fragment file name to display its content (or type 'Next' to finish):");
                if (fileName.ToUpper() == "NEXT") break;
                string fullPath = Path.Combine(_model.FragmentFolder, fileName);
                string data = _model.ReadFragment(fullPath);
                if (data != null && Array.Exists(fragmentNames, f => f == fileName))
                    _view.DisplayContent($"Content of {fileName}", data);
                else
                    _view.ShowMessage("File does not exist or is not a valid fragment.");
            }
            string response = _view.GetInput("\nDo you want to reassemble all fragments and create output.txt? (yes/no)").Trim().ToLower();
            if (response == "yes")
            {
                _model.Reassemble(fragments);
                string reassembledData = File.ReadAllText(_model.OutputFile);
                _view.DisplayContent($"All fragments combined into {_model.OutputFile}", reassembledData);
                bool isMatch = _model.CompareFiles();
                if (isMatch)
                    _view.ShowColoredMessage("Success: Input file and output file are identical!", ConsoleColor.Green);
                else
                    _view.ShowColoredMessage("Error: Something went wrong, files do not match.", ConsoleColor.Red);
            }
            else
            {
                _view.ShowMessage("Reassembly skipped.");
            }
        }
    }
}
