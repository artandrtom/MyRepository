using ExitGames.Client.Photon.LoadBalancing;

using Hashtable = ExitGames.Client.Photon.Hashtable;


public class MyPlayer : Player
{
    private int posX;
    private int posY;
    public bool isReady = false;

    protected internal MyPlayer(string name, int actorID, bool isLocal, Hashtable actorProperties) : base(name, actorID, isLocal, actorProperties)
    {
    }

    public override string ToString()
    {
        return base.ToString() + " Ready: " + isReady;
    }
}