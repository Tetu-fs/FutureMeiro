using UnityEngine;
using System.Collections;


[System.Serializable]
public class MapSize
{
    [SerializeField]
    private int XLIMIT;
    [SerializeField]
    private int YLIMIT;

    public int x
    {
        set{ this.XLIMIT = value; }
        get{ return this.XLIMIT; }
    }
    public int y
    {
        set { this.YLIMIT = value; }
        get { return this.YLIMIT; }
    }
}
