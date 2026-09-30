using UnityEngine;

public class TrainScript : MonoBehaviour
{
    public Rigidbody2D trainRB;
    public float trainSpeed = 5f;

    private bool onRailroad = false;

    void Update()
    {
        if (onRailroad)
        {
            var horizontalInput = Input.GetAxis("Horizontal");
            var verticalInput = Input.GetAxis("Vertical");

            trainRB.linearVelocity = new Vector2(
                horizontalInput * trainSpeed,
                verticalInput * trainSpeed
            );
        }
        else
        {
            trainRB.linearVelocity = Vector2.zero;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Railroad"))
        {
            onRailroad = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Railroad"))
        {
            onRailroad = false;
        }
    }
}