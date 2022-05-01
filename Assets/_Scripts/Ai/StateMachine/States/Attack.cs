using UnityEngine;

namespace Lily.Ai.ActionStates
{
	using Ai;
	public class Attack : IState
	{
		private readonly BasicAI ai;
		private readonly Rigidbody _rb;
		public Attack(BasicAI Attacker, Rigidbody rb)
		{
			ai = Attacker;
			_rb = rb;
		}
		public void Tick()
		{
			StopMoving();
    }

		void StopMoving()
		{
			_rb.velocity = Vector3.zero;
		}
		void AttackMove()
		{
			if(!ai.TargetObj.TryGetComponent<BasicAI>(out BasicAI tmp)) Debug.Log("Attack Failed TryGetComponent");

			// int ID = tmp.entityID;
			// Debug.Log("Attack " + ID);
			// e.EntityManager.InstaKill(ID);
			ai.TargetObj.GetComponent<BasicAI>().TakeDamage(10);

			// e.GameManager.InstaKill(ID);
			// ai.Target = null;
		}
		public void OnEnter()
    {
			AttackMove();
    }
		public void OnExit() { }
	}
}