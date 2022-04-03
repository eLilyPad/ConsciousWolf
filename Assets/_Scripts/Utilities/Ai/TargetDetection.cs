using UnityEngine;

namespace Lily.Ai.Utilities
{
	public class TargetDetection
	{
    public Transform ClosestTargetWithTag(Transform center, string targetTag)
    {  
      GameObject[] targets = GameObject.FindGameObjectsWithTag(targetTag);
        //Looks for targets with defined tag
      Transform closestTarget = null;
        //resets the closest targets transform
      float closestDistance = Mathf.Infinity;
      
      foreach (GameObject target in targets)
      {
        float distanceFromTarget = Vector3.Distance(center.position, target.transform.position);
        if(distanceFromTarget < closestDistance)
        { 
          closestTarget = target.transform;
          closestDistance = distanceFromTarget;
        }
      }
      return closestTarget ?? center;
    }
  }
}