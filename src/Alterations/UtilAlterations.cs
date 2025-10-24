public class AllFreeAir : Alteration {
    public override void Run(Map map)
    {
        map.StageAll();
        map.stagedBlocks.ForEach(block => block.IsAir = true);
        // TODO place pillars for All Blocks
        // Roads (incl. Themed)
        // Platforms (incl. Open)
        // DecoHills
        map.PlaceStagedBlocks(false);
    }
}