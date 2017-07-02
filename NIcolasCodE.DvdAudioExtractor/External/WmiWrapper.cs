using NIcolasCodE.DvdAudioExtractor.Utilities;
using System.Text.RegularExpressions;

namespace NIcolasCodE.DvdAudioExtractor.External
{
    internal class WmiWrapper
    {
        private char? driveLetterCache;

        public char? RetrieveOpticalDriveLetter()
        {
            if(!driveLetterCache.HasValue)
            {
                var wmicResult = ProcessWithOutput.ExecuteProcess("wmic.exe", "logicaldisk get deviceid, drivetype");

                var regex = new Regex("([A-Z]):\\s*(\\d)");

                foreach (Match match in regex.Matches(wmicResult))
                {
                    if (match.Groups.Count == 3)
                    {
                        if (int.TryParse(match.Groups[2].Value, out int driveType)
                            && driveType == (int)WmiDeviceTypes.CompactDisc
                            && match.Groups[1].Value.Length == 1)
                        {
                            driveLetterCache = match.Groups[1].ToString()[0];
                            break;
                        }
                    }
                }
            }

            return driveLetterCache;
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
    }
}
