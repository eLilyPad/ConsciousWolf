using UnityEngine;

namespace Lily.Ai.ActionStates
{
	using Ai;
	public class Rest : IState
	{
		private readonly BasicAI ai;
		private readonly Rigidbody _rb;
		public Rest(BasicAI _ai, Rigidbody rb)
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
		public void OnEnter() { }
		public void OnExit() { }
	}
}