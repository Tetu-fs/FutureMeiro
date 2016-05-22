using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HeroMove : MonoBehaviour {

    //最大値の指定
    private int XLIMIT;
    private int YLIMIT;

    private LoadJSON loadJson;


    private WorldGen WG;
    private GameObject worldGen;
    private Vector2 heroPos, direction, pastDirection = Vector2.zero;

    private bool angry, moving, done = false;
    private List<Vector2> Direction = new List<Vector2>();
    private float call = 0;
    private int stopCount = 0;
    // Use this for initialization
    void Start()
    {
        heroPos = transform.position;
    }

    void AngryMoving(Vector2 mypos)
    {
        //探索方向の指定とXYそれぞれをintにキャスト
        if (!moving && WG.mapdata != null)
        {
            int[] dire = new int[4] { 0, 1, 2, 3 };

            //探索を開始
            for (int i = 0; i < dire.Length; i++)
            {

                switch (dire[i])
                {
                    case 0:
                        direction = new Vector2(0, 1);
                        Direction.Add(direction);
                        break;
                    case 1:
                        direction = new Vector2(1, 0);
                        Direction.Add(direction);
                        break;
                    case 2:
                        direction = new Vector2(0, -1);
                        Direction.Add(direction);
                        break;
                    case 3:
                        direction = new Vector2(-1, 0);
                        Direction.Add(direction);
                        break;
                }
            }
        }

        int x = (int)mypos.x;
        int y = (int)mypos.y;
        int dx = (int)direction.x;
        int dy = (int)direction.y;
        pastDirection = new Vector2(-dx, -dy);

        for (int i = 0; i < Direction.Count; i++)
        {
            Debug.Log(Direction[i]);
            if (Direction[i] == pastDirection)
            {
                Direction.Remove(pastDirection);
            }
        }

        for (int i = 0; i < Direction.Count; i++)
        {
            if (WG.mapdata[x + dx, y + dy] == 1 && )
            {
                moving = true;
                transform.position = new Vector2(x + dx, y + dy);
            }
            else
            {
                moving = false;
                if (stopCount == 0)
                {
                    stopCount++;
                }
                else
                {
                    pastDirection = Vector2.zero;
                    stopCount = 0;
                }

            }
        }
    }
	// Update is called once per frame
	void Update () {

        if (worldGen == null)
        {
            worldGen = GameObject.FindWithTag("WorldGen");
            WG = worldGen.GetComponent<WorldGen>();

        }
        if (WG.setDone)
        {
            heroPos = transform.position;
            AngryMoving(heroPos);

            call += Time.deltaTime;
            if (call > 0.1)
            {
                call = 0;
            }
        }
        
    }
}
