using UnityEngine;

namespace Lily.Ai.ActionStates
{
	using Ai.Pathfinder;
	using Ai;
	using Lily.MovementSystem.Controller;
	public class FollowPath : IState
	{
		private BasicAI _ai;
		private Vector3 _lastPosition = Vector3.zero;
		MovementController _mc;
		Path _path;
		int pathIndex;

		private readonly Rigidbody _rb;

		public FollowPath(BasicAI ai, Path path, Rigidbody rb)
		{
			_ai = ai;
			_path = path;
			//_mc = ai.mc;
			_rb = rb;
		}

			#region [black] State Methods
		public void Tick()
		{
			LookAtLookPoint(pathIndex);
			MoveForward();
			CheckProgress();
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
		}
		void MoveForward()
		{
			Vector3 direction = new Vector3(-1,0,0);
			
			//	_ai.transform.Translate(Vector3.forward * Time.deltaTime * _ai.acceleration * 10, Space.Self);
			_rb.AddRelativeForce((Vector3.forward * _ai.acceleration * Time.deltaTime) * 100, ForceMode.Acceleration);
		}

		void StopMoving()
		{
			_rb.velocity = Vector3.zero;
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