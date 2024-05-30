class Article {
    public string Name;
    public BlockChange blockChange;
    public BlockType Type;
    public List<string> Keywords = new List<string>();
    public string Shape;//opt
    public string ToShape;//opt
    public string Surface;//opt

    public static char[] systemCharacters = new char[] { '&', '|', '!', '(', ')' };

    public Dictionary<string,bool> cacheFilter = new Dictionary<string, bool>();

    public Article() { }
    public Article(string name,BlockType type,List<string> keywords,BlockChange blockChange){ 
        this.Name = name;
        this.Type = type;
        this.Keywords = keywords;
        this.blockChange = blockChange;
    }
    public Article(string name){ 
        this.Name = name;
        blockChange = null;
        loadKeywords();
    }

    public bool hasKeyword(string keyword) =>
        Keywords.Any(k => k == keyword);

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
        //toshape
        if(name.Contains("To")){
            string[] toshapes = Alteration.shapeKeywords.Where(x => name.Substring(name.IndexOf("To") + 2).Contains(x)).ToArray();
            if (toshapes.Count() != 1) {
                Console.WriteLine("invalid to shape: " + name);
            } else {
                ToShape = toshapes.First();
                name = name.Remove(name.IndexOf("To"), toshapes.First().Length + 2);
            }
        }   
        //shape
        string[] shapes = Alteration.shapeKeywords.Where(x => name.Contains(x)).ToArray();
        if (shapes.Count() > 1) {
            Console.WriteLine("to many shapes: " + name);
        } else {
            Shape = shapes.First();
            name = name.Remove(name.IndexOf(shapes.First()), shapes.First().Length);
        }
        
        //shape
        string[] surface = Alteration.shapeKeywords.Where(x => name.Contains(x)).ToArray();
        if (shapes.Count() > 1) {
            Console.WriteLine("to many surfaces: " + name);
        } else {
            Shape = surface.First();
            name = name.Remove(name.IndexOf(surface.First()), surface.First().Length);
        }
        
        //Keywords
        foreach (var keywordLine in Alteration.Keywords) {
            if (name.Contains(keywordLine) && this.Name.Contains(keywordLine)) {
                Keywords.Add(keywordLine);
                name = name.Remove(name.IndexOf(keywordLine), keywordLine.Length);
            }
            if (name.Contains(keywordLine) && this.Name.Contains(keywordLine)) {
                Console.WriteLine("Article name contains shape: " + keywordLine + " multiple Times");
            }
        }
        
        CheckFullNameCoverage();
    }
    private void CheckFullNameCoverage() {
        int nameLength = Name.Length;
        int keywordLength = Keywords.Sum(k => k.Length);
        if (nameLength == keywordLength) {
            // Console.WriteLine($"Name {Name} is fully covered by keywords: {string.Join(", ", Keywords.Select(k => k))}");
        } else {
            Console.WriteLine($"Name {Name} is not fully covered by keywords. Remaining keywords: {string.Join(", ", Keywords.Select(k => k))}");
        }
    }
}