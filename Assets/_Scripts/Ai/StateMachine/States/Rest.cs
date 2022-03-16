using UnityEngine;

namespace Lily.Ai.ActionStates
{
	using Ai;
	public class Rest : IState
	{
		private readonly BasicAI ai;
		public Rest(BasicAI _ai)
		{
			ai = _ai;
		}
		public void Tick()
		{
      Debug.Log("Resting...");
    }
		public void OnEnter() { }
		public void OnExit() { }
	}
}