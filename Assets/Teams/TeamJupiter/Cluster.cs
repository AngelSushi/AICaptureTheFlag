using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DoNotModify;
using UnityEngine;

namespace Jupiter
{
    [Serializable]

    public class Cluster {

        [SerializeField] private List<WayPointView> waypoints;
        [SerializeField] private int score;
        [SerializeField] private Vector2 topLeftCorner;
        [SerializeField] private Vector2 botRightCorner;

        [SerializeField] private Vector2 center;
        public int Score
        {
            get => score;
            set => score = value;
        }

        public Vector2 Center { get => center; }

        public List<WayPointView> Waypoints
        {
            get => waypoints;
            private set => waypoints = value;
        }

        public Cluster(List<WayPointView> waypoints)
        {
            this.waypoints = waypoints;

            topLeftCorner = new Vector2(waypoints.Min(waypoint => waypoint.Position.x),waypoints.Max(waypoint => waypoint.Position.y));
            botRightCorner = new Vector2(waypoints.Max(waypoint => waypoint.Position.x),waypoints.Min(waypoint => waypoint.Position.y));

            float distanceX = Mathf.Abs(topLeftCorner.x - botRightCorner.x);
            float distanceY = Mathf.Abs(topLeftCorner.y - botRightCorner.y);

            center = new Vector2(topLeftCorner.x + distanceX / 2, topLeftCorner.y - distanceY / 2);
        }
    }
}
