using System.IO;
using System.Threading;
using System.Windows;
using Microsoft.Win32;
using slim_jre.Base;
using slim_jre.Service;
using slim_jre.Utils;

namespace slim_jre
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

        private string path;
        public MainWindow()
        {
            InitializeComponent();
            Adapter.dAppendText += AppendText;
        }

        private void ChooseJarClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.DefaultExt = ".jar";
            dialog.Filter = "Jar file|*.jar";
            if (dialog.ShowDialog() == false)
            {
                return;
            }
            string fileName = dialog.FileName;
            PathTextBox.Text = fileName;
            path = fileName;
        }

        private void StartClick(object sender, RoutedEventArgs e)
        {
            string path = PathTextBox.Text;
            if (StringUtils.isEmpty(path))
            {
                MessageBox.Show("请选择Jar包", "提示");
                return;
            }
            if (!CheckJdk()) 
            {
                // 无法找到本机的jdk目录
                // 后续增加手动选择或者设置功能
                MessageBox.Show("请检查是否设置jdk环境变量", "提示");
                return;
            }
            Console.Document.Blocks.Clear();
            Thread th = new Thread(new ThreadStart(StartWork)); //也可简写为new Thread(ThreadMethod);                
            th.Start();
            
        }

        private void StartWork()
        {
            MainService mainService = new MainService();
            mainService.StartWork(path);
        }


        private void AppendText(string text)
        {
            Dispatcher.Invoke(delegate
            {
                if (Console.Document.Blocks.Count > 2500)
                {
                    Console.Document.Blocks.Clear();
                }
                Console.AppendText(text);
                Console.ScrollToEnd();
            }); 
            Thread.Sleep(1);
        }

        private bool CheckJdk()
        {
            if (StringUtils.isEmpty(Dir.jreDirPath) || StringUtils.isEmpty(Dir.jarPath) || StringUtils.isEmpty(Dir.jdepsPath))
            {
                return false;
            }
            if (!Directory.Exists(Dir.jreDirPath) || !File.Exists(Dir.jarPath) || !File.Exists(Dir.jdepsPath))
            {
                return false;
            }
            return true;
        }
    }
}
