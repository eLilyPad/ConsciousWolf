using UnityEngine;

namespace Lily.Ai.ActionStates
{
	using Ai.Pathfinder;
	using Ai;
	using Lily.MovementSystem.Controller;
	public class FollowPath : IState
	{
		private BasicAI _ai;
		private readonly Rigidbody _rb;

		private Vector3 _lastPosition = Vector3.zero;
		MovementController _mc;
		Path _path;
		int pathIndex;

		float MsTurnPenalty = 1;
		public FollowPath(BasicAI ai, Path path, Rigidbody rb)
		{
			_ai = ai;
			_path = path;
			//_mc = ai.mc;
			_rb = rb;
		}

	#region [teal] State Methods
		public void Tick()
		{
			LookAtLookPoint(pathIndex);
			MoveForward();
			CheckProgress();
			//ControlDrag();
			if(_ai.newPathFound == true)
			{
				pathIndex = 0;
			}
		}
		public void OnEnter()
		{
			pathIndex = 0;
			_ai.PathComplete = false;
		}
		public void OnExit()
		{}

	#endregion

	#region [red] Movement 
		void LookAtLookPoint(int pathIndex)
		{
			_ai.transform.LookAt(_ai.currentPath.lookPoints[pathIndex]);
			//RotateYToTarget(_ai.currentPath.lookPoints[pathIndex], _ai.turnSpeed, false);
		}
		void MoveForward()
		{
			Vector3 direction = new Vector3(-1,0,0);
			float movementSpeed = _ai.acceleration / MsTurnPenalty;
			_ai.transform.Translate(Vector3.forward * Time.deltaTime * movementSpeed, Space.Self);
			//_rb.AddRelativeForce((Vector3.forward * _ai.acceleration * Time.deltaTime) * 100, ForceMode.Acceleration);
		}

		void StopMoving()
		{
			_rb.velocity = Vector3.zero;
		}

		void RotateYToTarget(Vector3 target, float turnSpeed, bool smoothing = true)
		{
			target.Normalize();
			
			
			float targetRotation = -1 * (Mathf.Atan2(target.z, target.x) * Mathf.Rad2Deg);;
			float rotation = targetRotation;
			/* If we have a non-zero direction then look towards that direciton otherwise do nothing */
			if (target.sqrMagnitude > 0.001f)
			{
				if(smoothing)
				{
					/* Mulitply by -1 because counter clockwise on the y-axis is in the negative direction */
					rotation = Mathf.LerpAngle(_ai.transform.rotation.eulerAngles.y, targetRotation, Time.deltaTime * turnSpeed);
					_ai.transform.rotation = Quaternion.Euler(0, rotation, 0);
				}else
				{
					//_ai.transform.rotation = Quaternion.Euler(0, rotation, 0);
					_ai.transform.LookAt(target);
				}
				
				// if(rotation > _ai.turnThreshold)
				// {
				// 	MsTurnPenalty = 2;
				// }else
				// {
				// 	MsTurnPenalty = 1;
				// }
				//TurnPenalty(toRotation);
				
				//_ai.transform.LookAt(targetRotation);
			}

			
		}

		void TurnPenalty(float turnAmount)
		{
			float movementPenalty = _ai.acceleration / (turnAmount * _ai.turnPenalty);

			_rb.AddRelativeForce((-Vector3.forward * movementPenalty * Time.deltaTime) * 100, ForceMode.Acceleration);
		}

		void ControlDrag()
		{
			float currentMaxSpeed = _ai.maxSpeed;
			float currentSpeed = Vector3.Magnitude(_ai.rb.velocity);
			//Vector3 maxVelocity = new Vector3(maxSpeed, maxSpeed, maxSpeed);
			if (currentSpeed > currentMaxSpeed)
			{
				//float dragPercentage = currentSpeed / currentMaxSpeed;
				float dragSpeed = currentSpeed - currentMaxSpeed;  // calculate the speed decrease

				Vector3 normalisedVelocity = _ai.rb.velocity.normalized;
				Vector3 dragVelocity = normalisedVelocity * (dragSpeed);  // make the brake Vector3 value

				_ai.rb.AddForce(-dragVelocity);  // apply opposing brake acceleration
													  //Debug.Log(gameObject.name + ", Current dragVelocity: " + dragVelocity);
			}
			//Debug.Log(CheckRotation());
			//SlowToRotate();
		}

	#endregion
		
	#region [blue] Pathway Conditions 
		void CheckProgress()
		{ 
			Vector2 pos2D = new Vector2(_ai.transform.position.x, _ai.transform.position.z);
			if(_ai.currentPath.turnBoundaries[pathIndex].HasCrossedLine(pos2D))
			{
				
				if (pathIndex == _ai.currentPath.finishLineIndex)
				{
					CompletedPath();
				}
				else
				{ 
					pathIndex++;
				}
			}
		}
		void CompletedPath()
		{
			_ai.PathComplete = true;

		}

	#endregion
	}
}