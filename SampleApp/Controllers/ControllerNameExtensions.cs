using Microsoft.AspNetCore.Mvc;

namespace SampleApp.Controllers;

public static class ControllerNameExtensions
{
    public static string GetControllerName(this Type controller)
    {
        return controller.Name.Replace("Controller", "");
    }
}
