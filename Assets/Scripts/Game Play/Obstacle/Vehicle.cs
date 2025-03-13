using UnityEngine;

public class Vehicle : Obstacle, IMoveable
{
    [SerializeField] float speed = 10f;
    private Rigidbody2D vehicleRb;

    private void Awake()
    {
        vehicleRb = GetComponent<Rigidbody2D>();    
    }
    void Update()
    {
        Move();
    }
    public void Move()
    {
        vehicleRb.linearVelocity = new Vector2(-speed, 0);
    }

}
