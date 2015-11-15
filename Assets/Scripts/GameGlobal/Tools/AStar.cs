using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * Framework Class
 * @author stellar
 * @c#_port Hubert and Jerry 
 */

public class AStar
{
	/**
	 * @author stellar
	 * Rewritten to CS for Unity3D by Jerry with pathetic help from Hubert ;)
	 */

	public class CustomComparerClassForAStar : IComparer<Node> 
	{	    	
    	public int Compare( Node node1, Node node2 )  
		{
			int returnInt = 0;
			if ( node1.f != node2.f ) returnInt = node1.f > node2.f ? -1 : 1; 
        	return( returnInt );
    	}  
	}
	// only straight line
	//public static const NEIGHBOURS_NO_BEVELS:string = "NEIGHBOURS_NO_BEVELS";
	
	//straight line smoothed
	//public static const NEIGHBOURS_NO_BEVELS_SMOOTH:string = "NEIGHBOURS_NO_BEVELS_SMOOTH";
	
	// all with bevels
	public const string NEIGHBOURS_ALL = "NEIGHBOURSS_ALL";
	
	// no if 2 bevels are locked
	//public static const NEIGHBOURS_WITH_SEMI_BLOCKED_BEVELS:string = "NEIGHBOURS_WITH_SEMI_BLOCKED_BEVELS";

	// no if any of 2 bevel is locked
	//public static const NEIGHBOURS_NO_SEMI_BLOCKED_BEVELS:string = "NEIGHBOURS_NO_SEMI_BLOCKED_BEVELS";
	
	private static int CurrentGridWidth;
	private static int _currentObjectType;
	private static GameObject _currentObjectSeekingPath;
	public static Node[][] _grid;
	private static List<Node> _openList;
	private static List<Node> _closedList;
	private delegate ArrayList SearchNeighbourAlghoritm (Node node);
	private const float BEVEL_COST = 13f;
	private const float STREIGHT_COST = 10f;
	private const float TURN_COST = 19f;
	private static CustomComparerClassForAStar _comparerOfNodesF;
	public static List<int[]> reservedSpots;
	public static bool diagonalConflict = false;


	/**
	 * Costructor - can inits algorithm with matrix
	 * Algoritm searches grid from start position defined as Point to end position defined as Point and retuns path defined as vector of Point. Grid elements have to implements Node interface
	 * @example example of usage
	 * <listing version="3.0f">
	 * 	Astar astar = new Astar(grid);
	 * 	Vector. path<Point> = astar.search(new Point(0, 0), new Point(15, 15), AStar.NEIGHBOURS_NO_BEVELS);
	 * </listing> 
	 * @param grid [optional] matrix with elements implmenting Node interface
	 * @warning grid has the form: grid[y][x] = Node (y is first!)
	 */

	

	/**
	 * Initializing new grid with elements
	 * @param grid matrix with elements implmenting Node interface
	 * @warning grid has the form: grid[y][x] = Node (y is first!)
	 */

	public static void init ( int width, int height )
	{			
		 _comparerOfNodesF = new CustomComparerClassForAStar(); //TODO Static
		
		int gridHeight = height;
		int gridWidth = width * 20;
		CurrentGridWidth = gridWidth;
		_grid = new Node[gridHeight][];
		for (int y = 0; y < gridHeight; y++)
		{
			_grid[y] = new Node[gridWidth];
			for (int x = 0; x < gridWidth; x++)
			{
				_grid[y][x] = new Node( y, x );
			}
		}
	}

