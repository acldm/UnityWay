using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
  
    public const float PRECISION = 0.000001f;  
    private Animator animator;
    private PlayerSimulation playerSimulation;

    public PlayerData playerData;
    private void Start() {
      playerSimulation = new PlayerSimulation();
      animator = GetComponent<Animator>();      
    }

    void Update()
    {
      float axis = 0.0f;
      if (Input.GetKey(KeyCode.LeftArrow)) {
        axis = -1.0f;
      } else if (Input.GetKey(KeyCode.RightArrow)) {
        axis = 1.0f;
      }

      handleAxis(axis);
      Vector3 incrementPos = playerSimulation.SimualtionUpdate(axis, Time.deltaTime);

      transform.localPosition += incrementPos * playerData.speed;
    }

    void handleAxis(float axis) {
      animator.SetBool("isRun", Math.Abs(axis) > PRECISION);
      transform.localScale = axis > 0.0f ? new Vector3(-1, 1, 1) : new Vector3(1, 1, 1);
    }

    void AddUpForce() {
      
    }
}
