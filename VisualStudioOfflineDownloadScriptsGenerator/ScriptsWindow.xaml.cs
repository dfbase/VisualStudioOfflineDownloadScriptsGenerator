using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace VisualStudioOfflineDownloadScriptsGenerator
{
    public partial class ScriptsWindow : Window
    {
        public ScriptsWindow()
        {
            InitializeComponent();
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
