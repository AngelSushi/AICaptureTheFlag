using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Jupiter
{
    [TaskCategory("Jupiter")]
    public class LookAt : Action
    {

        public SharedInt lookIndex;

        private JupiterController _controller;

        public override void OnAwake()
        {
            _controller = JupiterController.Instance;
            base.OnAwake();
        }

        public override TaskStatus OnUpdate()
        {
            _controller.LookPosition = _controller.TargetCluster.Waypoints[lookIndex.Value].Position;
            _controller.MovementTree.SetVariableValue("TargetWaypointOwner",_controller.TargetCluster.Waypoints[lookIndex.Value].Owner);
            
            Debug.Log("look at " + _controller.LookPosition + " " + _controller.TargetCluster.Waypoints[lookIndex.Value].Owner);
            
            return TaskStatus.Success;
        }
    }

}