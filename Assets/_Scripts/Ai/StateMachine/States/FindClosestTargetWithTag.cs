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
			ai.Target = TheNearestWithTag().transform;
			ai.AttackTarget = TheNearestWithTag();
		}

		private GameObject TheNearestWithTag()
		{
			GameObject[] targets = GameObject.FindGameObjectsWithTag(targetTag);
			float closestDistance = Mathf.Infinity;
			GameObject target = null;

			foreach (GameObject t in targets)
			{
				target = t;
				float distanceFromTarget = Vector3.Distance(ai.transform.position, target.transform.position);
				if (distanceFromTarget < closestDistance)
				{
					closestDistance = distanceFromTarget;
					//ai.oldTarget = TheNearestWithTag().name;
					return target;
				}
			}

			return target;
		}
		public void OnEnter() { }
		public void OnExit() { }
	}
}