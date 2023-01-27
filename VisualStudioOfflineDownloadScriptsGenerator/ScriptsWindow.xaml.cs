using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Ookii.Dialogs.Wpf;

namespace VisualStudioOfflineDownloadScriptsGenerator
{
    public partial class ScriptsWindow : Window
    {
        public ScriptsWindow()
        {
            InitializeComponent();

            btnExport.Click += BtnExport_Click;
        }

        private void BtnExport_Click(Object sender, RoutedEventArgs e)
        {
            var ofd = new VistaFolderBrowserDialog()
            {
                RootFolder = Environment.SpecialFolder.DesktopDirectory,
                ShowNewFolderButton = true
            };

            if (ofd.ShowDialog() == true && !String.IsNullOrWhiteSpace(ofd.SelectedPath))
            {
                File.WriteAllText(Path.Combine(ofd.SelectedPath, "下载脚本.txt"), txtDownloadScript.Text, Encoding.UTF8);
                File.WriteAllText(Path.Combine(ofd.SelectedPath, "校验脚本.txt"), txtVerifyScript.Text, Encoding.UTF8);

                MessageBox.Show("导出成功！", "系统提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public void SetDownloadScript(String script)
        {
            txtDownloadScript.Text = script;

            if (!String.IsNullOrWhiteSpace(script))
            {
                txtVerifyScript.Text = $"{script} --verify --fix";
            }
        }
    }
}
