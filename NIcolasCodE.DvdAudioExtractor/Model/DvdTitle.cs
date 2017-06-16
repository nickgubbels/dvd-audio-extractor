using System.Collections.Generic;

namespace NIcolasCodE.DvdAudioExtractor.Model
{
    internal class DvdTitle
    {
        public DvdTitle(int titleNo, int numberOfChapters)
        {
            TitleNo = titleNo;
            NumberOfChapters = numberOfChapters;
        }

        public int TitleNo { get; private set; }
        public int NumberOfChapters { get; private set; }
    }
}
