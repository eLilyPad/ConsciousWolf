

using UnityEngine;

namespace Lily.MathsUtils
{
  public class MathsUtils
  {

    /// <summary> Converts the Vector3 into a Vector2
    /// </summary>
    /// <param name="vector3"></param>
    /// <returns> Returns (X, Z) of the vector3 as a Vector2</returns>
    public static Vector2 V3ToV2(Vector3 vector3)
    {
      return new Vector2(vector3.x, vector3.z);
    }
    
    #region Line Maths 

      /// <summary> Gives the distance to the closest of two points of a from a given position 
      /// </summary>
      /// <param name="position"></param>
      /// <param name="PointA">first point of line</param>
      /// <param name="PointB">second point of line</param>
      /// <returns></returns>
      public static float DistanceToClosestVector3(Vector3 position, Vector3 PointA, Vector3 PointB)
      {
        Vector3 ShortestPath = PointB - PointA;

        float angle = Vector3.Dot(ShortestPath, ShortestPath);

        if (angle == 0)
        {
          return Vector3.Distance(position, PointA);
        }

        float t = Vector3.Dot(position - PointA, ShortestPath) / angle;

        if (t < 0)
        {
          return Vector3.Distance(position, PointA); //Distance from start to pointA
        }

        if (t > 1)
        {
          return Vector3.Distance(position, PointB); //Distance from start to pointB
        }

        Vector3 closestPointOnLine = Vector3.Lerp(PointA, PointB, t);
        return Vector3.Distance(position, closestPointOnLine);
      }

      /// <summary> Finds the closests value that is within the stopping Distance
      /// </summary>
      /// <param name="points"></param>
      /// <param name="stoppingDistance"></param>
      /// <returns></returns>
      public static int LastIndex(Vector3[] points, float stoppingDistance = 0)
      {
        int lastIndex = 0;

        float[] distances = DistancesBetween(points);
        float dstFromEndPoint = 0;
        for (int i = distances.Length - 1; i > 0; i--)
        {
          dstFromEndPoint += distances[i];
          if (dstFromEndPoint > stoppingDistance)
          {
            lastIndex = i;
          }
        }
        return lastIndex;
      }
      
      /// <summary> Calculate the distances to each value from the start position
      /// </summary>
      /// <param name="points"></param>
      /// <param name="start"></param>
      /// <returns></returns>
      public static float[] DistancesTo(Vector3[] points, Vector3 start = default(Vector3))
      {
        float[] distances = new float[points.Length];
        distances[0] = Vector3.Distance(points[0], start);

        for (int i = 0; i < points.Length - 1; i++)
        {
          distances[i + 1] = distances[i] + Vector3.Distance(points[i], points[i + 1]);
        }

        return distances;
      }
      
      /// <summary> finds the distance between each pair of points 
      /// </summary>
      /// <param name="points"></param>
      /// <returns></returns>
      public static float[] DistancesBetween(Vector3[] points)
      {
        float[] distances = new float[points.Length];

        for (int i = 0; i < points.Length - 1; i++)
        {
          distances[i + 1] = Vector3.Distance(points[i], points[i + 1]);
        }

        return distances;
      }

      public static float DistanceToEnd(Vector3[] points, float stoppingDistance = 0)
      {
        float[] distances = DistancesTo(points);
        float distanceToEnd = distances[LastIndex(points, stoppingDistance)];

        return distanceToEnd;
      }

      public static int ClosestSegment(Vector3[] points, Vector3 position)
      {
        float closestDistance = MathsUtils.DistanceToClosestVector3(position, points[0], points[1]);
        int closestWaypoint = 0;

        for (int i = 1; i < points.Length - 1; i++)
        {
          float distance = MathsUtils.DistanceToClosestVector3(position, points[i], points[i + 1]);

          if (distance < closestDistance)
          {
            closestDistance = distance;
            closestWaypoint = i;
          }
        }
        return closestWaypoint;
      }


      public static Vector3 PositionOnLine(Vector3[] points, float progress)
      {
        float[] distances = DistancesTo(points);
        float distanceToEnd = DistanceToEnd(points);

        Mathf.Clamp(progress, 0, distanceToEnd);

        /* Find the first node that is farther than given progress */
        int i = 0;
        for (; i < distances.Length; i++)
        {
          if (distances[i] > progress)
          {
            break;
          }
        }

        /* Convert it to the first node of the line segment that the progress is in */
        if (i > distances.Length - 2)
        {
          i = distances.Length - 2;
        }
        else
        {
          i -= 1;
        }

        /* Get how far along the line segment the progress is */
        float t = (progress - distances[i]) / Vector3.Distance(points[i], points[i + 1]);

        return Vector3.Lerp(points[i], points[i + 1], t);
      }

      public static float PositionOnLine(Vector3 position, Vector3[] points)
      {
        float progress = 0;
        int closestSegment = ClosestSegment(points, position);
        float[] distances = DistancesTo(points);

        progress = distances[closestSegment] + DistanceToClosestVector3(position, points[closestSegment], points[closestSegment + 1]);

        return progress;
      }
    #endregion
  }
}