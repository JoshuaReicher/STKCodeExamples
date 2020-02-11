using System;
using System.Collections.Generic;
using System.Text;
using AGI.Ui.Plugins;
using AGI.STKObjects;
using AGI.STKUtil;
using System.Threading;

using System.Configuration;

using System.Windows.Forms;

namespace Agi.Ui.GreatArc.Stk11
{
    class GreatArcUpdate
    {
        public string InstallDir { get; }

        public AgStkObjectRoot Root { get; }

        public GreatArcUpdate(AgStkObjectRoot m_root)
        {
            Root = m_root;
            IAgExecCmdResult result = m_root.ExecuteCommand("GetDirectory / STKHome");
            InstallDir = result[0];

        }

        public enum WaypointProperty
        {
            Speed,
            Altitude,
            TurnRadius,
            Latitude,
            Longitude
        }

        public void UpdateWaypoint(IAgStkObject greatArcVehicle, 
            WaypointProperty waypointProp, double waypointParameter, string parameterUnit)
        {
            IAgVePropagatorGreatArc route = null;

            switch (parameterUnit)
            {
                case "mph":
                    Root.UnitPreferences.SetCurrentUnit("Distance", "mi");
                    Root.UnitPreferences.SetCurrentUnit("Time", "hr");
                    break;
                case "km/sec":
                    Root.UnitPreferences.SetCurrentUnit("Distance", "km");
                    Root.UnitPreferences.SetCurrentUnit("Time", "sec");
                    break;
                case "knots":
                    Root.UnitPreferences.SetCurrentUnit("Distance", "nm");
                    Root.UnitPreferences.SetCurrentUnit("Time", "hr");
                    break;
                case "m":
                    Root.UnitPreferences.SetCurrentUnit("Distance", "m");                    
                    break;
                case "km":
                    Root.UnitPreferences.SetCurrentUnit("Distance", "km");                    
                    break;
                case "ft":
                    Root.UnitPreferences.SetCurrentUnit("Distance", "ft");                    
                    break;
                case "deg":
                    Root.UnitPreferences.SetCurrentUnit("Angle", "deg");
                    break;
                case "rad":
                    Root.UnitPreferences.SetCurrentUnit("Angle", "rad");
                    break;                
                default:
                    break;
            }


            switch (greatArcVehicle.ClassType)
            {
                case AgESTKObjectType.eAircraft:
                    if (((IAgAircraft)greatArcVehicle).RouteType == AgEVePropagatorType.ePropagatorGreatArc)
                    {
                        route = ((IAgAircraft)greatArcVehicle).Route as IAgVePropagatorGreatArc;
                    }
                    break;
                case AgESTKObjectType.eGroundVehicle:
                    if (((IAgGroundVehicle)greatArcVehicle).RouteType == AgEVePropagatorType.ePropagatorGreatArc)
                    {
                        route = ((IAgGroundVehicle)greatArcVehicle).Route as IAgVePropagatorGreatArc;
                    }
                    
                    break;
                case AgESTKObjectType.eShip:
                    if (((IAgShip)greatArcVehicle).RouteType == AgEVePropagatorType.ePropagatorGreatArc)
                    {
                        route = ((IAgShip)greatArcVehicle).Route as IAgVePropagatorGreatArc;
                    }
                    
                    break;
                    
                default:
                    break;
            }

            if (route != null)
            {
                foreach (IAgVeWaypointsElement waypoint in route.Waypoints)
                {
                    switch (waypointProp)
                    {
                        case WaypointProperty.Speed:
                            waypoint.Speed = waypointParameter;
                            break;
                        case WaypointProperty.Altitude:
                            waypoint.Altitude = waypointParameter;
                            break;
                        case WaypointProperty.TurnRadius:
                            waypoint.TurnRadius = waypointParameter;
                            break;
                        case WaypointProperty.Latitude:
                            waypoint.Latitude = (double)waypoint.Latitude + waypointParameter;
                            break;
                        case WaypointProperty.Longitude:
                            waypoint.Longitude= (double)waypoint.Longitude+ waypointParameter;
                            break;
                        default:
                            break;
                    }
                }
                route.Propagate();
                Root.UnitPreferences.ResetUnits();
            }

        }      


    }
}
