using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.UltimateIsometricToolkit.Scripts.Core;
using System;
public class Peak : MonoBehaviour
{
    public bool exist = false;
    //CircleCollider2D collider;
    void Start()
    {
        foreach (Transform peak in transform) {
            peak.GetChild(0).gameObject.SetActive(true);
            peak.GetChild(1).gameObject.SetActive(false);


        }
        //StartCoroutine(showHide());
        //collider = GetComponent<CircleCollider2D>();
        
    }

    IEnumerator showHide () {
        //collider.enabled = false;
        yield return new WaitForSeconds(0.4f);
        exist = true;
        //Debug.Log()
        if (Player.instance.transform.GetComponent<IsoTransform>().Position == GetComponent<IsoTransform>().Position + new Vector3(0, 0, 1)) StartCoroutine(Player.instance.death("Peak", GetComponent<IsoTransform>().Position));
        //collider.enabled = true;
        //GetComponent<Collider2D>().enabled = true;
        foreach (Transform peak in transform) {
            peak.GetChild(0).gameObject.SetActive(false);
            peak.GetChild(1).gameObject.SetActive(false);
            peak.GetChild(Convert.ToInt32(exist)).gameObject.SetActive(true);
            AudioManager.instance.peakSound.Play();
        }
        yield return new WaitForSeconds(0.5f);
        foreach (Transform peak in transform) {
            peak.GetChild(0).gameObject.SetActive(true);
            peak.GetChild(1).gameObject.SetActive(false);
            AudioManager.instance.peakSound.Play();


        }
        exist = false;
        
        //GetComponent<Collider2D>().enabled = false;

        //StartCoroutine(showHide());

    }



    private void OnTriggerEnter2D(Collider2D collision) {
       //Debug.Log("Peak OnTriggerEnter2D: " + collision.name);

        if (collision.name == "Player") {
            //Debug.Log("Peak Trigger Enter Player " + exist);
            if (exist) {
                Debug.Log("Peak Enable Trigger Player");
                StartCoroutine(Player.instance.death("Peak", GetComponent<IsoTransform>().Position)); 
            }
            else StartCoroutine(showHide());
        }
    }
    private void OnTriggerStay2D (Collider2D collision) {
        //Debug.Log("Peak OnTriggerEnter2D: " + collision.name);

        if (collision.name == "Player") {
            //Debug.Log("Peak Trigger Stay Player " + exist);

            if (exist) {
                Debug.Log("Peak Enable Trigger Player");
                StartCoroutine(Player.instance.death("Peak", GetComponent<IsoTransform>().Position));
            }

        }
    }

    public static void checkPeak (Vector2 pos, Player.Direction dirTemp) {


    }
}
