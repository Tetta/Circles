using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public static List <Tile> tilesUncolored = new List<Tile>();
    public static int colorOnLevel;
    public ParticleSystem ps;
    private void OnEnable()
    {
        tilesUncolored.Add(this);
        colorOnLevel = Random.Range(0, 5);
    }

    public void paint ()
    {
        if (LevelController.skin < 3)
        {
            int i = 0;
            transform.GetChild(i).GetChild(0).gameObject.SetActive(false);
            transform.GetChild(i).GetChild(1).gameObject.SetActive(false);
            transform.GetChild(i).GetChild(2).gameObject.SetActive(false);

            int r = Random.Range(0, 3);
            transform.GetChild(i).GetChild(r).gameObject.SetActive(true);
            transform.GetChild(i).GetChild(4).GetComponent<ParticleSystem>().Play();


        } else
        {
            transform.GetChild(0).GetChild(colorOnLevel).gameObject.SetActive(true);
            transform.GetChild(0).GetChild(7).GetComponent<ParticleSystem>().Play();
        }

        tilesUncolored.Remove(this);
        if (tilesUncolored.Count <= 0) Player.instance.hideChar();
    }

}
