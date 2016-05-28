using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HeroMove : MonoBehaviour
{

    //最大値の指定
    private int XLIMIT;
    private int YLIMIT;

    private LoadJSON loadJson;


    private WorldGen WG;
    private GameObject worldGen;
    public Vector2 heroPos, nextPos, direction, pastDirection = Vector2.zero;

    public bool angry, moving, otherWay, done, goal = false;
    private float call = 0;
    private int stopCount = 0;
    private int[] dire;
    // Use this for initialization
    void Start()
    {
        heroPos = transform.position;
    }

    void randomDire()
    {


    }
    void AngryMoving(Vector2 mypos)
    {
        //探索方向の指定とXYそれぞれをintにキャスト
        if (!moving && WG.mapdata != null)
        {
            dire = new int[4] { 0, 1, 2, 3 };
            //なんか有名な配列のシャッフルアルゴリズムを実行
            for (int i = 0; i < dire.Length; i++)
            {
                int rand = Random.Range(0, 4);
                dire[i] += dire[rand];
                dire[rand] = dire[i] - dire[rand];
                dire[i] -= dire[rand];
            }
        }
        //探索を開始
        for (int i = 0; i < dire.Length; i++)
        {
            if (!moving)
            {
                switch (dire[i])
                {
                    case 0:
                        direction = new Vector2(0, 1);
                        break;
                    case 1:
                        direction = new Vector2(1, 0);
                        break;
                    case 2:
                        direction = new Vector2(0, -1);
                        break;
                    case 3:
                        direction = new Vector2(-1, 0);
                        break;
                }
            }
            int x = (int)mypos.x;
            int y = (int)mypos.y;
            int dx = (int)direction.x;
            int dy = (int)direction.y;

            nextPos = new Vector2(x + dx, y + dy);
            if(WG.mapdata[x + dx, y + dy] == 4)
            {
                otherWay = false;
                transform.position = nextPos;
                pastDirection = new Vector2(dx, dy);
                goal = true;
                Debug.Log("goal");
                break;
            }
            if (WG.mapdata[x + dx, y + dy] != 0)
            {
                if (direction != -pastDirection)
                {
                    moving = true;
                    if (!otherWay)
                    {
                        if (dx != 0)
                        {
                            if (WG.mapdata[x, y + 1] == 1 || WG.mapdata[x, y - 1] == 1)
                            {
                                moving = false;
                                otherWay = true;
                            }
                        }
                        else
                        {
                            if (WG.mapdata[x + 1, y] == 1 || WG.mapdata[x - 1, y] == 1)
                            {
                                moving = false;
                                otherWay = true;
                            }
                        }
                    }
                    if (moving)
                    {
                        otherWay = false;
                        transform.position = nextPos;
                        pastDirection = new Vector2(dx, dy);
                        break;
                    }
                }
            }
            else
            {
                if (dx != 0)
                {
                    if (WG.mapdata[x, y + 1] == 0 && WG.mapdata[x, y - 1] == 0)
                    {
                        pastDirection = -pastDirection;
                    }
                }
                else
                {
                    if (WG.mapdata[x + 1, y] == 0 && WG.mapdata[x - 1, y] == 0)
                    {
                        pastDirection = -pastDirection;
                    }
                }
                moving = false;

            }
        }
    }
        
    

    // Update is called once per frame
    void Update()
    {

        if (worldGen == null)
        {
            worldGen = GameObject.FindWithTag("WorldGen");
            WG = worldGen.GetComponent<WorldGen>();

        }
        if (WG.setDone && !goal)
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
