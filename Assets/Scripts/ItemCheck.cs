using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCheck : MonoBehaviour
{
    [Header("All Items")]
    [SerializeField] bool canDash;

    #region DashAbility
    public void ActivateDash()
    {
        canDash = true;
    }

    public bool GetDashCheck()
    {
        return canDash;
    }
    #endregion
}
