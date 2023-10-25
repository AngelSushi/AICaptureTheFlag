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

		// Everywhere 
		private static JupiterController _instance;
		public static JupiterController Instance { get => _instance; }
		
		[SerializeField] private BehaviorTree behaviorTree;
		public BehaviorTree BehaviorTree { get => behaviorTree; }

		private SpaceShipView _spaceShip;
		public SpaceShipView SpaceShip { get => _spaceShip; }

		// #USED IN DivideArea
		private List<WayPointView> _allWaypoints;
		public List<WayPointView> AllWaypoints { get => _allWaypoints; }

		// USED IN DivideArea & ChooseArea
		[SerializeField] private List<Area> allAreas = new List<Area>();
		public List<Area> AllAreas { get => allAreas; }

		// USED IN ChooseArea
		private int _owner;
		public int Owner { get => _owner; }

		// USED IN ChooseArea & LookAt
		private Area _targetArea;
		public Area TargetArea { get => _targetArea; set => _targetArea = value; }

		// USED IN LookAt
		private Vector2 _lookPosition;
		public Vector2 LookPosition { get => _lookPosition; set => _lookPosition = value; }

		// USED IN ChooseBestWaypointToGo ChooseArea
		[SerializeField] private List<WaypointHeuristic> clusterWaypointHeuristics = new List<WaypointHeuristic>();
		public List<WaypointHeuristic> ClusterWaypointHeuristics { get => clusterWaypointHeuristics; }

		// USED IN Fire
		private bool _shoot;
		public bool Shoot { set => _shoot = value; }
		
		// Used in ShockWave
		private bool _shockWave;
		public bool ShockWave { set => _shockWave = value; }
		
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
			float thrust = 0f;
			float targetOrient = AimingHelpers.ComputeSteeringOrient(spaceship,_lookPosition);

			_owner = spaceship.Owner;
			_spaceShip = spaceship;
			behaviorTree.SetVariableValue("IAPosition",spaceship.Position);
			behaviorTree.SetVariableValue("NextWaypointPosition",_lookPosition);

			if (_shoot)
			{
				Debug.Log("ask for shoot");
			}

			if (_shockWave)
			{
				Debug.Log("ask for shockwave");
			}
			
			return new InputData(thrust, targetOrient, _shoot, false, _shockWave);
		}


		public IEnumerator UpdateAreaScore()
		{
			for (int i = 0; i < allAreas.Count; i++)
			{
				Area area = allAreas[i];
				area.Score = Random.Range(0, allAreas.Count);
			}

			yield return new WaitForSeconds(5f);
			//StartCoroutine(UpdateAreaScore());
		}

		private void OnDrawGizmos()
		{
			if (_spaceShip != null && behaviorTree != null && behaviorTree.GetVariable("RangeDetection") != null)
			{
				Gizmos.DrawSphere(_spaceShip.Position,(float)behaviorTree.GetVariable("RangeDetection").GetValue());	
			} 
		}
	}

}
