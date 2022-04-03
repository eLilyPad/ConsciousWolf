using UnityEngine;
using System.Collections;

namespace Lily.Ai.Pathfinder
{
  using System;
  using Ai;
  using Lily.Ai.ActionStates;
  using StateMachine;

  public class PathPlanner : MonoBehaviour
	{
  #region [black] Parameters

    const float minPathUpdateTime = .2f;
    const float pathUpdateMoveThreshold = .5f;
    private float sqrMoveThreshold = pathUpdateMoveThreshold * pathUpdateMoveThreshold;
		BasicAI ai;

    protected BasicStateMachine _stateMachine;

    public Vector3 targetPosition;

  #endregion

  #region [teal] Initialize
    public void StartPlanner(BasicAI _ai)
    {
      ai = _ai;

      _stateMachine = new BasicStateMachine();
			var Search = new FindClosestTargetWithTag(ai, ai.targetTag);
			var FindPath = new FindPath(ai);

      At(Search, FindPath, HasTarget());
      At(FindPath, Search, HasNoTarget());

      _stateMachine.SetState(Search);

      void At(IState from, IState to, Func<bool> condition) => _stateMachine.AddTransition(from, to, condition);
      Func<bool> HasTarget() => () => ai.target != null;
			Func<bool> HasNoTarget() => () => ai.target == null;

      //StartCoroutine(UpdatePath());
    }

    private void Update() 
		{
			_stateMachine.Tick();
		}

		// public void Tick()
		// {
    //   if ((ai.target.position - targetPosOld).sqrMagnitude > sqrMoveThreshold)
    //   {
    //     UpdatePath(currentPos, ai.target.position);
    //     // ai.PathFound = false;
    //   }
    // }
    
  #endregion

  #region [blue] Pathfinding

    IEnumerator UpdatePath()
    {
      if (Time.timeSinceLevelLoad < .3f)
      {
        yield return new WaitForSeconds(.3f);
      }
      PathRequestManager.RequestPath(new PathRequest(transform.position, ai.target.position, OnPathFound));

      float sqrMoveThreshold = pathUpdateMoveThreshold * pathUpdateMoveThreshold;
      Vector3 targetPosOld = ai.target.position;

      while (true)
      {
        yield return new WaitForSeconds(minPathUpdateTime);
        //print(((target.position - targetPosOld).sqrMagnitude) + "    " + sqrMoveThreshold);
        if ((ai.target.position - targetPosOld).sqrMagnitude > sqrMoveThreshold)
        {
          PathRequestManager.RequestPath(new PathRequest(transform.position, ai.target.position, OnPathFound));
          targetPosOld = ai.target.position;
          ai.newPathFound = true;
        }
      }
    }
    
    public void OnPathFound(Vector3[] waypoints, bool pathSuccessful)
    {
      if (pathSuccessful)
      {
        ai.currentPath = new Path(waypoints, ai.transform.position, ai.turnDst, ai.stoppingDst);
        ai.newPathFound = false;

        //Restart the path
      }
    }

  #endregion
	}
}