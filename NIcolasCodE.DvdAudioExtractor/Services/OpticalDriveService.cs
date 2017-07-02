using NIcolasCodE.DvdAudioExtractor.External;

namespace NIcolasCodE.DvdAudioExtractor.Services
{
    internal class OpticalDriveService : IOpticalDriveService
    {
        private WmiWrapper wmiWrapper;

        public OpticalDriveService()
        {
            wmiWrapper = new WmiWrapper();
        }

        public char? RetrieveOpticalDriveLetter()
        {
            //TODO: Use the following priority: 1) command-line argument, 2) app.config setting, 3) WMI command-line result
            return wmiWrapper.RetrieveOpticalDriveLetter();
        }
    }
}
