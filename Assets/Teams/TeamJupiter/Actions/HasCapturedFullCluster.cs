using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Jupiter
{
    [TaskCategory("Jupiter")]
    public class HasCapturedFullCluster : Conditional
    {

        private JupiterController _controller;

        public override void OnAwake()
        {
            _controller = JupiterController.Instance;
            base.OnAwake();
        }

        public override TaskStatus OnUpdate()
        {
            if (_controller.TargetCluster.Waypoints.All(waypoint => waypoint.Owner == _controller.Owner))
            {
                return TaskStatus.Success;
            }
            
            return TaskStatus.Failure;
        }
    }

}