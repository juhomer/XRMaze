using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateSlingDetector : MonoBehaviour
{
    BoxCollider boxCollider;
    [SerializeField] GameObject ball;

    public bool isSlingActivated = false;

    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ball == null)
        {
            ball = GameObject.FindGameObjectWithTag("Ball");
        }
    }

    private void OnTriggerEnter(Collider other)
    {        
        if (other.gameObject.CompareTag("ARPlayer"))
        {
            isSlingActivated = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("ARPlayer"))
        {
            isSlingActivated = false;
        }
    }

}
