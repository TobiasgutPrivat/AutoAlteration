using GBX.NET.Engines.Plug;

public class AlterationLogic {
    public static int mapCount = 0;
    private static List<Alteration> ?lastAlterations;
    public static void Alter(List<Alteration> alterations, Map map) {
        //cleanup
        if (Directory.Exists(Path.Join(AlterationConfig.CustomBlocksFolder,"Temp"))){
            Directory.Delete(Path.Join(AlterationConfig.CustomBlocksFolder,"Temp"),true);
        }
        if (Directory.Exists(Path.Join(AlterationConfig.CustomBlocksFolder,"Exports"))){
            Directory.Delete(Path.Join(AlterationConfig.CustomBlocksFolder,"Exports"),true);
        }
        Alteration.inventory.ClearSpecific();

        //create inventory for Alteration
        if (lastAlterations == null || alterations.Any(a => !lastAlterations.Select(lAs => lAs.GetType()).Contains(a.GetType())) || (alterations.Count != lastAlterations.Count)) {
            // needs Inventory recreation
            Alteration.CreateInventory(); //Resets Inventory to Vanilla
            foreach (Alteration alteration in alterations) {
                alteration.InventoryChanges.ForEach(change => {
                    (change as CustomBlockSet)?.GetAdditionalKeywords().ForEach(keyword => {
                        if (!AlterationConfig.CustomBlockSets.Contains(keyword)) {
                            AlterationConfig.CustomBlockSets.Add(keyword);
                            AlterationConfig.Keywords.Add(keyword);
                        }
                    });
                    change.ChangeInventory(Alteration.inventory);
                });
            }
            Alteration.DefaultInventoryChanges();

            if (AlterationConfig.devMode){ //logging
                Alteration.inventory.Export(string.Join("",alterations.Select(x => x.GetType().Name)));
            }
        }

        //Map specific custom blocks
        if (map.embeddedBlocks.Count != 0){
            
            Alteration.inventory.AddArticles(map.embeddedBlocks
                // check if embedded block without first folder is already in inventory (needed for NC2 maps)
                .Where(x => Alteration.inventory.GetArticle(x.Key.Substring(x.Key.IndexOf(Path.DirectorySeparatorChar)+1)) == null
                ).Select(x => new Article(x.Key, x.Value,"",true)).ToList());
            Alteration.inventory.cachedInventories.Clear();
            if (AlterationConfig.devMode)
            { //logging
                Alteration.inventory.Export("WithMapBlocks");
            }
        }

        //Generate Map specific custom blocks sets
        alterations
            .SelectMany(alteration => alteration.InventoryChanges)
            .Where(change => change is CustomBlockSet)
            .Cast<CustomBlockSet>().ToList().ForEach(
                map.GenerateCustomBlocks); //includes updating inventory
                
        if (AlterationConfig.devMode){ //logging
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
            customBlock.Models.ForEach(x => changed = alteration.AlterMesh(customBlock, x) || changed);
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