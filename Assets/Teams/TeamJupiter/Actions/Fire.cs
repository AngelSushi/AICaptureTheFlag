using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Jupiter
{
    [TaskCategory("Jupiter")]
    public class Fire : Action
    {

        private JupiterController _controller;

        public override void OnAwake()
        {
            _controller = JupiterController.Instance;
            base.OnAwake();
        }


        public override TaskStatus OnUpdate()
        {
            // Detect Mine qui return toujours false tant qu'il est pas dans la bonne direction et une fois dedans il tire 
            
            // Attentiona vec l'arriere du vaisseau quand changement de direction 

            _controller.Shoot = true;
            StartCoroutine(WaitEndOfFrame());
            
            return TaskStatus.Success;
        }
        
        private  IEnumerator WaitEndOfFrame()
        {
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            
            _controller.Shoot = false;
        }
    }

}