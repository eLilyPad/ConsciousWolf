using System.Collections;
using UnityEngine;

namespace Lily.Ai.Pathfinder
{
  using MovementSystem.Controller;
  public class WolfAI : MonoBehaviour
  {
    #region Variables

      #region Pathfinding
        const float minPathUpdateTime = .2f;
        const float pathUpdateMoveThreshold = .5f;
        Path path;

      #endregion
      public Transform target;
      public float speed = 20;
      public float turnSpeed = 3;
      public float turnDst = 5;
      public float stoppingDst = 10;

      MovementController movementController;

    #endregion

    #region Pathfinding
      void Start()
      {
        StartCoroutine(UpdatePath());
      }

      public void OnPathFound(Vector3[] waypoints, bool pathSuccessful)
      {
        if (pathSuccessful)
        {
          //check if all waypoints are correct
          path = new Path(waypoints, transform.position, turnDst, stoppingDst);

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

      IEnumerator FollowPath()
      {
        //signals that you are travelling path
        bool followingPath = true;
        //the initial Waypoint
        int pathIndex = 0;
        //looks at the first waypoint
        transform.LookAt(path.lookPoints[0]);//extract V3

        float speedPercent = 1;

        while (followingPath)
        {
          //finds curent point of the path
          Vector2 pos2D = new Vector2(transform.position.x, transform.position.z);
          while (path.turnBoundaries[pathIndex].HasCrossedLine(pos2D))
          {
            if (pathIndex == path.finishLineIndex)
            {
              followingPath = false;
              break;//Stops following Path when end is reached
            }
            else
            {
              pathIndex++;//goes to the next waypoint
            }
          }

          if (followingPath)
          {

            if (pathIndex >= path.slowDownIndex && stoppingDst > 0)
            {
              speedPercent = Mathf.Clamp01(path.turnBoundaries[path.finishLineIndex].DistanceFromPoint(pos2D) / stoppingDst);
              if (speedPercent < 0.01f)
              {
                followingPath = false;
              }
            }

            Quaternion targetRotation = Quaternion.LookRotation(path.lookPoints[pathIndex] - transform.position);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
            transform.Translate(Vector3.forward * Time.deltaTime * speed * speedPercent, Space.Self);
          }

          yield return null;

        }
      }

      public void OnDrawGizmos()
      {
        if (path != null)
        {
          path.DrawWithGizmos();
        }
      }

    #endregion
  }
}