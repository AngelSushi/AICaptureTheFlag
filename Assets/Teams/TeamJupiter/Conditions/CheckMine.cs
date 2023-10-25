using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DoNotModify;
using UnityEngine;

namespace Jupiter
{
    [TaskCategory("Jupiter")]
    public class CheckMine : Conditional
    {
        private JupiterController _controller;
        
        public override void OnAwake()
        {
            _controller = JupiterController.Instance;
            base.OnAwake();
        }

        public override TaskStatus OnUpdate()
        {

            Collider2D[] colliders = Physics2D.OverlapCircleAll(_controller.SpaceShip.Position,1, 1 << 13);

            if (colliders.Length > 0)
            {
                Debug.Log("detect mine " + colliders[0].transform.root.GetComponent<Mine>().Position);
                _controller.BehaviorTree.SetVariableValue("DetectPosition",colliders[0].transform.root.GetComponent<Mine>().Position);
                return TaskStatus.Success;
            }
            
            return TaskStatus.Failure;
        }
    }
}
