using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BehaviorDesigner.Runtime.Tasks;
using DoNotModify;
using UnityEngine;

namespace Jupiter
{
    [TaskCategory("Jupiter")]
    public class ChooseBestWaypointToGo : Action
    {

        private JupiterController _controller;

        public override void OnAwake()
        {
            _controller = JupiterController.Instance;
            base.OnAwake();
        }

        public override void OnStart()
        {
            // Calculer l'heuristique du point 
            foreach (WaypointHeuristic waypointHeuristic in _controller.ClusterWaypointHeuristics)
            {
                waypointHeuristic.Score = Random.Range(0, 100);
            }
            base.OnStart();
        }

        public override TaskStatus OnUpdate()
        {
            // Dans le targetArea trouver le waypoint avec le meilleur score 
            if (_controller.ClusterWaypointHeuristics.Count == 0)
            {
                return TaskStatus.Failure;
            }
            
            WaypointHeuristic bestWaypoint = _controller.ClusterWaypointHeuristics
                .OrderByDescending(waypointHeuristic => waypointHeuristic.Score)
                .Where(waypointHeuristic => waypointHeuristic.Waypoint.Owner != _controller.Owner).ToList()[0];

            if (bestWaypoint == null)
            {
                return TaskStatus.Failure;
            }
            
            _controller.BehaviorTree.SetVariableValue("AreaWaypointIndex", _controller.TargetCluster.Waypoints.IndexOf(bestWaypoint.Waypoint));
            
            return TaskStatus.Success;
        }
    }    
}

