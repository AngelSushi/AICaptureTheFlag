using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Jupiter
{
    [TaskCategory("Jupiter")]
    public class IsMineInPath : Conditional
    {

        private JupiterController _controller;
        public SharedVector2 minePosition;

        public override void OnAwake()
        {
            _controller = JupiterController.Instance;
            base.OnAwake();
        }
        // Comment faire si ya 2 mine

        public override TaskStatus OnUpdate()
        {
            
            Debug.Log("minePos " + minePosition);
            
            if (AimingHelpers.CanHit(_controller.SpaceShip, minePosition.Value, (float)_controller.BehaviorTree.GetVariable("DetectionAngleOffset").GetValue()))
            {
                Debug.Log("can hit");
                return TaskStatus.Success;
            }
            
            return TaskStatus.Failure;
        }
    }
}
