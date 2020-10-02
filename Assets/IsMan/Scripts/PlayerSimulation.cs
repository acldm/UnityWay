using UnityEngine;

class PlayerSimulation {
  // 物理系数
  public Vector3 SimualtionUpdate(float axis, float delta) {
    float moveX = axis * delta;
    return new Vector3(moveX, 0, 0);
  }
}