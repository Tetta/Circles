using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.UltimateIsometricToolkit.Scripts.Core;
using System;
public class Peak : MonoBehaviour
{
    public bool exist = false;

    void Start()
    {
        foreach (Transform peak in transform) {
            peak.GetChild(0).gameObject.SetActive(true);
            peak.GetChild(1).gameObject.SetActive(false);


        }
        //StartCoroutine(showHide());


    }

    IEnumerator showHide () {
        yield return new WaitForSeconds(0.3f);
        exist = true;
        //GetComponent<Collider2D>().enabled = true;
        foreach (Transform peak in transform) {
            peak.GetChild(0).gameObject.SetActive(false);
            peak.GetChild(1).gameObject.SetActive(false);
            peak.GetChild(Convert.ToInt32(exist)).gameObject.SetActive(true);

        }
        yield return new WaitForSeconds(0.5f);
        foreach (Transform peak in transform) {
            peak.GetChild(0).gameObject.SetActive(true);
            peak.GetChild(1).gameObject.SetActive(false);


        }
        exist = false;
        //GetComponent<Collider2D>().enabled = false;

        //StartCoroutine(showHide());

    }


    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision) {
       // Debug.Log("Peak OnTriggerEnter2D: " + collision.name);

        if (collision.name == "Player") {
            //Debug.Log("Peak Trigger Player");
            if (exist) {
                Debug.Log("Peak Enable Trigger Player");
                StartCoroutine(Player.instance.death()); 
            }
            else StartCoroutine(showHide());
        }
    }
    private void OnTriggerStay2D (Collider2D collision) {
        // Debug.Log("Peak OnTriggerEnter2D: " + collision.name);

        if (collision.name == "Player") {

            if (exist) {
                Debug.Log("Peak Enable Trigger Player");
                StartCoroutine(Player.instance.death());
            }

        }
    }

    public static void checkPeak (Vector2 pos, Player.Direction dirTemp) {


    }
}