	/**
	 * Searching path
	 * @param startPosition searching start position
	 * @param endPosition searching end position
	 * @param searchNeighbourType type of searching neighbours
	 * @return Vector.<Point> with Points cointaining steps from start position to end position
	 */
	public static int[][] search(int[] startPosition, int[] endPosition, bool ommitLastNode, int objectType, GameObject gameObjectOfElementSeekingPath, string searchNeighbourType = AStar.NEIGHBOURS_ALL)
	{
		if(LevelControl.getInstance() != null)
		{
			reservedSpots = GridReservationManager.getInstance ().myReservedSpacesForChars;
		}
		diagonalConflict = false;
		//Debug.Log("Node State " + _grid[2][54].walkable( _currentObjectType, _currentObjectSeekingPath, endPosition));
		_currentObjectType = objectType;
		_currentObjectSeekingPath = gameObjectOfElementSeekingPath;
		if ( ! _grid[endPosition[1]][endPosition[0]].walkable ( _currentObjectType, _currentObjectSeekingPath, endPosition ) && ( ! ommitLastNode ))
		{
			//Debug.Log(endPosition[1] + " : " + endPosition[0] + " : " + _currentObjectType);
			return null;
		}

		Node currentNode = null;
		List<Node> currentNeighbours;	
		float currentGScore;
		bool bestGscore;


		_openList = new List<Node>();
		_closedList = new List<Node>();
		_openList.Add (_grid[startPosition[1]][startPosition[0]]);
		_grid[startPosition[1]][startPosition[0]].parent = null;
	
		_grid[startPosition[1]][startPosition[0]].g = 0;
		_grid[startPosition[1]][startPosition[0]].h = getShortestDistance(_grid[startPosition[1]][startPosition[0]], endPosition);
		_grid[startPosition[1]][startPosition[0]].f = _grid[startPosition[1]][startPosition[0]].h;
		
		//FStructures.FPriorityQueue<Node> _priorityQueueOfNodes = new FStructures.FPriorityQueue<Node>();
		//FPriorityQueue<int, Node> _asd = new FPriorityQueue<int, Node>(50, _comparerOfNodesF);
		//FStructures.PriorityQueue<Node> _priorityQueueOfNodes = new FStructures.PriorityQueue<Node>();
		//_priorityQueueOfNodes.Enqueue(_grid[startPosition[1]][startPosition[0]]);
		
		int lCounter = 0;
		//bool lIsPlayer = _currentObjectSeekingPath.name.Contains("Player");
		//main loop	
		float lDistanceX;
		float lDistanceZ;
		while (_openList.Count > 0)
		{
			//==========Dave's Potential Mess==========
			//bool bN = true;
			//bool tN = true;
			//bool lN = true;
			//bool rN = true;
			//==========Dave's Potential Mess==========
			_openList.Sort(_comparerOfNodesF);			
			//my implementation of POP ???
			//currentNode = _openList.GetRange (_openList.Count - 1, _openList.Count - 1) as Node;
			
			currentNode = _openList[_openList.Count-1] ;				
			_openList.RemoveAt (_openList.Count - 1);		
			//currentNode = _priorityQueueOfNodes.Dequeue();
			
			//end condition - if current node is on end position - return from loop
			if (currentNode.positionY == endPosition[1] && currentNode.positionX == endPosition[0])
			{
				break;
			}
			//==========Dave's Potential Mess==========
			//Debug.Log(currentNode.positionX + " " + currentNode.positionY );
			/*if(currentNode.positionX < 1)
			{
				lN = false;
				//Debug.Log ("FLAG L");
			}
			//Debug.Log("Pos " + currentNode.positionX);
			//Debug.Log("Width " + (CurrentGridWidth/10 - 2));
			if(currentNode.positionX > (CurrentGridWidth/10 - 2))
			{
				rN = false;
				//Debug.Log ("FLAG R");
			}
			if(currentNode.positionY > 6)
			{
				tN = false;
				//Debug.Log ("FLAG T");
			}
			if(currentNode.positionY < 1)
			{
				bN = false;
				//Debug.Log ("FLAG B");
			}*/
			/*
			if(_openList.Count > 1)
			{
				//Checking for diagonal pinches
				if((currentNode.positionY < 7 && (!_grid[currentNode.positionY + 1 ][currentNode.positionX + 0].walkable( _currentObjectType, _currentObjectSeekingPath, endPosition ))) &&
				   ((currentNode.positionX ) < (CurrentGridWidth/10 - 2) && !_grid[currentNode.positionY + 0 ][currentNode.positionX + 1].walkable( _currentObjectType, _currentObjectSeekingPath, endPosition )))
				{
					Debug.Log("AT " + currentNode.positionX + "," + currentNode.positionY);
					if(!_grid[currentNode.positionY + 1 ][currentNode.positionX + 1].walkable( _currentObjectType, _currentObjectSeekingPath, endPosition ))
					{
						Debug.Log("NE BLK");
					}
					else
					{
						Debug.Log("NE FREE");
						break;
					}
				}

				if((currentNode.positionY > 0 && (!_grid[currentNode.positionY - 1 ][currentNode.positionX - 0].walkable( _currentObjectType, _currentObjectSeekingPath, endPosition ))) &&
				   ((currentNode.positionX + 1) < (CurrentGridWidth/10 - 2) && !_grid[currentNode.positionY + 0 ][currentNode.positionX + 1].walkable( _currentObjectType, _currentObjectSeekingPath, endPosition )))
				{
					Debug.Log("AT " + currentNode.positionX + "," + currentNode.positionY);
					if(!_grid[currentNode.positionY - 1 ][currentNode.positionX + 1].walkable( _currentObjectType, _currentObjectSeekingPath, endPosition ))
					{
						Debug.Log("SE BLK");
					}
					else
					{
						Debug.Log("SE FREE");
						break;
					}
				}
			
				if((currentNode.positionY > 0 && (!_grid[currentNode.positionY - 1 ][currentNode.positionX - 0].walkable( _currentObjectType, _currentObjectSeekingPath, endPosition ))) &&
				   ((currentNode.positionX > 0) && !_grid[currentNode.positionY - 0 ][currentNode.positionX - 1].walkable( _currentObjectType, _currentObjectSeekingPath, endPosition )))
				{
					Debug.Log("AT " + currentNode.positionX + "," + currentNode.positionY);
					if(!_grid[currentNode.positionY - 1 ][currentNode.positionX - 1].walkable( _currentObjectType, _currentObjectSeekingPath, endPosition ))
					{
						Debug.Log("SW BLK");
					}
					else
					{
						Debug.Log("SW FREE");
						break;
					}
				}

				if((currentNode.positionX > 0 && (!_grid[currentNode.positionY - 0 ][currentNode.positionX - 1].walkable( _currentObjectType, _currentObjectSeekingPath, endPosition ))) &&
				   ((currentNode.positionY < 7) && !_grid[currentNode.positionY + 1 ][currentNode.positionX + 0].walkable( _currentObjectType, _currentObjectSeekingPath, endPosition )))
				{
					Debug.Log("AT " + currentNode.positionX + "," + currentNode.positionY);
					if(!_grid[currentNode.positionY + 1 ][currentNode.positionX - 1].walkable( _currentObjectType, _currentObjectSeekingPath, endPosition ))
					{
						Debug.Log("NW BLK");
					}
					else
					{
						Debug.Log("NW FREE");
						break;
					}
				}
			}*/
			//==========Dave's Potential Mess==========
			_closedList.Add(currentNode);

			currentNeighbours = searchAllNeighbours( currentNode, endPosition );
			//checking neighbours
			foreach (Node neighbour in currentNeighbours)
			{
				// if Node is not closed - searching otherwise - skip
				if (_closedList.IndexOf(neighbour) < 0)
				{

					bestGscore = false;
					lDistanceX = currentNode.positionX - neighbour.positionX;
					lDistanceZ = currentNode.positionY - neighbour.positionY;
					currentGScore = (currentNode.g + neighbour.terrainCost + Mathf.Sqrt((lDistanceX) * (lDistanceX) + (lDistanceZ) * (lDistanceZ)));
					// already not seen
					if (_openList.IndexOf(neighbour) < 0)
					{
						_openList.Add( neighbour );
						// if first seen - it has to be best
						bestGscore = true;
					}
				
					//if was allready seen checking if path has lower g score
					else if (currentGScore < neighbour.g)
					{
						bestGscore = true;
						//_priorityQueueOfNodes.Dequeue();
					}
					// for now - best
					if (bestGscore)
					{
						neighbour.h = getShortestDistance( neighbour, endPosition );
						neighbour.g = currentGScore;
						neighbour.f = neighbour.g + neighbour.h;
						//setting up parameters
						neighbour.parent = currentNode;
						//_priorityQueueOfNodes.Enqueue(neighbour);
					}
				}
			}
			
			if(++lCounter > 200)
				return null;
		}
		
		//MonoBehaviour.print(gameObjectOfElementSeekingPath.ToString() + " : " + lCounter + " : " + endPosition[0] + ":" + endPosition[1]);
		Node node = _grid[endPosition[1]][endPosition[0]];
	
		//if no parent - no path
		if (( node.parent == null) && ( ! ommitLastNode ))
		{
			return null;
		}
		if(diagonalConflict == true)
		{
			return null;
		}

		//rewriting path to vector
		ArrayList pathArrayList = new ArrayList ();
		do
		{
			int[] pointToPathPush = new int[2];
			//Debug.Log("In Path " + node.positionX + " " + node.positionY);
			pointToPathPush[0] = node.positionX;
			pointToPathPush[1] = node.positionY;
		
			pathArrayList.Add(pointToPathPush);
			node = node.parent;
		}
		while (node != null);
		
	 	//return reversed path - from start to end
		pathArrayList.Reverse();
		//Jerry's - we dont' need start Node 
		//pathArrayList.RemoveAt(0);		
		
		//Jerry's - remove players position itself as it is target
		if (ommitLastNode) pathArrayList.RemoveAt (pathArrayList.Count - 1);				
	
		int[][] path = new int[128][];
		//List<int[]> path = new List<int[]>(20);
		int i = 0;
	
		foreach (int[] returnNode in pathArrayList)
		{			
			//path.Add(new int[2]);
			path[i] = new int[2];
			path[i][0] = returnNode[0];
			path[i][1] = returnNode[1];		
		
			i++;
			if(i > 127)	//some restrictions in others scripts
				break;
		}
		//Debug.Log("PATH LENGTH: " + path.Count);
		if (pathArrayList.Count > 0) return path;
		else return null;
	}

