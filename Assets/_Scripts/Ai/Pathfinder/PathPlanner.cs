using UnityEngine;
using System.Collections;

namespace Lily.Ai.Pathfinder
{
  public class PathPlanner : MonoBehaviour
  {  
    public bool PathReady => path != null;

    protected BasicAI _ai;

    public Transform[] availableTargets;
    protected Path path;

    protected Path prevPath;

    public PathPlanner instance;

    

    private void Awake()   
		{}

		#region Target Selection 
			// finds all targets with tags
			// select the closest target and generate path to it
			// does the same to the next target

				// float distanceFromTarget = Vector3.Distance(ai.transform.position, target.transform.position);
				// if (distanceFromTarget < closestDistance)
				// {
				// 	closestTarget = target.transform;
				// 	closestDistance = distanceFromTarget;
				// 	ai.oldTarget = TheNearestWithTag().name;
				// }
		}
			public void TargetWithTag(string targetTag)
			{
				GameObject[] targets = GameObject.FindGameObjectsWithTag(targetTag);

				foreach (GameObject target in targets)
				{

				}
			}
		#endregion
		#region ConstructPath
			// creates a path from the current position and target position 
		#endregion
		#region ListPaths
			// creates List of paths to take
		#endregion
		#region MergePaths
			//Merge Paths to chain the paths together
		#endregion
		#region ChangePath
			//Switch from the current path to the new path
		#endregion

  }
}