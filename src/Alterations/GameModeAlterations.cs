class Race : Alteration {
    public override void Run(Map map) {
        map.map.MapType = "TrackMania\\TM_Race";
        map.map.ChallengeParameters.MapType = "TrackMania\\TM_Race";
    }
}

//CP Order Manual
class Stunt : Alteration {
    public override void Run(Map map) {
        map.map.MapType = "TrackMania\\TM_Stunt";
        map.map.ChallengeParameters.MapType = "TrackMania\\TM_Stunt";
    }
}

class Platform : Alteration {
    public override void Run(Map map) {
        map.map.MapType = "TrackMania\\TM_Platform";
        map.map.ChallengeParameters.MapType = "TrackMania\\TM_Platform";
    }
}