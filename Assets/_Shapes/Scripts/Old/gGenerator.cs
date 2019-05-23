using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gGenerator : MonoBehaviour {
    public GameObject dotShapePrefab;
    public GameObject circlePrefab;


    //int layer;
    int dotshapesMin = 6;
    int dotshapesMax = 7;
    int circlesMin = 5;
    int circlesMax = 10;
    int colorsMin = 2;
    int colorsMax = 7;
    float scaleMin = 3;
    float scaleMax = 15; //20

    float withtMin = -2.5f;
    float withtMax = 2.5f;
    float heightMin = -4f;
    float heightMax = 4f;

    //List<gDotShape> dotshapes = new List<gDotShape>();

	private void Start()
	{
		gGame.instance.dotShapes.Clear();
		gGame.instance.circles.Clear();
        generateLevel();

        //generate();
	}

	// Use this for initialization
	void clear () {
		Debug.Log("clear start");
		for (int i = 0; i < gGame.instance.dotShapes.Count; i++) {
			Destroy(gGame.instance.dotShapes[i].gameObject);
		}
		for (int i = 0; i < gGame.instance.circles.Count; i++) {
			Destroy(gGame.instance.circles[i].gameObject);
		}
		gGame.instance.dotShapes.Clear();
		gGame.instance.circles.Clear();

		Debug.Log("clear end");

	}

	public void generate () {
	 	clear();
        int dotshapesCount = Random.Range(dotshapesMin, dotshapesMax);
		int circlesCount = Random.Range(circlesMin, circlesMax);
        int colorsCount = Random.Range(colorsMin, colorsMax);

        for (int i = 0; i < dotshapesCount; i ++) {
            GameObject go = Instantiate(dotShapePrefab);
            gDotShape dotshape = go.GetComponent<gDotShape>();
			dotshape.id = i; //colorsCount
			//dotshape.color = (Colors) Random.Range(1, colorsMax); // colorsCount
			dotshape.color = (Colors) i + 1; //colorsCount
			//float scale = Random.Range(scaleMin, scaleMax); //
			float scale = Random.Range(scaleMin, scaleMin + i * 3);

			dotshape.shape.transform.localScale = dotshape.shape.transform.localScale + new Vector3(scale, scale, 1);
			dotshape.transform.localPosition = getRandomPosition();


			paint(dotshape.dot.transform.GetChild(0).gameObject, dotshape.color, gGame.instance.colorsCircle);
			paint(dotshape.shapeImage.gameObject, dotshape.color, gGame.instance.colorsShape);
			paint(dotshape.shadowImage.gameObject, dotshape.color, gGame.instance.colorsShadow);

			dotshape.changeLayer(200 - i, false);
            //dotshapes.Add(dotshape);
			gGame.instance.dotShapes.Add(dotshape);


			//dotshape.c
		}
		for (int i = 0; i < circlesCount; i++) {
            GameObject go = Instantiate(circlePrefab);
            gCircle circle = go.GetComponent<gCircle>();

			circle.transform.localPosition = getRandomPosition();


			for (int j = 0; j < dotshapesCount; j++) {
                float magnitude = (circle.transform.localPosition - gGame.instance.dotShapes[j].transform.position).magnitude;
				if (magnitude * 4.4f - 2.5f < gGame.instance.dotShapes[j].shape.transform.localScale.x) {


					if (circle.id == -1) {
						//Debug.Log("_____");
						//Debug.Log(dotshapes[j].id);
					//Debug.Log(dotshapes[j].color); 
						circle.id = gGame.instance.dotShapes[j].id;
						circle.color = gGame.instance.dotShapes[j].color;
						paint(circle.transform.GetChild(0).gameObject, circle.color, gGame.instance.colorsCircle);
						paint(circle.filledGO.transform.gameObject, circle.color, gGame.instance.colorsShape);
						paint(circle.shadowImage.gameObject, circle.color, gGame.instance.colorsShadow);
						foreach (Transform child in circle.emptyGO.transform.GetChild(0)) {
							//paint(child.gameObject, circle.color, gGame.instance.colorsShadow);
							//paint(child.gameObject, circle.color, gGame.instance.colorsCircle);
							Color c = gGame.instance.colorsCircle[(int)circle.color - 1];
							child.GetComponent<SpriteRenderer>().color = new Color(c.r, c.g, c.b, child.GetComponent<SpriteRenderer>().color.a);

						}

						gGame.instance.circles.Add(circle);
					}
                }

            }
            if (circle.id == -1) {
                Destroy(circle.gameObject);
                i--;
            }
        }
		foreach (gDotShape ds in gGame.instance.dotShapes) {
			ds.OnMouseDown();
			ds.OnMouseUp();

		}


	}



	void paint (GameObject go, Colors colorId, List<Color32> colors) {
        go.GetComponent<SpriteRenderer>().color = colors[(int)colorId - 1];
    }

	Vector3 getRandomPosition()
	{
		Vector3 v = Vector3.zero;
		//bool flag = true;
		//int counter = 0;
		//while (flag) {
		for (int counter = 0; counter < 100; counter ++) { 
			bool near = false;
			v = new Vector3(Random.Range(withtMin, withtMax), Random.Range(heightMin, heightMax), 0);
			for (int i = 0; i < gGame.instance.dotShapes.Count; i++) {
				float magnitude = (v - gGame.instance.dotShapes[i].transform.position).magnitude;
					if (magnitude < 1f) near = true;
			}
			for (int i = 0; i < gGame.instance.circles.Count; i++) {
				float magnitude = (v - gGame.instance.circles[i].transform.position).magnitude;
				if (magnitude < 1f) near = true;
			}
			//counter++;
			if (!near) counter = 100;
			//Debug.Log(v);
			//Debug.Log(counter);
			//if (counter > 100) flag = false;

		}


		return v;

	}

    public void generateLevel() {
        int level = 1;
        clear();
        //int dotshapesCount = Random.Range(dotshapesMin, dotshapesMax);
        int circlesCount = Random.Range(circlesMin, circlesMax);
        int colorsCount = Random.Range(colorsMin, colorsMax);
        List<LevelPreset> presets = gLevel.levels[level - 1].preset;
        int dotshapesCount = 0;
        foreach (LevelPreset preset in presets) {
            //if dotshape
            if (preset.dot == 0) {
                GameObject go = Instantiate(dotShapePrefab);
                gDotShape dotshape = go.GetComponent<gDotShape>();
                dotshape.id = preset.color; // dotshapesCount; // colorsCount
                                              //dotshape.color = (Colors) Random.Range(1, colorsMax); // colorsCount
                dotshape.color = (Colors)preset.color; // colorsCount
                                                       //float scale = Random.Range(scaleMin, scaleMax); //
                                                       //float scale = Random.Range(scaleMin, scaleMin + dotshapesCount * 3);

                //dotshape.shape.transform.localScale = dotshape.shape.transform.localScale + new Vector3(scale, scale, 1);
                dotshape.transform.localPosition = preset.position;


                paint(dotshape.dot.transform.GetChild(0).gameObject, dotshape.color, gGame.instance.colorsCircle);
                paint(dotshape.shapeImage.gameObject, dotshape.color, gGame.instance.colorsShape);
                paint(dotshape.shadowImage.gameObject, dotshape.color, gGame.instance.colorsShadow);

                dotshape.changeLayer(200 - dotshapesCount, false);
                //dotshapes.Add(dotshape);
                gGame.instance.dotShapes.Add(dotshape);
                dotshapesCount++;
            } else {
                GameObject go = Instantiate(circlePrefab);
                gCircle circle = go.GetComponent<gCircle>();

                circle.transform.localPosition = preset.position;


                for (int j = 0; j < dotshapesCount; j++) {
                    //float magnitude = (circle.transform.localPosition - gGame.instance.dotShapes[j].transform.position).magnitude;
                    //if (magnitude * 4.4f - 2.5f < gGame.instance.dotShapes[j].shape.transform.localScale.x) {


                        //if (circle.id == -1) {
                            //Debug.Log("_____");
                            //Debug.Log(dotshapes[j].id);
                            //Debug.Log(dotshapes[j].color); 
                            circle.id = preset.color;
                            circle.color = (Colors)preset.color;
                            paint(circle.transform.GetChild(0).gameObject, circle.color, gGame.instance.colorsCircle);
                            paint(circle.filledGO.transform.gameObject, circle.color, gGame.instance.colorsShape);
                            paint(circle.shadowImage.gameObject, circle.color, gGame.instance.colorsShadow);
                            foreach (Transform child in circle.emptyGO.transform.GetChild(0)) {
                                //paint(child.gameObject, circle.color, gGame.instance.colorsShadow);
                                //paint(child.gameObject, circle.color, gGame.instance.colorsCircle);
                                Color c = gGame.instance.colorsCircle[(int)circle.color - 1];
                                child.GetComponent<SpriteRenderer>().color = new Color(c.r, c.g, c.b, child.GetComponent<SpriteRenderer>().color.a);

                            }

                            gGame.instance.circles.Add(circle);
                        //}
                    //}

                }


            }
        }

        /*

        foreach (gDotShape ds in gGame.instance.dotShapes) {
            ds.OnMouseDown();
            ds.OnMouseUp();

        }
        */

    }


}
