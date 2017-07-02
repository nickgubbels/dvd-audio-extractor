using System.Collections.Generic;
using NIcolasCodE.DvdAudioExtractor.Model;

namespace NIcolasCodE.DvdAudioExtractor.Services
{
    internal interface IDvdService
    {
        List<DvdTitle> RetrieveTitlesAndChapters();

        void ExtractToWav(int titleNo, int chapterNo, string filePath);
    }
}