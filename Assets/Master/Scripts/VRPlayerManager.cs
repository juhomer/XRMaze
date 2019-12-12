using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VRPlayerManager : MonoBehaviour {

    public int maxHealth = 3;
    public int currentHealth;
    public Slider healthSlider;
    public Canvas loseCanvas;
    

    // Start is called before the first frame update
    void Start() {
        loseCanvas.enabled = false;
        currentHealth = maxHealth;
        healthSlider = GameObject.Find("HealthSlider").GetComponent<Slider>();
        healthSlider.maxValue = maxHealth;
        healthSlider.value = maxHealth;
    }

    // Update is called once per frame
    void Update() {

        if (Input.GetKeyDown(KeyCode.M)) {
            TakeDamage(1);
        }

    }

    private void OnCollisionEnter(Collision col) {
        if (col.gameObject.tag == "Ball") {
            TakeDamage(1);
        }
    }
     public void TakeDamage(int damage) {
        currentHealth -= damage;
        healthSlider.value = currentHealth;
        Debug.Log("PlayerHealth is now " + currentHealth + " hp");
        if (currentHealth <= 0) {
            loseCanvas.enabled = true;
        }
    }

     private void OnTriggerEnter (Collider other)
     {
         Debug.Log ("something hit");
         if (other.gameObject.tag == "Ball")
         {
             Debug.Log ("hit by ball");
             TakeDamage (1);
         }
     }
}
