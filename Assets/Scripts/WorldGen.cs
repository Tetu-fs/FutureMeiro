using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldGen : MonoBehaviour
{
    //最大値の指定
    public int XLIMIT = 29;
    public int YLIMIT = 15;
    //壁となるオブジェクトを格納
    public GameObject wall;
   
    //マップのサイズを格納
    bool[,] mapdata;
    //スタート位置
    int startX, startY;
    //ゴール位置
    int goalX, goalY;
    //壁を格納する配列
    GameObject[,] wallsSet;

    // Use this for initialization
    void Start()
    {
        //マップを初期化
        mapdata = new bool[XLIMIT, YLIMIT];
        //迷路生成
        MakeMaze();
    }

    // Update is called once per frame
    void Update()
    {
        //いったいなにがかいてあったのか、しるものはいない
    }

    public void MakeMaze()
    {
        //リセット用かな？
        float destroyTime = Time.realtimeSinceStartup;
        //マップをのすべてをfalse（壁判定）に
        for (int i = 0; i < mapdata.GetLength(0); i++)
        {
            for (int j = 0; j < mapdata.GetLength(1); j++)
            {
                mapdata[i, j] = false;
            }
        }
        //生成された壁を総当りし、削除
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("wall"))
        {
            Destroy(g);
        }
        //破壊が終了した時間
        destroyTime = Time.realtimeSinceStartup - destroyTime;
        Debug.Log(destroyTime);
        //生成を行う
        float makingTime = Time.realtimeSinceStartup;

        //開始位置をランダムで設定
        /*
        29x15のとき
        intなので　29/2 = 14
        14*2-1で　1～27
        同じ計算で　1～5
        */
        startX = Random.Range(1, XLIMIT / 2) * 2 - 1;
        startY = Random.Range(1, YLIMIT / 4) * 2 - 1;
        //穴を掘る
        DigMaze(startX, startY);
        //穴が掘り終わり生成完了
        makingTime = Time.realtimeSinceStartup - makingTime;
        Debug.Log(makingTime);

        //壁を立てる
        float settingTime = Time.realtimeSinceStartup;
        SetWall();
        settingTime = Time.realtimeSinceStartup - settingTime;
        Debug.Log(settingTime);
    }
    //穴を掘る関数
    void DigMaze(int x, int y)
    {
        //渡された引数から配列の中身をtrue
        mapdata[x, y] = true;
        //上下左右の方向初期化
        int[] dires = new int[4] { 0, 1, 2, 3 };
        //方向指定後の座標宣言
        int dx = 0;
        int dy = 0;
        for (int i = 0; i < 4; i++)//方向シャッフル（3回試行
        {
            //ランダムで方向を設定（格納されている順番を入れ替え？
            int rand = Random.Range(0, 4);
            dires[i] += dires[rand];
            dires[rand] = dires[i] - dires[rand];
            dires[i] -= dires[rand];
        }
        //シャッフルされた方向を読む
        for (int i = 0; i < 4; i++)
        {
            //4方向を順番に試行
            switch (dires[i])
            {
                case 0:
                    dx = 0;
                    dy = -1;
                    break;
                case 1:
                    dx = 1;
                    dy = 0;
                    break;
                case 2:
                    dx = 0;
                    dy = 1;
                    break;
                case 3:
                    dx = -1;
                    dy = 0;
                    break;
            }
            //引数とランダム方向を倍にしたものを足し2つ先の要素をみる
            int xx = x + dx * 2;
            int yy = y + dy * 2;
            //壁や壁の外を参照しておらず、false(壁)であれば
            if (0 < xx && xx < XLIMIT && 0 < yy && yy < YLIMIT && !mapdata[xx, yy])
            {
                //渡された座標とランダム方向を加算した座標をtrueにして、自身を呼び出し
                mapdata[x + dx, y + dy] = true;
                DigMaze(xx, yy);
            }
        }
    }
    //実際に壁をセット
    void SetWall()
    {
        //
        int xHalf = XLIMIT / 2;
        int yQuarter = YLIMIT / 4;
        //マップ総当り
        for (int i = 0; i < mapdata.GetLength(0); i++)//ヨコ
        {
            for (int j = 0; j < mapdata.GetLength(1); j++)//タテ
            {
                //該当座標がfalseで、さらに真ん中の最下部、またはそのひとつ上でなければ
                if (!mapdata[i, j] && !(i == xHalf && (j == 0 || j == 1)))
                {
                    //壁をインスタンス化
                    GameObject wall = Instantiate<GameObject>(this.wall);
                    //位置を真ん中に来るように調整
                    wall.transform.position = new Vector2(i - xHalf, j - yQuarter);
                }
            }
        }
    }
}