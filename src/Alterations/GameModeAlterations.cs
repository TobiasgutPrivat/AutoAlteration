using GBX.NET.Engines.Game;

//TODO figure this out
class Race : Alteration {
    public override void Run(Map map) {
        map.map.Mode = CGameCtnChallenge.PlayMode.Race;
    }
}

class Stunt : Alteration {
    public override void Run(Map map) {
        map.map.Mode = CGameCtnChallenge.PlayMode.Stunts;
    }
}

class Platform : Alteration {
    public override void Run(Map map) {
        map.map.Mode = CGameCtnChallenge.PlayMode.Platform;
    }
}