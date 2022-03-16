using System;
using UnityEngine;

namespace Lily.Ai
{
	using ActionStates;
	using StateMachine;
	using Pathfinder;
	public class BasicAI : MonoBehaviour
	{
		#region Variables
			protected BasicStateMachine _stateMachine;
			protected BasicStateMachine _targetStateMachine;

			public Transform target;
			public string targetTag;
			
    	public float speed = 20;
    	public float turnSpeed = 3;
    	public float turnDst = 5;
  	  public float stoppingDst = 10;

			Path oldPath;
			public Path currentPath;
			public bool PathFound;

		#endregion

		void Awake()
		{
			PathFound = false;
			_targetStateMachine = new BasicStateMachine();
			_stateMachine = new BasicStateMachine();//calls a new state machine

			var Search = new FindClosestTargetWithTag(this, targetTag);
			var Rest = new Rest(this);
			var FollowPath = new FollowPath(this, currentPath);
			var FindPath = new FindPath(this);
			// var flee = new Flee(this, controller, enemyDetector);
			// var chase = new Chase(this, controller, enemyDetector);

			_targetStateMachine.AddAnyTransition(FindPath, HasTarget());
			_targetStateMachine.AddAnyTransition(Search, HasNoTarget());
			At(Rest, FollowPath, HasPath());
			At(FollowPath, Rest, HasNoPath());

			// 	_stateMachine.AddAnyTransition(flee, () => enemyDetector.EnemyInRange);
			// 	At(flee, search, () => enemyDetector.EnemyInRange == false);
			// 	At(search, flee, HasTarget());

			_targetStateMachine.SetState(Search);
			_stateMachine.SetState(Rest);


			void At(IState to, IState from, Func<bool> condition) => _stateMachine.AddTransition(to, from, condition);
			Func<bool> HasTarget() => () => target != null;
			Func<bool> HasNoTarget() => () => target == null;
			Func<bool> HasPath() => () => PathFound == true;
			Func<bool> HasNoPath() => () => PathFound == false;
		}

		private void Update() 
		{
			_stateMachine.Tick();
			_targetStateMachine.Tick();
		}
	}
}