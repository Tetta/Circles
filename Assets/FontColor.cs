using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FontColor : MonoBehaviour
{
    public Color32[] colors;

    void Start()
    {
        GetComponent<Text>().color = colors[LevelController.skin];
    }


}
