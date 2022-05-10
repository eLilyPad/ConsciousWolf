using UnityEngine;
using System;

namespace Lily.Ai.Pathfinder
{
  
  using Lily.Utilities;
  public class Path
  {
    
    public Vector3[] Waypoints;
    public readonly Line[] TurnBoundaries;

    [NonSerialized] public float[] Distances;
    [NonSerialized] public float DistanceToEnd;

    public Vector3 this[int i]
    {
      get { return Waypoints[i];}

      set { Waypoints[i] = value; }
    }
    public int Length 
    {
      get { return Waypoints.Length; }
    }
    public int finishLineIndex
    {
      get { return TurnBoundaries.Length - 1; }
    }
    public Line finishLine
    {
      get { return TurnBoundaries[finishLineIndex]; }
    }
    public readonly int slowDownIndex;

/// <summary>
/// Creates a new Path
/// </summary>
/// <param name="waypoints">Waypoints.</param>
/// <param name="startPos"></param>
/// <param name="turnDst"></param>
/// <param name="stoppingDst"></param>
    public Path(Vector3[] waypoints, Vector3 startPos, float turnDst, float stoppingDst)
    {
      Waypoints = waypoints;
      TurnBoundaries = new Line[Waypoints.Length];

      // V3ToV2;
      Vector2 previousPoint = MathsUtils.V3ToV2(startPos);
      for (int i = 0; i < Waypoints.Length; i++)
      {
        Vector2 currentPoint = MathsUtils.V3ToV2(Waypoints[i]);
        Vector2 dirToCurrentPoint = (currentPoint - previousPoint).normalized;
        Vector2 turnBoundaryPoint = (i == finishLineIndex) ? currentPoint : currentPoint - dirToCurrentPoint * turnDst;
        TurnBoundaries[i] = new Line(turnBoundaryPoint, previousPoint - dirToCurrentPoint * turnDst);
        previousPoint = turnBoundaryPoint;
      }
      
      slowDownIndex = GetSlowDownIndex(Waypoints, stoppingDst);
    }

    // Move To MathsUtils
    public int GetSlowDownIndex(Vector3[] points, float stoppingDistance)
    {
      int slowIndex = 0;
      float dstFromEndPoint = 0;
      for (int i = Waypoints.Length - 1; i > 0; i--)
      {
        dstFromEndPoint += Vector3.Distance(Waypoints[i], Waypoints[i - 1]);
        if (dstFromEndPoint > stoppingDistance)
        {
          slowIndex = i;
        }
      }
      return slowIndex;
    }

    public void CalculateDistances()
    {
      Distances = MathsUtils.CalculateDistances(Waypoints);
    }

    public float GetDistanceToEnd()
    {
      CalculateDistances();

      return DistanceToEnd = Distances[Distances.Length - 1];
    }

    public int GetClosestWaypointsIndex(Vector3 position)
    {
      float closestDistance = MathsUtils.DistanceToClosestVector3(position, Waypoints[0], Waypoints[1]);
      int closestWaypoint = 0;

      for (int i = 1; i < Waypoints.Length - 1; i++)
      {
        float distance = MathsUtils.DistanceToClosestVector3(position, Waypoints[i], Waypoints[i +1]);
        
        if(distance < closestDistance)
        {
          closestDistance = distance;
          closestWaypoint = i;
        }
      }
      return closestWaypoint;
    }

    public void RemoveWaypoint(int index)
    {
      Vector3[] newWaypoints = new Vector3[Waypoints.Length - 1];

      int newNodesIndex = 0;
      for (int j = 0; j < newWaypoints.Length; j++)
      {
          if (j != index)
          {
              newWaypoints[newNodesIndex] = Waypoints[j];
              newNodesIndex++;
          }
      }

      this.Waypoints = newWaypoints;
    }
    public void DrawWithGizmos()
    {

      Gizmos.color = Color.black;
      foreach (Vector3 p in Waypoints)
      {
        Gizmos.DrawCube(p + Vector3.up, Vector3.one);
      }

      Gizmos.color = Color.white;
      foreach (Line l in TurnBoundaries)
      {
        l.DrawWithGizmos(10);
      }

    }
  }
}