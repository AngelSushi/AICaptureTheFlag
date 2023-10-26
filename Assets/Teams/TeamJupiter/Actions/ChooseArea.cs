using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BehaviorDesigner.Runtime.Tasks;
using DoNotModify;
using Jupîter;
using UnityEngine;

namespace  Jupiter
{
    
    [TaskCategory("Jupiter")]
    public class ChooseArea : Action
    {

        private JupiterController _controller;

        public override void OnAwake()
        {
            _controller = JupiterController.Instance;
            base.OnAwake();
        }

        public override TaskStatus OnUpdate()
        {

           Cluster cluster = _controller.AllClusters.OrderBy(area => area.Score)
               .Where(area => area.Waypoints.Any(waypoint => waypoint.Owner != _controller.Owner))
               .ToList()[0];

           _controller.TargetCluster = cluster;
           Debug.Log("change area ( indexof : " + _controller.AllClusters.IndexOf(cluster) + " )");

           _controller.ClusterWaypointHeuristics.Clear();
           
           foreach (WayPointView waypoint in cluster.Waypoints)
           {
               WaypointHeuristic waypointHeuristic = new WaypointHeuristic(waypoint);
               _controller.ClusterWaypointHeuristics.Add(waypointHeuristic);
           }

           return TaskStatus.Success;
        }
    }    
}

