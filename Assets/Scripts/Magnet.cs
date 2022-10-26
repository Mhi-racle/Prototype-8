using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;

public class Magnet : MonoBehaviour
{
    private GameObject coinDetector;
    private Animator magnetAnimator;

    private void Awake()
    {
        magnetAnimator = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        magnetAnimator.SetTrigger("Spawn");
        coinDetector = GameObject.FindGameObjectWithTag("Coin Detector");

    }

    private void Update()
    {
      
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            StartCoroutine(ActivateMagnet());
            // Destroy(gameObject);
            magnetAnimator.SetTrigger("Taken");
        }
      
    }

    IEnumerator  ActivateMagnet()
    {
        coinDetector.SetActive(true);
        yield return new WaitForSeconds(4f);
        coinDetector.SetActive(false);
    }
}
