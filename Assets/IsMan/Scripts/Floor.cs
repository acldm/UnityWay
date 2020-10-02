using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Animator))]
public class Floor : MonoBehaviour
{

  bool isOnFloor = false;
  Player player;
  protected Animator animator;

  private void Start() {
    animator = GetComponent<Animator>();
  }

  private void OnCollisionEnter2D(Collision2D other) {
    GameObject gb = other.gameObject;
    string name = gb.name;
    if (name == "Player") {
      isOnFloor = true;
      if (!player) {
        player = gb.GetComponent<Player>();
      }
      OnFloorEnter(player);
    }
  }

  protected virtual void OnFloorEnter(Player player) {
    // pass
  }

  private void OnCollisionStay2D(Collision2D other) {
    if (isOnFloor) {
      OnFloorStay(player);
    }
  }
  
  protected virtual void OnFloorStay(Player player) {
    // pass
  }

  private void OnCollisionExit2D(Collision2D other) {
    string name = other.gameObject.name;

    if (name == "player") {
      isOnFloor = false;
      OnFloorExit(player);
    }
  }

  protected virtual void OnFloorExit(Player player) {
    // pass
  }
}
