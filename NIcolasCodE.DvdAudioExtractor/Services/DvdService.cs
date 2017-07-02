using NIcolasCodE.DvdAudioExtractor.External;
using NIcolasCodE.DvdAudioExtractor.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace NIcolasCodE.DvdAudioExtractor.Services
{
    internal class DvdService : IDvdService
    {
        private MPlayerWrapper mPlayerWrapper;

        public DvdService(IOpticalDriveService opticalDriveService)
        {
            mPlayerWrapper = new MPlayerWrapper(opticalDriveService);
        }

        public List<DvdTitle> RetrieveTitlesAndChapters()
        {
            var mPlayerResult = mPlayerWrapper.IdentifyDvd();

            return ParseMPlayerResult(mPlayerResult);
        }

        public void ExtractToWav(int titleNo, int chapterNo, string filePath)
        {
            mPlayerWrapper.ExtractAudioToWav(titleNo, chapterNo, new FileInfo(filePath));
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
    }
}
