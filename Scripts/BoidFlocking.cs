using System;
using System.Collections.Generic;
using DitzelGames.FastIK;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class BoidFlocking : MonoBehaviour
{
    private BoidController _controller;
    [HideInInspector] public ProceduralAnimeController Anime;

    private Vector3 POS;

    private void Awake()
    {
        Anime = GetComponent<ProceduralAnimeController>();
    }

    public void Init(BoidController controller)
    {
        _controller = controller;
    }

    private void Update()
    {
        Vector3 separation = Vector3.zero;
        Vector3 alignment = _controller.transform.forward;
        Vector3 cohesion = _controller.transform.position;

        alignment = GetAlignmentVector();
        cohesion = GetCohesionVector();
        separation = GetSeperationVector();

       Vector3 temp = (alignment * _controller.AlignmentStrength) + 
                      (cohesion * _controller.CohesionStrength) + (separation * _controller.SeperationStrength);
       temp.y = 0;

       Anime.FlockAdditon = temp.normalized;
    }
    
    
    private Vector3 GetAlignmentVector()
    {
        int count = 0;
        Vector3 point = new Vector3();
        
        foreach (BoidFlocking boid in _controller.Boids)
        {
            if(boid == this)
                continue;
            if (Vector3.Distance(transform.position, boid.transform.position) < 300)
            {
                point.x += boid.Anime._currentVelocity.x;
                point.z += boid.Anime._currentVelocity.z;
                count++;
            }
        }
        
        if (count == 0)
            return point; 

        point /= count;
        return point.normalized * _controller.MaxSpeed;
    }
    
    private Vector3 GetCohesionVector()
    {
        int count = 0;
        Vector3 cohesion = new Vector3();
        
        foreach (BoidFlocking boid in _controller.Boids)
        {
            if(boid == this)
                continue;
            
            if (Vector3.Distance(transform.position, boid.transform.position) < 300)
            {
                cohesion.x += boid.transform.position.x;
                cohesion.z += boid.transform.position.z;
                count++;
            }
            
           
        }

        cohesion /= count;

        cohesion.x = cohesion.x - transform.position.x;
        cohesion.z = cohesion.z - transform.position.z;
        return cohesion.normalized;
    }

    private Vector3 GetSeperationVector()
    {
        Vector3 sep = new Vector3();
        foreach (BoidFlocking boid in _controller.Boids)
        {
            if(boid == this)
                continue;
            if (Vector3.Distance(transform.position, boid.transform.position) < _controller.Radius)
            {
                Vector3 difference = transform.position - boid.transform.position;
                sep += difference.normalized / difference.magnitude;
            }
        }

        return sep /= _controller.Boids.Length - 1;
    }

    public void SetController(BoidController controller)
    {
        _controller = controller;
    }
    
}
