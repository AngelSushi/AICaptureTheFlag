using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BehaviorDesigner.Runtime;
using UnityEngine;
using DoNotModify;
using Random = UnityEngine.Random;

namespace Jupiter {

	public class JupiterController : BaseSpaceShipController
	{

		// Singleton 
		private static JupiterController _instance;

		public static JupiterController Instance
		{
			get => _instance;
			set => _instance = value;
		}

		private List<WayPointView> _allWaypoints;

		public List<WayPointView> AllWaypoints
		{
			get => _allWaypoints;
		}

		[SerializeField] private List<Area> allAreas = new List<Area>();

		public List<Area> AllAreas
		{
			get => allAreas;
		}

		private int _owner;

		public int Owner
		{
			get => _owner;
		}

		private Area _targetArea;

		public Area TargetArea
		{
			get => _targetArea;
			set => _targetArea = value;
		}

		private int _targetWaypoint;

		public int TargetWaypoint
		{
			get => _targetWaypoint;
			set
			{
				_targetWaypoint = value;
				_targetWaypoint = Mathf.Clamp(_targetWaypoint, 0, _targetArea.Waypoints.Count);
			}
		}


		private Vector2 _lookPosition;

		public Vector2 LookPosition
		{
			get => _lookPosition;
			set => _lookPosition = value;
		}

		[SerializeField] private BehaviorTree behaviorTree;

		public BehaviorTree BehaviorTree
		{
			get => behaviorTree;
		}

		private bool test;

		private void Awake()
		{
			if (_instance == null)
			{
				_instance = this;
			}
		}

		public override void Initialize(SpaceShipView spaceship, GameData data)
		{
			_allWaypoints = new List<WayPointView>(data.WayPoints);
			behaviorTree.SetVariableValue("IAOwner",spaceship.Owner);
		}

		public override InputData UpdateInput(SpaceShipView spaceship, GameData data)
		{
			SpaceShipView otherSpaceship = data.GetSpaceShipForOwner(1 - spaceship.Owner);
			float thrust = 1f;
			float targetOrient = AimingHelpers.ComputeSteeringOrient(spaceship,_lookPosition);

			bool needShoot = AimingHelpers.CanHit(spaceship, otherSpaceship.Position, otherSpaceship.Velocity, 0.15f);

			_owner = spaceship.Owner;
			behaviorTree.SetVariableValue("IAPosition",spaceship.Position);
			behaviorTree.SetVariableValue("NextWaypointPosition",_lookPosition);

			return new InputData(thrust, targetOrient, needShoot, false, false);
		}


		public IEnumerator UpdateAreaScore()
		{
			for (int i = 0; i < allAreas.Count; i++)
			{
				Area area = allAreas[i];
				area.Score = Random.Range(0, allAreas.Count);
			}

			yield return new WaitForSeconds(5f);
			StartCoroutine(UpdateAreaScore());
		}
	}

}
