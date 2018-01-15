//On Touching Player by STC
//contact: stc.ntu@gmail.com
//last maintained: 2018/01/07
//Usage: this can be used to count down and make effect, such as harmful area.

public class OnTouchingPlayer
{
    public Player player;
    public float touchTime;
    public OnTouchingPlayer(Player p)
    {
        player = p;
        touchTime = 0;
    }
}
