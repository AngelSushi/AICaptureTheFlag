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
		[SerializeField] private List<Cluster> allClusters = new List<Cluster>();
		public List<Cluster> AllClusters { get => allClusters; }

		// USED IN ChooseArea
		private int _owner;
		public int Owner { get => _owner; }

		// USED IN ChooseArea & LookAt
		private Cluster _targetCluster;
		public Cluster TargetCluster { get => _targetCluster; set => _targetCluster = value; }

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
		
		// Used in HeuristicHelper
		private float _timeLeft;
		public float TimeLeft { get => _timeLeft; }

		private float _maxTime;

		public float MaxTime
		{
			get => _maxTime;
		}

		private float _avoidAngle;
		
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

			_maxTime = data.timeLeft;
		}

		public override InputData UpdateInput(SpaceShipView spaceship, GameData data)
		{
			SpaceShipView otherSpaceship = data.GetSpaceShipForOwner(1 - spaceship.Owner);
			float thrust = 1f;
			float targetOrient = AimingHelpers.ComputeSteeringOrient(spaceship,_lookPosition); 
			// SignedAngle entre deux vecteurs (normal et direction) et ensuite ca nous donne un angle positiof ou négatif si positif tourner dans un sens si négatif tourner
			// dans l'autre

			_owner = spaceship.Owner;
			_spaceShip = spaceship;
			_timeLeft = data.timeLeft;
			
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

			foreach (Cluster cluster in allClusters)
			{
				cluster.Score = HeuristicHelper.CalculateZoneHeuristic(this, cluster);
			}

			foreach (WaypointHeuristic waypointHeuristic in clusterWaypointHeuristics)
			{
				waypointHeuristic.Score = HeuristicHelper.CalcuateWaypointHeuristic(this, waypointHeuristic.Waypoint);
			}

			RaycastHit2D hit = Physics2D.Raycast(spaceship.Position, spaceship.LookAt, 2, 1 << 12);
			
			if(hit.collider != null)
			{
				Debug.Log("raycast hit");
				float signedAngle = Vector2.SignedAngle(hit.normal, spaceship.LookAt);
				Debug.Log("signedANgle " + signedAngle);
				_avoidAngle = 90 * Mathf.Sign(signedAngle);
				StartCoroutine(Wait());

			}
			
			
			return new InputData(thrust, targetOrient + _avoidAngle, _shoot, false, _shockWave);
		}
		

		private void OnDrawGizmos()
		{
			if (_spaceShip != null && behaviorTree != null && behaviorTree.GetVariable("RangeDetection") != null)
			{
				Gizmos.DrawSphere(_spaceShip.Position,(float)behaviorTree.GetVariable("RangeDetection").GetValue());
				Gizmos.color = Color.yellow;
				Gizmos.DrawLine(_spaceShip.Position,_spaceShip.Position + _spaceShip.LookAt * 2);
			} 
		}

		private IEnumerator Wait()
		{
			yield return new WaitForEndOfFrame();
			_avoidAngle = 0;
		}
	}

}
