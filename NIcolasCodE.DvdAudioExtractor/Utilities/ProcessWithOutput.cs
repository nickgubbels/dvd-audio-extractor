using System.Diagnostics;
using System.Text;

namespace NIcolasCodE.DvdAudioExtractor.Utilities
{
    internal class ProcessWithOutput
    {
        public string ExecuteProcess(string fileName, string arguments)
        {
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = fileName,
                    Arguments = arguments,
                    RedirectStandardOutput = true,
                    UseShellExecute = false
                }
            };
            process.Start();

            var resultBuilder = new StringBuilder();
            while (!process.HasExited)
            {
                resultBuilder.Append(process.StandardOutput.ReadToEnd());
            }

            return resultBuilder.ToString();
        }
    }
}
