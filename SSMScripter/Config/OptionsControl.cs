using Microsoft.Win32;
using SSMScripter.Runner;
using SSMScripter.Scripter;
using System;
using System.Windows.Forms;

namespace SSMScripter.Config
{
    public partial class OptionsControl : UserControl
    {
        private IScripterConfigStorage _scripterConfigStorage;
        private ScripterConfig _scripterConfig;

        private IRunConfigStorage _runConfigStorage;
        private RunConfig _runConfig;


        public OptionsControl()
        {
            InitializeComponent();
            string registryMasterKey = Registry.CurrentUser.Name + "\\Software\\SSMScripter";
            _scripterConfigStorage = new ScripterConfigRegistryStorage(registryMasterKey);
            _runConfigStorage = new RunConfigRegistryStorage(registryMasterKey);
        }


        private void OptionsControl_Load(object sender, EventArgs e)
        {
            try
            {
                lblInfo.Text = String.Empty;

                _scripterConfig = _scripterConfigStorage.Load();
                cbScriptDatabaseContext.Checked = _scripterConfig.ScriptDatabaseContext;
                cbScriptDatabaseContext.CheckedChanged += cbScriptDatabaseContext_CheckedChanged;

                _runConfig = _runConfigStorage.Load();

                lblRunnerTool.Text = _runConfig.IsUndefined() || String.IsNullOrEmpty(_runConfig.RunTool) ?
                     "none" : _runConfig.RunTool;

                if (_runConfig.IsUndefined())
                    _runConfig.RunArgs = "$(Server) $(Database) $(User) $(Password)";
                lblRunnerArgs.Text = _runConfig.RunArgs;
            }
            catch (Exception ex)
            {
                cbScriptDatabaseContext.Enabled = false;
                lblInfo.Text = ex.Message;
            }
        }


        private void cbScriptDatabaseContext_CheckedChanged(object sender, EventArgs e)
        {
            _scripterConfig.ScriptDatabaseContext = cbScriptDatabaseContext.Checked;

            try
            {
                _scripterConfigStorage.Save(_scripterConfig);
            }
            catch (Exception ex)
            {
                lblInfo.Text = ex.Message;
            }
        }

        private void btnEditRunner_Click(object sender, EventArgs e)
        {
            using (RunnerConfigForm form = new RunnerConfigForm(_runConfig))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        _runConfigStorage.Save(_runConfig);
                        lblRunnerTool.Text = _runConfig.RunTool;
                        lblRunnerArgs.Text = _runConfig.RunArgs;
                    }
                    catch (Exception ex)
                    {
                        lblInfo.Text = ex.Message;
                    }
                }
            }
        }
    }
}