	/**
	 * Searching shortest distance to end
	 * @param node node for setting distance
	 * @param endPosition end position 
	 * @return distance between node and end position
	 */


	private static float getShortestDistance ( Node node ,   int[] endPosition  )
	{
		// manhatan algotithm
		return (Mathf.Abs(endPosition[0] - node.positionX) + Mathf.Abs(endPosition[1] - node.positionY));
	}	
			/**
	 * Searching neighbours algorithm - returns all nodes around also with bevels
	 * @param node node for searching neighbours
	 * @return vector with neighbours
	 */


	private static bool IsCharOnTile(Node addNode, List<Node> neighbours)
	{
		if(LevelControl.getInstance() == null)
		{
			//Debug.Log("Null?");
			return false;
		}

		bool whatToReturn = false;
		foreach(int[] toon in reservedSpots)
		{
			//Debug.Log("Each Char");
			//Debug.Log("XY Node " + addNode.positionX + " " + addNode.positionY);
			//Debug.Log("XY Toon " + toon[0] + " " + toon[1]);
			if(addNode.positionX == toon[0] && addNode.positionY == toon[1])
			{
				//Debug.Log("Match Found");
				//Debug.Log("XY Toon " + toon[0] + " " + toon[1]);
				whatToReturn = true;
			}
		}
		if(whatToReturn == true)
		{
			//Debug.Log ("True Returned");
			return true;
		}
		else
		{
			//Debug.Log ("False Returned");
			return false;
		}
	}


