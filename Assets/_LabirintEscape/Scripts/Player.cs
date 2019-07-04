using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.UltimateIsometricToolkit.Scripts.Core;
using DG.Tweening;
using UnityEngine.UI;
public class Player : MonoBehaviour
{
    public static Player instance;
    public static State state;
    public static Direction directionName = Direction.None;
    

    public Transform images;
    public ParticleSystem psEnable;
    public ParticleSystem psDeath;
    public Transform psTrail;
    public Transform cameraMain;
    public List<Color32> charColors;

    public int gemsCollected;
    public int dotsCollected;

    Vector2 pos;
    Vector2 direction;
    //int tilesCount;
    // Start is called before the first frame update
    bool mouseDrag;
    Vector3 mousePos;

    public static Direction dirTemp = Direction.None;
    public static float speed = 25f;
    //public static int lives;
    bool isDangerDeath = false;

    int imageSide = 0;
    void Start()
    {
        //point
        //Time.timeScale = 0.1f;

        instance = this;
        pos = LevelController. levelData.start[0];
        move((Vector3)pos + new Vector3(0, 0, 1));
        state = State.Stay;

        GetComponent<Animator>().Play("PlayerIdle");

        changeChar();


    }
    void OnMouseDown() {
        if (state == State.Stay) {
            //Debug.Log(Input.mousePosition);
            mousePos = Input.mousePosition;

            mouseDrag = true;

        }
    }
    void OnMouseUp() {
        if (state == State.Stay) {
            //Debug.Log(Input.mousePosition);
            //mousePos = Input.mousePosition;
            Vector3 v = mousePos - Input.mousePosition;
            if (v.x > 0 && v.y < 0) directionName = Direction.Up;
            else if (v.x < 0 && v.y > 0) directionName = Direction.Down;
            else if (v.x > 0 && v.y > 0) directionName = Direction.Left;
            else if (v.x < 0 && v.y < 0) directionName = Direction.Right;
            mouseDrag = false;

        }
    }
    // Update is called once per frame
    void FixedUpdate() {
        bool f = false;
        if (state == State.Stay) {
            if (Input.GetKeyDown(KeyCode.UpArrow) || directionName == Direction.Up) {
                f = true;
                direction = new Vector3(0, 1);
                imageSide = 0;
                //changeSprite(0);
            }
            if (Input.GetKeyDown(KeyCode.DownArrow) || directionName == Direction.Down) {
                f = true;
                direction = new Vector3(0,  -1);
                imageSide = 1;

                //changeSprite(1);

            }
            if (Input.GetKeyDown(KeyCode.LeftArrow) || directionName == Direction.Left) {
                f = true;
                direction = new Vector3(-1,  0);
                imageSide = 2;
                //changeSprite(2);

            }
            if (Input.GetKeyDown(KeyCode.RightArrow) || directionName == Direction.Right) {
                f = true;
                direction = new Vector3(1,  0);
                imageSide = 3;
                //changeSprite(3);

            }
        }
        
        
        if (f) {
            dirTemp = directionName;
            directionName = Direction.None;
            Vector2 tempPos = new Vector3(0, 0, 0);
            bool flag = true;
            int c = 0;
            AudioManager.instance.playerMoveSound.Play();
            while (flag) {
                tempPos = pos + direction;
                if (LevelController. levelData.tiles.Contains(tempPos)) {
                    pos = tempPos;

                    c++;
                    if (c == 100) flag = false;
                }
                else if (LevelController.levelData.dangers.Contains(tempPos)) {
                    flag = false;
                    //Debug.Log("------- Failed!!! -------------");
                    //StartCoroutine(death());
                    isDangerDeath = true;
                }
                else
                    flag = false;

            }

            f = false;
            state = State.Run;
            changeSprite(imageSide, (pos - (Vector2)GetComponent<IsoTransform>().Position).magnitude);
        }
        if (state == State.Run) {

            Vector2 nextPos = (Vector2)GetComponent<IsoTransform>().Position + Time.deltaTime * direction * speed;
            Vector2 diffV = (pos - nextPos) * direction;


            if (diffV.x < 0 || diffV.y < 0) {
                move((Vector3)pos + new Vector3(0, 0, 1));

                //if danger
                if (isDangerDeath && GetComponent<Collider2D>().enabled) {
                    Debug.Log("------- Failed!!! -------------");
                    StartCoroutine(death("Danger"));
                }

                //if teleport => change direction
                else if (Teleport.checkTeleport2(pos, dirTemp)) {
                    speed = 40;
                
                } 
                else 
                if (TeleportAnother.checkTeleport(pos, dirTemp).z != 100) {
                    pos = TeleportAnother.checkTeleport(pos, dirTemp);
                    move((Vector3)pos + new Vector3(0, 0, 1));

                }
                else {

                    onStopPlayer();
                }
                //point
                Taptic.Light();
                state = State.Stay;

            } else
                move((Vector3)nextPos + new Vector3(0, 0, 1));
        }

        //fix bad code
        if (TeleportAnother.enter) {
            if (TeleportAnother.enterTime < 0.2f) {
                TeleportAnother.enterTime += Time.deltaTime;
            }
            else
                TeleportAnother.enter = false;
        }
    }
    private void move(Vector3 v) {

        GetComponent<IsoTransform>().Position = v;
        cameraMain.position = transform.position;
    }

