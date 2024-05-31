using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class uiTransistion : MonoBehaviour
{
    public void uiChange(GameObject saveThisScene)
    {
        saveThisScene.SetActive(true);
    }
}