	private static List<Node> searchAllNeighbours( Node node, int[] endPosition  )
	{
		List<Node> neighbours = new List<Node> ();
		Node addNode;

		if (node.positionY != 0)
		{
			/*if (node.positionX != 0)
			{
				addNode = _grid[node.positionY - 1][node.positionX - 1];

				Node addNode1;
				Node addNode2;
				
				addNode1 = _grid[node.positionY - 1][node.positionX];
				addNode2 = _grid[node.positionY][node.positionX - 1];
			
				if ( addNode.walkable ( _currentObjectType, _currentObjectSeekingPath, endPosition ))
				{
					if((addNode1.walkable ( _currentObjectType, _currentObjectSeekingPath, endPosition ) && IsCharOnTile(addNode2,neighbours) == false)||
						addNode2.walkable ( _currentObjectType, _currentObjectSeekingPath, endPosition ) && IsCharOnTile(addNode1,neighbours) == false)
					{
						//addNode.cost = node.terrainCost * BEVEL_COST;
						if(addNode.positionX == 54 && addNode.positionY == 1)
						{
							
							Debug.Log ("Gotcha1");
							Debug.Log (addNode1.walkable ( _currentObjectType, _currentObjectSeekingPath, endPosition ));
							Debug.Log (IsCharOnTile(addNode2,neighbours));
							Debug.Log (addNode2.walkable ( _currentObjectType, _currentObjectSeekingPath, endPosition ));
							Debug.Log (IsCharOnTile(addNode1,neighbours));
							Debug.Log ("Combined");
							Debug.Log ((addNode1.walkable ( _currentObjectType, _currentObjectSeekingPath, endPosition ))&&(IsCharOnTile(addNode2,neighbours)));
							Debug.Log ((addNode2.walkable ( _currentObjectType, _currentObjectSeekingPath, endPosition ))&&(IsCharOnTile(addNode1,neighbours)));
						}
						neighbours.Add(addNode);
					}
					else
					{
						Debug.Log ("NodeNotAdded");
					}
				}
			}
		
			if (node.positionX < _grid[node.positionY].Length - 1)
			{
				addNode = _grid[node.positionY - 1][node.positionX + 1];
			
				Node addNode1;
				Node addNode2;
				
				addNode1 = _grid[node.positionY - 1][node.positionX];
				addNode2 = _grid[node.positionY][node.positionX + 1];
			
				if ( addNode.walkable ( _currentObjectType, _currentObjectSeekingPath, endPosition ))
				{
					if((addNode1.walkable ( _currentObjectType, _currentObjectSeekingPath, endPosition ) && IsCharOnTile(addNode2,neighbours) == false) == true||
					   (addNode2.walkable ( _currentObjectType, _currentObjectSeekingPath, endPosition ) && IsCharOnTile(addNode1,neighbours) == false) == true)
					{
						//addNode.cost = node.terrainCost * BEVEL_COST;
						if(addNode.positionX == 54 && addNode.positionY == 1)
						{

							Debug.Log ("Gotcha2");
							Debug.Log (addNode1.walkable ( _currentObjectType, _currentObjectSeekingPath, endPosition ));
							Debug.Log (IsCharOnTile(addNode2,neighbours));
							Debug.Log (addNode2.walkable ( _currentObjectType, _currentObjectSeekingPath, endPosition ));
							Debug.Log (IsCharOnTile(addNode1,neighbours));
							Debug.Log ("Combined");
							Debug.Log ((addNode1.walkable ( _currentObjectType, _currentObjectSeekingPath, endPosition ))&&(IsCharOnTile(addNode2,neighbours)));
							Debug.Log ((addNode2.walkable ( _currentObjectType, _currentObjectSeekingPath, endPosition ))&&(IsCharOnTile(addNode1,neighbours)));
						}
						neighbours.Add(addNode);
					}
					else
					{
						Debug.Log ("NodeNotAdded");
					}
				}
			}*/

			addNode = _grid[node.positionY - 1][node.positionX];				
			
			if (addNode.walkable (_currentObjectType, _currentObjectSeekingPath, endPosition) && IsCharOnTile(addNode,neighbours) == false)
			{
				//addNode.cost = node.terrainCost * STREIGHT_COST;
				//if(IsCharOnTile(addNode,neighbours) == true)
				//{
				//	Debug.Log("It was ME!!!");
				//}
				//if(IsCharOnTile(addNode,neighbours) == false)
				//{
				if(addNode.positionX == 54 && addNode.positionY == 1)
				{
					
					Debug.Log ("Gotcha D");

				}
					neighbours.Add(addNode);
				//}
			}
		}

		if (node.positionX != 0)
		{
			addNode = _grid[node.positionY][node.positionX - 1];
			if (addNode.walkable (_currentObjectType, _currentObjectSeekingPath, endPosition) && IsCharOnTile(addNode,neighbours) == false)
			{
				//addNode.cost = node.terrainCost * STREIGHT_COST;
				//if(IsCharOnTile(addNode,neighbours) == true)
				//{
				//	Debug.Log("It was ME!!!");
			//	}
				//if(IsCharOnTile(addNode,neighbours) == false)
				//{
				if(addNode.positionX == 54 && addNode.positionY == 1)
				{
					
					Debug.Log ("Gotcha C");
					
				}
					neighbours.Add(addNode);
				//}
			}
		}
		if (node.positionX < _grid[node.positionY].Length - 1)
		{
			addNode = _grid[node.positionY][node.positionX + 1];
			if (addNode.walkable (_currentObjectType, _currentObjectSeekingPath, endPosition) && IsCharOnTile(addNode,neighbours) == false)
			{
				//addNode.cost = node.terrainCost * STREIGHT_COST;
				//if(IsCharOnTile(addNode,neighbours) == true)
				//{
				//	Debug.Log("It was ME!!!");
				//}
				//if(IsCharOnTile(addNode,neighbours) == false)
				//{
				if(addNode.positionX == 54 && addNode.positionY == 1)
				{
					
					Debug.Log ("Gotcha B");
					
				}
					neighbours.Add(addNode);
				//}
			}
		}

		if (node.positionY < _grid.Length - 1)
		{
			/*if (node.positionX != 0)
			{
				addNode = _grid[node.positionY + 1][node.positionX - 1];
				
				Node addNode1;
				Node addNode2;
				
				addNode1 = _grid[node.positionY + 1][node.positionX];
				addNode2 = _grid[node.positionY][node.positionX - 1];
			
				if ( addNode.walkable ( _currentObjectType, _currentObjectSeekingPath, endPosition ))
				{
					if((addNode1.walkable ( _currentObjectType, _currentObjectSeekingPath, endPosition ) && IsCharOnTile(addNode2,neighbours) == false)||
					   addNode2.walkable ( _currentObjectType, _currentObjectSeekingPath, endPosition ) && IsCharOnTile(addNode1,neighbours) == false)
					{
						//addNode.cost = node.terrainCost * BEVEL_COST;
						if(addNode.positionX == 54 && addNode.positionY == 1)
						{
							
							Debug.Log ("Gotcha3");
							Debug.Log (addNode1.walkable ( _currentObjectType, _currentObjectSeekingPath, endPosition ));
							Debug.Log (IsCharOnTile(addNode2,neighbours));
							Debug.Log (addNode2.walkable ( _currentObjectType, _currentObjectSeekingPath, endPosition ));
							Debug.Log (IsCharOnTile(addNode1,neighbours));
							Debug.Log ("Combined");
							Debug.Log ((addNode1.walkable ( _currentObjectType, _currentObjectSeekingPath, endPosition ))&&(IsCharOnTile(addNode2,neighbours)));
							Debug.Log ((addNode2.walkable ( _currentObjectType, _currentObjectSeekingPath, endPosition ))&&(IsCharOnTile(addNode1,neighbours)));
						}
						neighbours.Add(addNode);
					}
					else
					{
						Debug.Log ("NodeNotAdded");
					}
				}
			}
		
			if (node.positionX < _grid[node.positionY].Length - 1)
			{
				addNode = _grid[node.positionY + 1][node.positionX + 1];
			
				
				Node addNode1;
				Node addNode2;
				
				addNode1 = _grid[node.positionY + 1][node.positionX];
				addNode2 = _grid[node.positionY][node.positionX + 1];
			
				if ( addNode.walkable ( _currentObjectType, _currentObjectSeekingPath, endPosition ))
				{
					if((addNode1.walkable ( _currentObjectType, _currentObjectSeekingPath, endPosition ) && IsCharOnTile(addNode2,neighbours) == false)||
					   addNode2.walkable ( _currentObjectType, _currentObjectSeekingPath, endPosition ) && IsCharOnTile(addNode1,neighbours) == false)
					{
						//addNode.cost = node.terrainCost * BEVEL_COST;
						if(addNode.positionX == 54 && addNode.positionY == 1)
						{
							
							Debug.Log ("Gotcha4");
							Debug.Log (addNode1.walkable ( _currentObjectType, _currentObjectSeekingPath, endPosition ));
							Debug.Log (IsCharOnTile(addNode2,neighbours));
							Debug.Log (addNode2.walkable ( _currentObjectType, _currentObjectSeekingPath, endPosition ));
							Debug.Log (IsCharOnTile(addNode1,neighbours));
							Debug.Log ("Combined");
							Debug.Log ((addNode1.walkable ( _currentObjectType, _currentObjectSeekingPath, endPosition ))&&(IsCharOnTile(addNode2,neighbours)));
							Debug.Log ((addNode2.walkable ( _currentObjectType, _currentObjectSeekingPath, endPosition ))&&(IsCharOnTile(addNode1,neighbours)));
						}
						neighbours.Add(addNode);
					}
					else
					{
						Debug.Log ("NodeNotAdded");
					}
				}
			}*/

			addNode = _grid[node.positionY + 1][node.positionX];
			if (addNode.walkable (_currentObjectType, _currentObjectSeekingPath, endPosition)  && IsCharOnTile(addNode,neighbours) == false)
			{
				//addNode.cost = node.terrainCost * STREIGHT_COST;
				//if(IsCharOnTile(addNode,neighbours) == true)
				//{
				//	Debug.Log("It was ME!!!");
				//}
				//if(IsCharOnTile(addNode,neighbours) == false)
				//{
				if(addNode.positionX == 54 && addNode.positionY == 1)
				{
					
					Debug.Log ("Gotcha A");
					
				}
					neighbours.Add(addNode);
				//}
			}
		}
		return neighbours;
	}

