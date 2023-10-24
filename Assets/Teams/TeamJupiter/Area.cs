using System;
using System.Collections;
using System.Collections.Generic;
using DoNotModify;
using UnityEngine;

namespace Jupiter
{
    [Serializable]

    public class Area
    {

        [SerializeField] private List<WayPointView> waypoints;
        [SerializeField] private int score;

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
        }
    }
}
