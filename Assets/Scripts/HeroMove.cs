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
    private Choices choices;
    private GameObject worldGen, canvas, talk;
    public Vector2 beforePos,heroPos, nextPos, dire, playerDire, pastDire = Vector2.zero;

    public bool angry, moving, canMove, choiced, otherWay, done, goal = false;
    private float call = 0;
    private int stopCount, angryPoint, life = 0;
    private int[] _dire;
    // Use this for initialization
    void Start()
    {
        heroPos = transform.position;
    }
    void CanMove()
    {
        canMove = true;
        otherWay = false;
        choiced = true;
    }
    void normalMoving(Vector2 mypos, Vector2 dire)
    {

        int x = (int)mypos.x;
        int y = (int)mypos.y;
        int dx = (int)dire.x;
        int dy = (int)dire.y;
        nextPos = new Vector2(x + dx, y + dy);
        if (WG.mapdata[x, y - 1] == 4)
        {
            otherWay = false;
            transform.position = new Vector2(x, y - 1);
            goal = true;
            Debug.Log("goal");
        }
        else if (WG.mapdata[x + dx, y + dy] != 0)
        {
            canMove = true;

            if (!otherWay && !choiced)
            {
                if (dire == pastDire)
                {
                    talk.SendMessage("BackWay");
                }
                if (dx != 0)
                {
                    if (WG.mapdata[x, y + 1] == 1 || WG.mapdata[x, y - 1] == 1)
                    {
                        canMove = false;
                        otherWay = true;
                    }
                }
                else
                {
                    if (WG.mapdata[x + 1, y] == 1 || WG.mapdata[x - 1, y] == 1)
                    {
                        canMove = false;
                        otherWay = true;
                    }
                }

            }

            if (canMove && !otherWay)
            {
                transform.position = nextPos;
                pastDire = new Vector2(-dx, -dy);
                choiced = false;
            }

        }
        else
        {
            canMove = false;
            if (dire == pastDire)
            {
                talk.SendMessage("GoWall");

            }
        }
        if ((Vector2)transform.position == beforePos)
        {
            choices.SendMessage("enableRoot");
        }

        beforePos = transform.position;

    }


    void AngryMoving(Vector2 mypos)
    {
        //探索方向の指定とXYそれぞれをintにキャスト
        if (!moving && WG.mapdata != null)
        {
            _dire = new int[4] { 0, 1, 2, 3 };
            //なんか有名な配列のシャッフルアルゴリズムを実行
            for (int i = 0; i < _dire.Length; i++)
            {
                int rand = Random.Range(0, 4);
                _dire[i] += _dire[rand];
                _dire[rand] = _dire[i] - _dire[rand];
                _dire[i] -= _dire[rand];
            }
        }
        //探索を開始
        for (int i = 0; i < _dire.Length; i++)
        {
            if (!moving)
            {
                switch (_dire[i])
                {
                    case 0:
                        dire = new Vector2(0, 1);
                        break;
                    case 1:
                        dire = new Vector2(1, 0);
                        break;
                    case 2:
                        dire = new Vector2(0, -1);
                        break;
                    case 3:
                        dire = new Vector2(-1, 0);
                        break;
                }
            }
            int x = (int)mypos.x;
            int y = (int)mypos.y;
            int dx = (int)dire.x;
            int dy = (int)dire.y;

            nextPos = new Vector2(x + dx, y + dy);
            if(WG.mapdata[x, y - 1] == 4)
            {
                otherWay = false;
                transform.position = new Vector2(x, y - 1);
                goal = true;
                Debug.Log("goal");
                break;
            }
            if (WG.mapdata[x + dx, y + dy] != 0)
            {
                if (dire != -pastDire)
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
                        pastDire = new Vector2(dx, dy);
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
                        pastDire = -pastDire;
                    }
                }
                else
                {
                    if (WG.mapdata[x + 1, y] == 0 && WG.mapdata[x - 1, y] == 0)
                    {
                        pastDire = -pastDire;
                    }
                }
                moving = false;

            }
        }
    }


    void DireNavi(int d)
    {
        switch (d)
        {
            case 0:
                playerDire = new Vector2(0, 1);
                break;
            case 1:
                playerDire = new Vector2(1, 0);
                break;
            case 2:
                playerDire = new Vector2(-1, 0);
                break;
            case 3:
                playerDire = new Vector2(0, -1);
                break;
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
        if(canvas == null)
        {
            canvas = GameObject.FindWithTag("GUI");
            choices = canvas.GetComponent<Choices>();
            talk = GameObject.FindWithTag("Talk");
        }
        if (WG.setDone && !goal)
        {
            heroPos = transform.position;

            call += Time.deltaTime;
            if (call > 0.1)
            {
                if (angry)
                {
                    AngryMoving(heroPos);
                }
                else if(!talk.GetComponent<TalkHero>().tutorialGet)
                {
                    normalMoving(heroPos, playerDire);
                }
                call = 0;
            }
        }
        if (goal)
        {
            if (!angry)
            {
                talk.GetComponent<TalkHero>().clearBool = true;
            }
        }
    }
}