	/**
	 * Sorting function for comparing nodes
	 * @param node1 first node to compare
	 * @param node2 second node to compare
	 * @return -1, 1, 0
	 */

/*
	protected int compareNodes ( Node node1 ,   Node node2  )
	{
		if (node1.f > node2.f)
		{
			return -1;
		}
		else if (node1.f < node2.f)
		{
			return 1;
		}
		return 0;
	}
 */

	/**
	 * Searching neighbours algorithm - searching only in straight lines (no bevels)
	 * @param node node for searching neighbours
	 * @return vector with neighbours
	 */
/*
	private function searchNoBevelsNeighbours( Node node  ):Vector.<Node>
	{
		Vector. neighbours<Node> = new Vector.<Node>();
		Node addNode;

		if (node.positionY)
		{
			addNode = _grid[node.positionY - 1][node.positionX];
			if (addNode.walkable)
			{
				addNode.cost = node.terrainCost * 10;
				neighbours.push(addNode);
			}
		}

		if (node.positionX)
		{
			addNode = _grid[node.positionY][node.positionX - 1];
			if (addNode.walkable (objectType))
			{
				addNode.cost = node.terrainCost * 10;
				neighbours.push(addNode);
			}
		}
		if (node.positionX < _grid[node.positionY].Length - 1)
		{
			addNode = _grid[node.positionY][node.positionX + 1];
			if (addNode.walkable (objectType))
			{
				addNode.cost = node.terrainCost * 10;
				neighbours.push(addNode);
			}
		}

		if (node.positionY < _grid.Length - 1)
		{
			addNode = _grid[node.positionY + 1][node.positionX];
			if (addNode.walkable (objectType))
			{
				addNode.cost = node.terrainCost * 10;
				neighbours.push(addNode);
			}
		}
		return neighbours;
	}
	*/
	
