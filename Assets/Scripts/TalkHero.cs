using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TalkHero : MonoBehaviour
{
    private LoadJSON loadJson = new LoadJSON();
    private HeroText heroText = new HeroText();
    private Text talkText;

    private List<string> tuto = new List<string>();
    private List<string> choice = new List<string>();
    private List<string> random = new List<string>();
    private List<string> okText = new List<string>();

    private List<string> damage = new List<string>();
    private List<string> ends = new List<string>();

    private List<string> viewText;
    private string init, wait, wall, clearText = "";

    private int tutoNum, playNum,randOne,randTwo = 0;
    private bool tutorial, playing, clear = false;
    public bool tutorialGet
    {
        get { return tutorial; }
    }
    public bool clearBool
    {
        get { return clear; }
        set { clear = clearBool; }
    }
    private WorldGen WG;
    private Choices choices;

    public GameObject worldGen, canvas;

    void SetNum(int i)
    {
        playNum = i;
    }

    void BackWay()
    {
        playNum = 4;
    }
    void GoWall()
    {
        playNum = 5;
    }
    // Use this for initialization
    void Start()
    {
        talkText = GetComponent<Text>();
        textInit();
        talkText.text = tuto[0];
        tutorial = true;
    }
    void textInit()
    {
        // tuto
        for (int i = 0; i <= 11; i++)
        {

            switch (i)
            {
                case 0:
                    init = "……！…通…くれ…！\r\n誰……！何…見えない…！";
                    break;
                case 1:
                    init = "どこかにつながった…？\r\nだれか、そこにいるのか？これが読めるか？";
                    break;
                case 2:
                    init = "本当か！？\r\nまだ運は残ってたか！\r\n頼む、助けてくれ！";
                    break;
                case 3:
                    init = "そうか…クソ…ってちょっと待て。\r\n返事できるって事は読めてるんだよな？\r\n冗談はよせ！頼む、助けてくれ！";
                    break;
                case 4:
                    init = "俺は仕事でとある迷宮を調査してたんだが、\r\n事前調査から漏れた罠があってな…\r\nマップやライトが全部ダメになっちまった！";
                    break;
                case 5:
                    init = "手の届く範囲だけが何とか見えてる状態だ…。\r\nイラついて通信端末ぶっ叩いてたら\r\n運よくアンタにつながったってわけだ！";
                    break;
                case 6:
                    init = "もしデータが吹っ飛んでなければ、\r\n画面にこの迷宮のマップが表示されてるはずだ。";
                    break;
                case 7:
                    init = "マップには発見された罠や、\r\n俺の現在地が表示されていると思う。\r\nこれを見ながら俺を出口に誘導してくれ！";
                    break;
                case 8:
                    init = "マップをみて、進む方向を教えてくれ。\r\n方向は大丈夫、アナログのコンパスがあるんだ。";
                    break;
                case 9:
                    init = "俺は指示された方向に進む。\r\n突き当りと分かれ道では止まって指示を待つ。\r\n簡単だろ？";
                    break;
                case 10:
                    init = "もし進んでいる道に罠があったら知らせてくれ。\r\nそしたらゆっくり移動する。\r\nそして罠の前に来たら教えてくれ。";
                    break;
                case 11:
                    init = "大丈夫、なんとかなるさ！\r\nいきなりで迷惑な話かもしれないが、頼む！\r\nもう頼れるのはアンタしかいない！";
                    break;
            }
            tuto.Add(init);
        }
        wait = "どう進めばいい？";
        //choice
        for (int i = 0; i <= 7; i++)
        {
            switch (i)
            {
                case 0:
                    init = "「上」";
                    break;
                case 1:
                    init = "「右」";
                    break;
                case 2:
                    init = "「左」";
                    break;
                case 3:
                    init = "「下」";
                    break;
                case 4:
                    init = "ん？今来た道を戻るのか…？\r\n…あまりビビらせないでくれ。";
                    break;
                case 5:
                    init = "罠があるんだな。了解、慎重に進む。目の前に来たら教えてくれ。";
                    break;
                case 6:
                    init = "罠を発見した！解除する。";
                    break;
                case 7:
                    init = "完了した！先に進むぞ。";
                    break;
                default:
                    break;
            }
            choice.Add(init);
        }
        //random
        for (int i = 0; i <= 2; i++)
        {

            switch (i)
            {
                case 0:
                    init = "だな。";
                    break;
                case 1:
                    init = "か。";
                    break;
                case 2:
                    init = "かい？";
                    break;

                default:
                    break;
            }
            random.Add(init);
        }
        //okText
        for (int i = 0; i <= 4; i++)
        {

            switch (i)
            {
                case 0:
                    init = "了解。";
                    break;
                case 1:
                    init = "OK！";
                    break;
                case 2:
                    init = "ありがとよ。";
                    break;
                case 3:
                    init = "助かる。";
                    break;
                case 4:
                    init = "信じてるぞ！";
                    break;
                default:
                    break;
            }
            okText.Add(init);
        }
        //wall
        wall = "そっちは壁じゃないか？頼むぜ…？";
        //damage
        for (int i = 0; i <= 5; i++)
        {

            switch (i)
            {
                case 0:
                    init = "！！！　罠か…！これも調査もれか…？\r\n「気にするな」とは言わないが、気に病まなくていい。\r\n無理を言ってるのは俺だ、そのまま頼む。";
                    break;
                case 1:
                    init = "ぐっ…！！　アンタのミスじゃなくて調査漏れならいいんだがな…！\r\n…悪い、疑いたくはないんだがそんな余裕もない状態だ…。\r\n頼む…。";
                    break;
                case 2:
                    init = "痛ぇ！！！クソッ！もうたくさんだ！\r\nあんたに頼んだ俺がバカだった！\r\n俺がのたれ死ぬのを画面越しに見てるがいいさ！";
                    break;
                case 3:
                    init = "っ！！\r\n…おい、まだ見てるのか？\r\nさっきは悪かった、アンタも悪気があったわけじゃないだろう。\r\nお互いに最後のチャンスだ。頼む、また指示をくれ。";
                    break;
                case 4:
                    init = "うぅ…。俺はもう駄目だ…。アンタには無理言って悪かったな…。\r\n俺が苦しんで死ぬ間際は見せたくない。端末はここに置いていく。\r\nじゃあな…。天国に行く前に一杯やっていくさ。";
                    break;
                case 5:
                    init = "ハハハ！おい！まだ見てるか？\r\nアンタのおかげで調査も生還も大失敗だ！\r\nハハハハハ！笑えよ！最高だろ！";
                    break;
                default:
                    break;
            }
            damage.Add(init);
        }
        //clearText = "出口だ…。助かった！助かったぞ！！\r\nアンタ最高だ！ありがとう！\r\n色々事が済んだら礼をさせてくれ！";
        clearText = "体験版クリアおめでとう！\r\nもう一度遊ぶならReloadボタン！";
    }

    void YesNo(bool y)
    {
        if (y)
        {
            tutoNum += 1;
        }
        else
        {
            tutoNum += 2;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (worldGen == null)
        {
            worldGen = GameObject.FindWithTag("WorldGen");
            WG = worldGen.GetComponent<WorldGen>();
            tutoNum = 1;
        }
        if (canvas == null)
        {
            canvas = GameObject.FindWithTag("GUI");
            choices = canvas.GetComponent<Choices>();
        }
        else
        {
            if (WG.setDone)
            {
                if (tutorial)
                {
                    if (tutoNum < tuto.Count)
                    {

                        if (talkText.text != tuto[tutoNum])
                        {
                            talkText.text = tuto[tutoNum];
                        }
                        if (tutoNum == 1)
                        {
                            choices.SendMessage("enableYesNo");
                        }
                        if (Input.GetMouseButtonDown(0))
                        {

                            if (tutoNum != 1 && tutoNum != 2)
                            {
                                tutoNum++;
                            }
                            else if (tutoNum == 2)
                            {
                                tutoNum += 2;
                            }
                            else if (tutoNum == tuto.Count-1)
                            {
                                tutoNum+=2;
                            }
                        }
                    }
                    else
                    {
                        talkText.text = wait;
                        tutorial = false;
                        choices.SendMessage("enableRoot");
                        playing = true;
                    }
                }
                else
                {
                    if (clear)
                    {
                        talkText.text = clearText;
                    }
                    else if (playing)
                    {
                        if (choices.directionRoot.activeSelf)
                        {
                            talkText.text = wait;
                            randOne = Random.Range(0, 3);
                            randTwo = Random.Range(0, 5);
                        }
                        else if (playNum == 4)
                        {
                            talkText.text = choice[playNum];

                        }
                        else if (playNum == 5)
                        {
                            talkText.text = choice[playNum];
                        }
                        else
                        {
                            talkText.text = choice[playNum] + random[randOne] + okText[randTwo];
                        }
                    }

                }
            }

        }
    }
}