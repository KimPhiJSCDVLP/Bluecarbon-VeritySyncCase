using System.Diagnostics;
using Windows.Devices.Enumeration;
using Windows.Storage;

namespace VeritySyncCase.Utils
{

    public static class FileHelper
    {
        public static string ReadTextFile(string filePath)
        {
            // Read the contents of the file
            string fileContent = File.ReadAllText(filePath);
            return fileContent;
        }
        public static void CreateFolder(string path)
        {
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error create folder: " + ex.Message);
            }
        }
    }

    public static class AdbHelper
    {
        public static string GetFilePathInDocuments(string destinationFolderPath, string deviceId)
        {
            var sourcePath = "/sdcard/Documents";
            // Set the ADB command to list the file path in the documents folder on the specific device
            string adbCommand = $"-s {deviceId} pull {sourcePath} {destinationFolderPath}";
            // Create a new process start info for executing ADB
            ProcessStartInfo processStartInfo = new ProcessStartInfo
            {
                FileName = "adb",
                Arguments = adbCommand,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            // Start the process
            Process process = new Process();
            process.StartInfo = processStartInfo;
            process.Start();

            // Read the output of the command
            string output = process.StandardOutput.ReadToEnd();

            // Wait for the process to exit
            process.WaitForExit();

            return output;
        }
    }
}
