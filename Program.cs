using FileFragmentationMVC.Models;
using FileFragmentationMVC.Views;
using FileFragmentationMVC.Controllers;

class Program
{
    static void Main()
    {
        var model = new FileModel();
        var view = new ConsoleView();
        var controller = new FileController(model, view);
        controller.Run();
    }
}
