public abstract class InventoryChange {
    public abstract void ChangeInventory(Inventory inventory, bool mapSpecific = false);
}

public class CustomBlockFolder(string subFolder) : InventoryChange {
    public readonly string folder = Path.Combine(AlterationConfig.CustomBlocksFolder, subFolder);

    public override void ChangeInventory(Inventory inventory, bool mapSpecific = false) {
        Directory.GetFiles(folder, "*.Block.Gbx", SearchOption.AllDirectories).ToList().ForEach(x =>
            inventory.Add(new Article(x.Replace(folder, ""), BlockType.CustomBlock, x, mapSpecific)));

        Directory.GetFiles(folder, "*.Item.Gbx", SearchOption.AllDirectories).ToList().ForEach(x =>
            inventory.Add(new Article(x.Replace(folder,""), BlockType.CustomItem, x, mapSpecific)));
    }
}

public class CustomBlockSet(CustomBlockAlteration customBlockAlteration, bool skipUnchanged = true) : InventoryChange {
    public readonly CustomBlockAlteration customBlockAlteration = customBlockAlteration;
    public readonly bool skipUnchanged = skipUnchanged;

    public virtual List<string> GetAdditionalKeywords() { return [GetSetName()]; }
    public virtual string GetFolder() { return Path.Combine(AlterationConfig.CacheFolder, GetSetName()); }
    public virtual string GetSetName() { return customBlockAlteration.GetType().Name; }
    public virtual string GetOrigin() { return Path.Combine(AlterationConfig.CustomBlocksFolder, "Vanilla"); }

    public override void ChangeInventory(Inventory inventory, bool mapSpecific = false) {
        if (!Directory.Exists(GetFolder())) { 
            GenerateBlockSet();
        }
        
        Directory.GetFiles(GetFolder(), "*.Block.Gbx", SearchOption.AllDirectories).ToList().ForEach(x =>
            inventory.Add(new Article(Path.GetFileName(x), BlockType.CustomBlock, x)));

        Directory.GetFiles(GetFolder(), "*.Item.Gbx", SearchOption.AllDirectories).ToList().ForEach(x =>
            inventory.Add(new Article(Path.GetFileName(x), BlockType.CustomItem, x)));
    }

    public void GenerateBlockSet() {
        Console.WriteLine("Generating " + customBlockAlteration.GetType().Name + " block set...");
        if (!Directory.Exists(GetFolder())) { 
            Directory.CreateDirectory(GetFolder() + "Temp");//Temp in case something goes wrong
        }
        AutoAlteration.AlterAll(customBlockAlteration, GetOrigin(), GetFolder() + "Temp", GetSetName(),skipUnchanged);
        Directory.Move(GetFolder() + "Temp", GetFolder());
    }
}

public class LightSurface(CustomBlockAlteration customBlockAlteration, bool skipUnchanged = true) : CustomBlockSet(customBlockAlteration, skipUnchanged) {
    public override List<string> GetAdditionalKeywords() { return [GetSetName(), customBlockAlteration.GetType().Name]; }
    public override string GetSetName() { return customBlockAlteration.GetType().Name + "Light"; }
    public override string GetOrigin() { return Path.Combine(AlterationConfig.CustomBlocksFolder, "LightSurface"); }
}

public class HeavySurface(CustomBlockAlteration customBlockAlteration, bool skipUnchanged = true) : CustomBlockSet(customBlockAlteration, skipUnchanged) {
    public override List<string> GetAdditionalKeywords() { return [GetSetName(), customBlockAlteration.GetType().Name]; }
    public override string GetSetName() { return customBlockAlteration.GetType().Name + "Heavy"; }
    public override string GetOrigin() { return Path.Combine(AlterationConfig.CustomBlocksFolder, "HeavySurface"); }
}


