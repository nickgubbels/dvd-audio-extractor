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
                .AddScoped<IOpticalDriveService, OpticalDriveService>()
                .AddScoped<IDvdService, DvdService>()
                .BuildServiceProvider();

            var dvdService = serviceProvider.GetService<IDvdService>();

            var result = dvdService.RetrieveTitlesAndChapters();

#if DEBUG
            Console.WriteLine("Press enter to close...");
            Console.ReadLine();
#endif
        }
    }
}
