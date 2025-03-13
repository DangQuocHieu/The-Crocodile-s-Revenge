using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class Missile : MonoBehaviour
{
    private Transform target;
    [SerializeField] private float speed = 10f;
    private Rigidbody2D rb;

    private void Awake()
    {
        target = GameObject.Find("Player").transform;
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Move();
        FollowPlayerY();
    }

    private void Move()
    {
        rb.linearVelocity = new Vector2(-speed, rb.linearVelocity.y);
        
    }

    void FollowPlayerY()
    {
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(rb.position);
        if(screenPosition.x >= 0 && screenPosition.x <= Screen.width && screenPosition.y >= 0 && screenPosition.y <= Screen.height)
        {
            return;
        }
        float yPosition = Mathf.Lerp(transform.position.y, target.position.y, Time.deltaTime);
        transform.position = new Vector2(transform.position.x, yPosition);
    }

}