	/**
	 * Searching neighbours algorithm - searching only in straight lines (no bevels) smootked
	 * @param node node for searching neighbours
	 * @return vector with neighbours
	 */

	/*
	private function searchNoBevelsNeighboursSmooth( Node node  ):Vector.<Node>
	{
		Vector. neighbours<Node> = new Vector.<Node>();
		Node addNode;

		if (node.positionY)
		{
			addNode = _grid[node.positionY - 1][node.positionX];
			if (addNode.walkable (objectType))
			{
				if (node.parent && node.parent.positionX != addNode.positionX)
				{
					addNode.cost = node.terrainCost * TURN_COST;
					
				}
				else
				{
					addNode.cost = node.terrainCost * STREIGHT_COST;
				}
				neighbours.push(addNode);
			}
		}

		if (node.positionX)
		{
			addNode = _grid[node.positionY][node.positionX - 1];
			if (addNode.walkable (objectType))
			{
				if (node.parent && node.parent.positionY != addNode.positionY)
				{
					addNode.cost = node.terrainCost * TURN_COST;
				}
				else
				{
					addNode.cost = node.terrainCost * STREIGHT_COST;
				}
				neighbours.push(addNode);
			}
		}
		if (node.positionX < _grid[node.positionY].Length - 1)
		{
			addNode = _grid[node.positionY][node.positionX + 1];
			if (addNode.walkable (objectType))
			{
				if (node.parent && node.parent.positionY != addNode.positionY)
				{
					addNode.cost = node.terrainCost * TURN_COST;
					
				}
				else
				{
					addNode.cost = node.terrainCost * STREIGHT_COST;
				}
				neighbours.push(addNode);
			}
		}

		if (node.positionY < _grid.Length - 1)
		{
			addNode = _grid[node.positionY + 1][node.positionX];
			if (addNode.walkable (objectType))
			{
				if (node.parent && node.parent.positionX != addNode.positionX)
				{
					addNode.cost = node.terrainCost * TURN_COST;
					
				}
				else
				{
					addNode.cost = node.terrainCost * STREIGHT_COST;
				}
				neighbours.push(addNode);
			}
		}
		return neighbours;
	}
	*/

