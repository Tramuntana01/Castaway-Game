using UnityEngine;

public class FloatingVisual : MonoBehaviour
{
    public float floatAmplitude = 0.2f; // Altura del movimiento
    public float floatFrequency = 1f;   // Velocidad del movimiento
    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float offset = Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        transform.position = new Vector3(startPos.x, startPos.y + offset, startPos.z);
    }
}
