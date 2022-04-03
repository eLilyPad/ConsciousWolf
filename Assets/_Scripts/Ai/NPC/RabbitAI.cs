using System;
using UnityEngine;
using System.Collections;

namespace Lily.Ai
{
	using ActionStates;
	using StateMachine;
	using Pathfinder;
	using MovementSystem.Controller;

  public class RabbitAI : BasicAI
	{
		#region Variables
			
			#region Pathfinding Constants
				const float minPathUpdateTime = .2f;
				const float pathUpdateMoveThreshold = .5f;
				
			#endregion

		#endregion

		void Awake()
		{
			//var _pathPlanner = new PathPlanner();
			rb = GetComponent<Rigidbody>();
			_stateMachine = new BasicStateMachine();//calls a new state machine

			var Search = new FindClosestTargetWithTag(this, targetTag);
			var Rest = new Rest(this, rb);
			var FollowPath = new FollowPath(this, currentPath, rb);
			var FindPath = new FindPath(this);
			// var flee = new Flee(this, controller, enemyDetector);
			// var chase = new Chase(this, controller, enemyDetector);

			At(Search, FindPath, HasTarget());
			At(FindPath, FollowPath, HasPath());
			At(FollowPath, FindPath, HasNoPath());
			_stateMachine.AddAnyTransition(Search, HasNoTarget());
			// 	_stateMachine.AddAnyTransition(flee, () => enemyDetector.EnemyInRange);
			// 	At(flee, search, () => enemyDetector.EnemyInRange == false);
			// 	At(search, flee, HasTarget());

			_stateMachine.SetState(Search);


			void At(IState from, IState to, Func<bool> condition) => _stateMachine.AddTransition(from, to, condition);
			Func<bool> HasTarget() => () => target != null;
			Func<bool> HasNoTarget() => () => target == null;
			Func<bool> HasPath() => () => currentPath != null;
			Func<bool> HasNoPath() => () => currentPath == null;

		}

		private void Update() 
		{
			_stateMachine.Tick();
		}
	}
}