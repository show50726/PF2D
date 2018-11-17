//Changing Property Giver made by STC
//contact:          stc.ntu@gmail.com
//last maintained:  2018/11/18
//Usage:           Just like Property Giver, but the giving property will keep changing.

using UnityEngine;

public class ChangingPropertyGiver : PropertyGiver2D
{
    [Tooltip("按照順序變換。Property留空視為清空屬性。時間差設定過小(<frame)時可能會被跳過")]
    public PropertyAndTime[] changeByOrder = new PropertyAndTime[1];
    ChangingPropertyGiver()
    {
        changeByOrder[0] = new PropertyAndTime(giveProperty, 0.5f);
    }

    protected virtual void SetPropertyAppearance(PlayerProperty2D property)
    {
        //DEV NOTE: 記得修改這邊改為適合的寫法。
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null) spriteRenderer = transform.parent.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.color = property.showingColor;
        }
        else
        {
            DebugMessage(LogType.Error, "trying to set giver's color while there's no renderer. Re-write your code.");
        }
    }
    private float timer = 0;
    protected override void Start()
    {
        base.Start();
        for (int i = 0; i < changeByOrder.Length; i++)
        {
            if (changeByOrder[i].time <= Time.deltaTime)
            {
                DebugMessage(LogType.Warning, "the interval set of the " + i + " property " + changeByOrder[i].property.GetType().Name + " is too small, it might be skipped.");
            }
        }
    }
    private void Update()
    {
        timer += Time.deltaTime;
        int newPropertyIndex = CheckTimePropertyIndex(timer);
        if(newPropertyIndex >= changeByOrder.Length)
        {
            DebugMessage(LogType.Normal, "found new property index larger than array. Force-fix it.");
            newPropertyIndex = 0;
        }
        if(changeByOrder[newPropertyIndex].property != giveProperty)
        {
            DebugMessage(LogType.Normal, "change property from " + giveProperty.GetType().Name + " to " + changeByOrder[newPropertyIndex].property.GetType().Name + ".");
            giveProperty = changeByOrder[newPropertyIndex].property;
            SetPropertyAppearance((PlayerProperty2D)giveProperty);
            if(newPropertyIndex == 0)
            {
                //has finished a cycle. Reset timer.
                timer = 0;
            }
        }
    }
    private int CheckTimePropertyIndex(float nowTime)
    {
        float countTime = 0;
        for (int i = 0; i < changeByOrder.Length; i++)
        {
            countTime += changeByOrder[i].time;
            if (countTime >= nowTime) return i;
        }
        // nowTime is too large. Assume that it has finished a cycle.
        return 0;
    }

}
[System.Serializable]
public class PropertyAndTime
{
    public UnitProperty property;
    public float time;
    public PropertyAndTime()
    {
        property = null;
        time = 0.5f;
    }
    public PropertyAndTime(UnitProperty assignProperty, float assignTime)
    {
        property = assignProperty;
        time = assignTime;
    }
}
