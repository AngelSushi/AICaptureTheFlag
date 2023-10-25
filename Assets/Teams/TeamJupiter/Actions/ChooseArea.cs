using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BehaviorDesigner.Runtime.Tasks;
using DoNotModify;
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

           Area area = _controller.AllAreas.OrderBy(area => area.Score)
               .Where(area => area.Waypoints.Any(waypoint => waypoint.Owner != _controller.Owner))
               .ToList()[0];

           _controller.TargetArea = area;
           Debug.Log("change area ( indexof : " + _controller.AllAreas.IndexOf(area) + " )");

           _controller.ClusterWaypointHeuristics.Clear();
           
           foreach (WayPointView waypoint in area.Waypoints)
           {
               WaypointHeuristic waypointHeuristic = new WaypointHeuristic(waypoint);
               _controller.ClusterWaypointHeuristics.Add(waypointHeuristic);
           }

           return TaskStatus.Success;
        }
    }    
}

