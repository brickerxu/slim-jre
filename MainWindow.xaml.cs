using Microsoft.Win32;
using slim_jre.Base;
using slim_jre.Service;
using slim_jre.Utils;
using System.IO;
using System.Windows;

namespace slim_jre
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
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
            pathTextBox.Text = fileName;
        }

        private void StartClick(object sender, RoutedEventArgs e)
        {
            string path = pathTextBox.Text;
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
            MainService mainService = new MainService();
            mainService.StartWork(path);
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
