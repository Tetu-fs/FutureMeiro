using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldGen : MonoBehaviour
{

    //最大値の指定
    private int XLIMIT;
    private int YLIMIT;

    private LoadJSON loadJson;

    //壁となるオブジェクトを格納
    public GameObject wall;
    public GameObject hero;
    public AudioClip loadSE;
    public AudioClip endSE;

    public bool setDone = false;

    private GameObject heroInst;
    private AudioSource se;
    private HeroMove heroMove;

    private bool setting = false;
    private bool setHero = false;
    //マップのデータを格納
    //0=壁 1=道  2=主人公　3=ギミック予定
    public int[,] mapdata;

    //スタート位置
    int startX, startY;
    //ゴール位置
    int goalX, goalY;

    //壁を格納する配列
    GameObject[] walls;

    int callNum = 0;
    float timer = 0;


    // Use this for initialization
    void Start()
    {
        loadJson = new LoadJSON();
        string read = loadJson.Reading("mapsize");
        MapSize mapSize = JsonUtility.FromJson<MapSize>(read);

        XLIMIT = mapSize.x;
        YLIMIT = mapSize.y;
        se = GetComponent<AudioSource>();
        //マップを初期化
        mapdata = new int[XLIMIT, YLIMIT];
        //迷路生成
        MakeMaze();
        walls = GameObject.FindGameObjectsWithTag("wall");
        heroMove = GameObject.FindWithTag("Hero").GetComponent<HeroMove>();
        setting = true;

    }


    // Update is called once per frame
    void Update()
    {
        if (setting)
        {
            callWallMesh();
        }
        else if(!setDone)
        {
            setDone = true;
        }
    }



    void callWallMesh()
    {
        if (callNum < walls.Length)
        {
            walls[callNum].SendMessage("MeshEnable");
            se.PlayOneShot(loadSE);

            callNum++;
        }
        else
        {
            se.PlayOneShot(endSE);

            setting = false;
        }

    }

    void MakeMaze()
    {
        //  全てを壁(0)に初期化
        for (int i = 0; i < mapdata.GetLength(0); i++) 
        {
            for (int j = 0; j < mapdata.GetLength(1); j++)
            {
                mapdata[i, j] = 0;
            }
        }
        startX = Random.Range(1, XLIMIT / 2) * 2 - 1;
        startY = Random.Range(1, YLIMIT / 4) * 2 - 1;
        Dig(startX, startY);
        SetWall();
    }

    void Dig(int x,int y)
    {
        //渡された値をで道(1)へ
        mapdata[x, y] = 1;
        int[] dire = new int[4] { 0, 1, 2, 3 };
        //なんか有名な配列のシャッフルアルゴリズムを実行
        for(int i = 0; i < dire.Length; i++)
        {
            int rand = Random.Range(0, 4);
            dire[i] += dire[rand];
            dire[rand] = dire[i] - dire[rand];
            dire[i] -= dire[rand];
        }
        //探索方向の指定とXYそれぞれをintにキャスト
        Vector2 direction = Vector2.zero;

        //探索を開始
        for (int i = 0; i < dire.Length; i++)
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
            int dx = (int)direction.x;
            int dy = (int)direction.y;

            int xx = x + dx * 2;
            int yy = y + dy * 2;
            //画面端ではなくかつ壁(0)であれば
            if (0 < xx && xx < XLIMIT && 0 < yy && yy < YLIMIT && mapdata[xx, yy] == 0)
            {
                int rand = Random.Range(0, 10);
                if (!setHero && yy > (YLIMIT/4)*3 && rand == 0)
                {
                    mapdata[x + dx, y + dy] = 2;
                    setHero = true;
                }
                else
                {
                    mapdata[x + dx, y + dy] = 1;
                }
                Dig(xx, yy);
            }
        }
    }


    //壁をセット
    void SetWall()
    {
        //位置調整用変数
        int xHarf = XLIMIT / 2;
        float time = 0;
        //マップ総当り

        for (int y = 0; y < mapdata.GetLength(1); y++)
        {
            for (int x = 0; x < mapdata.GetLength(0); x++)
            {
                //該当座標がfalseで、さらに真ん中の最下部、またはそのひとつ上でなければ
                if (!(x == xHarf && (y == 0 || y == 1)))
                {
                    if(mapdata[x, y] == 0)
                    {                    //壁をインスタンス化
                        GameObject wall = Instantiate<GameObject>(this.wall);
                        wall.transform.SetParent(transform);
                        //位置を真ん中に来るように調整
                        wall.transform.position = new Vector2(x, y);

                    }
                    else if (mapdata[x, y] == 2)
                    {
                        heroInst = Instantiate<GameObject>(this.hero);
                        //位置を真ん中に来るように調整
                        heroInst.transform.position = new Vector2(x, y);
                    }
                }
            }
        }
    }

}