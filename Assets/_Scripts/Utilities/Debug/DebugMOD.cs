using UnityEngine;

namespace Lily
{
	public class DebugMOD : UnityEngine.Debug
	{
    /// <summary>
		/// Creates a debug cross at the given position in the scene view to help with debugging.
		/// </summary>
		public static void DebugCross(Vector3 position, float size = 0.5f, Color color = default(Color), float duration = 0f, bool depthTest = true)
		{
			Vector3 xStart = position + Vector3.right * size * 0.5f;
			Vector3 xEnd = position - Vector3.right * size * 0.5f;

			Vector3 yStart = position + Vector3.up * size * 0.5f;
			Vector3 yEnd = position - Vector3.up * size * 0.5f;

			Vector3 zStart = position + Vector3.forward * size * 0.5f;
			Vector3 zEnd = position - Vector3.forward * size * 0.5f;

			UnityEngine.Debug.DrawLine(xStart, xEnd, color, duration, depthTest);
			UnityEngine.Debug.DrawLine(yStart, yEnd, color, duration, depthTest);
			UnityEngine.Debug.DrawLine(zStart, zEnd, color, duration, depthTest);
		}
  }
}