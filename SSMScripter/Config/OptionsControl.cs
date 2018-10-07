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
        

        public OptionsControl()
        {
            InitializeComponent();
            string registryMasterKey = Registry.CurrentUser.Name + "\\Software\\SSMScripter";
            _scripterConfigStorage = new ScripterConfigRegistryStorage(registryMasterKey);
        }


        private void OptionsControl_Load(object sender, EventArgs e)
        {            
            try
            {
                lblInfo.Text = String.Empty;
                _scripterConfig = _scripterConfigStorage.Load();

                cbScriptDatabaseContext.Checked = _scripterConfig.ScriptDatabaseContext;
                cbScriptDatabaseContext.CheckedChanged += cbScriptDatabaseContext_CheckedChanged;
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
    }
}
