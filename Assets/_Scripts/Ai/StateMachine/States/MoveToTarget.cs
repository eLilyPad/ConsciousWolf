using UnityEngine;

namespace Lily.Ai.ActionStates
{
	using Ai.Pathfinder;
	using Ai;
	public class MoveToTarget : IState
	{
		private BasicAI _ai;
		private readonly Rigidbody _rb;


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
			_ai.transform.LookAt(direction);
			//RotateYToTarget(_ai.waypoint, _ai.turnSpeed, false);
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