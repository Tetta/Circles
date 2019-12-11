using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class ButtonView : MonoBehaviour
{
    public Image bg;
    public Transform vipLine;


    // Start is called before the first frame update
    void OnEnable()
    {
        updateView();
        //sound
        gameObject.GetComponent<Button>() .onClick.AddListener( () => {
            AudioManager.instance.buttonSound.Play(); });
    }
    void updateView () {
        if (name == "VipButton") {
            if (bg != null) bg.color = GameController.instance. colorsBgVip[LevelController.skin];
            StartCoroutine(vipLineCoroutine());
        } else {
            if (bg != null) bg.color = GameController.instance.colorsBg[LevelController.skin];

        }

        if ((name == "VipButton" || name == "CharsButton") &&
            PlayerPrefs.GetInt("USER_GROUP_BUTTONS_VIEW", -1) == 0 &&
            LevelController.level <= 7)
        {
            Debug.Log(name);
            gameObject.SetActive(false);
        }
    }


    IEnumerator vipLineCoroutine() {
        vipLine.transform.localPosition = new Vector3(-300, 0, 0);
        vipLine.DOLocalMoveX(520, 0.5f);
        yield return new WaitForSecondsRealtime(UnityEngine.Random.Range(1f, 3f));
        StartCoroutine(vipLineCoroutine());
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
