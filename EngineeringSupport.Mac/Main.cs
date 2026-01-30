using ObjCRuntime;
using UIKit;
using Velopack;

namespace EngineeringSupport.Mac;

public class Program
{
    // This is the main entry point of the application.
    static void Main(string[] args)
    {
        VelopackApp.Build()
            .Run();

        // if you want to use a different Application Delegate class from "AppDelegate"
        // you can specify it here.
        UIApplication.Main(args, null, typeof(AppDelegate));
    }
}