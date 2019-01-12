using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gShape : MonoBehaviour {
    public SpriteRenderer shapeImage;
    public int id;

    int layer;
    

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void changeLayer(int amount) {
        if (amount == 200) layer = amount;
        else
        layer += amount;
        shapeImage.sortingOrder = layer;
    }
}
