using Avalonia;

namespace Sandbox
{
    public class Program
    {
        static void Main(string[] args)
        {
            AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .LogToTrace()
                .With(new Win32PlatformOptions { UseWgl = true })
                .StartWithClassicDesktopLifetime(args);
        }
    }
}
