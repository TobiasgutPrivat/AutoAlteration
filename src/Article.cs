class Article {
    public string Name;
    public BlockMove blockMove;
    public BlockType Type;
    public List<string> Keywords = new List<string>();
    public string Shape = "";//opt
    public string ToShape = "";//opt
    public string Surface = "";//opt

    public static char[] systemCharacters = new char[] { '&', '|', '!', '(', ')' };

    public Dictionary<string,bool> cacheFilter = new Dictionary<string, bool>();

    public Article() { }
    public Article(string name,BlockType type,List<string> keywords,string shape,string toShape,string surface,BlockMove blockMove = null){ 
        this.Name = name;
        this.Type = type;
        this.Keywords = keywords;
        this.Shape = shape;
        this.ToShape = toShape;
        this.Surface = surface;
        this.blockMove = blockMove;
    }
    public Article(string name){ 
        this.Name = name;
        blockMove = null;
        loadKeywords();
    }

    public bool hasKeyword(string keyword) {
        if (Alteration.shapeKeywords.Contains(keyword)) {
            return Shape == keyword || ToShape == keyword;
        } else if (Alteration.surfaceKeywords.Contains(keyword)) {
            return Surface == keyword;
        } else{
            return Keywords.Any(k => k == keyword);
        }
    }

    public bool match(string keywordFilter) {
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
                keywordFilter = keywordFilter.Substring(1);
                break;
            case '|':
                or = true;
                keywordFilter = keywordFilter.Substring(1);
                break;
            case '!':
                invert = true;
                keywordFilter = keywordFilter.Substring(1);
                break;
            case '(':
                result = match(keywordFilter.Substring(1, getEndBracePos(keywordFilter) - 1));
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
                keywordFilter = keywordFilter.Substring(getEndBracePos(keywordFilter) + 1);
                break;
            default:
                int next = nextPos(keywordFilter);
                result = hasKeyword(keywordFilter.Substring(0, next));
                keywordFilter = keywordFilter.Substring(next);
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

    private int nextPos(string text) {
        if(text.IndexOfAny(systemCharacters) == -1) {return text.Length;}
        return text.IndexOfAny(systemCharacters);
    }

    private int getEndBracePos(string text) {
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

    public void loadKeywords() {
        string name = this.Name;
        if (name.Contains("RallyRoadDirtLow")) {
            // Console.WriteLine("Debug");
        }
        //toshape
        if(name.Contains("To")){
            int toPos = name.IndexOf("To");
            string[] toshapes = Alteration.shapeKeywords.Where(x => name.Substring(toPos + 2).Contains(x)).ToArray();
            if (toshapes.Count() >= 1){
                ToShape = toshapes.First();
                String ToString = name.Substring(toPos + 2);
                name = name.Substring(0,toPos) + ToString.Remove(ToString.IndexOf(ToShape), ToShape.Length);
            }
            toshapes = Alteration.shapeKeywords.Where(x => name.Substring(toPos).Contains(x)).ToArray();
            if (toshapes.Count() > 1) {
                Console.WriteLine("more than 1 To-shape: " + name);
            }
        }
        
        //Keywords
        foreach (var keywordLine in Alteration.Keywords) {
            if (name.Contains(keywordLine) && this.Name.Contains(keywordLine)) {
                if (Alteration.shapeKeywords.Contains(keywordLine)) {
                    if (Shape != "") {
                        Console.WriteLine("Article " + Name + " contains multiple Shapes: " + Shape + " and " + keywordLine);
                        Keywords.Add(keywordLine);
                    } else {
                        Shape = keywordLine;
                    }
                } else if (Alteration.surfaceKeywords.Contains(keywordLine)) {
                    if (Surface != "") {
                        Console.WriteLine("Article " + Name + " contains multiple Surfaces: " + Surface + " and " + keywordLine);
                        Keywords.Add(keywordLine);
                    } else {
                        Surface = keywordLine;
                    }
                } else {
                    Keywords.Add(keywordLine);
                }
                name = name.Remove(name.IndexOf(keywordLine), keywordLine.Length);
            }
            if (name.Contains(keywordLine) && this.Name.Contains(keywordLine)) {
                if (Alteration.shapeKeywords.Contains(keywordLine)) {
                    if (Shape != "") {
                        Console.WriteLine("Article " + Name + " contains multiple Shapes: " + Shape + " and " + keywordLine);
                        Keywords.Add(keywordLine);
                    } else {
                        Shape = keywordLine;
                    }
                } else if (Alteration.surfaceKeywords.Contains(keywordLine)) {
                    if (Surface != "") {
                        Console.WriteLine("Article " + Name + " contains multiple Surfaces: " + Surface + " and " + keywordLine);
                        Keywords.Add(keywordLine);
                    } else {
                        Surface = keywordLine;
                    }
                } else {
                    Keywords.Add(keywordLine);
                }
                name = name.Remove(name.IndexOf(keywordLine), keywordLine.Length);
            }
            if (name.Contains(keywordLine) && this.Name.Contains(keywordLine)) {
                Console.WriteLine("Article " + Name + " contains Keyword: " + keywordLine + " three Times");
            }
        }
        
        CheckFullNameCoverage();
    }
    private void CheckFullNameCoverage() {
        int nameLength = Name.Length;
        int keywordLength = Keywords.Sum(k => k.Length) + ToShape.Length + Shape.Length + Surface.Length;
        if (ToShape != ""){
            keywordLength += 2;
        };
        if (nameLength == keywordLength) {
            // Console.WriteLine($"Name {Name} is fully covered by keywords: {string.Join(", ", Keywords.Select(k => k))}");
        } else {
            Console.WriteLine($"Name {Name} is not fully covered by keywords. keywords: {string.Join(", ", Keywords.Select(k => k))}, surface: {Surface}, shape: {Shape}, toshape: {ToShape}");
        }
    }
}