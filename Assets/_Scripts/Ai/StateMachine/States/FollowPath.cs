using UnityEngine;

namespace Lily.Ai.ActionStates
{
	using Ai.Pathfinder;
	using Ai;
	public class FollowPath : IState
	{
		private readonly BasicAI _ai;
		private Vector3 _lastPosition = Vector3.zero;
		
		Path _path;

		public FollowPath(BasicAI ai, Path path)
		{
			_ai = ai;
			_path = path;
		}
		public void Tick()
		{
			if(_ai.currentPath !=null)Path();
		}

		void Path()
		{
			Debug.Log("FollowPath");
			bool followingPath = true;
			int pathIndex = 0;
			_ai.transform.LookAt(_ai.currentPath.lookPoints[0]);

			float speedPercent = 1;

			while(followingPath)
			{
				Vector2 pos2D = new Vector2(_ai.transform.position.x, _ai.transform.position.z);
				while(_ai.currentPath.turnBoundaries[pathIndex].HasCrossedLine(pos2D))
				{
					if(pathIndex == _ai.currentPath.finishLineIndex)
					{
						followingPath = false;
						break;
					}
					else
					{
						pathIndex++;
					}

					if(followingPath)
					{
						if(pathIndex >= _ai.currentPath.slowDownIndex && _ai.stoppingDst > 0)
						{
							speedPercent = Mathf.Clamp01(_ai.currentPath.turnBoundaries[_ai.currentPath.finishLineIndex].DistanceFromPoint(pos2D)/ _ai.stoppingDst);
							if(speedPercent < 0.01f)
							{
								followingPath = false;
							}

							Quaternion targetRotation = Quaternion.LookRotation(_ai.currentPath.lookPoints[pathIndex]-_ai.transform.position);
							_ai.transform.rotation = Quaternion.Lerp(_ai.transform.rotation, targetRotation, Time.deltaTime * _ai.turnSpeed);
							_ai.transform.Translate(Vector3.forward * Time.deltaTime * _ai.speed * speedPercent, Space.Self); 
						}
					}
				}
			}
		}

		public void OnEnter()
		{}
		public void OnExit()
		{}
	}
}