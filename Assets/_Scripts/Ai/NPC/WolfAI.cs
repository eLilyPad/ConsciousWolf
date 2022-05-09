using System.Collections;
using UnityEngine;
using System;

namespace Lily.Ai
{
	using ActionStates;
	using StateMachine;
	using Pathfinder;
  using System.Collections.Generic;

  public class WolfAI : BasicAI
  {
		
		// public list<MovementModifier> moveModifiers = new List<MovementModifier>();
		void Awake()
		{
			rb = GetComponent<Rigidbody>();
			planner = GetComponent<PathPlanner>();

			_stateMachine = new BasicStateMachine();//calls a new state machine
			
			var Search = new FindClosestTargetWithTag(this, targetTag);
			var Rest = new Rest(this, rb);
			var MoveToTarget = new MoveToTarget(this, rb);
			var Attack = new Attack(this, rb);
			// var AvoidCollision = new AvoidCollision(this, rb);

			At(Search, Rest, HasTarget());
			At(Rest, MoveToTarget, HasPath());
			At(MoveToTarget, Rest, HasNoPath());
			At(MoveToTarget, Attack, InAttackRange());
			At(Attack, MoveToTarget, NotInAttackRange());

			_stateMachine.AddAnyTransition(Search, HasNoTarget());
			_stateMachine.SetState(Search);

			void At(IState from, IState to, Func<bool> condition) => _stateMachine.AddTransition(from, to, condition);
			
			Func<bool> HasTarget() => () => Target != null || Target.gameObject.activeInHierarchy;
			Func<bool> HasNoTarget() => () => Target == null || !Target.gameObject.activeInHierarchy;
			Func<bool> HasNoPath() => () => currentPath == null;
			Func<bool> HasPath() => () => currentPath != null;
			Func<bool> InAttackRange() => () => CheckAttackRange() == true;
			Func<bool> NotInAttackRange() => () => CheckAttackRange() == false;

			planner.StartPlanner(this);
		}

		void OnDisable()
		{
			planner.StopPlanner(this);
		}
		private void Update() 
		{
			_stateMachine.Tick();
			planner.CheckProgress();
			
			if (Target != null)CheckAttackRange();
		}
  }
}