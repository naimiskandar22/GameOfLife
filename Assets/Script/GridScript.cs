using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public struct TileStatus
{
	public bool isAlive;
	public SpriteRenderer frameRenderer;
	public SpriteRenderer fillRenderer;

	public GridScript[] tileNeighbours;
};

public class GridScript : MonoBehaviour {

	public bool heldMouse = false;
	public int[] gridPos = new int[2];
	public TileStatus mytile;

	void Update()
	{
		if(Input.GetMouseButtonDown(0))
		{
			heldMouse = false;
		}

		if(Input.GetMouseButtonUp(0))
		{
			heldMouse = false;
		}
	}

	public void OnMouseOver()
	{
		if (!EventSystem.current.IsPointerOverGameObject())
		{
			if(!heldMouse)
			{
				if(Input.GetMouseButton(0))
				{
					if(!mytile.isAlive)
					{
						mytile.isAlive = true;
						mytile.fillRenderer.color = Color.yellow;
						mytile.frameRenderer.color = Color.black;

						GridGenerationScript.instance.simulatedMap[gridPos[0], gridPos[1]] = mytile;

						GridGenerationScript.instance.CheckGridRule();
					}
					else
					{
						mytile.isAlive = false;
						mytile.fillRenderer.color = new Color((75f/255f), (75f/255f), (75f/255f));
						mytile.frameRenderer.color = Color.black;

						GridGenerationScript.instance.simulatedMap[gridPos[0], gridPos[1]] = mytile;

						GridGenerationScript.instance.CheckGridRule();
					}

					heldMouse = true;
				}
			}
		}

			

		//mytile.frameRenderer.color = Color.cyan;
	}

	public void OnMouseExit()
	{
		//mytile.frameRenderer.color = Color.black;
	}

	public void OnMouseDown()
	{
		if (!EventSystem.current.IsPointerOverGameObject())
		{
			Debug.Log(gridPos[1] + " x " + gridPos[0]);

			if(!mytile.isAlive)
			{
				mytile.isAlive = true;
				mytile.fillRenderer.color = Color.yellow;
				mytile.frameRenderer.color = Color.black;

				GridGenerationScript.instance.simulatedMap[gridPos[0], gridPos[1]] = mytile;

				GridGenerationScript.instance.CheckGridRule();
			}
			else
			{
				mytile.isAlive = false;
				mytile.fillRenderer.color = new Color((75f/255f), (75f/255f), (75f/255f));
				mytile.frameRenderer.color = Color.black;

				GridGenerationScript.instance.simulatedMap[gridPos[0], gridPos[1]] = mytile;

				GridGenerationScript.instance.CheckGridRule();
			}
		}

	}
}
