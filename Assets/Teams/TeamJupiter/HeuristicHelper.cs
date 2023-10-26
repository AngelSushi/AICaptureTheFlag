using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DoNotModify;
using UnityEngine;

namespace Jupiter
{
    public class HeuristicHelper
    {

        public static int CalcuateWaypointHeuristic(JupiterController controller,WayPointView waypoint)
        {
            // Faire en sorte que si le score de temps est élever il skip les trucs avec une mine , si c elever il y va
       
            int waypointHeuristic = 0;

            if (waypoint.Owner == -1)
            {
                waypointHeuristic += 1;
            }
            else if (waypoint.Owner != controller.Owner)
            {
                waypointHeuristic += 2;
            }
            else
            {
                waypointHeuristic += 0;
            }

            Collider2D[] mineColliders = Physics2D.OverlapCircleAll(waypoint.Position,waypoint.Radius,1 << 13);

            if (mineColliders.Length > 0)
            {
                waypointHeuristic -= 1;
            }

            return waypointHeuristic;
        }

        public static int CalculateZoneHeuristic(JupiterController controller,Cluster cluster)
        {
            int allyWaypoint = cluster.Waypoints.Count(waypoint => waypoint.Owner != -1 && waypoint.Owner == controller.Owner);
            int enemyWaypoint = cluster.Waypoints.Count(waypoint => waypoint.Owner != -1 && waypoint.Owner != controller.Owner);
            int neutralWaypoint = cluster.Waypoints.Count(waypoint => waypoint.Owner == -1);

            float allyPoint = (float) controller.BehaviorTree.GetVariable("AllyZonePoints").GetValue();
            float enemyPoint = (float) controller.BehaviorTree.GetVariable("EnemyZonePoints").GetValue();
            float neutralPoint = (float)controller.BehaviorTree.GetVariable("NeutralZonePoints").GetValue();

            float distancePoint = Vector2.Distance(cluster.Center,controller.SpaceShip.Position) + CalculateDistanceInfluence(controller);

            return (int) (allyWaypoint * allyPoint + enemyWaypoint * enemyPoint + neutralWaypoint * neutralPoint);
        }

        private static float CalculateDistanceInfluence(JupiterController controller) => controller.TimeLeft / controller.MaxTime;
        

        
    }
}
