using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public static bool audioFlag = true;
    //public List<GameObject> audioIcons;
    
    public AudioSource music;
    public AudioSource buttonSound;
    public AudioSource playerAwakeSound;
    public AudioSource playerMoveSound;
    public AudioSource playerDeathSound;
    public AudioSource playerDeath2Sound;
    public AudioSource gemSound;
    public AudioSource dotSound;
    public AudioSource shooterSound;
    public AudioSource peakSound;
    public AudioSource enemySound;
    public AudioSource fatSound;
    public AudioSource levelCompleteSound;
    public AudioSource wheelRewardSound;


    void Awake()
    {
        if (instance == null) {
            instance = this;
            audioFlag = Convert.ToBoolean(PlayerPrefs.GetInt("AUDIO", 1));
            AudioListener.volume = PlayerPrefs.GetInt("AUDIO", 1);
            music.Play();
        }
        else if (instance == this) {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
        
        if (GameObject.Find("CanvasUI/MainUI/AudioButton/") != null) {
            GameObject.Find("CanvasUI/MainUI/AudioButton/").transform.GetChild(0).gameObject.SetActive(audioFlag);
            GameObject.Find("CanvasUI/MainUI/AudioButton/").transform.GetChild(1).gameObject.SetActive(!audioFlag);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //public void play (AudioSource sound) {
        //sound.Play();
    //}


    public void audioOnOff () {
        audioFlag = !audioFlag;
        PlayerPrefs.SetInt("AUDIO", Convert.ToInt32(audioFlag));
        GameObject.Find("CanvasUI/MainUI/AudioButton/").transform.GetChild(0).gameObject.SetActive(audioFlag);
        GameObject.Find("CanvasUI/MainUI/AudioButton/").transform.GetChild(1).gameObject.SetActive(!audioFlag);
        AudioListener.volume = PlayerPrefs.GetInt("AUDIO", 1);

    }

}
