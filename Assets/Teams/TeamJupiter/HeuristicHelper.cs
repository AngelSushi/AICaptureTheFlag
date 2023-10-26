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
                waypointHeuristic += (int) ((float) (controller.MainTree.GetVariable("NeutralZonePoints").GetValue()));
            }
            else if (waypoint.Owner != controller.Owner)
            {
                waypointHeuristic += (int) ((float) (controller.MainTree.GetVariable("EnemyZonePoints").GetValue()));
            }

            Collider2D[] mineColliders = Physics2D.OverlapCircleAll(waypoint.Position,waypoint.Radius,1 << 13);

            if (mineColliders.Length > 0)
            {
                waypointHeuristic -= (int) ((float) controller.MainTree.GetVariable("MineDangerValue").GetValue() * mineColliders.Length);
            }

            return waypointHeuristic;
        }

        public static int CalculateZoneHeuristic(JupiterController controller,Cluster cluster)
        {
            int allyWaypoint = cluster.Waypoints.Count(waypoint => waypoint.Owner != -1 && waypoint.Owner == controller.Owner);
            int enemyWaypoint = cluster.Waypoints.Count(waypoint => waypoint.Owner != -1 && waypoint.Owner != controller.Owner);
            int neutralWaypoint = cluster.Waypoints.Count(waypoint => waypoint.Owner == -1);

            float allyPoint = (float) controller.MainTree.GetVariable("AllyZonePoints").GetValue();
            float enemyPoint = (float) controller.MainTree.GetVariable("EnemyZonePoints").GetValue();
            float neutralPoint = (float)controller.MainTree.GetVariable("NeutralZonePoints").GetValue();

            float distancePoint = Vector2.Distance(cluster.Center,controller.SpaceShip.Position) / CalculateDistanceInfluence(controller); 
            
            // Faire en sorte pour que plus le temps est faible ==> plus il va aller vers des zones proches 

            return (int) (allyWaypoint * allyPoint + enemyWaypoint * enemyPoint + neutralWaypoint * neutralPoint);
        }

        private static float CalculateDistanceInfluence(JupiterController controller) => controller.TimeLeft / controller.MaxTime;

        public static float CalculateClusterDangerHeuristic(JupiterController controller, Cluster cluster)
        {
            float clusterDangerHeuristic = 0;

            foreach (WayPointView waypoint in cluster.Waypoints)
            {
                Collider2D[] mineColliders = Physics2D.OverlapCircleAll(waypoint.Position,waypoint.Radius,1 << 13);

                if (mineColliders.Length > 0)
                {
                    clusterDangerHeuristic +=(float) controller.MainTree.GetVariable("MineDangerValue").GetValue() * mineColliders.Length;
                }
            }

            float maxClusterDistance = Vector2.Distance(cluster.Center, controller.OtherSpaceShip.Position);

            foreach (Cluster targetCluster in controller.AllClusters)
            {
                float targetDistance = Vector2.Distance(targetCluster.Center, controller.OtherSpaceShip.Position);
                if (targetDistance > maxClusterDistance)
                {
                    maxClusterDistance = targetDistance;
                }
            }
            
            float clusterToEnemyDistance = Vector2.Distance(cluster.Center, controller.OtherSpaceShip.Position);
            float distanceImpact = 1f - (clusterToEnemyDistance / maxClusterDistance);

            clusterDangerHeuristic -=  clusterDangerHeuristic * distanceImpact;
            
            // Plus le temps passe ==> moins le risque aura d'impact car l'ia s'en fout dans les dernieres secondes 
            

            return clusterDangerHeuristic;
        }

        
    }
}
