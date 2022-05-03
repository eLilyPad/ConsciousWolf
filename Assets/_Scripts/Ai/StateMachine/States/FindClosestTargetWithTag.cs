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
			
			if (!SetTarget()) StopMoving(); // sets the target to self if no target is foun
		}

    private void StopMoving()
    {
      ai.TargetObj = ai.gameObject;
    }

    bool SetTarget()
		{
			// var targetManager = ai.GameManager.GetManager(entityManagerKey);

			GameObject target;

			target = SearchClosestTarget(); 

			bool isAnyTargets = SearchClosestTarget() == null ? false : true;

			if(isAnyTargets)
			{
				ai.TargetObj = target;
				ai.Target = target.transform;
			}

			return isAnyTargets; // return false if no targets are active 
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