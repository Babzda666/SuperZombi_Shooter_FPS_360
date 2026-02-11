using UnityEngine;
using UnityEngine.EventSystems;

public class GunsMenu : MonoBehaviour
{
   // public GameObject Buttons;
    public GameObject[] Guns; // Переконайся, що порядок тут: 0-Pistol, 1-AK47, 2-Shotgun
    int currentGun = 0;

    public GameObject[] texts;
    
    void Start()
    {
        // Ховаємо всі, показуємо першу
        foreach (var gun in Guns) gun.SetActive(false);
        if(Guns.Length > 0) Guns[0].SetActive(true);
        foreach (var gun in texts) gun.SetActive(false);
        if(texts.Length > 0) texts[0].SetActive(true);
        
    }

    public void NextGun()
    {
        Guns[currentGun].SetActive(false);
        texts[currentGun].SetActive(false);
        currentGun++;
        if (currentGun >= Guns.Length)
            currentGun = 0;
        Guns[currentGun].SetActive(true); 
        texts[currentGun].SetActive(true);
    }
    
    public void PreviousGun()
    {
        Guns[currentGun].SetActive(false);
        texts[currentGun].SetActive(false);
        currentGun--;
        if (currentGun < 0)
            currentGun = Guns.Length - 1;
        Guns[currentGun].SetActive(true);
        texts[currentGun].SetActive(true);
    }
    
    // Нова функція для вибору конкретної зброї
    public void SelectGun(int index)
    {
        if (index >= 0 && index < Guns.Length && currentGun != index)
        {
            Guns[currentGun].SetActive(false);
            texts[currentGun].SetActive(false);
            currentGun = index;
            Guns[currentGun].SetActive(true);
            texts[currentGun].SetActive(true);
        }
    }

    private void Update()
    {
        // --- ЛОГІКА КЛАВІАТУРИ ---
        if (Input.GetKeyDown(KeyCode.Alpha1)) SelectGun(0); // Клавіша 1
        if (Input.GetKeyDown(KeyCode.Alpha2)) SelectGun(1); // Клавіша 2
        if (Input.GetKeyDown(KeyCode.Alpha3)) SelectGun(2); // Клавіша 3
        // -------------------------

        if ((Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && !EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)))
        {
            // Це ваша існуюча логіка ховання кнопок. 
            // УВАГА: Це може конфліктувати зі стрільбою (Input.GetButtonDown("Fire1")),
            // якщо Fire1 це ліва кнопка миші. Але я залишаю як є, бо ви просили.
            //Buttons.SetActive(false);
        }
        else if(Input.touchCount == 0 && !Input.GetMouseButton(0))
        {
           // Buttons.SetActive(true);
        }
    }
}
