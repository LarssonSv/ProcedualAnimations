using UnityEngine;

public class BoidController : MonoBehaviour
{

   [SerializeField] public BoidFlocking[] Boids;
   [Range(0.0f, 0.9f)] public float AlignmentStrength = 1.0f;
   [Range(0.1f, 20.0f)] public float CohesionStrength = 1.0f;
   [Range(0.1f, 10.0f)] public float SeperationStrength = 1.0f;
   [Range(0.1f, 10.0f)] public float MaxSpeed = 2.0f;
   [Range(0.1f, 40.0f)] public float Radius = 2.0f;
   

   
   private void Start()
   {
      foreach (BoidFlocking boid in Boids)
      {
         boid.Init(this);
      }

   }
}