    private void changeSprite(int s, float magnitude) {
        Debug.Log("changeSprite magnitude: " + magnitude);
        foreach (Transform child in images) {
            child.gameObject.SetActive(false);
        }

        images.GetChild(s).gameObject.SetActive(true);
        //ps trail
        
        foreach (Transform child in psTrail) {
            //child.gameObject.SetActive(false);
            //child.GetComponent<ParticleSystem>().Stop();
        }
        //psTrail.GetChild(s).gameObject.SetActive(true);
        psTrail.GetChild(s).GetComponent<ParticleSystem>().Play();

        //GetComponent<Animator>().Play("Empty");
        GetComponent<Animator>().enabled = false;
        //resize images
        //scale 0.1 == moveY -0.086
        images.DOKill();
        float scale = 0.1f * (magnitude) / 2;
        if (scale > 0.2) scale = 0.2f;
        
        //fix uncomment
        images.DOScaleY(1 - scale, 0.05f);  

        float moveY = -0.086f * (magnitude) / 2;
        if (moveY < -0.172f) moveY = -0.172f;
        //Debug.Log(1- scale);
        //Debug.Log(moveY);

        //fix uncomment
        images.gameObject.transform.DOLocalMoveY(moveY, 0.05f);
    }

    void onStopPlayer () {
        //ps trail
        foreach (Transform child in psTrail) {
            //child.gameObject.SetActive(false);
            //child.GetComponent<ParticleSystem>().Stop();
        }
        images.DOKill();
        //resize images
        
        images.gameObject.transform.DOScaleY(1, 0.05f).OnComplete(() => {
            GetComponent<Animator>().Rebind();
            GetComponent<Animator>().enabled = true;
            
        });
        images.DOLocalMove(new Vector3(0, 0, 0), 0.05f);

        

    }


    private void OnTriggerEnter2D(Collider2D collision) {
        Debug.Log("collision.name: " + collision.gameObject.name);
        //star
        if (collision.gameObject.name == "TouchPanel") return;
        else if (collision.gameObject.name == "CoinPrefab(Clone)" || collision.gameObject.name == "DotPrefab(Clone)") {

            StartCoroutine(collision.gameObject.GetComponent<Coin>().effectCoroutine());

            if (collision.gameObject.name == "CoinPrefab(Clone)") {
                GemsController.AddGems(10, "Level");
                AudioManager.instance.gemSound.Play();
                gemsCollected++;
            }
            else {
                GemsController.AddGems(1, "Level");
                AudioManager.instance.dotSound.Play();
                dotsCollected++;

            }


        }
        else if (collision.gameObject.name == "BirdPrefab(Clone)") {
            Debug.Log("------- Failed!!! Bird -------------");

            //collision.gameObject.SetActive(false);
            StartCoroutine(death("Enemy"));
        }
        else if (collision.gameObject.name == "Shoot") {
            Debug.Log("------- Failed!!! Shoot -------------");

            StartCoroutine(death("Shooter"));
        }
        else if (collision.transform.gameObject.name == "ExitPrefab(Clone)") {
            Debug.Log("------- Exit!!! -------------");
            hideChar();


        }
        else if (collision.transform.parent.parent.gameObject.name == "FatPrefab(Clone)") {
            Debug.Log("------- Failed!!! Fat -------------");

            StartCoroutine(death("Fat"));
        }
        //Debug.Log("TeleportAnother OnTriggerEnter2D: " + collision.name);
        //Debug.Log("TeleportAnother OnTriggerEnter2D: " + GetComponent<IsoTransform>().Position);

        else if (collision.name == "TeleportAnother1(Clone)" || collision.name == "TeleportAnother2(Clone)") {
            //Debug.Log(collision.GetComponent<IsoTransform>().Position);
            //Debug.Log(GetComponent<IsoTransform>().Position);
            //Debug.Log(TeleportAnother.enter);
            //Debug.Log(TeleportAnother.checkTeleport(collision.GetComponent<IsoTransform>().Position, Player.dirTemp));
            if (TeleportAnother.checkTeleport(collision.GetComponent<IsoTransform>().Position, Player.dirTemp).z != 100 && !TeleportAnother.enter) {
                Debug.Log("Jump");
                //point
                Taptic.Light();
                onStopPlayer();
                state = State.Stay;
                TeleportAnother.enterTime = 0;
                TeleportAnother.enter = true;
                Player.instance.pos = TeleportAnother.checkTeleport(collision.GetComponent<IsoTransform>().Position, Player.dirTemp);
                Player.instance.move((Vector3)Player.instance.pos + new Vector3(0, 0, 1));
                //Time.timeScale = 0;

            }
            else TeleportAnother.enter = false;
        }
        //else Teleport.checkTeleport(collision.transform.gameObject.name);

    }

