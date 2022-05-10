using UnityEngine;

namespace Lily.Ai.ActionStates
{
	using Ai.Pathfinder;
	using Ai;
  using Lily.Utilities;
  using System.Collections.Generic;

  public class MoveToTarget : IState
	{
		private BasicAI _ai;
		private readonly Rigidbody _rb;

		int numSamplesForSmoothing = 5;
    Queue<Vector3> velocitySamples = new Queue<Vector3>();

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
			if(WithinRangeOfTarget())StopMoving();
		}
		public void OnEnter()
		{}
		public void OnExit()
		{}

	#endregion

	#region [red] Movement 
		void LookAtWaypoint()
		{
			Vector3 direction = new Vector3(_ai.waypoint.x, _ai.transform.position.y, _ai.waypoint.z);
			// direction.Normalize();
			LookAtDirection(MathsUtils.SmoothSamples(_ai.waypoint));
			//RotateYToTarget(_ai.waypoint, _ai.turnSpeed, false);
		}

		void LookAtDirection(Vector3 direction)
		{
			direction.Normalize();

			if (direction.sqrMagnitude > 0.001f)
      {
				float toRotation = (Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg);
        float rotation = Mathf.LerpAngle(_rb.rotation.eulerAngles.y, toRotation, Time.deltaTime * _ai.turnSpeed);
        
				_rb.rotation = Quaternion.Euler(0, rotation, 0);
			}
		}
		void MoveForward()
		{
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
			if(IsLookingAt(_ai.waypoint, 10))
			{
				movementSpeed = _ai.acceleration;
			}
			else
			{
				movementSpeed = _ai.acceleration/2;
			}

			return movementSpeed;
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

		public bool WithinRangeOfTarget()
		{
			float distanceFromTarget = Vector3.Distance(_ai.transform.position, _ai.Target.position);

			if (distanceFromTarget <= _ai.AttackRange) return true;

			return false;
		}

	#endregion
	}
}