using System;
using UnityEngine;
using System.Collections;

namespace Lily.Ai
{
	using ActionStates;
	using StateMachine;
	using Pathfinder;
	using MovementSystem.Controller;
  using UnityEngine.VFX;

  public class BasicAI : MonoBehaviour
	{
		#region Variables
			
			#region Pathfinding Constants
				protected const float minPathUpdateTime = .2f;
				protected const float pathUpdateMoveThreshold = .5f;
				
			#endregion
			protected BasicStateMachine _stateMachine;
			public MovementController mc;
			
			public AIManager manager;
			public Rigidbody rb;
			public Transform Target;
			public GameObject AttackTarget;
			public Vector3 waypoint;
			public string targetTag;

    	public float maxSpeed = 20;
			public float acceleration;
    	public float turnSpeed = 3;
    	public float turnDst = 5;
			public float turnPenalty = 1;
  		public float stoppingDst = 10;
			public Path currentPath;
			public int pathIndex;
			public bool PathComplete = false;
			public bool targetMoved;
			public bool AtWayPoint = false;
			public bool CanAttack = false;
			public float AttackRange = 3;

			public bool drawGizmos = false;

			public float turnThreshold = 10f;

			public PathPlanner planner;

			public AudioManager audioManager;

			public VisualEffect deathEffect;

			private bool _isAlive = true;

			public bool IsAlive 
			{ 
				get { return _isAlive; }   // get method
				set { _isAlive = value; } 
			}

		#endregion

		void Awake()
		{}

		private void Update() 
		{}

		public void OnDrawGizmos() 
		{
			if (drawGizmos)
			{
				if (currentPath != null) 
				{
					currentPath.DrawWithGizmos ();
				}
			}
		}
	}
}