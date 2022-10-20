using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void OnEnable()
    {
        anim.SetTrigger("Spawn");
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            GameManager.Instance.GetCoin();
            anim.SetTrigger("Collected");
           // Destroy(gameObject, 1.5f);
        }
    }
}
