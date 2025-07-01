using UnityEngine;
using UnityEngine.UI;

public class StaminaSystem : MonoBehaviour
{
    public Image fillImage;
    public float maxStamina = 100f;
    public float drainRate = 3f;  // por segundo
    public float regenRate = 10f;

    private float currentStamina;

    public bool IsExhausted => currentStamina <= 0f;

    void Start()
    {
        currentStamina = maxStamina;
        UpdateFill();
    }

    public void Drain(float amount)
    {
        currentStamina -= amount * Time.deltaTime;
        currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
        UpdateFill();
    }

    public void Regenerate()
    {
        currentStamina += regenRate * Time.deltaTime;
        currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
        UpdateFill();
    }

    void UpdateFill()
    {
        if (fillImage != null)
            fillImage.fillAmount = currentStamina / maxStamina;
    }

    public float GetStamina()
    {
        return currentStamina;
    }
}
