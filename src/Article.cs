class Article {
    public string Name;
    public List<string> Keywords = new List<string>();

    public Article(string name){ 
        this.Name = name;
    }
    public Article(string name,string[] keywordLines){ 
        this.Name = name;
        loadKeywords(keywordLines);
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