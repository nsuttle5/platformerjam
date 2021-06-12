using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{

    public float waitToRespawn;
    public PlayerController thePlayer;
    public Rigidbody2D myRigidBody;

    public GameObject deathSplosion;

    public int coinCount;
    public int coinBonusLifeCount;
    public int bonusLifeThreshold;

    public AudioSource coindSound;

    public Text coinText;

    public Image heart1;
    public Image heart2;
    public Image heart3;

    public Sprite heartFull;
    public Sprite heartHalf;
    public Sprite heartEmpty;

    public int maxHealth;
    public int healthCount;

    private bool respawning;

    public ResetOnRespawn[] objectsToReset;

    public bool invincible;

    public Text livesText;
    public int startingLives;
    public int currentLives;

    public GameObject gameOverScreen;

    public AudioSource levelMusic;
    public AudioSource gameOverMusic;

    private ATM theATM;

    // Start is called before the first frame update
    void Start()
    {
        thePlayer = FindObjectOfType<PlayerController>();

        myRigidBody = FindObjectOfType<Rigidbody2D>();

        theATM = FindObjectOfType<ATM>();

        healthCount = maxHealth;

        myRigidBody.gravityScale = 3;
        thePlayer.moveSpeed = 5;

        objectsToReset = FindObjectsOfType<ResetOnRespawn>();

        if(PlayerPrefs.HasKey("CoinCount"))
        {
            coinCount = PlayerPrefs.GetInt("CoinCount");
        }

        coinText.text = "Coins: " + coinCount;

        if(PlayerPrefs.HasKey("PlayerLives"))
        {
            currentLives = PlayerPrefs.GetInt("PlayerLives");
        }

        else
        {
            currentLives = startingLives;
        }

        
        livesText.text = "Lives x " + currentLives;
    }

    // Update is called once per frame
    void Update()
    {
        if ((healthCount <= 0))
        {
            Respawn();
            
        }

        if(coinBonusLifeCount >= bonusLifeThreshold)
        {
            currentLives += 1;
            livesText.text = "Lives x " + currentLives;
            coinBonusLifeCount -= bonusLifeThreshold;
        }
    }


    public void Respawn()
    {
        coinCountDisplay();
        if (!respawning)
        {
            currentLives -= 1;
            livesText.text = "Lives x " + currentLives;

            if (currentLives > 0)
            {
                respawning = true;
                StartCoroutine("RespawnCo");
                
            }

            else
            {
                thePlayer.gameObject.SetActive(false);
                gameOverScreen.SetActive(true);
                levelMusic.Stop();
                //gameOverMusic.Play();
            }
        }
    }

    public IEnumerator RespawnCo()
    {

        thePlayer.gameObject.SetActive(false);

        Instantiate(deathSplosion, thePlayer.transform.position, thePlayer.transform.rotation);

        yield return new WaitForSeconds(waitToRespawn);

        healthCount = maxHealth;
        respawning = false;
        UpdateHeartMeter();

        coinCount = 0;
        theATM.coinStorage = 0;

        coinText.text = "Coins: " + coinCount;
        coinBonusLifeCount = 0;

        thePlayer.transform.position = thePlayer.respawnPosition;
        thePlayer.gameObject.SetActive(true);

        for(int i = 0; i < objectsToReset.Length; i++)
        {
            objectsToReset[i].gameObject.SetActive(true);
            objectsToReset[i].ResetObject();
            
        }

    }

    public void AddCoins(int coinsToAdd)
    {
        coinCount += coinsToAdd;
        coinBonusLifeCount += coinsToAdd;

        coinText.text = "Coins: " + coinCount;

        coindSound.Play();

        coinCountDisplay();
    }

    public void coinCountDisplay()
    {
        if (coinCount == 1)
        {
            thePlayer.moveSpeed = 4;
            myRigidBody.gravityScale = 4;
        }

        else if (coinCount == 2)
        {
            thePlayer.moveSpeed = 3;
            myRigidBody.gravityScale = 5;
        }

        else if (coinCount == 3)
        {
            thePlayer.moveSpeed = 2;
            myRigidBody.gravityScale = 6;
        }

        else if (coinCount >= 4)
        {
            thePlayer.moveSpeed = 1;
        }

        else
        {
            thePlayer.moveSpeed = 5;
            myRigidBody.gravityScale = 3;
        }
    }

    public void HurtPlayer(int damageToTake)
    {
        if (!invincible)
        {
            healthCount -= damageToTake;
            UpdateHeartMeter();

            thePlayer.Knockback();

            thePlayer.hurtSound.Play();
        }
    }

    public void GiveHealth(int healthToGive)
    {
        healthCount += healthToGive;

        if(healthCount > maxHealth)
        {
            healthCount = maxHealth;
        }
        coindSound.Play();

        UpdateHeartMeter();
    }

    public void UpdateHeartMeter()
    {
        switch (healthCount)
        {
            case 6: 
                    heart1.sprite = heartFull;
                    heart2.sprite = heartFull;
                    heart3.sprite = heartFull;
                    return;

            case 5:
                    heart1.sprite = heartFull;
                    heart2.sprite = heartFull;
                    heart3.sprite = heartHalf;
                    return;

            case 4:
                    heart1.sprite = heartFull;
                    heart2.sprite = heartFull;
                    heart3.sprite = heartEmpty;
                    return;

            case 3:
                    heart1.sprite = heartFull;
                    heart2.sprite = heartHalf;
                    heart3.sprite = heartEmpty;
                    return;

            case 2:
                    heart1.sprite = heartFull;
                    heart2.sprite = heartEmpty;
                    heart3.sprite = heartEmpty;
                    return;

            case 1:
                    heart1.sprite = heartHalf;
                    heart2.sprite = heartEmpty;
                    heart3.sprite = heartEmpty;
                    return;

            case 0:
                    heart1.sprite = heartEmpty;
                    heart2.sprite = heartEmpty;
                    heart3.sprite = heartEmpty;
                    return;

            default:
                    heart1.sprite = heartEmpty;
                    heart2.sprite = heartEmpty;
                    heart3.sprite = heartEmpty;
                    return;
        }

    }
    public void AddLives(int livesToAdd)
    {
        coindSound.Play();
        currentLives += livesToAdd;
        livesText.text = "Lives x " + currentLives;
    }
}
