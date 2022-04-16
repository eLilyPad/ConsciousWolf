using UnityEngine;

namespace Lily.Ai.ActionStates
{
	using Ai.Pathfinder;
	using Ai;
	using Lily.MovementSystem.Controller;
	public class MoveToTarget : IState
	{
		private BasicAI _ai;
		private readonly Rigidbody _rb;

		MovementController _mc;

		float MsTurnPenalty = 1;
		public MoveToTarget(BasicAI ai, Rigidbody rb)
		{
			_ai = ai;
			_rb = rb;
		}

	#region [teal] State Methods
		public void Tick()
		{
			LookAtWaypoint();
			MoveForward();
		}
		public void OnEnter()
		{}
		public void OnExit()
		{}

	#endregion

	#region [red] Movement 
		void LookAtWaypoint()
		{
			//_ai.transform.LookAt(_ai.waypoint);
			RotateYToTarget(_ai.waypoint, _ai.turnSpeed, false);
		}
		void MoveForward()
		{
			Vector3 direction = new Vector3(-1,0,0);
			_ai.transform.Translate(Vector3.forward * Time.deltaTime * GetSpeed(), Space.Self);
			//_rb.AddRelativeForce((Vector3.forward * _ai.acceleration * Time.deltaTime) * 100, ForceMode.Acceleration);
		}

		void StopMoving()
		{
			_rb.velocity = Vector3.zero;
		}

		float GetSpeed()
		{
			float movementSpeed = _ai.acceleration;
			if(IsLookingAt(_ai.waypoint))
			{
				movementSpeed = movementSpeed/2;
			}
			return movementSpeed;
		}

		void RotateYToTarget(Vector3 target, float turnSpeed, bool smoothing = true)
		{
			target.Normalize();
			
			float targetRotation = -1 * (Mathf.Atan2(target.z, target.x) * Mathf.Rad2Deg);;
			float rotation = targetRotation;
			/* If we have a non-zero direction then look towards that direciton otherwise do nothing */
			if (target.sqrMagnitude < 0.001f)
			{
				return;
			}
			if(!smoothing)
			{
				_ai.transform.LookAt(target);
				return;
			}
			/* Mulitply by -1 because counter clockwise on the y-axis is in the negative direction */
			rotation = Mathf.LerpAngle(_ai.transform.rotation.eulerAngles.y, targetRotation, Time.deltaTime * turnSpeed);
			_ai.transform.rotation = Quaternion.Euler(0, rotation, 0);

			//_ai.transform.rotation = Quaternion.Euler(0, rotation, 0);
		}
		bool IsLookingAt(Vector3 target, float angleThreshold = 0.5f)
		{
			Vector3 facing = _ai.transform.right.normalized;

			Vector3 directionToTarget = (target - _ai.transform.position);
			directionToTarget.Normalize();

			return Vector3.Dot(facing, directionToTarget) >= angleThreshold;
		}
	#endregion
		
	#region [blue] Movement Conditions 
		void CheckProgress()
		{ 
      float distanceFromTarget = Vector3.Distance(_ai.transform.position, _ai.waypoint);
			if(distanceFromTarget < _ai.stoppingDst)
			{
				AtWaypoint();
			}
      else 
      {
        NotAtWaypoint();
      }
		}
		void AtWaypoint()
		{
			_ai.AtWayPoint = true;
		}

		void NotAtWaypoint()
		{
			_ai.AtWayPoint = false;
		}

	#endregion
	}
}