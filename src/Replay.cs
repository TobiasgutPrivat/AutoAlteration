using GBX.NET;
using GBX.NET.Engines.Game;
using GBX.NET.Engines.Plug;
using GBX.NET.Engines.Scene;
using ManiaAPI.NadeoAPI;

public class Replay
{
    private CGameCtnGhost? WRGhost;
    public List<Position>? Positions = [];
    
    public Replay(CGameCtnChallenge map) {
        // authorize
        NadeoLiveServices nls = new();
        nls.AuthorizeAsync("AutoAlteration", "D),i8Fo_=/O9a*YU", AuthorizationMethod.DedicatedServer).GetAwaiter().GetResult();
        NadeoServices ns = new();
        ns.AuthorizeAsync("AutoAlteration", "D),i8Fo_=/O9a*YU", AuthorizationMethod.DedicatedServer).GetAwaiter().GetResult();

        //Get map info
        string mapUid = map.MapInfo.Id;
        MapInfoLive mapInfo = nls.GetMapInfoAsync(mapUid).GetAwaiter().GetResult();
        Guid mapId = mapInfo.MapId;
        // get wr
        TopLeaderboardCollection leaderboard = nls.GetTopLeaderboardAsync(mapUid, 1).GetAwaiter().GetResult();
        if (leaderboard.Tops.Count == 0 || leaderboard.Tops.First().Top.Count == 0)
        {
            Console.WriteLine($"Map has no WR data");
        }
        else
        {
            Record wr = leaderboard.Tops.First().Top.First();
            //Download Replay
            MapRecord wrRec = ns.GetMapRecordsAsync([wr.AccountId], mapId).GetAwaiter().GetResult().First();
            string downloadURL = wrRec.Url;
            using var httpClient = new HttpClient();
            using var response = httpClient.GetAsync(downloadURL).GetAwaiter().GetResult();
            response.EnsureSuccessStatusCode();
            string replayPath = Path.Combine(Path.GetTempPath(), $"wr{mapId}.gbx.replay");
            using var stream = response.Content.ReadAsStreamAsync().GetAwaiter().GetResult();
            using var replayfs = new FileStream(replayPath, FileMode.Create);
            stream.CopyToAsync(replayfs).GetAwaiter().GetResult();
            replayfs.Close();
            //Load Replay
            Gbx<CGameCtnGhost> replayGbx = Gbx.Parse<CGameCtnGhost>(replayPath); ;
            WRGhost = replayGbx.Node;
            //clear
            File.Delete(replayPath);
        }
        List<CPlugEntRecordData.EntRecordListElem>? entries = WRGhost?.RecordData?.EntList
            .Where(x => x.Samples.Count > 20 && x.Samples.First() is CSceneVehicleVis.EntRecordDelta).ToList(); //20 is an aproximate to not select incomplete entries
        Positions = entries?.SelectMany(entry => entry.Samples.Where(sample => sample is CSceneVehicleVis.EntRecordDelta).Cast<CSceneVehicleVis.EntRecordDelta>())
            .Select(sample => new Position(sample.Position, new Vec3(sample.PitchYawRoll.Y, sample.PitchYawRoll.X, sample.PitchYawRoll.Z))).ToList() ?? [];
    }
}