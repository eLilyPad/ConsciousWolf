using System.Collections.Generic;
using UnityEngine;

namespace Lily.Ai.Pathfinder
{
public class Grid : MonoBehaviour
{

	#region Variables

		#region Grid Settings
			public bool displayGridGizmos;
			public Vector2 gridWorldSize;
			int gridSizeX, gridSizeY;
			Node[,] grid;
			public int MaxSize
			{
				get
				{
					return gridSizeX * gridSizeY;
				}
			}

		#endregion

		#region Node Settings
			public float nodeRadius;
			float nodeDiameter;

		#endregion
	
		#region Mask Settings
			public LayerMask unwalkableMask;
			LayerMask walkableMask;

		#endregion

		#region Penalty Settings
			public int obstacleProximityPenalty = 10;
			int penaltyMin = int.MaxValue;
			int penaltyMax = int.MinValue;
	
		#endregion
		
		#region Region Settings
			Dictionary<int, int> walkableRegionsDictionary = new Dictionary<int, int>();
			public TerrainType[] walkableRegions;

		#endregion

	#endregion

	#region Setup
		void Awake()
		{
			
			nodeDiameter = nodeRadius * 2;
			//scales the gird size to fit the nodes
			gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
			gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);

			foreach (TerrainType region in walkableRegions)
			{
				walkableMask.value |= region.terrainMask.value;
				walkableRegionsDictionary.Add((int)Mathf.Log(region.terrainMask.value, 2), region.terrainPenalty);
				//Assign cost penalties to nodes in specified regions
			}
			CreateGrid();
		}

	#endregion

	#region Grid Creation
	public void CreateGrid()
	{

		grid = new Node[gridSizeX, gridSizeY];//creates nodes to fit the grid
		Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;
		//finds the bottom of the grid to start building the grid from the given centre and the gris size

		for (int x = 0; x < gridSizeX; x++)//runs code on each value of x
		{
			
			for (int y = 0; y < gridSizeY; y++)//runs code on each value of y
			{
				Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);//locks the position of the node with the grid in the world
				bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));//check if the node is touching a un-touchable region

				int movementPenalty = 0;//Resets the penalty of the node

				Ray ray = new Ray(worldPoint + Vector3.up * 50, Vector3.down);//Ray att node point pointing down
				RaycastHit hit;//holds the ray hit information
				if (Physics.Raycast(ray, out hit, 100, walkableMask))//cast the ray and check if it hits the walkable region
				{
					walkableRegionsDictionary.TryGetValue(hit.collider.gameObject.layer, out movementPenalty);//check if the region has any movement penalty
				}

				if (!walkable)
				{
					movementPenalty += obstacleProximityPenalty;//sets the penalty high to become unwalkable node
				}

				grid[x, y] = new Node(walkable, worldPoint, x, y, movementPenalty);//creats a new grid with nodes in place
			}
		}

		BlurPenaltyMap(3);

	}
	#endregion

	void BlurPenaltyMap(int blurSize)
	{
		int kernelSize = blurSize * 2 + 1;
		int kernelExtents = (kernelSize - 1) / 2;

		int[,] penaltiesHorizontalPass = new int[gridSizeX, gridSizeY];
		int[,] penaltiesVerticalPass = new int[gridSizeX, gridSizeY];

		for (int y = 0; y < gridSizeY; y++)
		{
			for (int x = -kernelExtents; x <= kernelExtents; x++)
			{
				int sampleX = Mathf.Clamp(x, 0, kernelExtents);
				penaltiesHorizontalPass[0, y] += grid[sampleX, y].movementPenalty;
			}

			for (int x = 1; x < gridSizeX; x++)
			{
				int removeIndex = Mathf.Clamp(x - kernelExtents - 1, 0, gridSizeX);
				int addIndex = Mathf.Clamp(x + kernelExtents, 0, gridSizeX - 1);

				penaltiesHorizontalPass[x, y] = penaltiesHorizontalPass[x - 1, y] - grid[removeIndex, y].movementPenalty + grid[addIndex, y].movementPenalty;
			}
		}

		for (int x = 0; x < gridSizeX; x++)
		{
			for (int y = -kernelExtents; y <= kernelExtents; y++)
			{
				int sampleY = Mathf.Clamp(y, 0, kernelExtents);
				penaltiesVerticalPass[x, 0] += penaltiesHorizontalPass[x, sampleY];
			}

			int blurredPenalty = Mathf.RoundToInt((float)penaltiesVerticalPass[x, 0] / (kernelSize * kernelSize));
			grid[x, 0].movementPenalty = blurredPenalty;

			for (int y = 1; y < gridSizeY; y++)
			{
				int removeIndex = Mathf.Clamp(y - kernelExtents - 1, 0, gridSizeY);
				int addIndex = Mathf.Clamp(y + kernelExtents, 0, gridSizeY - 1);

				penaltiesVerticalPass[x, y] = penaltiesVerticalPass[x, y - 1] - penaltiesHorizontalPass[x, removeIndex] + penaltiesHorizontalPass[x, addIndex];
				blurredPenalty = Mathf.RoundToInt((float)penaltiesVerticalPass[x, y] / (kernelSize * kernelSize));
				grid[x, y].movementPenalty = blurredPenalty;

				if (blurredPenalty > penaltyMax)
				{
					penaltyMax = blurredPenalty;
				}
				if (blurredPenalty < penaltyMin)
				{
					penaltyMin = blurredPenalty;
				}
			}
		}
	}
	//Finds the nodes that are next to the current node
	public List<Node> GetNeighbours(Node node)
	{
		List<Node> neighbours = new List<Node>();//creates a list of nodes next to the current node

		for (int x = -1; x <= 1; x++)
		{
			for (int y = -1; y <= 1; y++)
			{
				// checks the grid next to the current node
				if (x == 0 && y == 0)
					continue;//skips the current node

				int checkX = node.gridX + x;//finds the current node position and checks the x axis
				int checkY = node.gridY + y;//finds the current node position and checks the y axis 

				if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
				{
					neighbours.Add(grid[checkX, checkY]);//skips current node and put the rest in the neighbours list
				}
			}
		}

		return neighbours;
	}

	//Find the node relative to the world positions
	public Node NodeFromWorldPoint(Vector3 worldPosition)
	{
		float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
		float percentY = (worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y;
		percentX = Mathf.Clamp01(percentX);
		percentY = Mathf.Clamp01(percentY);

		int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
		int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
		return grid[x, y];
	}

	void OnDrawGizmos()
	{
		Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));
		if (grid != null && displayGridGizmos)
		{
			foreach (Node n in grid)
			{

				Gizmos.color = Color.Lerp(Color.white, Color.black, Mathf.InverseLerp(penaltyMin, penaltyMax, n.movementPenalty));
				Gizmos.color = (n.walkable) ? Gizmos.color : Color.red;
				Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter));

			}
		}
	}

	[System.Serializable]
	public class TerrainType
	{
		public LayerMask terrainMask;
		public int terrainPenalty;
	}
}
}