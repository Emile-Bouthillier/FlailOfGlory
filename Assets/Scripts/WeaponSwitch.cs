using UnityEngine;

public class WeaponSwitch : MonoBehaviour
{
    private int selectedWeapon;
    private Transform uiPreviewTransform;

    // Start is called before the first frame update
    void Start()
    {
        GameObject UiPreview = GameObject.FindGameObjectsWithTag("WeaponPreview")[0];
        uiPreviewTransform = UiPreview.transform;
        selectedWeapon = 0;
        SelectWeapon();
        UpdateWeaponUI();
    }

    // Update is called once per frame
    void Update()
    {
        int previousWeapon = selectedWeapon;

        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if (selectedWeapon >= transform.childCount - 1)
            {
                selectedWeapon = 0;
            }
            else selectedWeapon++;
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if (selectedWeapon <= 0)
            {
                selectedWeapon = transform.childCount - 1;
            }
            else selectedWeapon--;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedWeapon = 0;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            selectedWeapon = 1;
        }

        if (previousWeapon != selectedWeapon)
        {
            SelectWeapon();
            UpdateWeaponUI();
        }
    }

    // Weapon Switch
    void SelectWeapon()
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            if (i == selectedWeapon)
                weapon.gameObject.SetActive(true);
            else
                weapon.gameObject.SetActive(false);
            i++;
        }
    }

    // Update Weapon UI
    void UpdateWeaponUI()
    {
        int i = 0;
        foreach (Transform uiPreview in uiPreviewTransform)
        {
            if (i == selectedWeapon)
                uiPreview.gameObject.SetActive(true);
            else
                uiPreview.gameObject.SetActive(false);
            i++;
        }
    }
}
