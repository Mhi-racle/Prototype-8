using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Magnet : MonoBehaviour
{
    public float magnetTime;
    public Transform player;
    public List<Coin> coins;

    private void Awake()
    {
        if(player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }



    }

    private void OnTriggerEnter(Collider other)
    {
       
    }
}
