using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwipe : MonoBehaviour
{
    //For use with an enemy collider; Not actively using this while trying to prototype; build more important systems
    // Vector2 pos1;
    // Vector2 pos2;
    // void OnMouseEnter() {
    //     if (Player.getPlayer().getStatus()) {
    //         pos1 = Input.mousePosition;
    //         if (SP <= 0) {
    //             dealDamage();
    //         }
    //     }
    // }

    // void OnMouseExit() {
    //     if (Player.getPlayer().getStatus()) {
    //         pos2 = Input.mousePosition;
    //         if (pos1 != null) {
    //             Vector2 finalVector = (pos2 - pos1);
    //             float angle = (Mathf.Atan2(finalVector.y, finalVector.x) * Mathf.Rad2Deg);
    //             if (angle < 0) {
    //                 angle += 360;
    //             }
    //         }
    //     }
    // }

    //For use in the Player class, using Unity Input System to try to figure out when the mouse is getting pressed.
    // private static bool isPressed = false;
    // public bool getStatus() {
    //     return isPressed;
    // }
    //OnSwing will trigger when the LeftMouseButton is pressed and released
    //The InputValue of OnSwing will return 1 when the LeftMouseButton is pressed and null when the LeftMouseButton is released
    //Using this we will modify the value of isPressed
    // private void OnSwing(InputValue value) {
    //     if (value.Get() == null) {
    //         isPressed = false;
    //     } else {
    //         isPressed = true;
    //     }
    // }

    //For use in 8 directional swiping
    // Direction playerDirection1 = Direction.None; //EW
    // Direction playerDirection2 = Direction.None; //NS
    // if (playerInput.y > 0) {
    //     playerDirection2 = Direction.North;
    // } else if (playerInput.y < 0) {
    //     playerDirection2 = Direction.South;
    // }
    // if (playerInput.x > 0) {
    //     playerDirection1 = Direction.East;
    // } else if (playerInput.x < 0) {
    //     playerDirection1 = Direction.West;
    // }
}
