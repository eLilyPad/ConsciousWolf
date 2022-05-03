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

  public class RabbitAI : BasicAI
	{

    void Awake()
		{
			deathEffect.Stop();
			planner = GetComponent<PathPlanner>();
			rb = GetComponent<Rigidbody>();
			_stateMachine = new BasicStateMachine();//calls a new state machine

			var Search = new FindClosestTargetWithTag(this, targetTag);
			var Rest = new Rest(this, rb);
			var MoveToTarget = new MoveToTarget(this, rb);

			var Eat = new EatFood(this, rb);
			// var flee = new Flee(this, controller, enemyDetector);
			// var chase = new Chase(this, controller, enemyDetector);

			At(Search, Rest, HasTarget());
			
			At(Rest, MoveToTarget, HasPath());
			At(MoveToTarget, Rest, HasNoPath());
			At(MoveToTarget, Eat, InAttackRange());

			_stateMachine.AddAnyTransition(Search, HasNoTarget());

			_stateMachine.SetState(Search);


			void At(IState from, IState to, Func<bool> condition) => _stateMachine.AddTransition(from, to, condition);
			Func<bool> HasTarget() => () => Target != null || Target.gameObject.activeInHierarchy;
			Func<bool> HasNoTarget() => () => Target == null || !Target.gameObject.activeInHierarchy;
			Func<bool> HasPath() => () => currentPath != null;
			Func<bool> HasNoPath() => () => currentPath == null;
			Func<bool> InAttackRange() => () => CheckAttackRange() == true;

			planner.StartPlanner(this);
		}

		private void Update() 
		{
			_stateMachine.Tick();
			planner.CheckProgress();

			if (Target != null)CheckAttackRange();	
		}
	}
}