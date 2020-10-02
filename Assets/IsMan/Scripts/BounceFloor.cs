using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceFloor : Floor
{
  protected override void OnFloorEnter(Player player) {
    animator.SetBool("isCollide", true);
  }

  protected override void OnFloorStay(Player player) {
    // pass
  }

  protected override void OnFloorExit(Player player) {
    animator.SetBool("isCollide", false);
  }
}
