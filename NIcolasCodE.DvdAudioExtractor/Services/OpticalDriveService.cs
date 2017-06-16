using NIcolasCodE.DvdAudioExtractor.Utilities;
using System.Text.RegularExpressions;

namespace NIcolasCodE.DvdAudioExtractor.Services
{
    internal class OpticalDriveService : IOpticalDriveService
    {
        public char? RetrieveOpticalDriveLetter()
        {
            //TODO: Use the following priority: 1) command-line argument, 2) app.config setting, 3) WMI command-line result
            return RetrieveOpticalDriveLetterUsingWindowsManagementInstrumentationCommandLine();
        }

        private enum WmiDeviceTypes
        {
            Unknown = 0,
            NoRootDirectory = 1,
            RemovableDisk = 2,
            LocalDisk = 3,
            NetworkDrive = 4,
            CompactDisc = 5,
            RAMDisk = 6
        }

        private static char? RetrieveOpticalDriveLetterUsingWindowsManagementInstrumentationCommandLine()
        {
            var wmicResult = DoWmicCall();

            var regex = new Regex("([A-Z]):\\s*(\\d)");

            foreach (Match match in regex.Matches(wmicResult))
            {
                if (match.Groups.Count == 3)
                {
                    if (int.TryParse(match.Groups[2].Value, out int driveType)
                        && driveType == (int)WmiDeviceTypes.CompactDisc
                        && match.Groups[1].Value.Length == 1)
                    {
                        return match.Groups[1].ToString()[0];
                    }
                }
            }

            return null;
        }

        private static string DoWmicCall()
        {
            return ProcessWithOutput.ExecuteProcess("wmic.exe", "logicaldisk get deviceid, drivetype");
        }
    }
}
