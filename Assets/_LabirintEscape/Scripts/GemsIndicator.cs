using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//[RequireComponent(typeof(Button))]
[RequireComponent(typeof(Animation))]

public class GemsIndicator : MonoBehaviour {
    [SerializeField] private string _shopPopUpName;
    [SerializeField] public Text _gemsCount;
    [SerializeField] private int _addTime = 5;

    [SerializeField] private Text _addText;
    [SerializeField] private string _addAnimationName;

    private Coroutine _gemsCoroutine;

    void Awake() {
        //gameObject.GetComponent<Button>().onClick.AddListener(() => {
         //   PopUpQueue.instance.Show(_shopPopUpName);
        //});
        GemsController.UpdateGems += UpdateGems;
    }

    private void OnEnable() {
        _gemsCount.text = ((int)GemsController.gems) .ToString();
        //updateTextPos();
    }
    private void OnDestroy() {
        GemsController.UpdateGems -= UpdateGems;
    }

    public void UpdateGems(float amount) {
        if (!gameObject.activeInHierarchy) return;
        if (!gameObject.activeSelf) return;
        if (Convert.ToInt32(_gemsCount.text) == Mathf.RoundToInt(amount)) return;
        ShowAnimation(Mathf.RoundToInt( GemsController.LastAdd));
        if (_gemsCoroutine != null) StopCoroutine(_gemsCoroutine);
        //Debug.Log("amount: " + Mathf.RoundToInt(amount));
        //Debug.Log("UpdateCoinsCoroutine(Mathf.RoundToInt(amount)): " + UpdateCoinsCoroutine(Mathf.RoundToInt(amount)));

        _gemsCoroutine = StartCoroutine(UpdateCoinsCoroutine(Mathf.RoundToInt(amount)));



        //updateTextPos();
    }

    void updateTextPos () {

        int i = 0;
        if (GemsController.gems >= 1000) i = 0;

        else if (GemsController.gems >= 100) i = 1;
        else if (GemsController.gems >= 10) i = 2;
        else i = 3;

            _gemsCount.transform.localPosition = new Vector3(31 * i, 0, 0);
    }

    private void ShowAnimation(int add) {
        _addText.text = (add > 0 ? "+" : "-") + Math.Abs(add);
        gameObject.GetComponent<Animation>().Play(_addAnimationName);
    }

    private IEnumerator UpdateCoinsCoroutine(int amount) {

        var currentAmount = Convert.ToInt32(_gemsCount.text);
        var addamount = Math.Abs(amount - currentAmount);
        var addatsecond = 0;


        if (addamount <= _addTime)
            addatsecond = 1 * (amount > currentAmount ? 1 : -1);
        else {
            addatsecond = (addamount / _addTime) * (amount > currentAmount ? 1 : -1);
        }

        for (int i = 0; i < _addTime; i++) {
            currentAmount += addatsecond;
            _gemsCount.text = currentAmount.ToString();
            yield return null;
        }
        _gemsCount.text = amount.ToString();
    }
}
