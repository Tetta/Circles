using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gHint : MonoBehaviour {
	public static gHint instance;
	public GameObject hand;
	public static bool isRec = false;
	public static bool usingHint = false;

	public Dictionary<int, Dictionary<int, float>> hintSaves;

	int step;
	int currentId;
	float currentR;

	private void Start()
	{
		instance = this;
		//level - id - radius
		hintSaves = new Dictionary<int, Dictionary<int, float>>() {
			{ 1,  new Dictionary<int, float>(){{1, 1}, { 0, 2 }, {2, 3}, { 4, 4 }, { 3, 3 } } },
			{ 2,  new Dictionary<int, float>(){{1, 1} } },
			{ 3,  new Dictionary<int, float>(){{1, 1} } },
			{ 4,  new Dictionary<int, float>(){{1, 1} } },
			{ 5,  new Dictionary<int, float>(){{1, 1} } }
		};

	
	}

	void recHint ()
	{

	}

	//after rewarded ad
	public void useHint ()
	{
		Debug.Log("useHint");
		step = 0;
		usingHint = true;
		//foreach (var step in hintSaves[gGame.level]) {
		//	hand.transform.position = gGame.instance.dotShapes[step.Key].transform.position;


		stepHint();
		//}
	}

	public void stepHint()
	{
		Debug.Log("stepHint");
		int counter = 0;
		foreach (var stepH in hintSaves[gGame.level]) {
			if (step == counter) {

				hand.transform.position = gGame.instance.dotShapes[stepH.Key].transform.position + new Vector3(0.4f, - 0.4f, 0);
				currentId = stepH.Key;
				currentR = stepH.Value;
			}

			counter++;

		}
		step++;
	}

	public bool checkHint (int id)
	{
		Debug.Log("checkHint");
		if (!usingHint) return false;
		if (id == currentId) {
			gGame.instance.dotShapes[id].shape.transform.localScale = new Vector3(currentR, currentR, 1);
				          
			stepHint();
		}
		return true;
	}

}

public class HintSave  {
	int id;
	//Vector3 pos;
	float radius;
}
