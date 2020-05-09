using System;
using System.Diagnostics;
using slim_jre.Base;
using slim_jre.Os.Interfaces;
using slim_jre.Utils;

namespace slim_jre.Os
{
    class CommandExecutor
    {
        public static void Execute(string command, ICommandReceiver receiver)
        {
            Execute(null, command, false, receiver);
        }

        public static string Execute(string workDir, string command)
        {
            DefaultCommandReceiver receiver = new DefaultCommandReceiver();
            Execute(workDir, command, false, receiver);
            return receiver.GetOutput();
        }

        public static void Execute(string workDir, string command, bool showWindow, ICommandReceiver receiver)
        {
            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            if (StringUtils.isEmpty(workDir))
            {
                workDir = Dir.tempDirPath;
            }
            startInfo.WorkingDirectory = workDir;
            startInfo.FileName = "cmd.exe";//设定需要执行的命令  
            startInfo.Arguments = "";//“/C”表示执行完命令后马上退出  
            startInfo.UseShellExecute = false;//不使用系统外壳程序启动  
            startInfo.RedirectStandardInput = true;//不重定向输入  
            startInfo.RedirectStandardOutput = true; //重定向输出  
            startInfo.CreateNoWindow = !showWindow;//不创建窗口

            process.StartInfo = startInfo;
            if (process.Start()) //开始进程  
            {
                process.StandardOutput.ReadLine().Trim();
                process.StandardOutput.ReadLine().Trim();
                if (command.Length > 0)
                {
                    process.StandardInput.WriteLine(command);
                    process.StandardOutput.ReadLine().Trim();

                    process.StandardInput.WriteLine("\n");
                    string log = process.StandardOutput.ReadLine().Trim();
                    if (null != receiver)
                    {
                        receiver.Command(log);
                    }

                    string path = workDir + ">";
                    do
                    {
                        if (null != receiver && receiver.IsCancelled())
                        {
                            break;
                        }

                        string logm = process.StandardOutput.ReadLine().Trim();
                        if (path.Equals(logm))
                        {
                            break;
                        }

                        if (null != receiver)
                        {
                            receiver.Output(logm + CommonConstant.EOF);
                        }
                    } while (true);
                }

                process.Close();
            }
        }
    }
}