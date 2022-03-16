using UnityEngine;

namespace Lily.Ai.ActionStates
{
	using Ai;
	public class FindClosestTargetWithTag : IState
	{
		private readonly BasicAI ai;
		private readonly string targetTag;
		public FindClosestTargetWithTag(BasicAI _ai, string _targetTag)
		{
			ai = _ai;
			targetTag = _targetTag;
		}
		public void Tick()
		{
			ai.target = TheNearestWithTag();
			
		}

		private Transform TheNearestWithTag()
		{
			GameObject[] targets = GameObject.FindGameObjectsWithTag(targetTag);
			Transform closestTarget = null;
			float closestDistance = Mathf.Infinity;

			foreach (GameObject target in targets)
			{

				float distanceFromTarget = Vector3.Distance(ai.transform.position, target.transform.position);
				if (distanceFromTarget < closestDistance)
				{
					closestTarget = target.transform;
					closestDistance = distanceFromTarget;
					ai.oldTarget = TheNearestWithTag().name;
				}
			}

			return closestTarget;
		}
		public void OnEnter() { }
		public void OnExit() { }
	}
}