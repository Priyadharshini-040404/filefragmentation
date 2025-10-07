using System;

namespace FileFragmentationMVC.Views
{
    public class ConsoleView
    {
        public void ShowMessage(string message)
        {
            Console.WriteLine(message);
        }

        public void ShowColoredMessage(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public string GetInput(string prompt)
        {
            Console.WriteLine(prompt);
            return Console.ReadLine();
        }

        public void DisplayFragmentFiles(string[] files)
        {
            Console.WriteLine("\nCreated Fragment Files:");
            foreach (var file in files)
            {
                Console.WriteLine(file);
            }
        }

        public void DisplayContent(string header, string content)
        {
            Console.WriteLine($"\n{header}:\n{content}");
        }
    }
}
