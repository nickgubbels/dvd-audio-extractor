using Microsoft.Extensions.DependencyInjection;
using NIcolasCodE.DvdAudioExtractor.Services;
using System;

namespace NIcolasCodE.DvdAudioExtractor
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
                .AddScoped<IOpticalDriverService, OpticalDriverService>()
                .BuildServiceProvider();

            var opticalDriveLetterRetriever = serviceProvider.GetService<IOpticalDriverService>();
            Console.WriteLine(opticalDriveLetterRetriever.RetrieveOpticalDriveLetter());

#if DEBUG
            Console.WriteLine("Press enter to close...");
            Console.ReadLine();
#endif
        }
    }
}
