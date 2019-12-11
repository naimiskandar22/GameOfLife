using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridGenerationScript : MonoBehaviour {

	public static GridGenerationScript instance;

	public Transform gridPrefab;
	public Vector2 gridSize;
	public Vector2 simuMapSize = new Vector2(50f, 80f);
	public GridScript[,] gridMap = new GridScript[10,10];
	public TileStatus[,] simulatedMap;

	//AutoRun Speed
	public Slider speedSlider;
	float autorunTimer;
	float autorunDuration;
	public bool autorun = false;

	//Camera
	public Slider cameraSlider;

	//Grid Size
	public Slider gridXSlider;
	public Slider gridYSlider;
	public 

	void Awake()
	{
		if(instance == null) instance = this;
	}

	// Use this for initialization
	void Start () {
		//gridMap = new GridScript[(int)gridSize.y,(int)gridSize.x];
		GenerateGrid();
		autorunTimer = speedSlider.value;
	}
	
	// Update is called once per frame
	void Update () 
	{
//		gridXSlider.onValueChanged.AddListener(delegate {GridChange(); });
//		gridYSlider.onValueChanged.AddListener(delegate {GridChange(); });

		autorunDuration = speedSlider.value;

		Camera.main.orthographicSize = cameraSlider.value;

		if(!autorun)
		{
			if(Input.GetKeyDown(KeyCode.Space))
			{
				ImplementGridRule();
			}
		}
		else
		{
			if(autorunTimer <= 0f)
			{
				autorunTimer = autorunDuration;

				ImplementGridRule();
			}
			else
			{
				autorunTimer -= Time.deltaTime;
			}
		}

	}

	public void GridChange()
	{
		int gridX = (int)simuMapSize.x / 2 - (int)gridXSlider.value;
		int gridY = (int)simuMapSize.y / 2 - (int)gridYSlider.value;

		for(int x = 0; x < simuMapSize.x; x++)
		{
			for(int y = 0; y < simuMapSize.y; y++)
			{
				gridMap[y,x].mytile.fillRenderer.enabled = false;
				gridMap[y,x].mytile.frameRenderer.enabled = false;
			}
		}

		for(int x = gridX; x < gridX + gridXSlider.value * 2; x++)
		{
			for(int y = gridY; y < gridY + gridYSlider.value * 2; y++)
			{
				gridMap[y,x].mytile.fillRenderer.enabled = true;
				gridMap[y,x].mytile.frameRenderer.enabled = true;
			}
		}
	}

	public void SwitchAutoRun()
	{
		autorun = !autorun;
	}

	void GenerateGrid()
	{
		gridMap = new GridScript[(int)simuMapSize.y,(int)simuMapSize.x];
		simulatedMap = new TileStatus[(int)simuMapSize.y,(int)simuMapSize.x];

		//Creating grids
		for(int x = 0; x < simuMapSize.x; x++)
		{
			for(int y = 0; y < simuMapSize.y; y++)
			{
				Vector2 gridPosition = new Vector2((-simuMapSize.x/2 + 0.5f + x) / 2, (-simuMapSize.y/2 + 0.5f + y) / 2);
				Transform newTile = Instantiate(gridPrefab, gridPosition, Quaternion.identity) as Transform;
				GridScript newTileScript = newTile.GetComponent<GridScript>();
				newTileScript.mytile.tileNeighbours = new GridScript[8];

				gridMap[y,x] = newTileScript;
				newTileScript.gridPos[0] = y;
				newTileScript.gridPos[1] = x;
				//newTile.parent = transform;
				newTile.SetParent(transform);
			}
		}

		//Generating simulated map
//		for(int x = 0; x < simuMapSize.x; x++)
//		{
//			for(int y = 0; y < simuMapSize.y; y++)
//			{
//				simulatedMap[y,x] = null;
//			}
//		}
//
//		for(int x = (int)gridSize.x / 2 - 1; x >= 0; x--)
//		{
//			for(int y = (int)gridSize.y / 2 - 1; y >= 0; y--)
//			{
//
//				int gX = (int)gridSize.x / 2 - 1 - x;
//				int gY = (int)gridSize.y / 2 - 1 - y;
//
//				int sX = ((int)simuMapSize.x / 2 - 1) - x;
//				int sY = ((int)simuMapSize.y / 2 - 1) - y;
//				simulatedMap[sY, sX] = gridMap[gY, gX];
//			}
//		}
//
//		for(int x = 0; x < (int)gridSize.x / 2; x++)
//		{
//			for(int y = 0; y < (int)gridSize.y / 2; y++)
//			{
//				int gX = (int)gridSize.x / 2 + x;
//				int gY = (int)gridSize.y / 2 + y;
//
//				int sX = (int)simuMapSize.x / 2 + x;
//				int sY = (int)simuMapSize.y / 2 + y;
//				simulatedMap[sY, sX] = gridMap[gY, gX];
//			}
//		}

		//Assigning neighbours
		for(int x = 0; x < simuMapSize.x; x++)
		{
			for(int y = 0; y < simuMapSize.y; y++)
			{
				//SW neighbour
				if(x > 0 && y > 0)
				{
					gridMap[y,x].mytile.tileNeighbours[0] = gridMap[y-1, x-1];
				}
				//S neighbour
				if(y > 0)
				{
					gridMap[y,x].mytile.tileNeighbours[1] = gridMap[y-1, x];
				}
				//SE neighbour
				if(x < simuMapSize.x - 1 && y > 0)
				{
					gridMap[y,x].mytile.tileNeighbours[2] = gridMap[y-1, x+1];
				}
				//W neighbour
				if(x > 0)
				{
					gridMap[y,x].mytile.tileNeighbours[3] = gridMap[y, x-1];
				}
				//E neighbour
				if(x < simuMapSize.x - 1)
				{
					gridMap[y,x].mytile.tileNeighbours[4] = gridMap[y, x+1];
				}
				//NW neighbour
				if(x > 0 && y < simuMapSize.y - 1)
				{
					gridMap[y,x].mytile.tileNeighbours[5] = gridMap[y+1, x-1];
				}
				//N neighbour
				if(y < simuMapSize.y - 1)
				{
					gridMap[y,x].mytile.tileNeighbours[6] = gridMap[y+1, x];
				}
				//NE neighbour
				if(x < simuMapSize.x - 1 && y < simuMapSize.y - 1)
				{
					gridMap[y,x].mytile.tileNeighbours[7] = gridMap[y+1, x+1];
				}

				simulatedMap[y,x] = gridMap[y,x].mytile;
			}
		}

		GridChange();
	}

	public void ResetGrid()
	{
		for(int x = 0; x < simuMapSize.x; x++)
		{
			for(int y = 0; y < simuMapSize.y; y++)
			{
				gridMap[y,x].mytile.isAlive = false;
				gridMap[y,x].mytile.fillRenderer.color = new Color((75f/255f), (75f/255f), (75f/255f));

				gridMap[y,x].mytile.frameRenderer.color = Color.black;

				simulatedMap[y,x].isAlive = false;
			}
		}
	}

	public void CheckGridRule()
	{
		for(int x = 0; x < simuMapSize.x; x++)
		{
			for(int y = 0; y < simuMapSize.y; y++)
			{
				simulatedMap[y,x] = gridMap[y,x].mytile;
				simulatedMap[y,x].frameRenderer.color = Color.black;
			}
		}

		for(int x = 0; x < simuMapSize.x; x++)
		{
			for(int y = 0; y < simuMapSize.y; y++)
			{
				int liveNeighbours = 0;

				if(simulatedMap[y,x].isAlive)
				{
					for(int i = 0; i < simulatedMap[y,x].tileNeighbours.Length; i++)
					{
						if(simulatedMap[y,x].tileNeighbours[i] != null)
						{
							if(simulatedMap[y,x].tileNeighbours[i].mytile.isAlive)
							{
								liveNeighbours++;
							}
						}
					}

					if(liveNeighbours < 2 || liveNeighbours > 3)
					{
						gridMap[y,x].mytile.frameRenderer.color = Color.red;

						simulatedMap[y,x].isAlive = false;
						//simulatedMap[y,x].fillRenderer.color = new Color((75f/255f), (75f/255f), (75f/255f));
					}
				}
				else
				{
					for(int i = 0; i < simulatedMap[y,x].tileNeighbours.Length; i++)
					{
						if(simulatedMap[y,x].tileNeighbours[i] != null)
						{
							if(simulatedMap[y,x].tileNeighbours[i].mytile.isAlive)
							{
								liveNeighbours++;
							}
						}
					}

					if(liveNeighbours == 3)
					{
						gridMap[y,x].mytile.frameRenderer.color = Color.green;

						simulatedMap[y,x].isAlive = true;
						//simulatedMap[y,x].fillRenderer.color = Color.yellow;
					}
				}
			}
		}
	}

	public void ImplementGridRule()
	{
		for(int x = 0; x < simuMapSize.x; x++)
		{
			for(int y = 0; y < simuMapSize.y; y++)
			{
				gridMap[y,x].mytile = simulatedMap[y,x];

				if(gridMap[y,x].mytile.isAlive)
				{
					gridMap[y,x].mytile.fillRenderer.color = Color.yellow;
				}
				else
				{
					gridMap[y,x].mytile.fillRenderer.color = new Color((75f/255f), (75f/255f), (75f/255f));
				}
			}
		}

		CheckGridRule();
	}
}
