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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Extensions.Configuration;
using Microsoft.Win32;
using Ookii.Dialogs.Wpf;

namespace VisualStudioOfflineDownloadScriptsGenerator
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            btnSelectVSConfigFile.Click += BtnSelectVSConfigFile_Click;
            btnSelectBootFile.Click += BtnSelectBootFile_Click;
            btnSelectSaveDir.Click += BtnSelectSaveDir_Click;
            btnGenerate.Click += BtnGenerate_Click;
        }

        private void BtnSelectVSConfigFile_Click(Object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog()
            {
                Filter = "vsconfig|*.vsconfig",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)
            };

            if (ofd.ShowDialog() == true && !String.IsNullOrWhiteSpace(ofd.FileName))
            {
                txtVSConfigFile.Text = ofd.FileName;
            }
        }

        private void BtnSelectBootFile_Click(Object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog()
            {
                Filter = "EXE|*.exe",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)
            };

            if (ofd.ShowDialog() == true && !String.IsNullOrWhiteSpace(ofd.FileName))
            {
                txtBootFile.Text = ofd.FileName;
            }
        }

        private void BtnSelectSaveDir_Click(Object sender, RoutedEventArgs e)
        {
            var ofd = new VistaFolderBrowserDialog()
            {
                RootFolder = Environment.SpecialFolder.DesktopDirectory,
                ShowNewFolderButton = true
            };

            if (ofd.ShowDialog() == true && !String.IsNullOrWhiteSpace(ofd.SelectedPath))
            {
                txtSaveDir.Text = ofd.SelectedPath;
            }
        }

        private void BtnGenerate_Click(Object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(txtVSConfigFile.Text) || !File.Exists(txtVSConfigFile.Text))
            {
                MsgBox("请选择.vsconfig文件！", MessageBoxImage.Warning);
                return;
            }

            if (String.IsNullOrWhiteSpace(txtBootFile.Text) || !File.Exists(txtBootFile.Text))
            {
                MsgBox("请选择正确的引导文件！", MessageBoxImage.Warning);
                return;
            }

            if (String.IsNullOrWhiteSpace(txtSaveDir.Text) || !Directory.Exists(txtSaveDir.Text))
            {
                MsgBox("请选择已存在的下载目录！", MessageBoxImage.Warning);
                return;
            }

            var sb = new StringBuilder();
            sb.Append(txtBootFile.Text);
            sb.Append($" --layout {txtSaveDir.Text}");

            var cb = new ConfigurationBuilder();
            using var fs = File.OpenRead(txtVSConfigFile.Text);
            cb.AddJsonStream(fs);
            var config = cb.Build();
            var components = config.GetSection("components").GetChildren();
            foreach (var component in components)
            {
                sb.Append($" -add {component.Value}");
            }
            sb.Append($" --includeOptional --lang zh-CN");

            var script = sb.ToString();
            var scriptWindow = new ScriptsWindow() { Owner = this };
            scriptWindow.SetDownloadScript(script);
            scriptWindow.ShowDialog();
        }

        private static MessageBoxResult MsgBox(String content, MessageBoxImage icon)
        {
            return MessageBox.Show(content, "系统提示", MessageBoxButton.OK, icon);
        }
    }
}
