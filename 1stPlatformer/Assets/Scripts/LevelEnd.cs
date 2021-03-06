using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEnd : MonoBehaviour
{

    public string levelToLoad;
    public string levelToUnlock;

    private PlayerController thePlayer;
    private CameraController theCamera;
    private LevelManager theLevelManager;
    private ATM theATM;

    public float waitToMove;
    public float waitToLoad;

    private bool movePlayer;

    public Sprite flagOpen;
    private SpriteRenderer theSpriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        thePlayer = FindObjectOfType<PlayerController>();
        theCamera = FindObjectOfType<CameraController>();
        theLevelManager = FindObjectOfType<LevelManager>();
        theATM = FindObjectOfType<ATM>();

        theSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(movePlayer)
        {
            thePlayer.myRigidBody.velocity = new Vector3(thePlayer.moveSpeed, thePlayer.myRigidBody.velocity.y, 0f);
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            if (theLevelManager.coinCount == 4)
            {
                SceneManager.LoadScene(levelToLoad);
                PlayerPrefs.SetInt(levelToUnlock, 1);

                //theSpriteRenderer.sprite = flagOpen;
                //StartCoroutine("LevelEndCo");
            }
        }
    }

    public IEnumerator LevelEndCo()
    {
        thePlayer.canMove = false;
        theCamera.followTarget = false;
        theLevelManager.invincible = true;

        theLevelManager.levelMusic.Stop();
        theLevelManager.gameOverMusic.Play();

        thePlayer.myRigidBody.velocity = Vector3.zero;

        PlayerPrefs.SetInt("CoinCount", theLevelManager.coinCount);
        PlayerPrefs.SetInt("PlayerLives", theLevelManager.currentLives);

        

        yield return new WaitForSeconds(waitToMove);

        movePlayer = true;

        yield return new WaitForSeconds(waitToLoad);

        SceneManager.LoadScene(levelToLoad);
    }

}
