using UnityEngine;
using System.Collections;

namespace Lily.Ai.ActionStates
{
  using Ai;
  using Ai.Pathfinder;
	public class FindPath : IState
	{
    const float minPathUpdateTime = .2f;
    const float pathUpdateMoveThreshold = .5f;
    private float sqrMoveThreshold = pathUpdateMoveThreshold * pathUpdateMoveThreshold;

		private readonly BasicAI ai;
    private Vector3 targetPosOld;
    private Vector3 targetPos;
    private Vector3 currentPos;
		public FindPath(BasicAI _ai)
		{
			ai = _ai;
		}
		public void Tick()
		{
      if ((targetPos - targetPosOld).sqrMagnitude > sqrMoveThreshold)
      {
        UpdatePath(currentPos, targetPos);
        ai.PathFound = false;
      }
    }

    void UpdatePath(Vector3 _currentPos, Vector3 _targetPos)
    {
      PathRequestManager.RequestPath(new PathRequest(_currentPos, _targetPos, OnPathFound));
      targetPosOld = _targetPos;
    }

    public void OnPathFound(Vector3[] waypoints, bool pathSuccessful)
    {
      if (pathSuccessful)
      {
        ai.currentPath = new Path(waypoints, ai.transform.position, ai.turnDst, ai.stoppingDst);
        ai.PathFound = true;
      }
    }
		public void OnEnter() 
    {
      currentPos = ai.transform.position;
      targetPos = ai.target.position;
      UpdatePath(currentPos, targetPos);
    }
		public void OnExit() { }
	}
}