class Article {
    public string name;
    public Position position = new Position();
    public BlockType type;
    public List<string> keywords = new List<string>();
    public string shape = "";
    public string toShape = "";
    public string surface = "";
    public int width = 1;
    public int length = 1;

    public static char[] systemCharacters = new char[] { '&', '|', '!', '(', ')' };

    public Dictionary<string,bool> cacheFilter = new Dictionary<string, bool>();

    public Article() { }
    public Article(string name,BlockType type,List<string> keywords,string shape,string toShape,string surface,Position position = null){
        this.name = name;
        this.type = type;
        this.keywords = keywords;
        this.shape = shape;
        this.toShape = toShape;
        this.surface = surface;
        this.position.addPosition(position);
    }
    
    public Article(string name,BlockType type){
        this.name = name;
        loadKeywords();
        this.type = type;
    }

    public Article CloneArticle() {
        Article clonedArticle = new Article();
        clonedArticle.name = this.name;
        clonedArticle.position = new Position(this.position.coords, this.position.pitchYawRoll);
        clonedArticle.length = this.length;
        clonedArticle.width = this.width;
        clonedArticle.type = this.type;
        clonedArticle.keywords = new List<string>(this.keywords);
        clonedArticle.shape = this.shape;
        clonedArticle.toShape = this.toShape;
        clonedArticle.surface = this.surface;
        return clonedArticle;
    }

    public bool hasKeyword(string keyword) {
        if (Alteration.shapeKeywords.Contains(keyword)) {
            return shape == keyword || toShape == keyword;
        } else if (Alteration.surfaceKeywords.Contains(keyword)) {
            return surface == keyword;
        } else{
            return keywords.Any(k => k == keyword);
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
        string name = this.name;
        //toshape
        if(name.Contains("To")){
            int toPos = name.IndexOf("To");
            string[] toshapes = Alteration.shapeKeywords.Where(x => name.Substring(toPos + 2).Contains(x)).ToArray();
            if (toshapes.Count() >= 1){
                toShape = toshapes.First();
                String ToString = name.Substring(toPos + 2);
                name = name.Substring(0,toPos) + ToString.Remove(ToString.IndexOf(toShape), toShape.Length);
            }
            toshapes = Alteration.shapeKeywords.Where(x => name.Substring(toPos).Contains(x)).ToArray();
            if (toshapes.Count() > 1) {
                Console.WriteLine("more than 1 To-shape: " + name);
            }
        }
        
        //Keywords
        foreach (var keywordLine in Alteration.Keywords) {
            if (name.Contains(keywordLine) && this.name.Contains(keywordLine)) {
                if (Alteration.shapeKeywords.Contains(keywordLine)) {
                    if (shape != "") {
                        Console.WriteLine("Article " + this.name + " contains multiple Shapes: " + shape + " and " + keywordLine);
                        keywords.Add(keywordLine);
                    } else {
                        shape = keywordLine;
                    }
                } else if (Alteration.surfaceKeywords.Contains(keywordLine)) {
                    if (surface != "") {
                        Console.WriteLine("Article " + this.name + " contains multiple Surfaces: " + surface + " and " + keywordLine);
                        keywords.Add(keywordLine);
                    } else {
                        surface = keywordLine;
                    }
                } else {
                    keywords.Add(keywordLine);
                }
                name = name.Remove(name.IndexOf(keywordLine), keywordLine.Length);
            }
            if (name.Contains(keywordLine) && this.name.Contains(keywordLine)) {
                if (Alteration.shapeKeywords.Contains(keywordLine)) {
                    if (shape != "") {
                        Console.WriteLine("Article " + this.name + " contains multiple Shapes: " + shape + " and " + keywordLine);
                        keywords.Add(keywordLine);
                    } else {
                        shape = keywordLine;
                    }
                } else if (Alteration.surfaceKeywords.Contains(keywordLine)) {
                    if (surface != "") {
                        Console.WriteLine("Article " + this.name + " contains multiple Surfaces: " + surface + " and " + keywordLine);
                        keywords.Add(keywordLine);
                    } else {
                        surface = keywordLine;
                    }
                } else {
                    keywords.Add(keywordLine);
                }
                name = name.Remove(name.IndexOf(keywordLine), keywordLine.Length);
            }
            if (name.Contains(keywordLine) && this.name.Contains(keywordLine)) {
                Console.WriteLine("Article " + this.name + " contains Keyword: " + keywordLine + " three Times");
            }
        }
        
        CheckFullNameCoverage();
    }
    private void CheckFullNameCoverage() {
        int nameLength = name.Length;
        int keywordLength = keywords.Sum(k => k.Length) + toShape.Length + shape.Length + surface.Length;
        if (toShape != ""){
            keywordLength += 2;
        };
        if (nameLength == keywordLength) {
            // Console.WriteLine($"Name {Name} is fully covered by keywords: {string.Join(", ", Keywords.Select(k => k))}");
        } else {
            Console.WriteLine($"Name {name} is not fully covered by keywords. keywords: {string.Join(", ", keywords.Select(k => k))}, surface: {surface}, shape: {shape}, toshape: {toShape}");
        }
    }
}