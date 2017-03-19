using System;


namespace Acr.Settings.Net.Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            Hello("AppSettings", new AppConfigSettings("Acr.Settings.Net.Tests.exe.config"));
            //Hello("AppSettings", new AppConfigSettings());
            Hello("Registry", new RegistrySettings("Acr.Settings", true));
            Console.ReadLine();
        }


        static void Hello(string providerName, ISettings provider)
        {
            var value = provider.Get("Hello", 0);
            Console.WriteLine($"Hello from {providerName} - value: {value}");
            value++;
            provider.Set("Hello", value);


            var app = provider.Bind<AppSettings>();
            Console.WriteLine($"Binding Blah - {app.Blah}");
            app.Blah++;
        }
    }
}
