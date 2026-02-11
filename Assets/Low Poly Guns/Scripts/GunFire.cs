using UnityEngine;

public class GunFire : MonoBehaviour
{
    [Header("Weapon Settings")]
    public WeaponType gunType; // Тут в інспекторі вибираємо тип (Pistol/AK/Shotgun)
    
    public float damage;
    public float shootDelay = 2;
    private float Timer = 0;
    public Transform firePoint;
    public GameObject Shooteffect;
    public Animator animator;

    // Посилання на інвентар
    private AmmoInventory ammoInventory;

    private void Start()
    {
        // Знаходимо інвентар у батьківському об'єкті (Гравці)
        ammoInventory = FindFirstObjectByType<AmmoInventory>();
        if (ammoInventory == null)
            Debug.LogError("Не знайдено скрипт AmmoInventory на гравцеві!");
    }

    private void Update()
    {
        Timer += Time.deltaTime;
        
        // Додаємо перевірку: (Натиснули) І (Пройшов час) І (Є патрони)
        if (Input.GetButton("Fire1") && Timer > shootDelay)
        {
            if (ammoInventory != null && ammoInventory.TryConsumeAmmo(gunType))
            {
                Timer = 0;
                Shoot();
            }
            else
            {
                // Тут можна додати звук "клац" (пуста обойма)
                Debug.Log("Out of ammo!");
            }
        }
    }

    public void Shoot()
    {
        Ray gun = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(gun, out hit))
        {
            if (hit.collider.TryGetComponent<zombyhp>(out zombyhp zhp))
            {
                float minDist = 2f;
                float maxDist = 40f;

                float d = hit.distance;

                // лінійне зменшення урону
                float mappedDamage = damage * (maxDist - d) / (maxDist - minDist);

                // clamp між 0 і damage
                mappedDamage = Mathf.Clamp(mappedDamage, 0, damage);
                zhp.TakeDamage(mappedDamage);

                Debug.Log($"dist={d} dmg={mappedDamage}");
            }
        }

        animator.Play("Shootgun");
        Instantiate(Shooteffect, firePoint.position, firePoint.rotation).transform.parent = this.transform;
    }
}
