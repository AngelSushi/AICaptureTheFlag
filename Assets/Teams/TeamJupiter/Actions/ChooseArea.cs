using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BehaviorDesigner.Runtime.Tasks;
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
            
           // Trouver la zone avec le score le plus faible et dans lequel il n'a pas capturer les flags 

           Area area = _controller.AllAreas.OrderBy(area => area.Score)
               .Where(area => area.Waypoints.Any(waypoint => waypoint.Owner != _controller.Owner))
               .ToList()[0];

           Debug.Log("first of " + area.Waypoints.First().Position);


           return TaskStatus.Success;
        }
    }    
}
