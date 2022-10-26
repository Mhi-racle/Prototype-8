using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private Animator anim;
    private CoinMove coinMove;
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void OnEnable()
    {
        anim.SetTrigger("Spawn");
        coinMove = GetComponent<CoinMove>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            GameManager.Instance.GetCoin();
            anim.SetTrigger("Collected");
            // Destroy(gameObject, 1.5f);

            AudioManager.instance.Play("CoinPicked");
        }
        else if(other.tag == "CoinDetector")
        {

        }
    }
}
