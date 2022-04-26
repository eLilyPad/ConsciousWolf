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
    }

		void StopMoving()
		{
			_rb.velocity = Vector3.zero;
		}
		void AttackMove()
		{
			int layerMask = 0;
			ai.TakeDamage(1);
			RaycastHit hit;
			if (Physics.Raycast(ai.transform.position, ai.Target.position, out hit, Mathf.Infinity, layerMask))
			{
				Debug.DrawRay(ai.transform.position, ai.Target.position * hit.distance, Color.yellow);
				Debug.Log("Did Hit");
				
				if(hit.collider.gameObject == null) 
				{
					Debug.Log("Miss Hit: gameObject = null"); 
					return;
				}
				BasicAI hitAi = hit.collider.gameObject.GetComponent<BasicAI>();
				hitAi.AIManager.SetActive(hitAi.GetInstanceID());
			}
		}
		public void OnEnter()
    {
			Debug.Log("Attacking "+ ai.name);
      // ai.audioManager.PlayRandomSound();
    }
		public void OnExit() { }
	}
}