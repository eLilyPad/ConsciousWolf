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
		#region Variables
		#endregion

		void Awake()
		{
			deathEffect.Stop();
			planner = GetComponent<PathPlanner>();
			rb = GetComponent<Rigidbody>();
			_stateMachine = new BasicStateMachine();//calls a new state machine

			var Search = new FindClosestTargetWithTag(this, targetTag);
			var Rest = new Rest(this, rb);
			var MoveToTarget = new MoveToTarget(this, rb);

			var Death = new Death(this, rb);
			// var flee = new Flee(this, controller, enemyDetector);
			// var chase = new Chase(this, controller, enemyDetector);

			At(Search, Rest, HasTarget());
			
			At(Rest, MoveToTarget, HasPath());
			At(MoveToTarget, Rest, HasNoPath());
			At(MoveToTarget, Rest, CompletedPath());

			_stateMachine.AddAnyTransition(Search, HasNoTarget());

			_stateMachine.SetState(Search);


			void At(IState from, IState to, Func<bool> condition) => _stateMachine.AddTransition(from, to, condition);
			Func<bool> HasTarget() => () => target != null;
			Func<bool> HasNoTarget() => () => target == null;
			Func<bool> HasPath() => () => currentPath != null;
			Func<bool> HasNoPath() => () => currentPath == null;
			Func<bool> CompletedPath() => () => PathComplete == true;

			planner.StartPlanner(this);
		}

		private void Update() 
		{
			_stateMachine.Tick();
			planner.CheckProgress();		
		}
		void OnCollisionEnter(Collision collide)
		{
			if (collide.gameObject.tag == "Predator")
			{
				Die();
			}
		}
		void OnCollisionExit(Collision collide)
		{

		}
		void Die()
		{
			int lifetime = 3;
			deathEffect.Play();
			Destroy(this, lifetime);
		}
	}
}