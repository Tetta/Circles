using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gCircle : MonoBehaviour {
    public GameObject filledGO;
    public GameObject emptyGO;
	public SpriteRenderer shadowImage;
    public Colors color;

    public int id = -1;
    public bool filled = false;
    
    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    private void OnTriggerEnter2D(Collider2D collision) {

		//return; //fix
		Debug.Log(collision.name);
		if (collision.name == "DotShape(Clone)") return;
        
		//fix - bad code
        gDotShape dot = collision.transform.parent.parent.gameObject.GetComponent<gDotShape>();
		//if (dot.layer != 200) return;

        //Debug.Log((dot.transform.localPosition - transform.localPosition).magnitude);

        id = dot.id;
        if (color == dot.color) {
            fillCircle(true, dot.id);
        }
        else {
            fillCircle(false, -1);
        }


    }

    public void fillCircle (bool fill, int id) {
        filled = fill;
        filledGO.SetActive(fill);
        emptyGO.SetActive(!fill);
        this.id = id;
    }

    //void unfill


}