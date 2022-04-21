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
		BasicAI _ai;

    protected BasicStateMachine _stateMachine;

    int finishLineIndex;

  #endregion

  #region [teal] Initialize
    
    public void StartPlanner(BasicAI ai)
    {
      _ai = ai;
      StartCoroutine(UpdatePath());
    }
    
  #endregion

  #region [blue] Pathfinding

    IEnumerator UpdatePath()
    {
      if (Time.timeSinceLevelLoad < .3f || _ai.Target == null)
      {
        yield return new WaitForSeconds(.3f);
      }
      PathRequestManager.RequestPath(new PathRequest(transform.position, _ai.Target.position, OnPathFound));

      float sqrMoveThreshold = pathUpdateMoveThreshold * pathUpdateMoveThreshold;
      Vector3 targetPosOld = _ai.Target.position;

      while (true)
      {
        if (_ai.Target == null)
        {
          yield return new WaitForSeconds(minPathUpdateTime);
        }
        else
        {
          yield return new WaitForSeconds(minPathUpdateTime);
          //print(((Target.position - targetPosOld).sqrMagnitude) + "    " + sqrMoveThreshold);
          if ((_ai.Target.position - targetPosOld).sqrMagnitude > sqrMoveThreshold)
          {
            PathRequestManager.RequestPath(new PathRequest(transform.position, _ai.Target.position, OnPathFound));
            targetPosOld = _ai.Target.position;
          }
        }
      }
    }
    
    public void OnPathFound(Vector3[] waypoints, bool pathSuccessful)
    {
      if (pathSuccessful && _ai.Target != null)
      {
        Path _path = new Path(waypoints, _ai.transform.position, _ai.turnDst, _ai.stoppingDst);

        _ai.currentPath = new Path(waypoints, _ai.transform.position, _ai.turnDst, _ai.stoppingDst);
        finishLineIndex = _ai.currentPath.finishLineIndex;
        StartPath();
      }
    }
    
  #endregion

  #region [red]FollowPath

    public void StartPath()
    {
      _ai.pathIndex = 0;
      GetWaypoint();
      _ai.PathComplete = false;
    }

    public void NextWayPoint()
    {
      _ai.pathIndex++;
      GetWaypoint();
    }

    public void GetWaypoint()
    {
      if(_ai.pathIndex > _ai.currentPath.finishLineIndex)
      {
        _ai.pathIndex = _ai.currentPath.finishLineIndex;
      }
      else
      {
        _ai.waypoint = _ai.currentPath.lookPoints[_ai.pathIndex];
      }
    }
  #endregion

  #region [purple]Pathway Checks
	  public void CheckProgress()
		{ 
      float distanceFromTarget = Vector3.Distance(_ai.transform.position, _ai.waypoint);
			if(distanceFromTarget < _ai.stoppingDst+2)
			{
				WaypointPointReached();
			}
			else
			{
				_ai.AtWayPoint = false;
			}
		}

    void WaypointPointReached()
		{ 
			_ai.AtWayPoint = true;
			if (_ai.pathIndex == finishLineIndex)
			{
				CompletedPath();
			}
			else
			{ 
				NextWayPoint();
			}
		}

		void CompletedPath()
		{
			_ai.PathComplete = true;
		}

  #endregion
	}
}