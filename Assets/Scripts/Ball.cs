using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("References")] [SerializeField]
    private Rigidbody2D rb;

    [SerializeField] private LineRenderer lr;

    [Header("Attributes")] [SerializeField]
    private float maxPower = 10f;

    [SerializeField] private float power = 2f;
    [SerializeField] private float maxGoalSpeed = 4f;

    private bool _isDragging;
    private bool _inHole;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        PlayerInput();
    }

    private bool IsReady()
    {
        return rb.linearVelocity.magnitude <= 0f;
    }

    private void PlayerInput()
    {
        if (!IsReady()) return;
        Vector2 inputPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float distance = Vector2.Distance(transform.position, inputPos);

        if (Input.GetMouseButtonDown(0) && distance < 0.5f) DragStart();
        if (Input.GetMouseButton(0) && _isDragging) DragChange(inputPos);
        if (Input.GetMouseButtonUp(0) && _isDragging) DragRelease(inputPos);
    }

    private void DragStart()
    {
        _isDragging = true;
        lr.positionCount = 2;
    }

    private void DragChange(Vector2 inputPos)
    {
        Vector2 dir = ((Vector2)transform.position - inputPos);
        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, (Vector2)transform.position + Vector2.ClampMagnitude((dir * power) / 2, (maxPower) / 2));
    }

    private void DragRelease(Vector2 inputPos)
    {
        float distance = Vector2.Distance(transform.position, inputPos);
        _isDragging = false;
        lr.positionCount = 0;
        if (distance < 1f)
        {
            return;
        }

        Vector2 direction = (Vector2)transform.position - inputPos;
        rb.linearVelocity = Vector2.ClampMagnitude(direction * power, maxPower);
    }
}