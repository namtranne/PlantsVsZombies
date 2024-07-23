using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Background : MonoBehaviour
{
    public Sprite nightBackground;
    public Sprite dayBackground;

    private void Start()
    {
        if (GameManager.level > 25)
        {
            GetComponent<SpriteRenderer>().sprite = dayBackground;
        }
        else if (GameManager.level > 10)
        {
            GetComponent<SpriteRenderer>().sprite = nightBackground;
        } 
    }
}
