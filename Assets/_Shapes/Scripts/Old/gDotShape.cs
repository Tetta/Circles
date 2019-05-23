using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gDotShape : MonoBehaviour {
    public GameObject dot;
    public GameObject shape;
	public GameObject shapeShadow;
	public SpriteRenderer shapeImage;
	public SpriteRenderer shadowImage;

	public Colors color;
    bool tapped = false;
    public int id;
    

    int layer;
    float speedFill = 8;

    //float radius; //for generator

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (tapped) {
			float t = Time.deltaTime;

			shape.transform.localScale = shape.transform.localScale + new Vector3(t * speedFill, t * speedFill, 1);
			shapeShadow.transform.localScale = shapeShadow.transform.localScale + new Vector3(t * speedFill, t * speedFill, 1);

		}
	}

    public void OnMouseDown() {
		Debug.Log("xOnMouseDown");
		if (gHint.instance.checkHint(id)) return;

		foreach (gCircle circle in gGame.instance.circles) {
            if (circle.id == id) {
                circle.fillCircle(false, -1);

            }
        }
        foreach (gDotShape sh in gGame.instance.dotShapes) {
            sh.changeLayer(-1, true);
            if (sh.id == id) sh.changeLayer(200, false);
        }
        

        tapped = true;
        shape.transform.localScale = new Vector2(1.15f, 1.15f);
		shapeShadow.transform.localScale = new Vector2(1.15f, 1.15f);
	}
	public void OnMouseUp() {
        tapped = false;

        //shapeBig.transform.localScale = new Vector2();

    }
	

    public void changeLayer(int amount, bool inc) {
        if (inc) layer += amount;
        else layer = amount;
        shapeImage.sortingOrder = layer;
    }
}
