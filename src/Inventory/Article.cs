public class Article {
    public string Name;
    public MoveChain MoveChain = new();
    public BlockType Type;
    public List<string> Keywords = [];
    // public List<string> Shapes = [];
    public List<string> ToShapes = [];
    // public List<string> Surfaces = [];
    public int Width = 1;
    public int Length = 1;
    public int Height = 1;
    public string Path = "";
    public bool DefaultRotation;
    public bool Theme;

    public static char[] systemCharacters = ['&', '|', '!', '(', ')'];

    public Dictionary<string,bool> cacheFilter = [];

    public Article(string name,BlockType type,SList<string> keywords,SList<string>? toShape = null,MoveChain ?moveChain = null,int length = 1, int width = 1){
        Name = name;
        Type = type;
        Keywords = keywords;
        // Shapes = shape ?? [];
        ToShapes = toShape ?? [];
        // Surfaces = surface ?? [];
        MoveChain = moveChain ?? new MoveChain();
        Length = length;
        Width = width;
    }
    // public Article(string name,BlockType type,List<string> ?keywords,string ?shape,string ?toShape,string ?surface,MoveChain ?moveChain = null,int length = 1, int width = 1){
    //     Name = name;
    //     Type = type;
    //     Keywords = keywords ?? [];
    //     if (shape != null) Shapes = [shape];
    //     if (toShape != null) ToShapes = [toShape];
    //     if (surface != null) Surfaces = [surface];
    //     MoveChain = moveChain ?? new MoveChain();
    //     Length = length;
    //     Width = width;
    // }
    
    public Article(string name,BlockType type){
        Name = name;
        LoadKeywords();
        Type = type;
    }
    public Article(int Height, int Width, int Length, string type, string Name, bool Theme, bool DefaultRotation){
        this.Name = Name;
        LoadKeywords();
        switch (type) {
            case "Block":
                Type = BlockType.Block;
                break;
            case "Item":
                Type = BlockType.Item;
                break;
            case "Pillar":
                Type = BlockType.Pillar;
                break;
        }
        this.Length = Length;
        this.Width = Width;
        this.Height = Height;
        this.DefaultRotation = DefaultRotation;
        this.Theme = Theme;
    }

    public Article(string name,BlockType type, string Path){
        Name = name;
        this.Path = Path;
        LoadKeywords();
        Type = type;
        string vanillaName = Name;
        AutoAlteration.customBlockAltNames.ToList().ForEach(k => vanillaName = vanillaName.Replace(k,""));
        List<Article> vanillaVersion = Alteration.inventory.articles.Where(a => a.Name == vanillaName).ToList();
        if (vanillaVersion.Count > 0) {
            Width = vanillaVersion.First().Width;
            Length = vanillaVersion.First().Length;
            Height = vanillaVersion.First().Height;
        }
    }

    public Article CloneArticle() =>
        new(Name,Type,Keywords.ToList(),ToShapes.ToList(),MoveChain.Clone(),Length,Width);

    public bool HasKeyword(string keyword) {
        // if (AutoAlteration.shapeKeywords.Contains(keyword)) {
        //     return Shapes.Any(k => k == keyword);
        // } else if (AutoAlteration.surfaceKeywords.Contains(keyword)) {
        //     return Surfaces.Any(k => k == keyword);
        // } else{
            return Keywords.Any(k => k == keyword);
        // }
    }

    public bool Match(string keywordFilter) {
        string oldKeywordFilter = keywordFilter;
        bool current = false;
        bool and = false;
        bool or = false;
        bool invert = false;
        bool result;
        if (cacheFilter.ContainsKey(keywordFilter)) {
            return cacheFilter[keywordFilter];
        }
        while (keywordFilter.Length > 0) {
            switch (keywordFilter[0]) {
            case '&':
                and = true;
                keywordFilter = keywordFilter[1..];
                break;
            case '|':
                or = true;
                keywordFilter = keywordFilter[1..];
                break;
            case '!':
                invert = true;
                keywordFilter = keywordFilter[1..];
                break;
            case '(':
                result = Match(keywordFilter[1..GetEndBracePos(keywordFilter)]);
                if (invert) {
                    result = !result;
                    invert = false;
                }
                if (and) {
                    current &= result;
                    and = false;
                } else if (or) {
                    current |= result;
                    or = false;
                } else {
                    current = result;
                }
                keywordFilter = keywordFilter[(GetEndBracePos(keywordFilter) + 1)..];
                break;
            default:
                int next = NextPos(keywordFilter);
                result = HasKeyword(keywordFilter[..next]);
                keywordFilter = keywordFilter[next..];
                if (invert) {
                    result = !result;
                    invert = false;
                }
                if (and) {
                    current &= result;
                    and = false;
                } else if (or) {
                    current |= result;
                    or = false;
                } else {
                    current = result;
                }
                break;
            }
        }
        cacheFilter.Add(oldKeywordFilter,current);
        return current;
    }

    private static int NextPos(string text) {
        if(text.IndexOfAny(systemCharacters) == -1) {return text.Length;}
        return text.IndexOfAny(systemCharacters);
    }

    private static int GetEndBracePos(string text) {
        int depth = 0;
        int i = 0;
        foreach (char character in text)
        {
            if (character == '(') {
                depth++;
            }
            if (character == ')'){
                depth--;
            }
            if(depth == 0){
                return i;
            }
            i++;
        }
        return 0;
    }

    public bool Match(Article article) {
        if (Name == "DecoPlatformDirtSlopeBaseCurve1InFull" && article.Name == "DecoPlatformDirtSlopeBaseCurve1InFullHeavyDirt") {
            Console.WriteLine("Debug");
        }
        // if (Type != article.Type) {
        //     return false;
        // };
        if (Keywords.Count != article.Keywords.Count || ToShapes.Count != article.ToShapes.Count) {
            return false;
        }
        foreach (var keyword in Keywords) {
            if (Keywords.Where(k => k == keyword).Count() != article.Keywords.Where(k => k == keyword).Count()) {
                return false;
            }
        }
        // foreach (var shape in Shapes) {
        //     if (Shapes.Where(k => k == shape).Count() != article.Shapes.Where(k => k == shape).Count()) {
        //         return false;
        //     }
        // }
        foreach (var toShape in ToShapes) {
            if (ToShapes.Where(k => k == toShape).Count() != article.ToShapes.Where(k => k == toShape).Count()) {
                return false;
            }
        }
        // foreach (var surface in Surfaces) {
        //     if (Surfaces.Where(k => k == surface).Count() != article.Surfaces.Where(k => k == surface).Count()) {
        //         return false;
        //     }
        // }
        return true;
    }

    public void LoadKeywords() {
        string name = Name;
        //toshape 
        if(name.Contains("To")){//TODO Probably issue when having To-Keyword like Torch before To Keyword
            int toPos = name.IndexOf("To");
            string ToString = name[(toPos + 2)..];
            if (!AutoAlteration.Keywords.Where(k => k.Contains("To")).Any(k => name[toPos..].IndexOf(k) == 0)) {
                AutoAlteration.shapeKeywords.ToList().ForEach(k => {
                    if (ToString.Contains(k)) {
                        ToShapes.Add(k);
                        ToString = ToString.Remove(ToString.IndexOf(k), k.Length);
                    }
                });
                Keywords.Add("To");
                name = name[..toPos] + ToString;
            };
        }
        
        //Keywords
        foreach (var keywordLine in AutoAlteration.Keywords) {
            if (name.Contains(keywordLine) && Name.Contains(keywordLine)) {
                // if (AutoAlteration.shapeKeywords.Contains(keywordLine)) {
                //     Shapes.Add(keywordLine);
                // } else if (AutoAlteration.surfaceKeywords.Contains(keywordLine)) {
                //     Surfaces.Add(keywordLine);
                // } else {
                    Keywords.Add(keywordLine);
                // }
                name = name.Remove(name.IndexOf(keywordLine), keywordLine.Length);
            }
            if (name.Contains(keywordLine) && Name.Contains(keywordLine)) {
                // if (AutoAlteration.shapeKeywords.Contains(keywordLine)) {
                //     Shapes.Add(keywordLine);
                // } else if (AutoAlteration.surfaceKeywords.Contains(keywordLine)) {
                //     Surfaces.Add(keywordLine);
                // } else {
                    Keywords.Add(keywordLine);
                // }
                name = name.Remove(name.IndexOf(keywordLine), keywordLine.Length);
            }
            if (name.Contains(keywordLine) && Name.Contains(keywordLine)) {
                Console.WriteLine("Article " + Name + " contains Keyword: " + keywordLine + " three Times");
            }
        }
        
        CheckFullNameCoverage();
    }
    private void CheckFullNameCoverage() {
        int nameLength = Name.Length;
        int keywordLength = Keywords.Sum(k => k.Length) + ToShapes.Sum(k => k.Length);
        if (nameLength == keywordLength) {
            // Console.WriteLine($"Name {Name} is fully covered by keywords: " + KeywordString());
        } else {
            Console.WriteLine($"Name {Name} is not fully covered by keywords. keywords: " + KeywordString());
        }
    }

    public string KeywordString() =>
        string.Join(", ", Keywords.Select(k => k)) + " " + string.Join(", ", ToShapes);
}