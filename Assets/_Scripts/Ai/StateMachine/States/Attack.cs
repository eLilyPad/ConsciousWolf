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
		public void OnEnter()
    {
      ai.audioManager.PlayRandomSound();
    }
		public void OnExit() { }
	}
}