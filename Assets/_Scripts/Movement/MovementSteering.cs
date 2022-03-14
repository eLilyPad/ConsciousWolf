using UnityEngine;

namespace Lily.MovementSystem.Steering
{
	[RequireComponent(typeof(Rigidbody))]
	public class MovementSteering : MonoBehaviour
	{
		#region Variables
		[SerializeField] float dotThreshold = 0.9f;

		float timeCount = 0.0f;

		private bool smoothing = true;
		private Rigidbody rb;

		#endregion

		#region Setup
		void Awake()
		{
			rb = GetComponent<Rigidbody>();
		}
		#endregion

		#region Target Steering
		public void TurnTowardsTarget(Vector3 target, float turnSpeed)
		{
			RotateToTarget(target, turnSpeed);
		}

		void RotateToTarget(Vector3 target, float turnSpeed)
		{

			target.Normalize();

			/* If we have a non-zero direction then look towards that direciton otherwise do nothing */
			if (target.sqrMagnitude > 0.001f)
			{
				/* Mulitply by -1 because counter clockwise on the y-axis is in the negative direction */
				float toRotation = -1 * (Mathf.Atan2(target.z, target.x) * Mathf.Rad2Deg);
				float rotation = Mathf.LerpAngle(transform.rotation.eulerAngles.y, toRotation, Time.deltaTime * turnSpeed);

				transform.rotation = Quaternion.Euler(0, rotation, 0);
			}
		}
		#endregion

		#region Steering Checks
		// finds the difference of the angle of the target and the current Quaternion return true if the angle is above the threshold 
		bool IsLookingAt(Vector3 target, float angleThreshold = 0f)
		{
			Vector3 facing = transform.right.normalized;

			Vector3 directionToTarget = (target - transform.position);
			directionToTarget.Normalize();

			return Vector3.Dot(facing, directionToTarget) >= angleThreshold;
		}
		#endregion
	}
}