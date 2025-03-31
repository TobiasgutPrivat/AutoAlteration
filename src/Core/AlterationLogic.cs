using GBX.NET.Engines.Plug;

public class AlterationLogic {
    public static int mapCount = 0;
    private static List<Alteration> ?lastAlterations;
    public static void Alter(List<Alteration> alterations, Map map) {
        //cleanup
        if (Directory.Exists(Path.Join(AltertionConfig.CustomBlocksFolder,"Temp"))){
            Directory.Delete(Path.Join(AltertionConfig.CustomBlocksFolder,"Temp"),true);
        }
        if (Directory.Exists(Path.Join(AltertionConfig.CustomBlocksFolder,"Exports"))){
            Directory.Delete(Path.Join(AltertionConfig.CustomBlocksFolder,"Exports"),true);
        }
        Alteration.inventory.ClearSpecific();

        //create inventory for Alteration
        if (lastAlterations == null || alterations.Any(a => !lastAlterations.Select(lAs => lAs.GetType()).Contains(a.GetType())) || (alterations.Count != lastAlterations.Count)) {
            // needs Inventory recreation
            Alteration.CreateInventory(); //Resets Inventory to Vanilla
            foreach (Alteration alteration in alterations) {
                alteration.InventoryChanges.ForEach(x => x.ChangeInventory(Alteration.inventory));
            }
            Alteration.DefaultInventoryChanges();

            if (AltertionConfig.devMode){ //logging
                Alteration.inventory.Export(string.Join("",alterations.Select(x => x.GetType().Name)));
            }
        }

        //Map specific custom blocks
        Alteration.inventory.AddArticles(map.embeddedBlocks.Select(x => new Article(x.Key, x.Value,"",true)).ToList());

        //Generate Map specific custom blocks sets
        alterations
            .SelectMany(alteration => alteration.InventoryChanges)
            .Where(change => change is CustomBlockSet)
            .Cast<CustomBlockSet>().ToList().ForEach(
                x => map.GenerateCustomBlocks(x.customBlockAlteration)); //includes updating inventory
                
        if (AltertionConfig.devMode){ //logging
            Alteration.inventory.Export("WithMapBlocks");
        }

        //alteration
        foreach (Alteration alteration in alterations) {
            alteration.Run(map);
        }

        lastAlterations = alterations;
        mapCount++;
    }
    
    public static bool Alter(List<CustomBlockAlteration> alterations, CustomBlock customBlock) {
        bool changed = false;
        foreach (CustomBlockAlteration alteration in alterations) {
            changed = alteration.Run(customBlock);
            customBlock.MeshCrystals.ForEach(x => changed = AlterMeshCrystal(alteration, customBlock, x) || changed);
            // if (customBlock.Type == BlockType.Block) {
            //     customBlock.Block.CustomizedVariants.ToList().ForEach(x => {
            //         if (x.Crystal != null) {
            //             changed = AlterMeshCrystal(alteration, customBlock, x.Crystal) || changed;
            //         }
            //     });
            // } else {
            //     changed = AlterMeshCrystal(alteration, customBlock, customBlock.Item.MeshCrystal) || changed;
            // }
        }
        mapCount++;
        return changed;
    }

    private static bool AlterMeshCrystal(CustomBlockAlteration alteration, CustomBlock customBlock, CPlugCrystal MeshCrystal) {
        bool changed = false;
        alteration.AlterMeshCrystal(customBlock, MeshCrystal);
        MeshCrystal.Layers.Where(x => x.GetType() == typeof(CPlugCrystal.GeometryLayer)).ToList().ForEach(x => {
            CPlugCrystal.GeometryLayer layer = (CPlugCrystal.GeometryLayer)x;
            changed = alteration.AlterGeometry(customBlock, layer) || changed;
        });
        MeshCrystal.Layers.Where(x => x.GetType() == typeof(CPlugCrystal.TriggerLayer)).ToList().ForEach(x => {
            CPlugCrystal.TriggerLayer layer = (CPlugCrystal.TriggerLayer)x;
            changed = alteration.AlterTrigger(customBlock, layer) || changed;
        });
        MeshCrystal.Layers.Where(x => x.GetType() == typeof(CPlugCrystal.SpawnPositionLayer)).ToList().ForEach(x => {
            CPlugCrystal.SpawnPositionLayer layer = (CPlugCrystal.SpawnPositionLayer)x;
            changed = alteration.AlterSpawn(customBlock, layer) || changed;
        });
        return changed;
    }
}