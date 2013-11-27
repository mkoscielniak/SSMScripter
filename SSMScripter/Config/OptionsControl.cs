using System;
using System.Windows.Forms;

namespace SSMScripter.Config
{
    public partial class OptionsControl : UserControl
    {
        private Options _options;
        

        public OptionsControl()
        {
            InitializeComponent();
        }


        private void OptionsControl_Load(object sender, System.EventArgs e)
        {            
            try
            {
                lblInfo.Text = String.Empty;
                _options = new Options();
                _options.Load();

                cbScriptDatabaseContext.Checked = _options.ScriptDatabaseContext;
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
            _options.ScriptDatabaseContext = cbScriptDatabaseContext.Checked;
            StoreOptions();
        }


        private void StoreOptions()
        {
            try
            {
                _options.Store();
            }
            catch (Exception ex)
            {
                lblInfo.Text = ex.Message;
                throw;
            }
        }
    }
}
