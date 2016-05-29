using UnityEngine;
using System.Collections;

public class Choices : MonoBehaviour
{
    public GameObject hero;
    public GameObject Text;
    public GameObject yesNo;
    public GameObject directionRoot;

    void Start()
    {
        yesNo.SetActive(false);
        directionRoot.SetActive(false);
    }
    void enableYesNo()
    {
        yesNo.SetActive(true);

    }
    void enableRoot()
    {
        directionRoot.SetActive(true);
    }
    public void YesNo(bool choice)
    {
        Text.SendMessage("YesNo", choice);
        yesNo.SetActive(false);

    }
    public void Direction(int d)
    {
        switch (d)
        {
            case 0:
            case 1:
            case 2:
            case 3:
                Text.SendMessage("SetNum", d);
                hero.SendMessage("DireNavi", d);
                hero.SendMessage("CanMove");
                break;
            default:
                break;
        }
        directionRoot.SetActive(false);
    }
    public void trap(int t)
    {

    }
    void Update()
    {
        if(hero == null)
        {
            hero = GameObject.FindWithTag("Hero");
        }
    }
}
