using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class TimerManager : MonoBehaviour {
    public static Dictionary<string, Timer> timers = new Dictionary<string, Timer>();

	
	// Update is called once per frame
	void Update () {

        foreach (KeyValuePair<string, Timer> t in timers) {

            t.Value.update();

        }
    }

}


public class Timer {

    public string name;
    public int duration;
    public Action actionEnd;
    public DateTime start;
    public TimeSpan timer;
    public bool enable;
    //Text text;

    public Timer(string nameIn, int durationIn, Action actionEndIn) {
        name = nameIn;
        duration = durationIn;
        actionEnd = actionEndIn;
        //enable = true;

        init();
    }

    public void init(bool set = false) {
        Debug.Log("Timer init: " + name + " " + set);
        string startStr = PlayerPrefs.GetString(name + "Timer", "");
        enable = true;

        if (set) {
            start = DateTime.UtcNow;
            PlayerPrefs.SetString(name + "Timer", start.ToString());
        }
        else if(startStr == "") {
            enable = false;
        }
        else
            start = Convert.ToDateTime(startStr);

        //update();
    }

    public void update() {

        if (enable) {
            timer = start.AddSeconds(duration) - DateTime.UtcNow;

            if (timer.TotalSeconds < 1) {
                Debug.Log(name + " timer end");
                enable = false;
                if (actionEnd != null) actionEnd();
            }
        }

    }

}
