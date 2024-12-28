using GBX.NET;
using GBX.NET.Engines.Game;

public class Replay { //AR
    public Gbx<CGameCtnReplayRecord> gbx; //maybe change CGameCtnReplayRecord
    public CGameCtnReplayRecord replay;
    public Replay(string ReplayPath)
    { 
        gbx = Gbx.Parse<CGameCtnReplayRecord>(ReplayPath);
        replay = gbx.Node;
    }

    //Make some functions to get replay data (used finish, cporder, nearest path rotation etc.)
}