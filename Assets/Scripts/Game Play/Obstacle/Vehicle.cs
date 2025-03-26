using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.SceneManagement;

public class Vehicle : Obstacle, IMoveable, IDifficultyScaler
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float distanceToWarning = 10f;

    private float baseSpeed;
    private float baseDistanceToWarning;
    private Rigidbody2D vehicleRb;
    private bool hasWarned = false;
    private bool hasDisabledWarning = false;

    private void Awake()
    {
        vehicleRb = GetComponent<Rigidbody2D>();
        Observer.AddObserver(GameEvent.OnGameDifficultyIncreasing,OnIncreaseDifficulty);
        baseSpeed = speed;
        baseDistanceToWarning = distanceToWarning;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadSceneAsync("Game Scene");
        }
        HandleWarning();
        HandleDisableWarning();
    
    }

    void OnDestroy()
    {
        Observer.RemoveListener(GameEvent.OnGameDifficultyIncreasing, OnIncreaseDifficulty);
    }
    private void HandleWarning()
    {
        if (hasWarned) return;

        var player = PlayerController.Instance;
        if (player == null) return;

        if (transform.position.x - player.transform.position.x <= distanceToWarning)
        {
            hasWarned = true;
            Move();
            Observer.Notify(GameEvent.OnVehicleWarning, transform.position);
            Debug.Log("WARNING");
        }
    }

    private void HandleDisableWarning()
    {
        if (hasDisabledWarning) return;

        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        if (screenPos.x >= 0 && screenPos.x <= Screen.width &&
            screenPos.y >= 0 && screenPos.y <= Screen.height)
        {
            hasDisabledWarning = true;
            Observer.Notify(GameEvent.OnDisableVehicleWarning);
            Debug.Log("DISABLE WARNING");
        }
    }

    public void Move()
    {
        vehicleRb.linearVelocity = new Vector2(-speed, 0);
    }

    public void OnIncreaseDifficulty(object[] datas)
    {
        float t = (float)datas[0];
        float difficultyScale = (float)datas[1];
        speed = Mathf.Lerp(baseSpeed, baseSpeed * difficultyScale, t);
    }
}
