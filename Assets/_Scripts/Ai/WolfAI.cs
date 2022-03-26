using System.Collections;
using UnityEngine;
using System;

namespace Lily.Ai
{
  using ActionStates;
	using StateMachine;
	using Pathfinder;
	using MovementSystem.Controller;
  public class WolfAI : BasicAI
  {
    #region Variables
			
			#region Pathfinding Constants
				const float minPathUpdateTime = .2f;
				const float pathUpdateMoveThreshold = .5f;
    		private float sqrMoveThreshold = pathUpdateMoveThreshold * pathUpdateMoveThreshold;
				
			#endregion

			PathPlanner planner;
			Vector3 oldTargetPosition;

		#endregion

		void Awake()
		{
			rb = GetComponent<Rigidbody>();
			planner = new PathPlanner(this);
			_stateMachine = new BasicStateMachine();//calls a new state machine
			var Search = new FindClosestTargetWithTag(this, targetTag);
			var Rest = new Rest(this);
			var FollowPath = new FollowPath(this, currentPath, rb);
			var FindPath = new FindPath(this);
			// var flee = new Flee(this, controller, enemyDetector);
			// var chase = new Chase(this, controller, enemyDetector);

			// At(Search, FindPath, HasTarget());
			// At(FindPath, FollowPath, HasPath());
			// At(FollowPath, FindPath, HasNoPath());
			// At(FollowPath, FindPath, HasTargetMoved());
			// At(FollowPath, FindPath, HasPathCompleted());

			At(Search, FollowPath, HasPath());

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
			Func<bool> HasTargetMoved() => () => targetMoved == true;
			Func<bool> HasPathCompleted() => () => PathComplete == true;

			oldTargetPosition = target.transform.position;
		}

		private void Update() 
		{
			_stateMachine.Tick();
			if(target)
			{
				planner.Tick();
			}
		}

		bool CheckTargetMoved()
		{ 
			if ((target.transform.position - oldTargetPosition).sqrMagnitude > sqrMoveThreshold)
      {
        oldTargetPosition = target.transform.position;

				targetMoved = true;
      }
			return targetMoved;
		}
  }
}