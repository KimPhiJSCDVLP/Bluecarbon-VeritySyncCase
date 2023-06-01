#if WINDOWS
using SharpAdbClient;
using SharpAdbClient.Exceptions;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using VeritySyncCase.Constants;
using VeritySyncCase.Models;
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
        public static List<DeviceData> GetDevices()
        {
            var adbClient = new AdbClient();
            return adbClient.GetDevices();
        }

        public static async Task<string> PullFilesFromDevices(string deviceId, List<string> destinationFolderPaths)
        {
            var tasks = new List<Task<string>>();

            foreach (var destinationFolderPath in destinationFolderPaths)
            {
                tasks.Add(PullFilesFromDevice(deviceId, Constant.VERIRY_SOURCE_FOLDER, destinationFolderPath));
            }

            var results = await Task.WhenAll(tasks);
            return string.Join(Environment.NewLine, results);
        }

        private static async Task<string> PullFilesFromDevice(string deviceId, string sourceFolderPath, string destinationFolderPath)
        {
            var adbCommand = $"-s {deviceId} pull \"{sourceFolderPath}\" \"{destinationFolderPath}\"";

            var processInfo = new ProcessStartInfo
            {
                FileName = "adb",
                Arguments = adbCommand,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            var output = await ExecuteCommandAsync(processInfo);
            return $"Device ID: {deviceId}{Environment.NewLine}{output}";
        }

        private static async Task<string> ExecuteCommandAsync(ProcessStartInfo processInfo)
        {
            using (var process = new Process())
            {
                process.StartInfo = processInfo;

                var outputBuilder = new StringBuilder();
                var errorBuilder = new StringBuilder();

                process.OutputDataReceived += (sender, e) =>
                {
                    if (!string.IsNullOrEmpty(e.Data))
                        outputBuilder.AppendLine(e.Data);
                };

                process.ErrorDataReceived += (sender, e) =>
                {
                    if (!string.IsNullOrEmpty(e.Data))
                        errorBuilder.AppendLine(e.Data);
                };

                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                process.WaitForExit();

                var output = outputBuilder.ToString();
                var error = errorBuilder.ToString();

                return output + error;
            }
        }
    }
}
#endif