using AGI.STKObjects;
using AGI.STKUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stk11.PlanetsToggle
{
    public static class StkPlanetToggler
    {

        public static AgStkObjectRoot StkRoot;

        public static bool PlanetsShown;

        public static string[] Planets = { "Mercury", "Venus", "Earth", "Mars", "Saturn", "Jupiter", "Uranus", "Neptune", "Pluto" };

        public static void CreatePlanet(string planetName)
        {
            StkRoot.ExecuteCommand("New / Planet " + planetName);
            StkRoot.ExecuteCommand("Define */Planet/" + planetName + " CentralBody " + planetName + " Analytic");
        }
        public static void SetViewVGT()
        {
            IAgExecCmdResult result = StkRoot.ExecuteCommand("VectorTool_R * Exists \"CentralBody/Sun SunView Point\"");
            if (result[0].CompareTo("1") != 0)
            {
                StkRoot.ExecuteCommand("VectorTool * CentralBody/Sun Create Point SunView \"Fixed in System\" Cartesian 1e13 1e13 1e13 \"CentralBody/Sun J2000\"");
            }
            StkRoot.ExecuteCommand("VO * ViewFromTo Normal From \"CentralBody/Sun SunView Point\" to CentralBody/Sun");

        }

        public static void SetViewCameraPath()
        {
            StkRoot.ExecuteCommand("VO * CameraControl SetAllPaths Off");
            try
            {
                StkRoot.ExecuteCommand("VO * CameraControl CameraPath Add Name \"SolarSystemView\"");
                StkRoot.ExecuteCommand("VO * CameraControl KeyframeProps \"SolarSystemView\" ReferenceAxes \"CentralBody/Sun J2000 Axes\"");
                StkRoot.ExecuteCommand("VO * CameraControl Keyframes \"SolarSystemView\" Add");
                StkRoot.ExecuteCommand("VO * CameraControl Keyframes \"SolarSystemView\" Modify 1 Position 1e13 1e13 1e13");
            }
            catch { }
            StkRoot.ExecuteCommand("VO * CameraControl Follow \"SolarSystemView\" SoftTransition Yes");
            StkRoot.ExecuteCommand("VO * CameraControl 3DWindowProps FollowMode On");

        }
        public static void SetPlanetView()
        {
            if (PlanetsShown)
            {
                SetViewCameraPath();
            }
            else
            {
                StkRoot.ExecuteCommand("VO * View Home");
            }
        }
        public static void TogglePlanetsView()
        {
            if (PlanetsShown)
            {
                StkRoot.ExecuteCommand("Window3D * ViewVolume MaxVisibleDist 1e11");
                foreach (IAgStkObject planetObj in StkRoot.CurrentScenario.Children.GetElements(AgESTKObjectType.ePlanet))
                {
                    planetObj.Unload();
                }
                PlanetsShown = false;

            }
            else
            {
                StkRoot.ExecuteCommand("Graphics * GlobalAttributes ShowPlanetOrbits On ShowPlanetCbiPos On ShowPlanetCbiLabel On ShowPlanetGroundPos Off ShowPlanetGroundLabel Off");

                foreach (string planet in Planets)
                {
                    CreatePlanet(planet);
                }
                StkRoot.ExecuteCommand("Window3D * ViewVolume MaxVisibleDist 1e14");
                //StkRoot.ExecuteCommand("VO * Celestial Stars ShowTx On File \"" + @"C:\Program Files (x86)\AGI\Imagery\AGI Celestial Imagery\mwpan2.ctm" + "\"");

                PlanetsShown = true;

            }
        }
    }
}
