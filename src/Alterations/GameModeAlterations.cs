public class GameMode(string mode) : Alteration {
    public override string Description => $"sets Map to {mode}Mode";
    public override bool Published => false;
    public override bool LikeAN => true;
    public override bool Complete => false; //untested

    public override void Run(Map map) {
        map.map.MapType = "TrackMania\\" + mode;
        map.map.ChallengeParameters.MapType = "TrackMania\\" + mode;
    }
}
public class Race : GameMode {
    public Race() : base("TM_Race") { }
}

public class Stunt : GameMode {
    public Stunt() : base("TM_Stunt") { }
}

public class Platform : GameMode {
    public Platform() : base("TM_Platform") { }
}

public class Secret : GameMode {
    public Secret() : base("TM_Secret") { }
}