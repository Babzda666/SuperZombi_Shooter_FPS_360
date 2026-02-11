using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    public WeaponType ammoType; // Вибираємо тип в інспекторі
    public int amount = 30;     // Скільки даємо

    private void OnTriggerEnter(Collider other)
    {
        // Перевіряємо, чи це гравець (за тегом або наявністю скрипта інвентаря)
        if (other.CompareTag("Player") || other.GetComponent<AmmoInventory>() != null)
        {
            AmmoInventory inventory = other.GetComponent<AmmoInventory>();
            
            // Якщо скрипт не на самому колайдері, шукаємо в батьках (корисно для складних ієрархій гравця)
            if(inventory == null) inventory = other.GetComponentInParent<AmmoInventory>();

            if (inventory != null)
            {
                inventory.AddAmmo(ammoType, amount);
                Destroy(gameObject); // Знищуємо патрони після підбору
            }
        }
    }
}