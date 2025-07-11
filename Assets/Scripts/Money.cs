using System;
using UnityEngine;

public class Money : MonoBehaviour
{
    public Action actionOnDestroy;

    public int Weight => weight;

    [SerializeField] private int amount;
    [Range(1.0f, 10f)][SerializeField] private int weight;

    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            collider.GetComponent<CarManager>().AddMoney(amount);
            CollectMoney();
        }
    }

    public void CollectMoney()
    {
        actionOnDestroy?.Invoke();
        Destroy(gameObject);
    }
}
