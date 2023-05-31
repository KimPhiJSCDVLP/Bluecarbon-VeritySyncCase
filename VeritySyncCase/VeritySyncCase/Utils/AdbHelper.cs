using System.Diagnostics;
using Windows.Devices.Enumeration;
using Windows.Storage;

namespace VeritySyncCase.Utils
{
    public static class AdbHelper
    {
        public static string GetFilePathInDocuments(string fileName, string deviceName)
        {
            // Set the ADB command to list the file path in the documents folder on the specific device
            //string adbCommand = $"-s {deviceName} shell find /sdcard/Documents -name {fileName}";
            string adbCommand = $"shell find /sdcard/Documents -name {fileName}";
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

            // Remove any line breaks or whitespace characters from the output
            output = output.Trim();

            // Check if the file path exists
            if (string.IsNullOrEmpty(output))
            {
                throw new FileNotFoundException($"File '{fileName}' not found in the documents folder on the device '{deviceName}'.");
            }

            return output;
        }

        public static void CopyFileToDevice(string sourceFilePath, string destinationFolderPath)
        {
            try
            {
                if (!File.Exists(sourceFilePath))
                {
                    Console.WriteLine("Source file does not exist.");
                    return;
                }

                if (!Directory.Exists(destinationFolderPath))
                {
                    Directory.CreateDirectory(destinationFolderPath);
                }

                string fileName = Path.GetFileName(sourceFilePath);

                string destinationFilePath = Path.Combine(destinationFolderPath, fileName);

                File.Copy(sourceFilePath, destinationFilePath, true);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error copying file: " + ex.Message);
            }
        }
    }
}
