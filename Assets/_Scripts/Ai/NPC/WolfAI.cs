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
			
		const float minPathUpdateTime = .2f;
		const float pathUpdateMoveThreshold = .5f;
    private float sqrMoveThreshold = pathUpdateMoveThreshold * pathUpdateMoveThreshold;

		PathPlanner planner;
		Vector3 oldTargetPosition;

	#endregion

		void Awake()
		{
			rb = GetComponent<Rigidbody>();
			//planner.StartPlanner(this);

			_stateMachine = new BasicStateMachine();//calls a new state machine
			var Search = new FindClosestTargetWithTag(this, targetTag);
			var FindPath = new FindPath(this);
			var Rest = new Rest(this, rb);
			var FollowPath = new FollowPath(this, currentPath, rb);
			// var flee = new Flee(this, controller, enemyDetector);
			// var chase = new Chase(this, controller, enemyDetector);

			// At(Search, FindPath, HasTarget());
			// At(FindPath, FollowPath, HasPath());
			// At(FollowPath, FindPath, HasNoPath());
			// At(FollowPath, FindPath, HasTargetMoved());
			// At(FollowPath, FindPath, HasPathCompleted());
			At(Search, Rest, HasTarget());
			
			At(Rest, FollowPath, HasPath());
			At(FollowPath, Rest, HasNoPath());
			

			_stateMachine.AddAnyTransition(Search, HasNoTarget());

			_stateMachine.SetState(Search);


			void At(IState from, IState to, Func<bool> condition) => _stateMachine.AddTransition(from, to, condition);
			Func<bool> HasTarget() => () => target != null;
			Func<bool> HasNoTarget() => () => target == null;
			Func<bool> HasNoPath() => () => currentPath == null;
			Func<bool> HasPath() => () => currentPath != null;

			StartCoroutine(UpdatePath());
		}

		private void Update() 
		{
			_stateMachine.Tick();
		}

  #region [blue] Pathfinding

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
          newPathFound = true;
        }
      }
    }
    
    public void OnPathFound(Vector3[] waypoints, bool pathSuccessful)
    {
      if (pathSuccessful)
      {
        currentPath = new Path(waypoints, transform.position, turnDst, stoppingDst);
        newPathFound = false;
        //Restart the path
      }
    }
  #endregion
  }
}