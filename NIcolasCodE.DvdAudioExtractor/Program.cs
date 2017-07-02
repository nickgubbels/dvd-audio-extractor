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
            foreach(var title in result)
            {
                for(var i = 1; i <= title.NumberOfChapters; i++)
                {
                    dvdService.ExtractToWav(title.TitleNo, i, $"C:\\Users\\nick\\desktop\\test\\{title.TitleNo}-{i}.wav");
                }
            }

#if DEBUG
            Console.WriteLine("Press enter to close...");
            Console.ReadLine();
#endif
        }
    }
}
