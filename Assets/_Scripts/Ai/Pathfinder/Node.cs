using UnityEngine;

namespace Lily.Ai.Pathfinder
{
	public class Node : IHeapItem<Node>
	{
		#region Variables
			public bool walkable;
			public Vector3 worldPosition;
			public int gridX;
			public int gridY;
			public int movementPenalty;

			public int gCost;
			public int hCost;
			public Node parent;
			int heapIndex;

		#endregion


		public Node(bool _walkable, Vector3 _worldPos, int _gridX, int _gridY, int _penalty)
		{
			walkable = _walkable;
			worldPosition = _worldPos;
			gridX = _gridX;
			gridY = _gridY;
			movementPenalty = _penalty;
		}

		//calculate the cost to move to the node
		public int fCost
		{
			get
			{
				return gCost + hCost;
			}
		}

		//sets the HeapIndex
		public int HeapIndex
		{
			get
			{
				return heapIndex;
			}
			set
			{
				heapIndex = value;
			}
		}

		public int CompareTo(Node nodeToCompare)
		{
			int compare = fCost.CompareTo(nodeToCompare.fCost);
			if (compare == 0)
			{
				compare = hCost.CompareTo(nodeToCompare.hCost);
			}
			return -compare;
		}
	}
}
