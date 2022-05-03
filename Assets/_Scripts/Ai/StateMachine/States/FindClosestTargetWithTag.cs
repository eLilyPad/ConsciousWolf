using UnityEngine;

namespace Lily.Ai.ActionStates
{
  using System;
  using System.Collections.Generic;
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
			// ai.Target = TheNearestWithTag();
			// ai.TargetObj = TheNearestGOWithTag();
			
			SetTarget();
		}

		void SetTarget()
		{
			// var targetManager = ai.GameManager.GetManager(entityManagerKey);

			GameObject target;
			// = ai.GameManager.FindClosestTarget(ai.transform.position, targetManager);

			// if(target == null) 
			// {
				target = SearchClosestTarget(); 
				// Debug.Log("Failed to get target from Manager");
			// }

			ai.TargetObj = target;
			ai.Target = target.transform;
		}

		private GameObject SearchClosestTarget()
		{
			GameObject[] targets = GameObject.FindGameObjectsWithTag(targetTag);
			GameObject closestTarget = null;
			float closestDistance = Mathf.Infinity;

			foreach (GameObject target in targets)
			{

				float distanceFromTarget = Vector3.Distance(ai.transform.position, target.transform.position);
				if (distanceFromTarget < closestDistance)
				{
					closestTarget = target;
					closestDistance = distanceFromTarget;
					//ai.oldTarget = TheNearestWithTag().name;
				}
			}

			return closestTarget;
		}
		
		public void OnEnter() { }
		public void OnExit() { }
	}
}