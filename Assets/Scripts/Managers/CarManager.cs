using System;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class CarManager : MonoBehaviour
{
    public Action<int, int> onMoneyChanged;
    public int TotalMoney { get; private set; }
    public Rigidbody Rigidbody => rigidbody;
    public float Speed => speed;

    [SerializeField] private MeshRenderer carMesh;

    //Movement
    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private float speed = 12f;

    [SerializeField] private BaseCarController carType;

    [Range(-1, 1)]
    private int movementInputValue; // The current value for movement
    private bool slowingDown;
    private Tween slowDownTween;

    private void OnEnable()
    {
        rigidbody.isKinematic = false;
        // Also reset the input values just incase
        movementInputValue = 0;
        slowDownTween = DOTween.To(() => movementInputValue, x => movementInputValue = x, 0, 0.5f).OnComplete(() => slowingDown = false);
    }

    private void OnDisable()
    {
        rigidbody.isKinematic = true;
    }

    public void Setup(BaseCarController controller, Transform spawnPoint, Color carColor)
    {
        //Putting the car in the correct position
        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;
        carMesh.material.color = carColor;

        carType = controller;
        carType.Initialize(this);
    }

    public void SetSpeed(float speedAmount)
    {
        speed = speedAmount;
    }

    public void Steer(Movement forwardBackward)
    {
        slowDownTween.Kill(); //Stopping the slowing down animation
        movementInputValue = (int)forwardBackward;
    }

    public void SlowDown()
    {
        if (movementInputValue > 0 && !slowingDown)
        {
            slowingDown = true;
            DOTween.To(() => movementInputValue, x => movementInputValue = x, 0, 0.5f).OnComplete(() => slowingDown = false);
        }
    }

    public void AddMoney(int amount)
    {
        TotalMoney += amount;
        onMoneyChanged?.Invoke(TotalMoney, amount);
    }

    public void RemoveMoney(int amount)
    {
        TotalMoney -= amount;
        onMoneyChanged?.Invoke(TotalMoney, -amount);
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.Playing)
        {
            //Updating Movement
            carType.CarUpdate(this);
            Move();
        }
    }

    private void Move()
    {
        // Create a vector in the direction the car is facing with a magnitude based on speed and the time between frames.
        Vector3 movement = transform.forward * movementInputValue * speed * Time.deltaTime;
        rigidbody.MovePosition(rigidbody.position + movement);
    }
}

public enum Movement
{
    Backward = -1,
    Stop = 0,
    Forward = 1
}
