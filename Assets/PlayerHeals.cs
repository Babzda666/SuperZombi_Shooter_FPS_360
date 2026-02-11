using System;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHeals : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Image healthBarFill;
    [SerializeField] private TMP_Text[] healthBarTexts;

    
   public float health=100;

   public GameObject [] PlayerObjects;
   
   public void TakeDamage(float damage)
   {
       health -= damage;
       if (health <= 0)
       {
           foreach (GameObject obj in PlayerObjects)
           {
               obj.SetActive(false);
           }
           GetComponent<ThirdPersonController>().enabled = false;
           
       }
       
       UpdateUI();
   }

   private void Start()
   {
       UpdateUI();
   }

   private void UpdateUI()
   {
       // Це працює у всіх, бо stats.currentHealth синхронізується через OnPhotonSerializeView
       foreach (var healthBarText in healthBarTexts)
       {
           if (healthBarText) healthBarText.text = health.ToString("F0");
       }
           float max = Mathf.Max(200, 1f);
           float percent = health / max;

           RectTransform rt = healthBarFill.rectTransform;
           rt.sizeDelta = new Vector2(180f * percent, rt.sizeDelta.y);
       
   }


}
