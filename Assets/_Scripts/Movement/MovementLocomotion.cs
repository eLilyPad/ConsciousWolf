using UnityEngine;

namespace Lily.MovementSystem.Locomotion
{
	using Controller;

	[RequireComponent(typeof(Rigidbody))]
	[RequireComponent(typeof(Transform))]
	public class MovementLocomotion : MonoBehaviour
	{
		#region Variables
		Rigidbody rb;
		MovementController mc;
		[SerializeField] Animator anim;
		//-----------------------------------------------------------
		//Movement Parameters
		[SerializeField] Transform orientation;
		float _acceleration;
		float maxSpeed = 10f;
		float currentSpeed;

		bool slowingDown;
		//-----------------------------------------------------------
		//Travel Directions
		float sidewaysMovement;
		float forwardMovement;

		// float velocityX;
		// float velocityZ;
		Vector3 moveDirection;
		//-----------------------------------------------------------
		//The Type of Movement
		protected MoveType moveType;
		//-----------------------------------------------------------
		float timeCount = 0.0f;

		#endregion

		#region Setup
		void Awake()
		{
			//conditions = GetComponent<MovementConditions>();
			// //orientation = GetComponent<Transform>();
			// this.maxSpeed = movementSettings.MaxSpeed;
			// this.acceleration = movementSettings.Acceleration;
			anim = GetComponent<Animator>();

		}

		#endregion
		void Move(int _forwardMovement, int _horiMovement, float acceleration)
		{
			forwardMovement = _forwardMovement;
			sidewaysMovement = _horiMovement;

			//this.moveType = moveType;
			moveDirection = ((orientation.forward * forwardMovement + orientation.right * sidewaysMovement));
			moveDirection.Normalize();
			//Debug.Log(moveDirection);
			Vector3 movementDir = new Vector3(sidewaysMovement, 0, forwardMovement);
			//bool isGrounded = mc.IsGrounded();
			//rb.AddForce(moveDirection.normalized * acceleration * movementMultiplier * Time.deltaTime, ForceMode.Acceleration);
			// if(isGrounded)
			// {
			switch (moveType)
			{
				case MoveType.Walking:
					//Walk(rb, movementDir, acceleration);
					Walk(rb, moveDirection, acceleration);
					break;
				case MoveType.Running:
					Running(movementDir, acceleration);
					break;
				case MoveType.Strafing:
					Strafe(movementDir, acceleration);
					break;
				case MoveType.Jumping:
					Jump(movementDir, acceleration);
					break;
				default:
					Resting();
					break;
			}
			// }
			// else
			// {
			//     switch (moveType)
			//     {
			//         case MoveType.Jumping:
			//             AirJump(moveDirection, acceleration);
			//             break;
			//         default:
			//             Resting();
			//             break;
			//     }
			// }
			//ControlDrag();
			//stickFloor();
		}
		
		#region Walking
		public void WalkForward(Rigidbody rb, float acceleration)
		{
			moveType = MoveType.Walking;
			Move(1, 0, acceleration);
			ControlDrift();
		}

		void Walk(Rigidbody rb, Vector3 direction, float acceleration)
		{
			// slow movement forward
			//rb.AddRelativeForce(direction * acceleration * Time.deltaTime, ForceMode.Acceleration);
			rb.AddForce(direction * acceleration * Time.deltaTime);
			Debug.Log("Walking");
		}

		#endregion

		#region Running

		void Running(Vector3 direction, float acceleration)
		{
			//slow movement forward
			//Debug.Log("Running");
			rb.AddForce(direction.normalized * (acceleration * 2) * Time.deltaTime, ForceMode.Acceleration);
		}

		void Sprinting(Vector3 direction, float acceleration)
		{
			rb.AddForce(transform.forward * (acceleration * 3) * Time.deltaTime);
		}

		#endregion
		
		#region Jumping
		void Jump(Vector3 direction, float acceleration)
		{
			//jump
			Debug.Log("Jumping");
			rb.AddForce(transform.up * acceleration * Time.deltaTime);
		}

		void AirJump(Vector3 direction, float acceleration)
		{
			Debug.Log("Air Jumping");
			rb.AddForce(transform.up * acceleration * Time.deltaTime);
		}
		#endregion

		#region Haulting

		void Resting()
		{
			//Debug.Log("Resting");
			ZeroVelocity();
		}
		public void StopMoving(float stoppingDistance = 0f)
		{
			moveType = MoveType.Resting;
			Move(0, 0, 0);
			Debug.Log(gameObject.ToString() + " is Stopping");
		}
		#endregion

		void Strafe(Vector3 direction, float acceleration)
		{
			//Side step
			Debug.Log("Strafing");
			rb.AddForce(transform.right * acceleration * Time.deltaTime);
		}

		void ControlDrag()
		{
			float currentMaxSpeed = maxSpeed;
			currentSpeed = Vector3.Magnitude(rb.velocity);
			//Vector3 maxVelocity = new Vector3(maxSpeed, maxSpeed, maxSpeed);
			if (currentSpeed > currentMaxSpeed)
			{
				//float dragPercentage = currentSpeed / currentMaxSpeed;
				float dragSpeed = currentSpeed - currentMaxSpeed;  // calculate the speed decrease

				Vector3 normalisedVelocity = rb.velocity.normalized;
				Vector3 dragVelocity = normalisedVelocity * (dragSpeed);  // make the brake Vector3 value

				rb.AddForce(-dragVelocity);  // apply opposing brake acceleration
													  //Debug.Log(gameObject.name + ", Current dragVelocity: " + dragVelocity);
			}
			//Debug.Log(CheckRotation());
			//SlowToRotate();
		}
		void ControlDrift()
		{
			float velocityX = rb.velocity.x;
			if (velocityX < 0.5f || velocityX > -0.5f)
			{
				Vector3 xVelocity = new Vector3(velocityX * 2, 0, 0);
				rb.AddForce(-xVelocity * Time.deltaTime);
				//Debug.Log(xVelocity);
			}
		}
		void stickFloor()
		{
			RaycastHit hit;
			Ray ray = new Ray(transform.position, -transform.up);
			if (Physics.Raycast(ray, out hit))
			{
				transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
			}
			//transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal);

		}
		void ZeroVelocity()
		{
			rb.velocity = Vector3.zero;
		}
	}
	//TODO dictionary for MovementTypes
	public enum MoveType
	{
		Walking,
		Running,
		Strafing,
		Jumping,
		Resting
	}
}