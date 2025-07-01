using UnityEngine;

public class Player_movement : MonoBehaviour
{
    public float walkSpeed = 3f;
    public float swimSpeed = 2f;
    public float rotationSpeed = 400f;
    public float waterLevel = 7f;
    public float floatStrength = 1f;

    private Rigidbody rb;
    private Animator animator;
    private bool isInWater = false;
    private bool hasInput = false;

    private StaminaSystem stamina;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        stamina = GetComponent<StaminaSystem>();
    }

    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 input = new Vector3(horizontal, 0f, vertical);
        hasInput = input.sqrMagnitude > 0.01f;

        bool movingForwardOrBack = Mathf.Abs(vertical) > 0.01f;
        float currentSpeed = isInWater ? swimSpeed : walkSpeed;

        if (hasInput)
        {
            input.Normalize();

            if (movingForwardOrBack)
            {
                Vector3 moveDir = transform.forward * input.z + transform.right * input.x;
                rb.MovePosition(rb.position + moveDir * currentSpeed * Time.deltaTime);
            }

            if (horizontal != 0f)
            {
                float rotationAmount = horizontal * rotationSpeed;
                transform.Rotate(Vector3.up, rotationAmount * Time.deltaTime);
            }

            animator.SetFloat("Speed", Mathf.Abs(vertical));
        }
        else
        {
            animator.SetFloat("Speed", 0f);
        }

        animator.SetBool("isSwimming", isInWater);

        // Estamina: gastar si está en agua, regenerar si no
        if (isInWater)
        {
            stamina?.Drain(10f); // Puedes ajustar la cantidad aquí
        }
        else
        {
            stamina?.Regenerate();
        }

        // (Opcional) Si se queda sin estamina en el agua
        if (isInWater && stamina != null && stamina.IsExhausted)
        {
            Debug.Log("¡Estás agotado!");
            // Aquí puedes añadir penalización: velocidad más lenta, daño, etc.
        }
    }

    void FixedUpdate()
    {
        if (isInWater)
        {
            float difference = waterLevel - rb.position.y;

            if (difference > 0.01f)
            {
                Vector3 floatForce = new Vector3(0f, difference * floatStrength, 0f);
                rb.AddForce(floatForce, ForceMode.Acceleration);
            }
            else
            {
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            }

            if (!hasInput)
            {
                Vector3 stopXZ = new Vector3(0f, rb.linearVelocity.y, 0f);
                rb.linearVelocity = stopXZ;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            isInWater = true;
            rb.useGravity = false;

            Vector3 pos = rb.position;
            pos.y -= 0.3f;
            rb.position = pos;

            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            isInWater = false;
            rb.useGravity = true;
        }
    }
}
