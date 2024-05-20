class Article {
    public string Name;
    public Keyword[] Keywords;

    public Article(string name,string[] keywordLines){ 
        this.Name = name;
        loadKeywords(keywordLines);
    }

    public void loadKeywords(string[] keywordLines) {
        string name = this.Name;
        List<Keyword> keywords = new List<Keyword>();
        foreach (var keywordLine in keywordLines) {//Max 2 of one keyword
            if (name.Contains(keywordLine) && this.Name.Contains(keywordLine)) {
                var keyword = new Keyword { Name = keywordLine };
                keywords.Add(keyword);
                name = name.Remove(name.IndexOf(keywordLine), keywordLine.Length);
                // name = name.Replace(keywordLine, "");
            }
            if (name.Contains(keywordLine) && this.Name.Contains(keywordLine)) {
                var keyword = new Keyword { Name = keywordLine };
                keywords.Add(keyword);
                name = name.Remove(name.IndexOf(keywordLine), keywordLine.Length);
            }
        }
        Keywords = keywords.ToArray();
        CheckFullNameCoverage();
    }
    private void CheckFullNameCoverage() {
        int nameLength = Name.Length;
        int keywordLength = Keywords.Sum(k => k.Name.Length);
        if (nameLength == keywordLength) {
            // Console.WriteLine($"Name {Name} is fully covered by keywords: {string.Join(", ", Keywords.Select(k => k.Name))}");
        } else {
            Console.WriteLine($"Name {Name} is not fully covered by keywords. Remaining keywords: {string.Join(", ", Keywords.Select(k => k.Name))}");
        }
    }
}