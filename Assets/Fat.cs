using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fat : MonoBehaviour
{
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        StartCoroutine(setBig());
    }

    IEnumerator setBig () {

        yield return new WaitForSeconds(1);
        anim.Play("FatIdle");
        yield return new WaitForSeconds(1);
        anim.Rebind();
        anim.Play("Fat2");

        StartCoroutine(setBig());
    }

}
