using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using AGI.Ui.Plugins;
using AGI.Ui.Core;
using AGI.STKObjects;
using AGI.Ui.Application;
using System.Windows.Forms;
using System.Reflection;
using System.Drawing;
using AGI.STKUtil;

namespace Stk11.PlanetsToggle
{
    [Guid("692569be-0013-4f49-ae1b-b6c3e2a72a95")]
    [ProgId("PlanetsToggle_10")]
    [ClassInterface(ClassInterfaceType.None)]
    public class PlanetsTogglePlugin : IAgUiPlugin, IAgUiPluginCommandTarget, IAgUiPlugin2
    {
        private IAgUiPluginSite m_pSite;
        private AgStkObjectRoot m_root;
        internal IAgUiPluginSite Site { get { return m_pSite; } }

        #region IAgUiPlugin Members

        public void OnDisplayConfigurationPage(IAgUiPluginConfigurationPageBuilder ConfigPageBuilder)
        {
           }

        public void OnDisplayContextMenu(IAgUiPluginMenuBuilder MenuBuilder)
        {
           
        }

        public void OnInitializeToolbar(IAgUiPluginToolbarBuilder ToolbarBuilder)
        {
            //converting an ico file to be used as the image for toolbat button
            stdole.IPictureDisp picture, picture2;
            
            picture = (stdole.IPictureDisp)Microsoft.VisualBasic.Compatibility.VB6.Support.ImageToIPicture(Stk11.PlanetsToggle.Properties.Resources.addPlanets);
            picture2 = (stdole.IPictureDisp)Microsoft.VisualBasic.Compatibility.VB6.Support.ImageToIPicture(Stk11.PlanetsToggle.Properties.Resources.zoomSolarSystem);

            //Add a Toolbar Button
            ToolbarBuilder.AddButton("AGI.PlanetsToggleCSharpPlugin.PlanetsToggle", "Toggle Plents", "Toggle Plents", AgEToolBarButtonOptions.eToolBarButtonOptionAlwaysOn, picture);
            ToolbarBuilder.AddButton("AGI.PlanetsToggleCSharpPlugin.SetPlanetView", "Set Planetary View", "Set Planetary View", AgEToolBarButtonOptions.eToolBarButtonOptionAlwaysOn, picture2);
        }

        public void OnShutdown()
        {
            m_pSite = null;
        }

        public void OnStartup(IAgUiPluginSite PluginSite)
        {
            m_pSite = PluginSite;
            //Get the AgStkObjectRoot
            IAgUiApplication AgUiApp = m_pSite.Application;
            m_root = AgUiApp.Personality2 as AgStkObjectRoot;
            m_root.Isolate();
            StkPlanetToggler.StkRoot = m_root;
            StkPlanetToggler.PlanetsShown = false;
        }

        #endregion

        #region IAgUiPlugin2 Members

        public void OnDisplayMenu(string MenuTitle, AgEUiPluginMenuBarKind MenuBarKind, IAgUiPluginMenuBuilder2 MenuBuilder)
        {
        }

        #endregion

        #region IAgUiPluginCommandTarget Members



        public void Exec(string CommandName, IAgProgressTrackCancel TrackCancel, IAgUiPluginCommandParameters Parameters)
        {
            //Controls what a command does
            if (string.Compare(CommandName, "AGI.PlanetsToggleCSharpPlugin.SetPlanetView", true) == 0)
            {
                StkPlanetToggler.SetPlanetView();
            }
            if (string.Compare(CommandName, "AGI.PlanetsToggleCSharpPlugin.PlanetsToggle", true) == 0 ) 
            {
                StkPlanetToggler.TogglePlanetsView();
            }
        }


        public AgEUiPluginCommandState QueryState(string CommandName)
        {
            //Enable commands
            if (string.Compare(CommandName, "AGI.PlanetsToggleCSharpPlugin.PlanetsToggle", true) == 0 || string.Compare(CommandName, "AGI.PlanetsToggleCSharpPlugin.SetPlanetView", true) == 0)
            {
                return AgEUiPluginCommandState.eUiPluginCommandStateEnabled | AgEUiPluginCommandState.eUiPluginCommandStateSupported;
            }
            return AgEUiPluginCommandState.eUiPluginCommandStateNone;
        }

        #endregion



        internal AgStkObjectRoot STKRoot
        {
            get { return m_root; }
        }


    }
}
