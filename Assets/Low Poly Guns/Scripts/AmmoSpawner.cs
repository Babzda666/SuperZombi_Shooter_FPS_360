using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoSpawner : MonoBehaviour
{
    [Header("Prefabs Setup")]
    // Важливо: 0=Pistol, 1=Shotgun, 2=AK47 (дотримуючись твого порядку)
    public GameObject[] AmmoPrefabs; 

    [Header("Spawn Chances (Sliders)")]
    [Range(0, 100)] public float pistolChanse;
    [Range(0, 100)] public float shotgunChanse;
    [Range(0, 100)] public float AK47chanse;

    [Header("Settings")]
    public float delay = 15;
    public int numbermin = 3;
    public int numbermax = 7;

    [Header("Debug Info")]
    public GameObject SpawnedAmmo; // Тут зберігається посилання на поточний об'єкт

    private void Start()
    {
        // Запускаємо корутину при старті
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        // Нескінченний цикл
        while (true)
        {
            // 1. Якщо патрони вже існують - чекаємо, поки вони стануть null (хтось підбере)
            if (SpawnedAmmo != null)
            {
                yield return new WaitUntil(() => SpawnedAmmo == null);
            }

            // 2. Як тільки патрони зникли (null) - запускаємо таймер затримки
            yield return new WaitForSeconds(delay);

            // 3. Спавнимо нові патрони
            SpawnAmmo();
        }
    }

    private void SpawnAmmo()
    {
        // Рахуємо загальну "вагу" всіх шансів
        float totalWeight = pistolChanse + shotgunChanse + AK47chanse;

        // Захист від ділення на нуль, якщо всі шанси 0
        if (totalWeight <= 0) return;

        float randomValue = Random.Range(0, totalWeight);
        
        GameObject prefabToSpawn = null;
        int amountMultiplier = 1;

        // Визначаємо, який префаб спавнити на основі "рулетки"
        if (randomValue < pistolChanse)
        {
            // Pistol (Index 0)
            prefabToSpawn = AmmoPrefabs[0];
            amountMultiplier = 3; // Для пістолета множимо на 3
        }
        else if (randomValue < pistolChanse + shotgunChanse)
        {
            // Shotgun (Index 1)
            prefabToSpawn = AmmoPrefabs[1];
            amountMultiplier = 2; // Для дробовика множимо на 2
        }
        else
        {
            // AK47 (Index 2)
            prefabToSpawn = AmmoPrefabs[2];
            amountMultiplier = 1; // Для АК беремо базу
        }

        // Спавнимо на позиції та з ротацією спавнера
        SpawnedAmmo = Instantiate(prefabToSpawn, transform.position, transform.rotation);

        // Налаштовуємо кількість патронів у скрипті AmmoPickup
        AmmoPickup pickupScript = SpawnedAmmo.GetComponent<AmmoPickup>();
        if (pickupScript != null)
        {
            // Random.Range для int: макс значення ексклюзивне, тому +1
            int baseAmount = Random.Range(numbermin, numbermax + 1);
            
            // Встановлюємо фінальну кількість
            pickupScript.amount = baseAmount * amountMultiplier;
        }
    }
}
