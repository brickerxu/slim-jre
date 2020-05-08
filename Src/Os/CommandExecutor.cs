using System.Diagnostics;
using slim_jre.Base;
using slim_jre.Utils;

namespace slim_jre.Os
{
    class CommandExecutor
    {
        public static string Execute(string command)
        {
            return Execute(null, command, false);
        }
        
        public static string Execute(string workDir, string command)
        {
            return Execute(workDir, command, false);
        }

        public static string Execute(string workDir, string command, bool showWindow)
        {
            Process process = new Process();
            if (StringUtils.isNotEmpty(workDir))
            {
                process.StartInfo.WorkingDirectory = workDir;
            }
            else
            {
                process.StartInfo.WorkingDirectory = Dir.tempDirPath;
            }

            
            //设置要启动的应用程序
            process.StartInfo.FileName = "cmd.exe";
            //是否使用操作系统shell启动
            process.StartInfo.UseShellExecute = false;
            // 接受来自调用程序的输入信息             
            process.StartInfo.RedirectStandardInput = true;
            //输出信息
            process.StartInfo.RedirectStandardOutput = true;
            // 输出错误
            process.StartInfo.RedirectStandardError = true;
            //不显示程序窗口
            process.StartInfo.CreateNoWindow = !showWindow;
            //启动程序
            process.Start();
            
            //向cmd窗口发送输入信息
            process.StandardInput.WriteLine(command + "&exit");
            
            process.StandardInput.AutoFlush = true;
            //获取输出信息
            string output = process.StandardOutput.ReadToEnd();
            //等待程序执行完退出进程
            process.WaitForExit();
            process.Close();
            return output;
        }
    }
}
