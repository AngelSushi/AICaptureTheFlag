using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DoNotModify;
using UnityEngine;


namespace Jupiter
{
    [TaskCategory("Jupiter")]
    public class DivideArea : Action
    {

        private JupiterController _controller;
        
        public override void OnStart()
        {
            if (_controller == null)
            {
                _controller = JupiterController.Instance;
                
            }
            
            base.OnStart();
        }

        public override TaskStatus OnUpdate()
        {
            if (_controller.AllWaypoints == null || _controller.AllWaypoints.Count == 0)
            {
                return TaskStatus.Failure;
            }

            Debug.Log("log");
            List<WayPointView> checkedWaypoints = new List<WayPointView>(_controller.AllWaypoints);

            foreach (WayPointView waypoint in checkedWaypoints.ToList())
            {
                
                if (!IsAlreadyInArea(waypoint))
                {
                    List<WayPointView> areaWaypoints = new List<WayPointView>();
                    
                    foreach (WayPointView nearWaypoint in checkedWaypoints.ToList())
                    {
                        float distance = Vector2.Distance(waypoint.Position, nearWaypoint.Position);
                        
                        if (distance <= 3.0f && areaWaypoints.Count < 4) 
                        {
                            areaWaypoints.Add(nearWaypoint);
                            checkedWaypoints.Remove(nearWaypoint);
                        }
                        
                    }

                    Cluster cluster = new Cluster(areaWaypoints);
                    _controller.AllClusters.Add(cluster);
                }
            }

            //StartCoroutine(_controller.UpdateAreaScore());
            return TaskStatus.Success;
        }

        private bool IsAlreadyInArea(WayPointView waypoint)
        {
            foreach (Cluster cluster in _controller.AllClusters)
            {
                if (cluster.Waypoints.Contains(waypoint))
                {
                    return true;
                }
            }

            return false;
        }
    }
}

