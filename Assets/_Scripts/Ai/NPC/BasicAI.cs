using UnityEngine;
using UnityEngine.VFX;
namespace Lily.Ai
{
  using Lily;
  using StateMachine;
  using Pathfinder;
  using Lily.MovementSystem;

  public class BasicAI : Entity
	{
		#region Variables

			protected BasicStateMachine _stateMachine;
			
			public Transform Target;

			public Vector3 waypoint;
			public string targetTag;

			public float acceleration;
    	public float turnDst = 5;
  		public float stoppingDst = 10;
			public Path currentPath;
			public int pathIndex;
			public bool PathComplete = false;
			public bool AtWayPoint = false;
			public float AttackRange = 3;

			public bool drawGizmos = false;

			public PathPlanner planner;

			public VisualEffect deathEffect;

			public GameObject TargetObj;

		#endregion

		void Awake()
		{
			// steering = GetComponent<MovementController>();
      rb = GetComponent<Rigidbody>();
		}

		private void Update() 
		{
		}

		public bool CheckAttackRange()
		{
			float distanceFromTarget = Vector3.Distance(this.transform.position, Target.position);

			if (distanceFromTarget <= AttackRange) return true;

			return false;
			//audioManager.PlayRandomSound();
		}

		public void OnDrawGizmos() 
		{
			if (drawGizmos)
			{
				if (currentPath != null) 
				{
					currentPath.DrawWithGizmos ();
				}
			}
		}
	}
}