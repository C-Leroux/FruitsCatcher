using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FruitType
{
    Apple,
    Banana,
    Lemon,
    Empty
}

public class Fruit : MonoBehaviour
{
    [SerializeField]
    private FruitType type; // Type of fruit
    private float speed;    // The speed at which the fruit fall
    private bool isFalling; // True until the fruit reach the crate or the ground

    [SerializeField]
    private GameObject particles;

    public void Init(float speed)
    {
        this.speed = speed;
        transform.Rotate(0, 33, 0);
        isFalling = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isFalling)
        {
            transform.Translate(Vector3.down * Time.deltaTime * speed, Space.World);
            transform.Rotate(Time.deltaTime * 100, 0, 0);
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Crate")
        {
            GameManager.Instance().Catch(type);
            Instantiate(particles, transform.position, transform.rotation);
        }

        FruitsGenerator.Instance().DestroyFruit(this);
    }
}