using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Colors {
    Blue = 1,
    Red = 2,
    Green = 3,
    Yellow = 4

}
public class gGame : MonoBehaviour {
    public static gGame instance;
    //fix need autofill
    public List<gCircle> circles;
    public List<gDotShape> dotShapes;
    public List<Color32> colorsShape;
    public List<Color32> colorsCircle;
	public List<Color32> colorsShadow;
	public static int level = 1;

	// Use this for initialization
	void Awake () {
        instance = this;

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
