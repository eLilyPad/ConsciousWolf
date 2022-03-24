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
		private bool completedPath;
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
		public void Tick()
		{
			LookAtLookPoint(pathIndex);
			MoveForward();
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
		public void OnEnter()
		{
			pathIndex = 0;
		}
		public void OnExit()
		{}
		
		void FollowWayPoint(int pathIndex)
		{
			LookAtLookPoint(pathIndex);
		}
		void LookAtLookPoint(int pathIndex)
		{
			_ai.transform.LookAt(_ai.currentPath.lookPoints[pathIndex]);
			// Quaternion targetRotation = Quaternion.LookRotation(_ai.currentPath.lookPoints[pathIndex] - _ai.transform.position);
      // _ai.transform.rotation = Quaternion.Lerp(_ai.transform.rotation, targetRotation, Time.deltaTime * _ai.turnSpeed);
		}
		void MoveForward()
		{
			Vector3 direction = new Vector3(1,0,0);
			
      _ai.transform.Translate(Vector3.forward * Time.deltaTime * _ai.acceleration * 100, Space.Self);
			//_rb.AddForce((direction * _ai.acceleration * Time.deltaTime) * 100, ForceMode.Acceleration);
		}
		void CompletedPath()
		{
			completedPath = true;
		}
	}
}