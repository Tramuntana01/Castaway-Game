using UnityEngine;
using UnityEngine.UI;

public class PickupSystem : MonoBehaviour
{
    public float pickupTime = 2f;
    private float holdTimer = 0f;
    private bool isNearItem = false;
    private GameObject itemToPickup;

    [Header("UI Elements")]
    public Image pickupUI;
    public Image pickupBarFill;
    public GameObject pickupBarBackground;

    void Start()
    {
        if (pickupUI != null)
            pickupUI.gameObject.SetActive(false);

        if (pickupBarFill != null)
        {
            pickupBarFill.fillAmount = 0f;
            pickupBarFill.gameObject.SetActive(false);
        }

        if (pickupBarBackground != null)
            pickupBarBackground.SetActive(false);
    }

    void Update()
    {
        if (isNearItem && itemToPickup != null)
        {
            if (Input.GetKey(KeyCode.E))
            {
                holdTimer += Time.deltaTime;

                if (pickupBarFill != null)
                {
                    pickupBarFill.fillAmount = holdTimer / pickupTime;
                    pickupBarFill.gameObject.SetActive(true);
                }

                if (pickupBarBackground != null)
                    pickupBarBackground.SetActive(true);

                if (holdTimer >= pickupTime)
                {
                    PickupItem();
                    holdTimer = 0f;
                }
            }
            else
            {
                holdTimer = 0f;

                if (pickupBarFill != null)
                {
                    pickupBarFill.fillAmount = 0f;
                    pickupBarFill.gameObject.SetActive(false);
                }

                if (pickupBarBackground != null)
                    pickupBarBackground.SetActive(false);
            }
        }
    }

    private void PickupItem()
{
    if (itemToPickup != null)
    {
        CollectibleItem collectible = itemToPickup.GetComponent<CollectibleItem>();
        if (collectible != null)
        {
            Debug.Log($"Recogido: {collectible.itemName}");
            
        }

        Destroy(itemToPickup);
        itemToPickup = null;
        isNearItem = false;

        if (pickupUI != null) pickupUI.gameObject.SetActive(false);
        if (pickupBarFill != null)
        {
            pickupBarFill.fillAmount = 0f;
            pickupBarFill.gameObject.SetActive(false);
        }
        if (pickupBarBackground != null) pickupBarBackground.SetActive(false);
    }
}


    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CollectibleItem>() != null)
        {
            isNearItem = true;
            itemToPickup = other.gameObject;

            if (pickupUI != null)
                pickupUI.gameObject.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == itemToPickup)
        {
            isNearItem = false;
            itemToPickup = null;
            holdTimer = 0f;

            if (pickupUI != null)
                pickupUI.gameObject.SetActive(false);

            if (pickupBarFill != null)
            {
                pickupBarFill.fillAmount = 0f;
                pickupBarFill.gameObject.SetActive(false);
            }

            if (pickupBarBackground != null)
                pickupBarBackground.SetActive(false);
        }
    }
}
