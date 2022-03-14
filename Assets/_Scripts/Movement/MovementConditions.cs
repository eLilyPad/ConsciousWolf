using UnityEngine;

namespace Lily.MovementSystem.Conditions
{
	public class MovementConditions : MonoBehaviour
	{
		#region Variables
		private Vector3 normalVector = Vector3.up;
		[Header("Ground Detection")]
		[SerializeField] Transform groundCheck;
		[SerializeField] LayerMask groundMask;

		RaycastHit slopeHit;


		public float maxSlopeAngle = 35f;

		public bool isGrounded;
		public bool cancellingGrounded;
		
		#endregion
	
		#region Ground conditions
		
		//Check the gradient of the ground
		public bool onSlope()
		{
			if (Physics.Raycast(transform.position, Vector3.down, out slopeHit))
			{
				if (slopeHit.normal != Vector3.up)
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			return false;
		}
		//Return true if gradient is below threshold to be considered floor(Walkable)
		private bool IsFloor(Vector3 v)
		{
			float angle = Vector3.Angle(Vector3.up, v);
			return angle < maxSlopeAngle;
		}
		#endregion

		#region Ground Collisions

		private void OnCollisionStay(Collision other)
		{
			//Make sure we are only checking for walkable layers
			int layer = other.gameObject.layer;
			if (groundMask != (groundMask | (1 << layer))) return;

			//Iterate through every collision in a physics update
			for (int i = 0; i < other.contactCount; i++)
			{
				Vector3 normal = other.contacts[i].normal;
				//FLOOR
				if (IsFloor(normal))
				{
					isGrounded = true;
					cancellingGrounded = false;
					normalVector = normal;
					CancelInvoke(nameof(StopGrounded));
				}
			}

			//Invoke ground/wall cancel, since we can't check normals with CollisionExit
			float delay = 3f;
			if (!cancellingGrounded)
			{
				cancellingGrounded = true;
				Invoke(nameof(StopGrounded), Time.deltaTime * delay);
			}
		}

		private void StopGrounded()
		{
			isGrounded = false;
		}

		public bool IsGrounded()
		{
			return isGrounded;
		}
		#endregion
	}
}