	/**
	 * Searching neighbours algorithm - returns bevel neighbours if there's no blocked (!walkable (objectType)) nodes around
	 * @param node node for searching neighbours
	 * @return vector with neighbours
	 */

	/*
	private function searchNoSemiBlockedBevels( Node node  ):Vector.<Node>
	{
		Vector. neighbours<Node> = new Vector.<Node>();
		Node addNode;

		if (node.positionY)
		{
			if (node.positionX)
			{
				addNode = _grid[node.positionY - 1][node.positionX - 1];
				if (addNode.walkable (objectType) && _grid[node.positionY - 1][node.positionX].walkable (objectType) && _grid[node.positionY][node.positionX - 1].walkable (objectType))
				{
					addNode.cost = node.terrainCost * BEVEL_COST;
					neighbours.push(addNode);
				}
			}
			if (node.positionX < _grid[node.positionY].Length - 1)
			{
				addNode = _grid[node.positionY - 1][node.positionX + 1];
				if (addNode.walkable (objectType) && _grid[node.positionY - 1][node.positionX].walkable (objectType) && _grid[node.positionY][node.positionX + 1].walkable (objectType))
				{
					addNode.cost = node.terrainCost * BEVEL_COST;
					neighbours.push(addNode);
				}
			}

			addNode = _grid[node.positionY - 1][node.positionX];
			if (addNode.walkable (objectType))
			{
				addNode.cost = node.terrainCost * STREIGHT_COST;
				neighbours.push(addNode);
			}
		}

		if (node.positionX)
		{
			addNode = _grid[node.positionY][node.positionX - 1];
			if (addNode.walkable (objectType))
			{
				addNode.cost = node.terrainCost * STREIGHT_COST;
				neighbours.push(addNode);
			}
		}
		if (node.positionX < _grid[node.positionY].Length - 1)
		{
			addNode = _grid[node.positionY][node.positionX + 1];
			if (addNode.walkable (objectType))
			{
				addNode.cost = node.terrainCost * STREIGHT_COST;
				neighbours.push(addNode);
			}
		}

		if (node.positionY < _grid.Length - 1)
		{
			if (node.positionX)
			{
				addNode = _grid[node.positionY + 1][node.positionX - 1];
				if (addNode.walkable (objectType) && _grid[node.positionY + 1][node.positionX].walkable (objectType) && _grid[node.positionY][node.positionX - 1].walkable (objectType))
				{
					addNode.cost = node.terrainCost * BEVEL_COST;
					neighbours.push(addNode);
				}
			}
			if (node.positionX < _grid[node.positionY].Length - 1)
			{
				addNode = _grid[node.positionY + 1][node.positionX + 1];
				if (addNode.walkable (objectType) && _grid[node.positionY + 1][node.positionX].walkable (objectType) && _grid[node.positionY][node.positionX + 1].walkable (objectType))
				{
					addNode.cost = node.terrainCost * BEVEL_COST;
					neighbours.push(addNode);
				}
			}

			addNode = _grid[node.positionY + 1][node.positionX];
			if (addNode.walkable (objectType))
			{
				addNode.cost = node.terrainCost * STREIGHT_COST;
				neighbours.push(addNode);
			}
		}
		return neighbours;
	}
 */

