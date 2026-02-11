using TMPro;
using UnityEngine;

public class AmmoInventory : MonoBehaviour
{
    // Початкова кількість патронів (можна міняти в інспекторі)
    public int pistolAmmo = 20;
    public int akAmmo = 60;
    public int shotgunAmmo = 10;

    // Метод додавання патронів
    
    public TMP_Text[] ammoText;
    
    public void AddAmmo(WeaponType type, int amount)
    {
        switch (type)
        {
            case WeaponType.Pistol:
                pistolAmmo += amount;
                Debug.Log($"Added {amount} Pistol ammo. Total: {pistolAmmo}");
                break;
            case WeaponType.AK47:
                akAmmo += amount;
                Debug.Log($"Added {amount} AK47 ammo. Total: {akAmmo}");
                break;
            case WeaponType.Shotgun:
                shotgunAmmo += amount;
                Debug.Log($"Added {amount} Shotgun ammo. Total: {shotgunAmmo}");
                break;
        }
        UpdateAmmoText();
    }

    public void UpdateAmmoText()
    {
        ammoText[0].text=pistolAmmo.ToString();
        ammoText[1].text=akAmmo.ToString();
        ammoText[2].text=shotgunAmmo.ToString();
    }
    // Метод спроби використати патрон
    public bool TryConsumeAmmo(WeaponType type)
    {
        switch (type)
        {
            case WeaponType.Pistol:
                if (pistolAmmo > 0) { pistolAmmo--; UpdateAmmoText(); return true; }
                break;
            case WeaponType.AK47:
                if (akAmmo > 0) { akAmmo--; UpdateAmmoText(); return true; }
                break;
            case WeaponType.Shotgun:
                if (shotgunAmmo > 0) { shotgunAmmo--;UpdateAmmoText(); return true; }
                break;
        }
        
        Debug.Log("No Ammo!");
        return false;
    }
}