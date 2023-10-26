using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BehaviorDesigner.Runtime;
using UnityEngine;
using DoNotModify;
using Jupîter;
using Random = UnityEngine.Random;

namespace Jupiter {

	public class JupiterController : BaseSpaceShipController
	{

		// Everywhere 
		private static JupiterController _instance;
		public static JupiterController Instance { get => _instance; }
		
		[SerializeField] private BehaviorTree mainTree;
		public BehaviorTree MainTree { get => mainTree; }

		[SerializeField] private BehaviorTree movementTree;
		public BehaviorTree MovementTree { get => movementTree; }
		
		[SerializeField] private BehaviorTree mineTree;
		public BehaviorTree MineTree { get => mineTree; }

		private SpaceShipView _spaceShip;
		public SpaceShipView SpaceShip { get => _spaceShip; }

		private SpaceShipView _otherSpaceShip;
		public SpaceShipView OtherSpaceShip { get => _otherSpaceShip; }

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
			movementTree.SetVariableValue("IAOwner",spaceship.Owner);

			_maxTime = data.timeLeft;
		}

		public override InputData UpdateInput(SpaceShipView spaceship, GameData data)
		{
			_otherSpaceShip = data.GetSpaceShipForOwner(1 - spaceship.Owner);
			float thrust = 1f;
			float targetOrient = AimingHelpers.ComputeSteeringOrient(spaceship,_lookPosition); 
			// SignedAngle entre deux vecteurs (normal et direction) et ensuite ca nous donne un angle positiof ou négatif si positif tourner dans un sens si négatif tourner
			// dans l'autre

			_owner = spaceship.Owner;
			_spaceShip = spaceship;
			_timeLeft = data.timeLeft;
			
			movementTree.SetVariableValue("IAPosition",spaceship.Position);
			movementTree.SetVariableValue("NextWaypointPosition",_lookPosition);

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

			return new InputData(thrust, targetOrient + CalculateAvoidanceAngle(), _shoot, false, _shockWave);
		}
		

		private void OnDrawGizmos()
		{
			if (_spaceShip != null && mainTree != null && mainTree.GetVariable("RangeDetection") != null)
			{
				Gizmos.DrawSphere(_spaceShip.Position,(float)mainTree.GetVariable("RangeDetection").GetValue());
				Gizmos.color = Color.yellow;
				Gizmos.DrawLine(_spaceShip.Position,_spaceShip.Position + _spaceShip.LookAt * 2);
			} 
		}

		private int CalculateAvoidanceAngle()
		{
			// Top : 0;1
			// Top Right : 0.5 ; 0.5
			// Right 1; 0
			// Bottom Right : 0.5 ; -0.5
			// Bottom : 0; -1
			// Bottom Left : -0.5 ; -0.5
			// Left : -0.5; 0
			// Top Left : -0.5 ; 0.5f

			Vector2[] allDirections =
			{
				new Vector2(0,1), new Vector2(1f,1f), new Vector2(1,0),
				new Vector2(1f,-1f), new Vector2(0,-1), new Vector2(-1f,-1f),
				new Vector2(-1f,0), new Vector2(-1f,1f)
			};

			int avoidAngle = 0;

			foreach (Vector2 direction in allDirections)
			{
				RaycastHit2D hit = Physics2D.Raycast(_spaceShip.Position, direction,2, 1 << 12);

				if (hit.collider != null)
				{
					Debug.DrawLine(_spaceShip.Position,_spaceShip.Position + direction *2,Color.red);

					avoidAngle += (int) (Vector2.Angle(_spaceShip.LookAt,hit.normal));
				}
				else
				{
					Debug.DrawLine(_spaceShip.Position,_spaceShip.Position + direction *2,Color.yellow);
				}
			}
			
			return 0;
		}
	}

}