	/**
	 * Searching neighbours algorithm - returns bevel neighbours if there's one blocked (!walkable (objectType)) node around
	 * @param node node for searching neighbours
	 * @return vector with neighbours
	 */

/*
		private function searchWithSemiBlockedBevels( Node node  ):Vector.<Node>
		{
			Vector. neighbours<Node> = new Vector.<Node>();
			Node addNode;

			if (node.positionY)
			{
				if (node.positionX)
				{
					addNode = _grid[node.positionY - 1][node.positionX - 1];
					if (addNode.walkable (objectType) && (_grid[node.positionY - 1][node.positionX].walkable (objectType) || _grid[node.positionY][node.positionX - 1].walkable (objectType)))
					{
						addNode.cost = node.terrainCost * BEVEL_COST;
						neighbours.push(addNode);
					}
				}
				if (node.positionX < _grid[node.positionY].Length - 1)
				{
					addNode = _grid[node.positionY - 1][node.positionX + 1];
					if (addNode.walkable (objectType) && (_grid[node.positionY - 1][node.positionX].walkable (objectType) || _grid[node.positionY][node.positionX + 1].walkable (objectType)))
					{
						addNode.cost = node.terrainCost * BEVEL_COST;
						neighbours.push(addNode);
					}
				}

				addNode = _grid[node.positionY - 1][node.positionX];
				if (addNode.walkable (objectType))
				{
					addNode.cost = node.terrainCost * STREIGHT_COST;
					neighbours.push(addNode);
				}
			}

			if (node.positionX)
			{
				addNode = _grid[node.positionY][node.positionX - 1];
				if (addNode.walkable (objectType))
				{
					addNode.cost = node.terrainCost * STREIGHT_COST;
					neighbours.push(addNode);
				}
			}
			if (node.positionX < _grid[node.positionY].Length - 1)
			{
				addNode = _grid[node.positionY][node.positionX + 1];
				if (addNode.walkable (objectType))
				{
					addNode.cost = node.terrainCost * STREIGHT_COST;
					neighbours.push(addNode);
				}
			}

			if (node.positionY < _grid.Length - 1)
			{
				if (node.positionX)
				{
					addNode = _grid[node.positionY + 1][node.positionX - 1];
					if (addNode.walkable (objectType) && (_grid[node.positionY + 1][node.positionX].walkable (objectType) || _grid[node.positionY][node.positionX - 1].walkable (objectType)))
					{
						addNode.cost = node.terrainCost * BEVEL_COST;
						neighbours.push(addNode);
					}
				}
				if (node.positionX < _grid[node.positionY].Length - 1)
				{
					addNode = _grid[node.positionY + 1][node.positionX + 1];
					if (addNode.walkable (objectType) && (_grid[node.positionY + 1][node.positionX].walkable (objectType) || _grid[node.positionY][node.positionX + 1].walkable (objectType)))
					{
						addNode.cost = node.terrainCost * BEVEL_COST;
						neighbours.push(addNode);
					}
				}

				addNode = _grid[node.positionY + 1][node.positionX];
				if (addNode.walkable (objectType))
				{
					addNode.cost = node.terrainCost * STREIGHT_COST;
					neighbours.push(addNode);
				}
			}
			return neighbours;
		}
		*/

}
