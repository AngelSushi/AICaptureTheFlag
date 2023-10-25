using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Jupiter
{
    public class Shockwave : Action
    {

        private JupiterController _controller;

        public override void OnAwake()
        {
            _controller = JupiterController.Instance;
            base.OnAwake();
        }

        public override TaskStatus OnUpdate()
        {

            _controller.ShockWave = true;
            Debug.Log("enable shock wave");
            StartCoroutine(WaitNextFrame());
            
            return TaskStatus.Success;
        }

        private IEnumerator WaitNextFrame()
        {
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();

            _controller.ShockWave = false;
        }
    }
}
