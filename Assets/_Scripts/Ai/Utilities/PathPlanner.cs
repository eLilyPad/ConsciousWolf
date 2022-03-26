using UnityEngine;
using System.Collections;

namespace Lily.Ai
{
  using Ai;
  using Ai.Pathfinder;
	public class PathPlanner : MonoBehaviour
	{
    const float minPathUpdateTime = .2f;
    const float pathUpdateMoveThreshold = .5f;
    private float sqrMoveThreshold = pathUpdateMoveThreshold * pathUpdateMoveThreshold;

		BasicAI ai;
    private Vector3 targetPosOld;
    private Vector3 targetPos;
    private Vector3 currentPos;

    public PathPlanner(BasicAI _ai)
    {
      ai = _ai;
    }

		public void Tick()
		{
      if ((ai.target.position - targetPosOld).sqrMagnitude > sqrMoveThreshold)
      {
        UpdatePath(currentPos, ai.target.position);
        // ai.PathFound = false;
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
	}
}