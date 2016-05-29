using UnityEngine;
using UnityEngine.UI;

using System.Collections;

public class Icon : MonoBehaviour {

    public  Sprite[] icons;
    private Image icon;
    private int count = 0;
    private float timer = 0;

    // Use this for initialization
    void Start () {
        icon = GetComponent<Image>();
	}

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > 0.1f)
        {
            count++;

            if (count == icons.Length - 1)
            {
                count = 0;
            }
            icon.sprite = icons[count];

            timer = 0;
        }
    }
}
