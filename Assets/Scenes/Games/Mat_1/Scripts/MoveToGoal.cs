using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToGoal : MonoBehaviour
{
    //public GameObject Hand;
    public Vector3 basket;

    public float speed = 2.0f;
    public float accuracy = 0.01f;

    public FruitInitialization fruit;

    void  LateUpdate() {
        Move(fruit.goal);
    }
    void Move(Vector3 meta){
        Vector3 direction = meta - this.transform.position;
        if(direction.magnitude > accuracy){
            this.transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);
        }
           
    }
}
