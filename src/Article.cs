class Article {
    public string Name;
    public List<string> Keywords = new List<string>();
    public static char[] systemCharacters = new char[] { '&', '|', '!', '(', ')' };

    public Dictionary<string,bool> cacheFilter = new Dictionary<string, bool>();

    public Article(string name){ 
        this.Name = name;
    }
    public Article(string name,string[] keywordLines){ 
        this.Name = name;
        loadKeywords(keywordLines);
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
        if (Name == "PlatformIceSlope2Start"){
            //Debug
        }
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

    public void loadKeywords(string[] keywordLines) {
        string name = this.Name;
        foreach (var keywordLine in keywordLines) {//Max 2 of one keyword
            if (name.Contains(keywordLine) && this.Name.Contains(keywordLine)) {
                Keywords.Add(keywordLine);
                name = name.Remove(name.IndexOf(keywordLine), keywordLine.Length);
                // name = name.Replace(keywordLine, "");
            }
            if (name.Contains(keywordLine) && this.Name.Contains(keywordLine)) {
                Keywords.Add(keywordLine);
                name = name.Remove(name.IndexOf(keywordLine), keywordLine.Length);
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