    public IEnumerator death(string reason) {
        onStopPlayer();
        AnalyticsController.sendEvent("PlayerDeath", new Dictionary<string, object> { { "Reason", reason } });

        //bug transform player
        GetComponent<Collider2D>().enabled = false;
        if (GameController.instance.lives > 0) {
            Debug.Log("minusLives");
            GameController.instance.minusLives();
            revive();
 
        }
        else {

            //ps trail
            //GameController.enableObg(psTrail, -1);

            Debug.Log("IEnumerator death start");
            images.gameObject.SetActive(false);
            state = State.Death;
            psDeath.Play();
            AudioManager.instance.playerDeathSound.Play();
            yield return new WaitForSeconds(0.1f);
            AudioManager.instance.playerDeath2Sound.Play();
            yield return new WaitForSeconds(0.1f);
            AudioManager.instance.playerDeath2Sound.Play();
            yield return new WaitForSeconds(0.1f);
            AudioManager.instance.playerDeath2Sound.Play();
            yield return new WaitForSeconds(0.1f);
            AudioManager.instance.playerDeath2Sound.Play();
            yield return new WaitForSeconds(0.1f);
            AudioManager.instance.playerDeath2Sound.Play();
            yield return new WaitForSeconds(0.1f);
            AudioManager.instance.playerDeath2Sound.Play();
            yield return new WaitForSeconds(0.1f);
            GameController.instance.showScreen("GameoverUI");



        }

    }

    public void revive() {
        StartCoroutine(reviveCoroutine());
        AnalyticsController.sendEvent("PlayerRevive");
    }
    public IEnumerator reviveCoroutine() {
        Debug.Log("IEnumerator revive start");
        GameController.levelPaused = false;
        GameController.instance.showScreen("GameUI");
        move(new Vector3((int)GetComponent<IsoTransform>().Position.x, (int)GetComponent<IsoTransform>().Position.y, (int)GetComponent<IsoTransform>().Position.z));
        
        images.gameObject.SetActive(true);
        state = State.Stay;
        foreach (Transform child in images) {
            child.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 150);
        }
        yield return new WaitForSeconds(2f);
        foreach (Transform child in images) {
            child.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
        }
        GetComponent<Collider2D>().enabled = true;

        isDangerDeath = false;
    }

    public void changeChar () {
        int i = 0;
        foreach (Transform child in images ) {
            child.GetComponent<SpriteRenderer>().sprite = GameController.instance.chars[GameController.charId].sprites[i];
            i++;
        }
        GameController.instance.playerMainMenu.sprite = GameController.instance.chars[GameController.charId].sprites[2];
    }

    public void showChar () {
        psEnable.Play();
        images.GetChild(0).GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 0);
        images.GetChild(0).GetComponent<SpriteRenderer>().DOColor(new Color32(255, 255, 255, 0), 0.2f).OnComplete(() => {
            images.GetChild(0).GetComponent<SpriteRenderer>().DOColor(new Color32(255, 255, 255, 255), 0.2f);
            AudioManager.instance.playerAwakeSound.Play();
        });

        
    }
    public void hideChar() {
        psEnable.Play();
        //images.GetChild(0).GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 0);
        images.GetChild(0).GetComponent<SpriteRenderer>().DOColor(new Color32(255, 255, 255, 255), 0.2f).OnComplete(() => {
            images.GetChild(0).GetComponent<SpriteRenderer>().DOColor(new Color32(255, 255, 255, 0), 0.2f).OnComplete(() => {
                GameController.instance.complete();
            });
        });

    }
    public enum State {

        Stay = 0,

        Run = 1,
        Death = 2


    }
    public enum Direction {

        Up = 0,

        Down = 1,
        Left = 2,
        Right = 3,
        None = 4
    }
}
