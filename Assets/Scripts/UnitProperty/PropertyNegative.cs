//Property Negative made by STC
//contact:          stc.ntu@gmail.com
//last maintained:  2017/11/24
//Usage:            this is a example / base property that do negative effects to player such as damaging.

using UnityEngine;
using System.Collections;

public class PropertyNegative : PlayerProperty2D
{
    [Header("HP reduce setting")]
    public double damage = 1f;
    [Tooltip("unit: seconds.")]
    public float damagePeriod = 2f;
    private float damageTimer = 0;

    protected virtual void Update()
    {
        damageTimer += Time.deltaTime;
        if (damageTimer >= damagePeriod)
        {
            player.TakeDamage(damage);
            damageTimer = 0;
        }
    }


}
