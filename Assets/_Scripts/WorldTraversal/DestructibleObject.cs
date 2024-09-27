using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class DestructibleObject : MonoBehaviour
{
    public GameObject droppedOnBreak;
    public Direction directionToBreak;
    bool hit;
    static bool playerSlashing = false;
    static Direction slashDirection;

    public bool destructibleByCombo;
    static string comboString;

    public static void PlayerSlash(Vector2 slashDirection)
    {
        //Debug.Log(DirectionHelper.GetVectorDirection(slashDirection));
        playerSlashing = true;
        DestructibleObject.slashDirection = DirectionHelper.GetVectorDirection(slashDirection);
    }
    public static void PlayerSlashDone()
    {
        playerSlashing = false;
    }
    public static void PlayerCombo(string comboString)
    {
        //Debug.Log(DirectionHelper.GetVectorDirection(slashDirection));
        playerSlashing = true;
        DestructibleObject.comboString = comboString;
    }
    public void OnTriggerStay(Collider other)
    {
        if (!playerSlashing)
        {
            hit = false;
        }
        if(!destructibleByCombo)
        {
            if (other.gameObject.name == "slash_region" && playerSlashing && !hit)
            {
                //Debug.Log(slashDirection);
                if (directionToBreak == Direction.None || directionToBreak == slashDirection)
                {
                    //Debug.Log("i have been destro");
                    gameObject.GetComponent<Collider>().enabled = false;
                    if (droppedOnBreak != null)
                    {
                        Instantiate(droppedOnBreak, transform.position, transform.rotation);
                    }
                    Object.Destroy(gameObject);
                }
            hit = true;
            }
        }
        else 
        {
            if (other.gameObject.name == "slash_region" && playerSlashing && !hit)
            {
                //Debug.Log(slashDirection);
                if (comboString == "WEW")
                {
                    //Debug.Log("i have been destro");
                    gameObject.GetComponent<Collider>().enabled = false;
                    if (droppedOnBreak != null)
                    {
                        Instantiate(droppedOnBreak, transform.position, transform.rotation);
                    }
                    Object.Destroy(gameObject);
                }
            hit = true;
            }
        }
        
    }
}
