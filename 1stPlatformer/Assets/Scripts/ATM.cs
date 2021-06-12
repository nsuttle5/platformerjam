using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ATM : MonoBehaviour
{
    public GameObject coinTest;
    public int coinStorage;
    private LevelManager theLevelManager;


    // Start is called before the first frame update
    void Start()
    {
        theLevelManager = FindObjectOfType<LevelManager>();
        coinTest.SetActive(false);
        coinStorage = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerStay2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            if(Input.GetButtonDown("ATM Deposit") && coinStorage < 4 && theLevelManager.coinCount > 0)
            {
                coinStorage += 1;
                theLevelManager.coinCount = theLevelManager.coinCount - 1;
                theLevelManager.coinText.text = "Coins: " + theLevelManager.coinCount;
                theLevelManager.coinCountDisplay();
            }
            if (Input.GetButtonDown("ATM Withdraw") && coinStorage > 0)
            {
                coinStorage -= 1;
                theLevelManager.AddCoins(1);
                //theLevelManager.coinCount = theLevelManager.coinCount + 1;
                //theLevelManager.coinText.text = "Coins: " + theLevelManager.coinCount;
                //theLevelManager.coinCountDisplay();
            }
        }
    }
}
