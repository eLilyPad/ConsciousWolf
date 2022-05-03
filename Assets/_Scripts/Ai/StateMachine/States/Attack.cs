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

			if(ai.TargetObj != null) AttackMove();
    }

		void StopMoving()
		{
			_rb.velocity = Vector3.zero;
		}
		void AttackMove()
		{
			// int ID = tmp.entityID;
			// Debug.Log("Attack " + ai.TargetObj.name);
			// e.EntityManager.InstaKill(ID);
			ai.TargetObj.GetComponent<Entity>().TakeDamage(10);

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