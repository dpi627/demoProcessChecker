using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

class Program
{
    static void Main(string[] args)
    {
        string targetProcessName = args.Length > 0 ? args[0] : "TiWorker";
        string logFilePath = $"log_{targetProcessName}.log";
        int checkInterval = 10000; // 10 seconds

        Console.WriteLine($"目標程序名稱: {targetProcessName}");
        Console.WriteLine($"開始監控 {targetProcessName}.exe... 按 Ctrl+C 停止");
        
        while (true)
        {
            try
            {
                // 動態生成日誌檔案名稱（每小時一個檔案）
                string logFilePath = $"log_{targetProcessName}_{DateTime.Now:yyyy-MM-dd_HH}.txt";
                bool found = false;

                // 檢查所有運行中的程序
                foreach (var process in Process.GetProcesses())
                {
                    if (process.ProcessName.Equals(targetProcessName, StringComparison.OrdinalIgnoreCase))
                    {
                        found = true;
                        // 記錄日誌
                        string logMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {targetProcessName}.exe 正在運行 (PID: {process.Id})";
                        File.AppendAllText(logFilePath, logMessage + Environment.NewLine);
                        Console.WriteLine(logMessage);
                    }
                }

                // 如果未找到目標程序，記錄資訊（可選）
                if (!found)
                {
                    Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - 未找到 {targetProcessName}.exe");
                }
            }
            catch (Exception ex)
            {
                string logFilePath = $"log_{targetProcessName}_{DateTime.Now:yyyy-MM-dd_HH}.txt";
                string errorMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - 錯誤: {ex.Message}";
                File.AppendAllText(logFilePath, errorMessage + Environment.NewLine);
                Console.WriteLine(errorMessage);
            }
            
            Thread.Sleep(checkInterval);
        }
    }
}