using Microsoft.AspNetCore.Mvc;

namespace SampleApi.Controllers;

public static class ControllerNameExtensions
{
    public static string GetControllerName(this Type controller)
    {
        return controller.Name.Replace("Controller", "");
    }
}
