using UnityEngine;

namespace Lily.Ai.ActionStates
{
	using Ai.Pathfinder;
	using Ai;
	public class MoveToTarget : IState
	{
		private BasicAI ai;
		private readonly Rigidbody rb;


		float MsTurnPenalty = 1;
		public MoveToTarget(BasicAI _ai, Rigidbody _rb)
		{
			ai = _ai;
			rb = _rb;
		}

	#region [teal] State Methods
		public void Tick()
		{
			// LookAtWaypoint();
			// MoveForward();
			// if(WithinRangeOfTarget())StopMoving();

			FX();
		}
		public void OnEnter()
		{
			// ai.followPath.
		}
		public void OnExit()
		{}

	#endregion

	#region [red] Movement 
  	
		void FX()
		{
			Path path = ai.currentPath;
			Vector3 acceleration = ai.followPath.GetSteering(path);

			ai.followPath.Steer(acceleration);
			ai.followPath.LookWhereYoureGoing();
		}
		void LookAtWaypoint()
		{
			Vector3 direction = new Vector3(ai.waypoint.x, ai.transform.position.y, ai.waypoint.z);
			// direction.Normalize();
			ai.transform.LookAt(direction);
			//RotateYToTarget(_ai.waypoint, _ai.turnSpeed, false);
		}
		void MoveForward()
		{
			Vector3 direction = new Vector3(-1,0,0);
			ai.transform.Translate(Vector3.forward * Time.deltaTime * GetSpeed(), Space.Self);
			//_rb.AddRelativeForce((Vector3.forward * _ai.acceleration * Time.deltaTime) * 100, ForceMode.Acceleration);
		}

		void StopMoving()
		{
			rb.velocity = Vector3.zero;
		}

		float GetSpeed()
		{
			float movementSpeed = ai.acceleration;
			if(IsLookingAt(ai.waypoint))
			{
				movementSpeed = movementSpeed/2;
			}
			return movementSpeed;
		}

		bool IsLookingAt(Vector3 target, float angleThreshold = 0.5f)
		{
			Vector3 facing = ai.transform.right.normalized;

			Vector3 directionToTarget = (target - ai.transform.position);
			directionToTarget.Normalize();

			return Vector3.Dot(facing, directionToTarget) >= angleThreshold;
		}
	#endregion
		
	#region [blue] Movement Conditions 
		void CheckProgress()
		{ 
      float distanceFromTarget = Vector3.Distance(ai.transform.position, ai.waypoint);
			if(distanceFromTarget < ai.stoppingDst)
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
			ai.AtWayPoint = true;
		}

		void NotAtWaypoint()
		{
			ai.AtWayPoint = false;
		}

		public bool WithinRangeOfTarget()
		{
			float distanceFromTarget = Vector3.Distance(ai.transform.position, ai.Target.position);

			if (distanceFromTarget <= ai.AttackRange) return true;

			return false;
		}

	#endregion
	}
}