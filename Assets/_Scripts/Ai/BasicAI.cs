using System;
using UnityEngine;

namespace Lily.Ai
{
	using ActionStates;
	using StateMachine;
	using Pathfinder;
  using System.Collections;

  public class BasicAI : MonoBehaviour
	{
		#region Variables
			protected BasicStateMachine _stateMachine;
			protected BasicStateMachine _targetStateMachine;

			public Transform target;
			public string targetTag;

			public string oldTarget;

			const float minPathUpdateTime = .2f;
    	const float pathUpdateMoveThreshold = .5f;
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
			StartCoroutine(UpdatePath());
			PathFound = false;
			_targetStateMachine = new BasicStateMachine();
			_stateMachine = new BasicStateMachine();//calls a new state machine

			var Search = new FindClosestTargetWithTag(this, targetTag);
			var Rest = new Rest(this);
			var FollowPath = new FollowPath(this, currentPath);
			var FindPath = new FindPath(this);
			// var flee = new Flee(this, controller, enemyDetector);
			// var chase = new Chase(this, controller, enemyDetector);

			//Bt(Search, FindPath, ));
			//Bt(FindPath, Search, HasNoTarget());

			At(Rest, FollowPath, HasTarget());
			At(FollowPath, Rest, HasNoTarget());

			// 	_stateMachine.AddAnyTransition(flee, () => enemyDetector.EnemyInRange);
			// 	At(flee, search, () => enemyDetector.EnemyInRange == false);
			// 	At(search, flee, HasTarget());

			_targetStateMachine.SetState(Search);
			_stateMachine.SetState(Rest);


			void At(IState from, IState to, Func<bool> condition) => _stateMachine.AddTransition(from, to, condition);
			void Bt(IState from, IState to, Func<bool> condition) => _targetStateMachine.AddTransition(from, to, condition);
			Func<bool> HasTarget() => () => target != null;
			Func<bool> HasNoTarget() => () => target == null;
			Func<bool> HasPath() => () => currentPath != null;
			Func<bool> HasNoPath() => () => currentPath == null;

		}

		private void Update() 
		{
			_stateMachine.Tick();
			//_targetStateMachine.Tick();
			
			if(oldTarget != target.name)
			{
				StartCoroutine(UpdatePath());
			}
		}

		public void OnPathFound(Vector3[] waypoints, bool pathSuccessful)
		{
			if (pathSuccessful)
			{
				//check if all waypoints are correct
				currentPath = new Path(waypoints, transform.position, turnDst, stoppingDst);

				StopCoroutine("FollowPath");
				StartCoroutine("FollowPath");
			}
		}

		IEnumerator UpdatePath()
		{

			if (Time.timeSinceLevelLoad < .3f)
			{
				yield return new WaitForSeconds(.3f);
			}
			PathRequestManager.RequestPath(new PathRequest(transform.position, target.position, OnPathFound));

			float sqrMoveThreshold = pathUpdateMoveThreshold * pathUpdateMoveThreshold;
			Vector3 targetPosOld = target.position;

			while (true)
			{
				yield return new WaitForSeconds(minPathUpdateTime);
				//print(((target.position - targetPosOld).sqrMagnitude) + "    " + sqrMoveThreshold);
				if ((target.position - targetPosOld).sqrMagnitude > sqrMoveThreshold)
				{
					PathRequestManager.RequestPath(new PathRequest(transform.position, target.position, OnPathFound));
					targetPosOld = target.position;
				}
			}
		}
	}
}