using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegIKStepper : MonoBehaviour
{
  [SerializeField] private Transform _homeTransform;
  [SerializeField] private float _stepDistance;
  [SerializeField] private float _moveDuration;
  [SerializeField] private float _stepOvershoot;
  
  [HideInInspector] public bool Moving;



  public void TryMove()
  {
    if (Moving)
      return;

    float distFromHome = Vector3.Distance(transform.position, _homeTransform.position);
    if (distFromHome > _stepDistance)
      StartCoroutine(Step());
  }


  IEnumerator Step()
  {
    //Begin step values
    Quaternion startRotation = transform.rotation;
    Vector3 startPosition = transform.position;

    //Step over new position a little
    float overshootDistance = _stepDistance * _stepOvershoot; //How much overstep
    Vector3 towardHome = _homeTransform.position - transform.position; //Heading
    Vector3 overshootVector = towardHome * overshootDistance; //Overstep Point
    
    Quaternion endRotation = _homeTransform.rotation;
    Vector3 endPosition = _homeTransform.position + overshootVector; //Combine points for an endPosition

    //Step between this point to mimic lifting the leg
    Vector3 centerPosition = (startPosition + endPosition) / 2;
    centerPosition += _homeTransform.up * Vector3.Distance(startPosition, endPosition) / 2f;
    
    float timer = 0;
    Moving = true;
    
    do
    {
      timer += Time.deltaTime;
      float normalizedTime = timer / _moveDuration;
      normalizedTime = Easing.Cubic.InOut(normalizedTime);

      //Cubic Bezier lerp
      transform.position = Vector3.Lerp(
        Vector3.Lerp(startPosition, centerPosition, normalizedTime),
        Vector3.Lerp(centerPosition, endPosition, normalizedTime),
        normalizedTime);

      transform.rotation = Quaternion.Slerp(startRotation, endRotation, normalizedTime);

      yield return null; //Wait one frame
    } 
    while (timer < _moveDuration);

    Moving = false;
  }
}
