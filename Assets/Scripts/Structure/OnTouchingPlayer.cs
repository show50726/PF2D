//On Touching Living by STC
//contact: stc.ntu@gmail.com
//last maintained: 2018/06/26
//Usage: this can be used to count down and make effect, such as harmful area.

using CMSR;
public class OnTouchingLiving
{
    public SLivingStater targetLiving;
    public float touchTime;
    public OnTouchingLiving(SLivingStater target)
    {
        targetLiving = target;
        touchTime = 0;
    }
}
