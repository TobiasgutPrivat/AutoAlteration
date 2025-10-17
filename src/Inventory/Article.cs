using GBX.NET;

public class Article {
    public string Name = "";
    public MoveChain MoveChain = new();
    public BlockType Type;
    public List<string> Keywords = [];
    public List<string> ToShapes = [];
    public int Width = 1;
    public int Length = 1;
    public int Height = 1;
    public string Path = "";
    // public bool DefaultRotation;
    public bool MapSpecific = false;
    public Move? DefaultRotation;

    public Article() { }

    public Article(string name, BlockType type, SList<string> keywords, SList<string>? toShape = null, MoveChain? moveChain = null, int length = 1, int width = 1)
    {
        Name = name;
        Type = type;
        Keywords = keywords;
        ToShapes = toShape ?? [];
        MoveChain = moveChain ?? new MoveChain();
        Length = length;
        Width = width;
    }
    
    public Article(int Height, int Width, int Length, BlockType Type, string Name){
        this.Name = Name;
        LoadKeywords();
        this.Type = Type;
        this.Length = Length;
        this.Width = Width;
        this.Height = Height;
    }

    public Article(string name,BlockType type, string Path, bool mapSpecific = false){
        Name = name;
        this.Path = Path;
        LoadKeywords();
        Type = type;
        string vanillaName = Name;

        // for customblocksets get size from unaltered version
        AlterationConfig.CustomBlockSets.ToList().ForEach(k => vanillaName = vanillaName.Replace(k,""));
        List<Article> vanillaVersion = Alteration.inventory.articles.Where(a => a.Name == vanillaName).ToList(); 
        if (vanillaVersion.Count > 0) { 
            Width = vanillaVersion.First().Width;
            Length = vanillaVersion.First().Length;
            Height = vanillaVersion.First().Height;
        }

        MapSpecific = mapSpecific;
    }

    public Article CloneArticle() =>
        new(Name,Type,Keywords.ToList(),ToShapes.ToList(),MoveChain.Clone(),Length,Width);

    public bool HasKeyword(string keyword) =>
        Keywords.Contains(keyword) || ToShapes.Contains(keyword);

    public bool Match(Article article) {
        if (Keywords.Count != article.Keywords.Count || ToShapes.Count != article.ToShapes.Count) {
            return false;
        }
        foreach (var keyword in Keywords) {
            if (Keywords.Where(k => k == keyword).Count() != article.Keywords.Where(k => k == keyword).Count()) {
                return false;
            }
        }
        foreach (var toShape in ToShapes) {
            if (ToShapes.Where(k => k == toShape).Count() != article.ToShapes.Where(k => k == toShape).Count()) {
                return false;
            }
        }
        return true;
    }

    #region LoadKeywords
    public void LoadKeywords() {
        //Path
        string[] splits = Name.Split(["/", "\\"],StringSplitOptions.RemoveEmptyEntries); // seperate Foldernames
        splits[..^1].ToList().ForEach(Keywords.Add); // Folders as Keywords

        //Name
        string name = splits.Last(); // the filename/blockname
        name = name.Replace(".Block.Gbx", "", StringComparison.OrdinalIgnoreCase).Replace(".Item.gbx", "", StringComparison.OrdinalIgnoreCase); // remove file ending if present
        int nameLength = name.Replace("_", "").Length + Keywords.Sum(k => k.Length);
        List<string> nameSplits = []; // Individual Parts of the name, Keywords get cut out, spereating the string

        //ToKeywords
        int toPos = GetToPos(name) ?? -1; // Extract ToKeywords (mostly shapes) to avoid naming conflicts

        if (toPos != -1) {
            string ToString = name[(toPos + 2)..];
            nameSplits.AddRange(name[..toPos].Split("_"));

            List<string> ToSplits = ToString.Split("_").ToList();

            SplitByKeywords(ToSplits, AlterationConfig.ToKeywords, ToShapes);
            Keywords.Add("To");
            nameSplits.AddRange(ToSplits);
        } else {
            nameSplits = name.Split("_").ToList();
        }

        //CustomblockSets, could have issues if set is part of toKeywords
        SplitByKeywords(nameSplits, AlterationConfig.CustomBlockSets, Keywords);
        
        //Keywords
        SplitByKeywords(nameSplits, AlterationConfig.Keywords, Keywords);

        Keywords.AddRange(nameSplits);
        
        if (nameLength != Keywords.Sum(k => k.Length) + ToShapes.Sum(k => k.Length)) {
            Console.WriteLine($"Name {Name} is not fully covered by keywords. keywords: " + KeywordString());
        }
    }

    private static void SplitByKeywords(List<string> splits, IEnumerable<string> keywords, List<string> collection) {
        foreach (var keyword in keywords) {
            while (splits.Any(split => split.Contains(keyword))) { //also
                splits.Where(split => split.Contains(keyword)).ToList().ForEach(split => {
                        int count = split.Split(keyword, StringSplitOptions.None).Length - 1;
                        collection.AddRange(Enumerable.Repeat(keyword, count));
                        splits.Remove(split); //replace split with itself splitted by the keyword
                        splits.AddRange(split.Split(keyword, StringSplitOptions.RemoveEmptyEntries));
                    });
            }
        }
    }

    public int? GetToPos(string name) {
        if(name.Contains("To")){
            int toPos = name.IndexOf("To"); //check if "to" is not part of another keyword
            if (AlterationConfig.Keywords.Where(k => k.Contains("To")).Any(k => name[toPos..].StartsWith(k))) {
                return GetToPos(name[(toPos + 2)..]) + toPos + 2;
            } else {
                return toPos;
            }
        }else {
            return null;
        }
    }
    #endregion

    public string KeywordString() =>
        string.Join(", ", Keywords.Select(k => k)) + " " + string.Join(", ", ToShapes);
}