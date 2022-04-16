using System.Collections;
using UnityEngine;
using System;

namespace Lily.Ai
{
  using ActionStates;
	using StateMachine;
	using Pathfinder;
  public class WolfAI : BasicAI
  {
  #region [black] Parameters
    private float sqrMoveThreshold = pathUpdateMoveThreshold * pathUpdateMoveThreshold;
		
	#endregion

		void Awake()
		{
			rb = GetComponent<Rigidbody>();
			planner = GetComponent<PathPlanner>();

			_stateMachine = new BasicStateMachine();//calls a new state machine
			var Search = new FindClosestTargetWithTag(this, targetTag);
			var Rest = new Rest(this, rb);
			var MoveToTarget = new MoveToTarget(this, rb);
			var Attack = new Attack(this, rb);

			At(Search, Rest, HasTarget());
			
			At(Rest, MoveToTarget, HasPath());
			At(MoveToTarget, Rest, HasNoPath());
			At(MoveToTarget, Rest, CompletedPath());
			

			_stateMachine.AddAnyTransition(Search, HasNoTarget());

			_stateMachine.SetState(Search);


			void At(IState from, IState to, Func<bool> condition) => _stateMachine.AddTransition(from, to, condition);
			Func<bool> HasTarget() => () => target != null;
			Func<bool> HasNoTarget() => () => target == null;
			Func<bool> HasNoPath() => () => currentPath == null;
			Func<bool> HasPath() => () => currentPath != null;
			Func<bool> CompletedPath() => () => PathComplete == true;

			planner.StartPlanner(this);
		}

		private void Update() 
		{
			_stateMachine.Tick();
			planner.CheckProgress();
			InRange();
		}

		void InRange()
		{
			float distanceFromTarget = Vector3.Distance(this.transform.position, target.position);

			if (distanceFromTarget <= AttackRange) audioManager.PlayRandomSound();
		}
  }
}