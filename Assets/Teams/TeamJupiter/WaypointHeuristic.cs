using System;
using System.Collections;
using System.Collections.Generic;
using DoNotModify;
using UnityEngine;

[Serializable]
public class WaypointHeuristic
{

    [SerializeField] private WayPointView waypoint;
    [SerializeField] private int score;

    public WayPointView Waypoint
    {
        get => waypoint;
    }

    public int Score
    {
        get => score;
        set => score = value;
    }

    public WaypointHeuristic(WayPointView waypoint)
    {
        this.waypoint = waypoint;
    }
}
