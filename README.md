Dichotomy
=========

This is a super lightweight .NET library that allows a console application to be installed and ran as a Windows Service. This starting version has a minimalistic feature set, with only a few short steps for getting started. Inspiration for usability taken from [RavenDB](https://github.com/ravendb/ravendb)'s server command line user experience.

Some of the benefits of this library:

- Very simple to use
- Easy to customize
- Installer automatically elevates permissions (no more installutil or admin command prompts!)
- Makes your application more versatile

The NuGet package can be found [here](https://nuget.org/packages/Dichotomy).

####Recipe Ideas

You can mix Dichotomy with lots of exciting projects, like:

- NancyFx
- ASP.NET Web API (self host)
- WCF Services
- Much more!

###What does this package do to my project?

When you install the NuGet package, you will notice that a new file has been added to your project named `ConsoleApplicationInstaller.cs`. This file contains an installer class used to interact with the install functionality:

	[RunInstaller(true)]
    public class ConsoleApplicationInstaller : CustomInstaller
    {
        
    }

Rather than drop all of the boilerplate code into this class, the Dichotomy library includes a basic happy-path implementation. If you need to customize how the application is installed as a service, simply change your inheritance from `CustomInstaller` to `System.Configuration.Install.Installer` and go from there.

By installing this package, a reference to `System.Configuration.Install` will also be added to your project.

###Okay, it's installed. What next?

After installing the package, you need to write a class to encapsulate the core code of your program in a service. All you need to do is implement `IDichotomyService`:

    public class YourCustomService : IDichotomyService
    {
        public void Start()
        {
            Console.WriteLine("Starting");
        }

        public void Stop()
        {
            Console.WriteLine("Stopping");
        }

        public void Dispose()
        {
            Console.WriteLine("Disposing");
        }
    }

Then, after you have your service all you need to do is add these two lines to your console application:

    var runner = new Runner(new YourCustomService());
    runner.Run();

Now that everything is all set up, you can invoke your application like this:

	yourapp.exe -help

The application will then print out something like this:

    Command Line Arguments:
        -install    Installs as a Windows Service and starts immediately.
        -uninstall  Stops the Windows Service, and uninstalls it.
        -start      Starts the Windows Service.
        -stop       Stops the Windows Service.
        -restart    Restarts the Windows Service.
        -help       Displays information about available command line flags.
        -?          Displays information about available command line flags.

If this is what you get, then you are all set to go. As you can see, these are the base flags that are included in the library. To change the command line argument convention, you can set the leading string in a configuration object:

    var config = new ConfigurationOptions("--");
    var runner = new Runner(new YourCustomService(), config);
    runner.Run();

If you want to let Dichotomy give you a hand with some basic command line argument support, you can do so:

    var config = new ConfigurationOptions();
    config.AddCommandlineFlag(new Flag
    {
        Name = "hello-world",
        Action = () => Console.WriteLine("Hello, world!"),
        Description = "Greet the world."
    });
    var runner = new Runner(new Service(), config);
    runner.Run();
