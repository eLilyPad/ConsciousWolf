using UnityEngine;

namespace Lily.Ai.ActionStates
{
	using Ai;
	public class Attack : IState
	{
		private readonly BasicAI ai;
		private readonly Rigidbody _rb;
		public Attack(BasicAI _ai, Rigidbody rb)
		{
			ai = _ai;
			_rb = rb;
		}
		public void Tick()
		{
			StopMoving();
			AttackMove();
    }

		void StopMoving()
		{
			_rb.velocity = Vector3.zero;
		}
		void AttackMove()
		{
			ai.TargetObj.TryGetComponent<Entity>(out Entity e);

			int ID = e.entityID;
			// Debug.Log("Attack " + e.Name);
			e.EntityManager.InstaKill(ID);
		}
		public void OnEnter()
    {
			
    }
		public void OnExit() { }
	}
}