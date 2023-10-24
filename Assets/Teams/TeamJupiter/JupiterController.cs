using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DoNotModify;

namespace Jupiter {

	public class JupiterController : BaseSpaceShipController
	{

		// Singleton 
		private JupiterController _instance;

		public JupiterController Instance
		{
			get => _instance;
			set => _instance = value;
		}

		private List<WayPoint> _allWaypoints;

		public List<WayPoint> AllWaypoints
		{
			get => _allWaypoints;
			set => _allWaypoints = value;
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
			
			
		}

		public override InputData UpdateInput(SpaceShipView spaceship, GameData data)
		{
			SpaceShipView otherSpaceship = data.GetSpaceShipForOwner(1 - spaceship.Owner);
			float thrust = 1.0f;
			float targetOrient = spaceship.Orientation + 90.0f;
			bool needShoot = AimingHelpers.CanHit(spaceship, otherSpaceship.Position, otherSpaceship.Velocity, 0.15f);
			return new InputData(thrust, targetOrient, needShoot, false, false);
		}
	}

}
