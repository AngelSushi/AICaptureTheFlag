using System;
using System.Collections;
using System.Collections.Generic;
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
		}

		public override InputData UpdateInput(SpaceShipView spaceship, GameData data)
		{
			SpaceShipView otherSpaceship = data.GetSpaceShipForOwner(1 - spaceship.Owner);
			float thrust = 1.0f;
			float targetOrient = spaceship.Orientation + 90.0f;

			bool needShoot = AimingHelpers.CanHit(spaceship, otherSpaceship.Position, otherSpaceship.Velocity, 0.15f);

			_owner = spaceship.Owner;
			
			return new InputData(thrust, targetOrient, needShoot, false, false);
		}


		public IEnumerator UpdateAreaScore()
		{
			Debug.Log("all areas " + allAreas.Count);
			for (int i = 0; i < allAreas.Count; i++)
			{
				Area area = allAreas[i];
				area.Score = Random.Range(0, allAreas.Count);
				Debug.Log("update score ");
			}

			yield return new WaitForSeconds(5f);
			StartCoroutine(UpdateAreaScore());
		}
	}

}
