using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Splines;

public class NpcCar : MonoBehaviour
{
    [SerializeField] private SplineAnimate animate; //The script that moves the object down a spline

    private bool isSet = false;

    public void SetCar(SplineContainer pathToFollow)
    {
        animate.Container = pathToFollow; //Setting the spline to follow 
        animate.Play();
        isSet = true;
    }

    public void Update()
    {
        if (isSet && !animate.IsPlaying)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            collider.GetComponent<CarManager>().RemoveMoney(10000);
            CarCollision();
        }
    }

    private void CarCollision()
    {
        Destroy(gameObject);
    }
}
