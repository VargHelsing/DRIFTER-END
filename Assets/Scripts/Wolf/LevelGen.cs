using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGen : MonoBehaviour {
    public GameObject[] rooms;
    public GameObject tile;
    public GameObject barrier;

    public GameObject roomAbove;
    public GameObject roomBellow;

    private int direction;
    public float moveAmountX;
    public float moveAmountY;
    public float maxX;
    public float minX;
    public float maxY;
    public float minY;
    public float startTimeBtwRoom = 0.25f;
    public LayerMask roomLayerMask;

    //private int roomNum;
    private bool stop = false;

    private float timeBtwRoom;
    private int downCounter;
    // Use this for initialization
    void Start () {
        for(float k = minX-80; k <= maxX+40; k+=2)
        {
            Vector2 newPos = new Vector2(k, maxY+20);
            Instantiate(tile, newPos, Quaternion.identity, barrier.transform);

            newPos = new Vector2(k, minY - 20);
            Instantiate(tile, newPos, Quaternion.identity, barrier.transform);
        }

        for (float j = maxY+20 ; j >= minY-20; j -= 2)
        {
            Vector2 newPos = new Vector2(minX-80, j);
            Instantiate(tile, newPos, Quaternion.identity, barrier.transform);

            newPos = new Vector2(maxX+40, j);
            Instantiate(tile, newPos, Quaternion.identity, barrier.transform);
        }

        direction = Random.Range(1, 6);
	}
	
	// Update is called once per frame
	void Update () {
        if (timeBtwRoom <= 0 && !stop)
        {
            Move();
            timeBtwRoom = startTimeBtwRoom;
        } else
        {
            timeBtwRoom -= Time.deltaTime;
        }
	}

    private void Move()
    {
        if (direction == 1 || direction == 2)
        {
            if (transform.position.x < maxX)
            {
                downCounter = 0;
                //Move Right
                Vector2 newPos = new Vector2(transform.position.x + moveAmountX, transform.position.y);
                transform.position = newPos;

                int rand = Random.Range(0, rooms.Length);
                Instantiate(rooms[rand], transform.position, Quaternion.identity);

                direction = Random.Range(1, 6);
                if (direction == 3)
                {
                    direction=2;
                }
                else
                {
                    if (direction == 4)
                    {
                        direction = 5;
                    }
                }

            } else
            {
                direction = 5;
            }
        }
        else
        {
            if (direction == 3 || direction == 4)
            {
                if (transform.position.x > minX)
                {
                    downCounter = 0;
                    //Move Left
                    Vector2 newPos = new Vector2(transform.position.x - moveAmountX, transform.position.y);
                    transform.position = newPos;

                    int rand = Random.Range(0, rooms.Length);
                    Instantiate(rooms[rand], transform.position, Quaternion.identity);

                    direction = Random.Range(3, 6);
                   
                } else
                {
                    direction = 5;
                }
            }
            else
            {
                if (direction == 5)
                {
                    RandRoomGen(transform.position.x,transform.position.y);
                    if (transform.position.y > minY)
                    {
                        downCounter++;
                        //Check Upper Room layer mask
                        Collider2D roomDetection = Physics2D.OverlapCircle(transform.position, 10, roomLayerMask);

                        if (roomDetection != null)
                        {
                            roomAbove = roomDetection.gameObject;

                            if (roomDetection.GetComponent<RoomType>().type != 1 && roomDetection.GetComponent<RoomType>().type != 3)
                            {
                                if (downCounter >= 2)
                                {
                                    //Normalize room opening
                                    roomAbove = Instantiate(rooms[3], transform.position, Quaternion.identity);
                                    roomAbove.GetComponent<RoomType>().roomAbove = roomDetection.GetComponent<RoomType>().roomAbove;
                                    roomAbove.GetComponent<RoomType>().isPath = true;
                                    roomDetection.GetComponent<RoomType>().RoomDestruction();
                                }
                                else
                                {
                                    roomDetection.GetComponent<RoomType>().RoomDestruction();

                                    int randBottomRoom = Random.Range(1, 4);
                                    if (randBottomRoom == 2)
                                    {
                                        randBottomRoom = 1;
                                    }
                                    roomAbove = Instantiate(rooms[randBottomRoom], transform.position, Quaternion.identity);
                                }
                            }
                        }
                        //Move Down
                        Vector2 newPos = new Vector2(transform.position.x, transform.position.y - moveAmountY);
                        transform.position = newPos;

                        int rand = Random.Range(2, 4);
                        roomBellow = Instantiate(rooms[rand], transform.position, Quaternion.identity);

                        //Normalize room opening
                        if (roomAbove != null && roomBellow != null)
                        {
                            roomBellow.GetComponent<RoomType>().roomAbove = roomAbove;
                            roomBellow.GetComponent<RoomType>().isPath = true;
                        }

                        direction = Random.Range(1, 6);
                    }
                    else {
                        stop = true;
                    }
                }
                else
                {
                    if (direction == 6)
                    {
                        //Move Up
                        Vector2 newPos = new Vector2(transform.position.x, transform.position.y + moveAmountY);
                        transform.position = newPos;
                    }
                }
            }
        }

        //roomNum = Random.Range(0, 3);
    }

    private void RandRoomGen(float x, float y)
    {
        float subXRight=x;
        float subXLeft=x;
        Collider2D roomDetection;

        while (subXRight < maxX)
        {
            subXRight += moveAmountX;
            Vector2 newPos = new Vector2(subXRight, transform.position.y);
            transform.position = newPos;
            roomDetection = Physics2D.OverlapCircle(transform.position, 10, roomLayerMask);
            if (roomDetection == null)
            {         
                int rand = Random.Range(0, 4);
                Instantiate(rooms[rand], transform.position, Quaternion.identity);
            }
        }

        while (subXLeft > minX)
        {
            subXLeft -= moveAmountX;
            Vector2 newPos = new Vector2(subXLeft, transform.position.y);
            transform.position = newPos;
            roomDetection = Physics2D.OverlapCircle(transform.position, 10, roomLayerMask);
            if (roomDetection == null)
            {
                int rand = Random.Range(0, 4);
                Instantiate(rooms[rand], transform.position, Quaternion.identity);
            }
        }

        Vector2 oldPos = new Vector2(x, transform.position.y);
        transform.position = oldPos;

    }
}
