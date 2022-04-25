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
			// if(ai.manager.HasSpawn())
			// {
			// 	GetTarget();
			// }
			while (ai.Target == null) GetTarget();
		}

		void GetTarget()
		{
			//ai.Target = ClosestTarget().transform;
			ai.AttackTarget = ClosestTarget();
			//ai.Target = ai.AttackTarget.transform;
		}
		private GameObject ClosestTarget()
		{
			return ai.manager.GetClosestTarget(ai.transform.position);
		}
		public void OnEnter() {}
		public void OnExit() {}
	}
}