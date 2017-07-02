using System.Diagnostics;
using System.Text;

namespace NIcolasCodE.DvdAudioExtractor.Utilities
{
    internal class ProcessWithOutput
    {
        public static string ExecuteProcess(string fileName, string arguments)
        {
            return ExecuteProcess(fileName, arguments, null, out string dummy);
        }

        public static string ExecuteProcess(string fileName, string arguments, out string stdError)
        {
            return ExecuteProcess(fileName, arguments, null, out stdError);
        }

        public static string ExecuteProcess(string fileName, string arguments, string workingDirectory)
        {
            return ExecuteProcess(fileName, arguments, workingDirectory, out string dummy);
        }

        public static string ExecuteProcess(string fileName, string arguments, string workingDirectory, out string stdError)
        {
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = fileName,
                    Arguments = arguments,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false
                }
            };

            if(!string.IsNullOrWhiteSpace(workingDirectory))
            {
                process.StartInfo.WorkingDirectory = workingDirectory;
            }

            process.Start();

            var resultBuilder = new StringBuilder();
            var errorBuilder = new StringBuilder();

            while (!process.HasExited)
            {
                resultBuilder.Append(process.StandardOutput.ReadToEnd());
                errorBuilder.Append(process.StandardError.ReadToEnd());
            }

            stdError = errorBuilder.ToString();

            return resultBuilder.ToString();
        }
    }
}
