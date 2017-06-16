using NIcolasCodE.DvdAudioExtractor.Model;
using NIcolasCodE.DvdAudioExtractor.Utilities;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace NIcolasCodE.DvdAudioExtractor.Services
{
    internal class DvdService : IDvdService
    {
        private MPlayerDvdIdentifier mPlayerDvdIdentifier;

        public DvdService(IOpticalDriveService opticalDriveService)
        {
            mPlayerDvdIdentifier = new MPlayerDvdIdentifier(opticalDriveService);
        }

        public List<DvdTitle> RetrieveTitlesAndChapters()
        {
            var mPlayerResult = mPlayerDvdIdentifier.IdentifyDvd();

            return ParseMPlayerResult(mPlayerResult);
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
                string path = "C:\\Users\\Nick\\Desktop\\MPlayer-x86_64-r37905+g1f5630a\\MPlayer.exe";
                return ProcessWithOutput.ExecuteProcess(path, $"-identify dvd:// -dvd-device {opticalDriveService.RetrieveOpticalDriveLetter()}: -frames 0");
            }
        }
    }
}
