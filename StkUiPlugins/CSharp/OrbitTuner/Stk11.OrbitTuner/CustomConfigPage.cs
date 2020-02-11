using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using AGI.Ui.Plugins;
using AGI.STKObjects;

namespace OrbitTunerUiPlugin
{
    public partial class CustomConfigPage : UserControl, IAgUiPluginConfigurationPageActions2
    {
        public CustomConfigPage()
        {
            InitializeComponent();
        }
        
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            //Enable apply button
            if (_site != null)
                _site.SetModified(true);
        }

        #region Members

            IAgUiPluginConfigurationPageSite _site;
            CSharpPlugin _plugin;

        #endregion

            #region IAgUiPluginConfigurationPageActions2 Members

            public bool OnApply()
            {
                _plugin.Integrate = checkBox1.Checked;
                return true;
            }

            public void OnCancel()
            {
                // Intentionally left empty
            }

            public void OnCreated(IAgUiPluginConfigurationPageSite Site)
            {
                _site = Site;
                _plugin = _site.Plugin as CSharpPlugin;
                checkBox1.Checked = _plugin.Integrate;
            }

            public void OnOK()
            {
                _plugin.Integrate = checkBox1.Checked;
            }

            public void OnHelp()
            {
                IAgUiPluginSite2 site2 = (_site.Plugin as CSharpPlugin).Site as IAgUiPluginSite2;
                site2.ShowHelp( @"C:\Program Files (x86)\AGI\STK 11\Help\stk.chm", "", AgEUiPluginHelpDisplayType.eUiPluginHelpTOC);
            }
            #endregion
    }
}
