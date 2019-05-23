using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public ParticleSystem psDestroy;

    // Start is called before the first frame update
    
    IEnumerator playAnim()
    {
        if (name == "CoinPrefab(Clone)") {
            yield return new WaitForSeconds(Random.Range(0, 5));
            transform.GetChild(0).GetComponent<Animator>().Play("MoveY");
        }
        else
            yield return null;
    }

    private void OnEnable() {
        StartCoroutine(playAnim());
    }



    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator effectCoroutine() {
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(false);

        psDestroy.Play();
        yield return new WaitForSecondsRealtime(1);
        gameObject.SetActive(false);
        //Destroy(gameObject);
        /*
        //yield return new WaitForSecondsRealtime(111);
        //yield return new WaitForSecondsRealtime(0.1f);
        //Vector2 v1 = psDestroy.transform.position;
        Vector2 v1 = (Vector2) psDestroy.transform.position ;
        Vector2 v2 = (Vector2)GameObject.Find("/Canvas/GameUI/Gems").transform.position +
            
            new Vector2(Player.go.GetComponent<Camera>().cameraToWorldMatrix.m03, Player.go.GetComponent<Camera>().cameraToWorldMatrix.m13);


        ///ParticleSystem particle2 = Instantiate(psDestroy, psDestroy.transform.position, Quaternion.identity) as ParticleSystem;
        //ParticleSystem particle3 = Instantiate(psDestroy, psDestroy.transform.position, Quaternion.identity) as ParticleSystem;
        //Vector2 scaleChest = ChestUI.instance.chestImage.transform.localScale;
        for (int i = 0; i < 30; i++) {
            v2 = (Vector2)GameObject.Find("/Canvas/GameUI/Gems").transform.position +

            new Vector2(Player.go.GetComponent<Camera>().cameraToWorldMatrix.m03, Player.go.GetComponent<Camera>().cameraToWorldMatrix.m13);

            psDestroy.transform.position = v1 + (v2 - v1) * i / 20;
            //particle2.transform.position = v1 + (v2 - v1) * i / 20;
            //particle3.transform.position = v1 + (v2 - v1) * i / 20;
            yield return new WaitForSecondsRealtime(0.01f);
        }
        //yield return new WaitForSecondsRealtime(5f);
        Destroy(gameObject);
        //LeanTween.scale(ChestUI.instance.chestImage.GetComponent<RectTransform>(), new Vector3(0.65f, 0.65f, 1), 0.2f).setEase(LeanTweenType.pingPong);
        //yield return new WaitForSecondsRealtime(0.2f);
        //LeanTween.scale(ChestUI.instance.chestImage.GetComponent<RectTransform>(), new Vector3(0.5f, 0.5f, 1), 0.2f).setEase(LeanTweenType.pingPong);
        /*
        for (int i = 0; i < 10; i++) {
            ChestUI.instance.chestImage.transform.localScale = ChestUI.instance.chestImage.transform.localScale * 1.01f;
            yield return new WaitForSeconds(0.01f);
        }
        for (int i = 0; i < 10; i++) {
            ChestUI.instance.chestImage.transform.localScale = ChestUI.instance.chestImage.transform.localScale * 0.99f;
            yield return new WaitForSeconds(0.01f);
        }
        ChestUI.instance.chestImage.transform.localScale = scaleChest;
        */
        //yield return new WaitForSeconds(0.03f);
        //Destroy(particle2.gameObject, 2f);
        //Destroy(particle3.gameObject, 2f);
    }
}
