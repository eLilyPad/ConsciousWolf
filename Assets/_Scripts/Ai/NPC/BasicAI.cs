using System;
using UnityEngine;
using System.Collections;

namespace Lily.Ai
{
	using ActionStates;
	using StateMachine;
	using Pathfinder;
	using MovementSystem.Controller;

  public class BasicAI : MonoBehaviour
	{
		#region Variables
			
			#region Pathfinding Constants
				const float minPathUpdateTime = .2f;
				const float pathUpdateMoveThreshold = .5f;
				
			#endregion
			protected BasicStateMachine _stateMachine;
			public MovementController mc;
			public Rigidbody rb;
			public Transform target;
			public string targetTag;

    	public float maxSpeed = 20;
			public float acceleration;
    	public float turnSpeed = 3;
    	public float turnDst = 5;
			public float turnPenalty = 1;
  		public float stoppingDst = 10;
			public Path currentPath;
			public bool PathComplete;
			public bool targetMoved;

			public float turnThreshold = 10f;

			public bool newPathFound;

		#endregion

		void Awake()
		{}

		private void Update() 
		{}
	}
}