using NIcolasCodE.DvdAudioExtractor.Services;
using NIcolasCodE.DvdAudioExtractor.Utilities;
using System;
using System.IO;

namespace NIcolasCodE.DvdAudioExtractor.External
{
    internal class MPlayerWrapper
    {
        private const string mPlayerPath = "C:\\Users\\Nick\\Desktop\\MPlayer-x86_64-r37905+g1f5630a\\MPlayer.exe";

        private IOpticalDriveService opticalDriveService;

        private string identifyCache;

        public MPlayerWrapper(IOpticalDriveService opticalDriveService)
        {
            this.opticalDriveService = opticalDriveService;
        }

        public string IdentifyDvd()
        {
            if(String.IsNullOrWhiteSpace(identifyCache))
            {
                identifyCache = ProcessWithOutput.ExecuteProcess(mPlayerPath, $"-identify dvd:// -dvd-device {opticalDriveService.RetrieveOpticalDriveLetter()}: -frames 0");
            }

            return identifyCache;
        }

        public void ExtractAudioToWav(int titleNo, int chapterNo, FileInfo targetFile)
        {
            var arguments = $"-vo null -vc null -ao pcm:fast:file={targetFile.Name} dvd://{titleNo} -chapter {chapterNo}-{chapterNo} -dvd-device {opticalDriveService.RetrieveOpticalDriveLetter()}:";

            ProcessWithOutput.ExecuteProcess(mPlayerPath, arguments, targetFile.Directory.FullName, out string stdError);
        }
    }
}
