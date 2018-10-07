using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SSMScripter.Runner;

namespace SSMScripter.Config
{
    public partial class RunnerConfigForm : Form
    {
        private RunConfig _runConfig;


        public RunnerConfigForm()
        {
            InitializeComponent();
        }


        public RunnerConfigForm(RunConfig runConfig)
            : this()
        {
            _runConfig = runConfig;
            tbTool.Text = _runConfig.IsUndefined() ? String.Empty : (_runConfig.RunTool ?? String.Empty);
            tbArgs.Text = _runConfig.RunArgs ?? String.Empty;
        }


        private void btnOk_Click(object sender, EventArgs e)
        {
            _runConfig.RunTool = tbTool.Text;
            _runConfig.RunArgs = tbArgs.Text;
            DialogResult = DialogResult.OK;
        }


        private void btnTool_Click(object sender, EventArgs e)
        {
            using(OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.InitialDirectory = Path.GetDirectoryName(tbTool.Text) ?? String.Empty;
                dialog.FileName = Path.GetFileName(tbTool.Text) ?? String.Empty;
                dialog.Filter = "Exe files (*.exe)|*.exe|All files (*.*)|*.*";
                dialog.RestoreDirectory = true;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    tbTool.Text = dialog.FileName;
                }
            }
        }
    }
}
