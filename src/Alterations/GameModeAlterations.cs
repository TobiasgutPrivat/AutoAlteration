public class Race : Alteration {
    public override string Description => "sets Map to Racemode";
    public override bool Published => false;
    public override bool LikeAN => true;
    public override bool Complete => false; //untested

    public override void Run(Map map) {
        map.map.MapType = "TrackMania\\TM_Race";
        map.map.ChallengeParameters.MapType = "TrackMania\\TM_Race";
    }
}

//CP Order Manual
public class Stunt : Alteration {
    public override string Description => "sets Map to Stuntmode";
    public override bool Published => false;
    public override bool LikeAN => true;
    public override bool Complete => false; //untested

    public override void Run(Map map) {
        map.map.MapType = "TrackMania\\TM_Stunt";
        map.map.ChallengeParameters.MapType = "TrackMania\\TM_Stunt";
    }
}

public class Platform : Alteration {
    public override string Description => "sets Map to Platformmode";
    public override bool Published => false;
    public override bool LikeAN => true;
    public override bool Complete => false; //untested

    public override void Run(Map map) {
        map.map.MapType = "TrackMania\\TM_Platform";
        map.map.ChallengeParameters.MapType = "TrackMania\\TM_Platform";
    }
}