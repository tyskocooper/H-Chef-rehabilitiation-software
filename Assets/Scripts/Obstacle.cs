using UnityEngine;

public class Obstacle : MonoBehaviour
{
    // global variables

    public float minSize = 0.5f;
    public float maxSize = 2.0f;
    public float minSpeed = 50f;
    public float maxSpeed = 150f;

    public float maxSpinSpeed = 10f;



    Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //randomSize gives every obstacle a random size between 0.5 and 2.0
       float randomSize = Random.Range(minSize,maxSize); 
       transform.localScale = new Vector3(randomSize, randomSize,1);


       rb = GetComponent<Rigidbody2D>();

       //assigns random speed to obstacles
      //makes larger obstacles move relatively slower based on their size
       float randomSpeed = Random.Range(minSpeed, maxSpeed) / randomSize;


       //assigns obstacles to fire in a random direction 
       Vector2 randomDirection = Random.insideUnitCircle;


       //assigns the randomSpeed and randomDireciton variables above to RigidBody2D

       rb.AddForce(randomDirection * randomSpeed);

       //Makes objects rotate unpredictably when they spawn
       float randomTorque = Random.Range(-maxSpinSpeed, maxSpinSpeed);
       rb.AddTorque(randomTorque);



      


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
