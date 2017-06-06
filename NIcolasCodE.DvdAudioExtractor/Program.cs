using NIcolasCodE.DvdAudioExtractor.Utilities;
using System;

namespace NIcolasCodE.DvdAudioExtractor
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var opticalDriveLetterRetriever = new OpticalDriverLetterRetriever();
            Console.WriteLine(opticalDriveLetterRetriever.RetrieveOpticalDriveLetter());

#if DEBUG
            Console.WriteLine("Press enter to close...");
            Console.ReadLine();
#endif
        }
    }
}
