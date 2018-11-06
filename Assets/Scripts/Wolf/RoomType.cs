using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomType : MonoBehaviour {
    public int type;
    public GameObject tile;
    public GameObject roomAbove;
    public float minX;
    public float minY;
    public float maxX;
    public float maxY;

    public int randXSize1;
    public int randXSize2;
    public int randXMid1;
    public int randXMid2;

    public bool isPath= false;
    public float forcedXSize;
    public float forcedXMid;

    private List<GameObject> ceilingTiles = new List<GameObject>();

    //private int count = 0;
    private bool alreadyModded = false;

    public void RoomDestruction()
    {
        Destroy(gameObject);
    }

    private void CeilingDestruction(GameObject tiles)
    {
        Destroy(tiles);
    }

	// Use this for initialization
	void Start() {
         minX = transform.position.x - 39;
         minY = transform.position.y - 19;

         maxX = transform.position.x + 39;
         maxY = transform.position.y + 19;

         randXSize1 = Random.Range(6, 14);
         randXSize2 = Random.Range(6, 14);
         randXMid1 = Random.Range(12, 68);
         randXMid2 = Random.Range(12, 68);


        for (float k = minX ; k <= maxX ; k += 2)
        {
            
            Vector2 newPos = new Vector2(k, maxY);
            if (type == 2 || type == 3)
            {
                if ((k > minX + randXMid1 + randXSize1 / 2) || (k < minX + randXMid1 - randXSize1 / 2))
                {
                    GameObject ceilingTile =(Instantiate(tile, newPos, Quaternion.identity, transform));
                    this.ceilingTiles.Add(ceilingTile);
                }
            } 
            else
            {
                GameObject ceilingTile = Instantiate(tile, newPos, Quaternion.identity, transform);
                this.ceilingTiles.Add(ceilingTile);
                
            }

            newPos = new Vector2(k, minY);
            if (type == 1 || type == 3)
            {
                if ((k > minX + randXMid2 + randXSize2 / 2) || (k < minX + randXMid2 - randXSize2 / 2))
                {
                    Instantiate(tile, newPos, Quaternion.identity, transform);
                }
            } else
            {
                Instantiate(tile, newPos, Quaternion.identity, transform);
            }
        }

        int randYMax1 = Random.Range(14, 20);
        int randYMax2 = Random.Range(14, 20);
        int randYLow1= Random.Range(2, 6);
        int randYLow2 = Random.Range(2, 6);

        for (float j = maxY ; j >= minY ; j -= 2)
        {
            Vector2 newPos = new Vector2(minX , j);
            if (j > minY+randYLow1+randYMax1 || j < minY+randYLow1)
            {
                Instantiate(tile, newPos, Quaternion.identity, transform);
            }

            newPos = new Vector2(maxX, j);
            if (j > minY + randYLow2 + randYMax2 || j < minY + randYLow2)
            {
                Instantiate(tile, newPos, Quaternion.identity, transform);
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (isPath && !alreadyModded)
        {
            forcedXMid = roomAbove.GetComponent<RoomType>().randXMid2;
            forcedXSize = roomAbove.GetComponent<RoomType>().randXSize2;

            alreadyModded = true;
            while (this.ceilingTiles.Count > 0)
            {
                Object.DestroyImmediate(this.ceilingTiles[0]);
                this.ceilingTiles.RemoveAt(0);
            }
            for (float k = minX; k <= maxX; k += 2)
            {

                Vector2 newPos = new Vector2(k, maxY);
                    if ((k > minX + forcedXMid + forcedXSize / 2) || (k < minX + forcedXMid - forcedXSize / 2))
                    {
                        
                        Instantiate(tile, newPos, Quaternion.identity, transform);
                    }
            }
        }
	}
}
