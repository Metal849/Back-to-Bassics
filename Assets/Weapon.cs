using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    // [SerializeField] float xEast = 0;
    // [SerializeField] float yEast = 0;
    // [SerializeField] float xWest = 0;
    // [SerializeField] float yWest = 0;
    // [SerializeField] float xNorth = 0;
    // [SerializeField] float yNorth = 0;
    // [SerializeField] float xSouth = 0;
    // [SerializeField] float ySouth = 0;

    // public void findWeaponPosition(Direction dir) {
    //     if (dir == Direction.North) {
    //         transform.position = new Vector3(xNorth, yNorth, 0);
    //         transform.rotation = Quaternion.Euler(0, 0, 0);
    //     } else if (dir == Direction.South) {
    //         transform.position = new Vector3(xSouth, ySouth, 0);
    //         transform.rotation = Quaternion.Euler(0, 0, 180f);
    //     } else if (dir == Direction.East) {
    //         transform.position = new Vector3(xEast, yEast, 0);
    //         transform.rotation = Quaternion.Euler(0, 180, 90f);
    //     } else if (dir == Direction.West) {
    //         transform.position = new Vector3(xWest, yWest, 0);
    //         transform.rotation = Quaternion.Euler(0, 0, 90f);
    //     }
        
    // }

    private Vector2[] weaponPos = {new Vector2(0, 2), new Vector2(0, -2), 
    new Vector2(2, 0), new Vector2(-2, 0),
    new Vector2(0, -2), new Vector2(0, 2), 
    new Vector2(-2, 0), new Vector2(2, 0)};
    private Quaternion[] rotation = {Quaternion.Euler(0, 0, 0), Quaternion.Euler(0, 0, 180f),
    Quaternion.Euler(0, 180, 90f), Quaternion.Euler(0, 0, 90f), 
    Quaternion.Euler(0, 0, 180f), Quaternion.Euler(0, 0, 0), 
    Quaternion.Euler(0, 0, 90f), Quaternion.Euler(0, 180, 90f)
    };

    public void findWeaponPosition(int pos) {
        transform.position = new Vector3(weaponPos[pos].x, weaponPos[pos].y, 0f);
        transform.rotation = rotation[pos];
    }
}
