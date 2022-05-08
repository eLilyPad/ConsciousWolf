

using UnityEngine;

namespace Lily.MathsUtils
{
  public class MathsUtils
  {

    /// <summary>
    /// Converts the Vector3 into a Vector2
    /// </summary>
    /// <param name="vector3"></param>
    /// <returns> Returns (X, Z) of the vector3 as a Vector2</returns>
    public static Vector2 V3ToV2(Vector3 vector3)
    {
      return new Vector2(vector3.x, vector3.z);
    }
    /// <summary>
    /// Gives the Distance from the center of the Vector
    /// </summary>
    /// <param name="point"></param>
    /// <param name="LinePointA"></param>
    /// <param name="LinePointB"></param>
    /// <returns></returns>
    public static float DistanceToClosestVector3(Vector3 point, Vector3 linePointA, Vector3 linePointB)
    {
      Vector3 AB = linePointB - linePointA;

      float l2 = Vector3.Dot(AB, AB);

      if (l2 == 0)
      {
          return Vector3.Distance(point, linePointA);
      }

      float t = Vector3.Dot(point - linePointA, AB) / l2;

      if (t < 0)
      {
          return Vector3.Distance(point, linePointA);
      }

      if (t > 1)
      {
          return Vector3.Distance(point, linePointB);
      }

      Vector3 closestPoint = Vector3.Lerp(linePointA, linePointB, t);
      return Vector3.Distance(point, closestPoint);
    }

    public static float[] CalculateDistances(Vector3[] points, Vector3? start = null)
    {
      Vector3 startPosition = (Vector3)start;
      if (start == null) startPosition = points[0];

      float[] distances = new float[points.Length];
      distances[0] = Vector3.Distance(points[0], startPosition);

      for (int i = 0; i < points.Length - 1; i++)
      {
        distances[i + 1] = distances[i] + Vector3.Distance(points[i], points[i + 1]);
      }

      return distances;
    }
  }
}