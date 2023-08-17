using MobileCommander.Const;
using MobileCommander.Interfaces;

using System.Diagnostics;
using System.Xml;

namespace MobileCommander.Services
{
    public class AdbService : IAdbService
    {
        public async Task KillAllBackgroundTasksAsync()
        {
            await ExecuteAdbCommandAsync(AdbCommandKeywords.KillBackgroundTasks);
        }

        private async Task ExecuteAdbCommandAsync(string command)
        {
            var processStartInfo = new ProcessStartInfo
            {
                FileName = AdbCommandKeywords.FileName,
                Arguments = command,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (var process = new Process { StartInfo = processStartInfo })
            {
                process.Start();
                await process.WaitForExitAsync();
            }
        }

        private async Task<string> ExecuteAdbCommandAndGetResponseAsync(string command)
        {
            var processStartInfo = new ProcessStartInfo
            {
                FileName = AdbCommandKeywords.FileName,
                Arguments = command,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (var process = new Process { StartInfo = processStartInfo })
            {
                process.Start();
                string output = await process.StandardOutput.ReadToEndAsync();
                await process.WaitForExitAsync();
                return output;
            }
        }

        public async Task<string> GetChromeSearchAsync(string searchQuery)
        {
            if (searchQuery != "my ip address")
            {
                await SearchQueryWithoutResponseAsync(searchQuery);

                return string.Empty;
            }

            var response = await SearchQueryWithResponseAsync(searchQuery);

            return response;
        }

        public async Task SearchQueryWithoutResponseAsync(string searchQuery)
        {
            await ExecuteAdbCommandAsync(AdbCommandKeywords.InitChrome);
            await Task.Delay(5000);
            await ExecuteAdbCommandAsync($"{string.Format(AdbCommandKeywords.SearchQueryForChrome, searchQuery)}");
            await ExecuteAdbCommandAsync(AdbCommandKeywords.Enter);
        }

        public async Task<string> SearchQueryWithResponseAsync(string searchQuery)
        {
            await ExecuteAdbCommandAsync(AdbCommandKeywords.InitChrome);
            await Task.Delay(5000);
            await ExecuteAdbCommandAsync($"{string.Format(AdbCommandKeywords.SearchQueryForChrome, searchQuery)}");
            await ExecuteAdbCommandAsync(AdbCommandKeywords.Enter);
            await Task.Delay(10000);
            await ExecuteAdbCommandAsync(AdbCommandKeywords.GetCurrentScreenAndSave);
            await Task.Delay(2000);
            var xml = await ExecuteAdbCommandAndGetResponseAsync(AdbCommandKeywords.DownloadScreen);
            var ipAddress = ExtractIpAddressFromXml(xml);

            return ipAddress;
        }

        private string ExtractIpAddressFromXml(string xmlDump)
        {
            using (var reader = XmlReader.Create(new StringReader(xmlDump)))
            {
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "node")
                    {
                        var text = reader.GetAttribute("text");
                        if (!string.IsNullOrEmpty(text) && text.Contains("IP Address"))
                        {
                            var index = text.IndexOf("IP Address");
                            return text[(index + "IP Address:".Length)..].Trim();
                        }
                    }
                }
            }

            return "IP address is not found";
        }
    }
}
