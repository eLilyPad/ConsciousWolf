using System.Collections.Generic;
using UnityEngine;

namespace Lily.MovementSystem
{
	[RequireComponent(typeof(Rigidbody))]
	public class MovementController : MonoBehaviour
	{
		#region Parameters

				[Header("General")]
			
			public float maxVelocity = 3f;

			public float maxAcceleration = 10f;

			public float turnSpeed = 20f;

				[Header("Arrive")]
				
			/// <summary>
			/// The radius from the target that means we are close enough and have arrived
			/// </summary>
			public float targetRadius = 0.005f;

			public float slowRadius = 1f;

			public float timeToTarget = 0.1f;

				[Header("Look Smoothing")]
			
			/// <summary>
			/// Smoothing Limits exaggerated Movements by averaging them current look direction with previous directions
			/// </summary>
			public bool smoothing = true;

			public int SmoothingSampleAmount = 5;

			Queue<Vector3> velocitySamples = new Queue<Vector3>();

			protected Rigidbody rb;

		#endregion

		#region Setup
			void Awake()
			{
				rb = GetComponent<Rigidbody>();
			}
		#endregion

		#region Target Steering

			public void Steer(Vector3 linearAcceleration)
			{
				rb.velocity = linearAcceleration * Time.deltaTime;

				if (rb.velocity.magnitude > maxVelocity)
					{
						rb.velocity = rb.velocity.normalized * maxVelocity;
					}
			}

			public Vector3 Seek(Vector3 targetPosition, float maxSeekAccel)
			{
				/* Get the direction */
				Vector3 acceleration = 	(targetPosition - transform.position);

				acceleration.Normalize();

				/* Accelerate to the target */
				acceleration *= maxSeekAccel;

				return acceleration;
			}

			public Vector3 Seek(Vector3 targetPosition)
			{
				return Seek(targetPosition, maxAcceleration);
			}

			/// <summary>
			/// Makes the current game object look where he is going
			/// </summary>
			public void LookWhereYoureGoing()
			{
				Vector3 direction = rb.velocity;

				if (smoothing)
				{
					if (velocitySamples.Count == SmoothingSampleAmount)
					{
						velocitySamples.Dequeue();
					}

					velocitySamples.Enqueue(rb.velocity);

					direction = Vector3.zero;

					foreach (Vector3 v in velocitySamples)
					{
						direction += v;
					}

					direction /= velocitySamples.Count;
				}

				LookAtDirection(direction);
			}

			public void LookAtDirection(Vector3 direction)
			{
				direction.Normalize();

				/* If we have a non-zero direction then look towards that direction otherwise do nothing */
				if (direction.sqrMagnitude > 0.001f)
				{
						/* Mulitply by -1 because counter clockwise on the y-axis is in the negative direction */
						float toRotation = -1 * (Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg);
						float fromRotation = rb.rotation.eulerAngles.y;

						float progress = Time.deltaTime * turnSpeed;
						
						float rotation = Mathf.LerpAngle(fromRotation, toRotation, progress);

						rb.rotation = Quaternion.Euler(0, rotation, 0);
				}
			}

			public void LookAtDirection(Quaternion toRotation)
			{
				LookAtDirection(toRotation.eulerAngles.y);
			}

			public void LookAtDirection(float toRotation)
      {
				float rotation = Mathf.LerpAngle(rb.rotation.eulerAngles.y, toRotation, Time.deltaTime * turnSpeed);

        rb.rotation = Quaternion.Euler(0, rotation, 0);
			}

			public Vector3 Arrive(Vector3 targetPosition)
			{
				Debug.DrawLine(transform.position, targetPosition, Color.cyan, 0f, false);

				/* Get the right direction for the linear acceleration */
				Vector3 targetVelocity = targetPosition - rb.position;
				//Debug.Log("Displacement " + targetVelocity.ToString("f4"));

				/* Get the distance to the target */
				float dist = targetVelocity.magnitude;

				/* If we are within the stopping radius then stop */
				if (dist < targetRadius)
				{
					rb.velocity = Vector3.zero;
					return Vector3.zero;
				}

				/* Calculate the target speed, full speed at slowRadius distance and 0 speed at 0 distance */
				float targetSpeed;
				if (dist > slowRadius)
				{
					targetSpeed = maxVelocity;
				}
				else
				{
					targetSpeed = maxVelocity * (dist / slowRadius);
				}

				/* Give targetVelocity the correct speed */
				targetVelocity.Normalize();
				targetVelocity *= targetSpeed;

				/* Calculate the linear acceleration we want */
				Vector3 acceleration = targetVelocity - rb.velocity;
				/* Rather than accelerate the character to the correct speed in 1 second, 
					* accelerate so we reach the desired speed in timeToTarget seconds 
					* (if we were to actually accelerate for the full timeToTarget seconds). */
				acceleration *= 1 / timeToTarget;

				/* Make sure we are accelerating at max acceleration */
				if (acceleration.magnitude > maxAcceleration)
				{
					acceleration.Normalize();
					acceleration *= maxAcceleration;
				}

				return acceleration;
			}

			public Vector3 Interpose(Rigidbody target1, Rigidbody target2)
			{
				Vector3 midPoint = (target1.position + target2.position) / 2;

				float timeToReachMidPoint = Vector3.Distance(midPoint, transform.position) / maxVelocity;

				Vector3 futureTarget1Pos = target1.position + target1.velocity * timeToReachMidPoint;
				Vector3 futureTarget2Pos = target2.position + target2.velocity * timeToReachMidPoint;

				midPoint = (futureTarget1Pos + futureTarget2Pos) / 2;

				return(midPoint);
			}
			// public void TurnTowardsTarget(Vector3 target, float turnSpeed)
			// {
			// 	RotateToTarget(target, turnSpeed);
			// }

			// void RotateToTarget(Vector3 target, float turnSpeed)
			// {

			// 	target.Normalize();

			// 	/* If we have a non-zero direction then look towards that direciton otherwise do nothing */
			// 	if (target.sqrMagnitude > 0.001f)
			// 	{
			// 		/* Mulitply by -1 because counter clockwise on the y-axis is in the negative direction */
			// 		float toRotation = -1 * (Mathf.Atan2(target.z, target.x) * Mathf.Rad2Deg);
			// 		float rotation = Mathf.LerpAngle(transform.rotation.eulerAngles.y, toRotation, Time.deltaTime * turnSpeed);

			// 		transform.rotation = Quaternion.Euler(0, rotation, 0);
			// 	}
			// }
		#endregion

		#region Steering Checks
			public bool IsLookingAt(Vector3 target)
					{
							return IsLooking(target, 0);
					}
			// finds the difference of the angle of the target and the current Quaternion return true if the angle is above the threshold 
			public bool IsLooking(Vector3 target, float angleThreshold = 0f)
			{
				Vector3 facing = transform.right.normalized;

				Vector3 directionToTarget = (target - transform.position);
				directionToTarget.Normalize();

				return Vector3.Dot(facing, directionToTarget) >= angleThreshold;
			}
		#endregion
	}
}