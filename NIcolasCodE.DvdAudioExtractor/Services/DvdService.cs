using NIcolasCodE.DvdAudioExtractor.Model;
using NIcolasCodE.DvdAudioExtractor.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace NIcolasCodE.DvdAudioExtractor.Services
{
    internal class DvdService : IDvdService
    {
        private MPlayerDvdIdentifier mPlayerDvdIdentifier;
        private MPlayerDvdAudioExtractor mPlayerDvdAudioExtractor;

        public DvdService(IOpticalDriveService opticalDriveService)
        {
            mPlayerDvdIdentifier = new MPlayerDvdIdentifier(opticalDriveService);
            mPlayerDvdAudioExtractor = new MPlayerDvdAudioExtractor(opticalDriveService);
        }

        public List<DvdTitle> RetrieveTitlesAndChapters()
        {
            var mPlayerResult = mPlayerDvdIdentifier.IdentifyDvd();

            return ParseMPlayerResult(mPlayerResult);
        }

        public void ExtractToWav(int titleNo, int chapterNo, string filePath)
        {
            mPlayerDvdAudioExtractor.ExtractAudioToWav(titleNo, chapterNo, filePath);
        }

        private static List<DvdTitle> ParseMPlayerResult(string mplayerResult)
        {
            var result = new List<DvdTitle>();

            var regex = new Regex("ID_DVD_TITLE_(\\d+)_CHAPTERS=(\\d+)");
            foreach(Match match in regex.Matches(mplayerResult))
            {
                if(match.Groups.Count == 3)
                {
                    if (!Int32.TryParse(match.Groups[1].ToString(), out int titleNr))
                    {
                        continue;
                    }

                    if (!Int32.TryParse(match.Groups[2].ToString(), out int numberOfChapters))
                    {
                        continue;
                    }

                    result.Add(new DvdTitle(titleNr, numberOfChapters));
                }
            }

            return result;
        }

        private class MPlayerDvdIdentifier
        {
            private string cache;
            private IOpticalDriveService opticalDriveService;

            public MPlayerDvdIdentifier(IOpticalDriveService opticalDriveService)
            {
                this.opticalDriveService = opticalDriveService;
            }

            public string IdentifyDvd()
            {
                if(String.IsNullOrWhiteSpace(cache))
                {
                    cache = DoMPlayerCall();
                }

                return cache;
            }

            private string DoMPlayerCall()
            {
                var path = "C:\\Users\\Nick\\Desktop\\MPlayer-x86_64-r37905+g1f5630a\\MPlayer.exe";
                return ProcessWithOutput.ExecuteProcess(path, $"-identify dvd:// -dvd-device {opticalDriveService.RetrieveOpticalDriveLetter()}: -frames 0");
            }
        }

        private class MPlayerDvdAudioExtractor
        {
            //vlc.exe -I dummy --no-sout-video --sout-audio --no-sout-rtp-sap --no-sout-standard-sap --ttl=1 --sout-keep --sout "#transcode{acodec=s16l}:std{access=file,mux=wav,dst=C:\Users\Nick\Desktop\test1.wav}" dvd:///F:\#1:8-1:8 vlc://quit
            private IOpticalDriveService opticalDriveService;

            public MPlayerDvdAudioExtractor(IOpticalDriveService opticalDriveService)
            {
                this.opticalDriveService = opticalDriveService;
            }

            public void ExtractAudioToWav(int titleNo, int chapterNo, string fileLocation)
            {
                var fileInfo = new FileInfo(fileLocation);

                var path = "C:\\Users\\Nick\\Desktop\\MPlayer-x86_64-r37905+g1f5630a\\MPlayer.exe";
                var arguments = $"-vo null -vc null -ao pcm:fast:file={fileInfo.Name} dvd://{titleNo} -chapter {chapterNo}-{chapterNo} -dvd-device {opticalDriveService.RetrieveOpticalDriveLetter()}:";

                var output = ProcessWithOutput.ExecuteProcess(path, arguments, fileInfo.Directory.FullName, out string stdError);
            }
        }
    }
}
