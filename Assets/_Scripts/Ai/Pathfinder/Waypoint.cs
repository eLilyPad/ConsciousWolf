using System.Collections;
using UnityEngine;

namespace Lily.Ai.Pathfinder
{
  public class Waypoint
  {
  //   const float minPathUpdateTime = .2f;
  //   const float pathUpdateMoveThreshold = .5f;

  //   Transform _target;

  //   int pathIndex;
  //   public float speed = 20;
  //   public float turnSpeed = 3;
  //   public float turnDst = 5;
  //   public float stoppingDst = 10;

  //   Path path;

  //   #region Pathfinding
  //   public Vector3 Waypoint(Transform target, int pathIndex)
  //   {
  //     this._target = target;
  //     StartCoroutine(UpdatePath());
  //   }

  //   public void OnPathFound(Vector3[] waypoints, bool pathSuccessful)
  //   {
  //     if (pathSuccessful)
  //     {
  //       path = new Path(waypoints, transform.position, turnDst, stoppingDst);

  //       StopCoroutine("FollowPath");
  //       StartCoroutine("FollowPath");
  //     }
  //   }

  //   IEnumerator UpdatePath()
  //   {

  //     if (Time.timeSinceLevelLoad < .3f)
  //     {
  //       yield return new WaitForSeconds(.3f);
  //     }
  //     PathRequestManager.RequestPath(new PathRequest(transform.position, target.position, OnPathFound));

  //     float sqrMoveThreshold = pathUpdateMoveThreshold * pathUpdateMoveThreshold;
  //     Vector3 targetPosOld = target.position;

  //     while (true)
  //     {
  //       yield return new WaitForSeconds(minPathUpdateTime);
  //       //print(((target.position - targetPosOld).sqrMagnitude) + "    " + sqrMoveThreshold);
  //       if ((target.position - targetPosOld).sqrMagnitude > sqrMoveThreshold)
  //       {
  //         PathRequestManager.RequestPath(new PathRequest(transform.position, target.position, OnPathFound));
  //         targetPosOld = target.position;
  //       }
  //     }
  //   }

  //   public Vector3 Waypoint(){}
  //   IEnumerator FollowPath()
  //   {
  //     bool followingPath = true;
  //     int pathIndex = 0;

  //     float speedPercent = 1;

  //     while (followingPath)
  //     {
  //       Vector2 pos2D = new Vector2(transform.position.x, transform.position.z);
  //       while (path.turnBoundaries[pathIndex].HasCrossedLine(pos2D))
  //       {
  //         if (pathIndex == path.finishLineIndex)
  //         {
  //           followingPath = false;
  //           break;
  //         }
  //         else
  //         {
  //           pathIndex++;
  //         }
  //       }

  //       if (followingPath)
  //       {

  //         if (pathIndex >= path.slowDownIndex && stoppingDst > 0)
  //         {
  //           speedPercent = Mathf.Clamp01(path.turnBoundaries[path.finishLineIndex].DistanceFromPoint(pos2D) / stoppingDst);
  //           if (speedPercent < 0.01f)
  //           {
  //             followingPath = false;
  //           }
  //         }
  //       }

  //       yield return null;

  //     }
  //   }

  //   public void OnDrawGizmos()
  //   {
  //     if (path != null)
  //     {
  //       path.DrawWithGizmos();
  //     }
  //   }
  //   #endregion
  }
}