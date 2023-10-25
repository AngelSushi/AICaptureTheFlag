using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Jupiter
{
    [TaskCategory("Jupiter")]
    public class CheckBullet : Conditional
    {

        private JupiterController _controller;

        public override void OnAwake()
        {
            _controller = JupiterController.Instance;
            base.OnAwake();
        }

        public override TaskStatus OnUpdate()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(_controller.SpaceShip.Position,1, 1 << 14);


            Debug.Log("length of bullet " + colliders.Length);
            
            if (colliders.Length >= 3)
            {
                return TaskStatus.Success;
            }
            
            return TaskStatus.Failure;
        }
    }
}
