using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DoNotModify;
using UnityEngine;

namespace Jupiter
{
    [Serializable]

    public class Area
    {

        [SerializeField] private List<WayPointView> waypoints;
        [SerializeField] private int score;
        [SerializeField] private Vector2 topLeftCorner;
        [SerializeField] private Vector2 botRightCorner;
        
        
        public int Score
        {
            get => score;
            set => score = value;
        }

        public List<WayPointView> Waypoints
        {
            get => waypoints;
            private set => waypoints = value;
        }

        public Area(List<WayPointView> waypoints)
        {
            this.waypoints = waypoints;

            topLeftCorner = new Vector2(waypoints.Min(waypoint => waypoint.Position.x),waypoints.Max(waypoint => waypoint.Position.y));
            botRightCorner = new Vector2(waypoints.Max(waypoint => waypoint.Position.x),waypoints.Min(waypoint => waypoint.Position.y));
        }
    }
}
