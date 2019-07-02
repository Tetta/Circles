using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fat : MonoBehaviour
{
    Animator anim;
    // Start is called before the first frame update
    CircleCollider2D[] colliders = new CircleCollider2D[9];

    void Start()
    {
        anim = GetComponent<Animator>();
        for (int i = 0; i < 9; i ++) { 
            colliders[i] = transform.GetChild(0).GetChild(i).GetComponent<CircleCollider2D>();
        }


        StartCoroutine(setBig());
    }

    IEnumerator setBig () {

        yield return new WaitForSeconds(1);
        foreach (CircleCollider2D collider in colliders) {
            collider.enabled = false;
        }
        anim.Play("FatIdle");
        if (!GameController.levelPaused) AudioManager.instance.fatSound.Play();
        yield return new WaitForSeconds(1);
        foreach (CircleCollider2D collider in colliders) {
            collider.enabled = true;
        }
        anim.Rebind();
        anim.Play("Fat2");
        if (!GameController.levelPaused) AudioManager.instance.fatSound.Play();
        StartCoroutine(setBig());
    }

}
