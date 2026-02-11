using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
   public GameObject  zombie;

   public int waveNumber = 7;
   
   public int waveSize = 3;
   public float waveDelay = 6f;
   public float SpawnDelay = 1f;
   public GameObject boss;

   void Start()
   {
      StartCoroutine(SpawnZombie());
   }

   IEnumerator SpawnZombie()
   {
      
      for (int i = 0; i < waveNumber; i++)
      {
         for (int j = 0; j < waveSize; j++)
         {
            yield return new WaitForSeconds(SpawnDelay);
            SpawnZombi();
         }
         yield return new WaitForSeconds(waveDelay);
      }
      Spawnboss();
   }

   void Spawnboss()
   {
      Instantiate(boss, transform.position, transform.rotation);
   }
   void SpawnZombi()
   {
      
      Instantiate(zombie, transform.position, transform.rotation);
      
   }
}
