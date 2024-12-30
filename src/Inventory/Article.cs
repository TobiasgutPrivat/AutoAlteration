public class Article {
    public string Name;
    public MoveChain MoveChain = new();
    public BlockType Type;
    public List<string> Keywords = [];
    public List<string> ToShapes = [];
    public int Width = 1;
    public int Length = 1;
    public int Height = 1;
    public string Path = "";
    public bool DefaultRotation;
    public bool Theme;
    public bool MapSpecific = false;

    public Article(string name,BlockType type,SList<string> keywords,SList<string>? toShape = null,MoveChain ?moveChain = null,int length = 1, int width = 1){
        Name = name;
        Type = type;
        Keywords = keywords;
        ToShapes = toShape ?? [];
        MoveChain = moveChain ?? new MoveChain();
        Length = length;
        Width = width;
    }
    
    public Article(int Height, int Width, int Length, BlockType Type, string Name, bool Theme, bool DefaultRotation){
        this.Name = Name;
        LoadKeywords();
        this.Type = Type;
        this.Length = Length;
        this.Width = Width;
        this.Height = Height;
        this.DefaultRotation = DefaultRotation;
        this.Theme = Theme;
    }

    public Article(string name,BlockType type, string Path, bool mapSpecific = false){
        Name = name;
        this.Path = Path;
        LoadKeywords();
        Type = type;
        string vanillaName = Name;

        // for customblocksets get size from unaltered version
        AltertionConfig.customBlockAltNames.ToList().ForEach(k => vanillaName = vanillaName.Replace(k,""));
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
    public void LoadKeywords() {//TODO think about integrating full Path as Keywords -> Example: A01: PlatformBase in Map and Vanilla
                                //-> Maybe if two blocks have the same keywords thex can be matched based on full name similarity

        // string[] splits = Name.Split(["/", "\\"],StringSplitOptions.None).Where(p => !string.IsNullOrEmpty(p)).ToArray(); // seperate Foldernames
        // string name = splits.Last(); // filename/blockname
        // splits[..^1].ToList().ForEach(Keywords.Add); // Folders as Keywords

        string name = Name.Split(['/', '\\'],StringSplitOptions.None).Last(); // filename/blockname

        int toPos = GetToPos(name) ?? -1; // Extract ToKeywords (mostly shapes) to avoid naming conflicts
        if (toPos != -1) {
            string ToString = name[(toPos + 2)..];
            AltertionConfig.ToKeywords.ToList().ForEach(k => {
                    if (ToString.Contains(k)) {
                        ToShapes.Add(k);
                        ToString = ToString.Remove(ToString.IndexOf(k), k.Length);
                    }
                });
                Keywords.Add("To");
                name = name[..toPos] + ToString;
        }
        
        //Keywords
        foreach (var keywordLine in AltertionConfig.Keywords) {
            while (name.Contains(keywordLine) && Name.Contains(keywordLine)) { //also check original Name, to avoid Keywords created by 2 parts which where previously seperated
                Keywords.Add(keywordLine);
                name = name.Remove(name.IndexOf(keywordLine), keywordLine.Length);
            }
            // if (name.Contains(keywordLine) && Name.Contains(keywordLine)) {
            //     Keywords.Add(keywordLine);
            //     name = name.Remove(name.IndexOf(keywordLine), keywordLine.Length);
            // }
            // if (name.Contains(keywordLine) && Name.Contains(keywordLine)) {
            //     Console.WriteLine("Article " + Name + " contains Keyword: " + keywordLine + " three Times");
            // }
        }
        
        CheckFullNameCoverage();
    }

    public int? GetToPos(string name) {
        if(name.Contains("To")){
            int toPos = name.IndexOf("To"); //check if "to" is not part of another keyword
            if (AltertionConfig.Keywords.Where(k => k.Contains("To")).Any(k => name[toPos..].StartsWith(k))) {
                return GetToPos(name[(toPos + 2)..]) + toPos + 2;
            } else {
                return toPos;
            }
        }else {
            return null;
        }
    }
    
    private void CheckFullNameCoverage() {
        int keywordLength = Keywords.Sum(k => k.Length) + ToShapes.Sum(k => k.Length);
        if (Name.Split(['/', '\\'],StringSplitOptions.None).Last().Length != keywordLength) {
            Console.WriteLine($"Name {Name} is not fully covered by keywords. keywords: " + KeywordString());
        }
    }
    #endregion

    public string KeywordString() =>
        string.Join(", ", Keywords.Select(k => k)) + " " + string.Join(", ", ToShapes);
}