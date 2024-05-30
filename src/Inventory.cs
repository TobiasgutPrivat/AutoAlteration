using Newtonsoft.Json;
class Inventory {
    public List<Article> articles = new List<Article>();

    public Inventory() {}
    public Inventory(List<Article> articles) {this.articles = articles;}
    
    public Inventory select(string keywordFilter) =>
        new Inventory(GetArticles(keywordFilter));

    public Inventory selectString(string nameString) =>
        new Inventory(articles.Where(a => a.Name.Contains(nameString)).ToList());

    public string[] names() =>
        articles.Select(a => a.Name).ToArray();

    public List<Article> GetArticles(string keywordFilter) =>
        articles.Where(a => a.match(keywordFilter)).ToList();

    public List<Article> GetArticles(string[] keywords) =>
        articles.Where(a => keywords.All(k => a.Keywords.Contains(k))).ToList();

    public Article ArticleReplaceKeyword(Article article, string addKeyword, string removeKeyword) =>
        ArticleReplaceKeyword(article, new[] {addKeyword}, new[] {removeKeyword});
    
    public Article ArticleReplaceKeyword(Article article, string[] addKeywords, string[] removeKeywords) {
        List<string> keywords = article.Keywords.ToList();
        keywords.AddRange(addKeywords);
        keywords.RemoveAll(k => removeKeywords.Contains(k));
        List<Article> newArticle = GetArticles(keywords.ToArray());
        if (newArticle.Count() > 1) {
            Console.WriteLine(article.Name + ": More than one found article with keywords: " + string.Join(", ", keywords));
            Console.WriteLine("Articles: " + string.Join(", ", newArticle.Select(a => a.Name).ToArray()));
            return null;
        }
        if (newArticle.Count() == 0) {
            Console.WriteLine(article.Name + ": No found article with keywords: " + string.Join(", ", keywords));
            return null;
        }
        return newArticle.First();
    }

    public bool hasArticle(string name) {
        return articles.Any(article => article.Name == name);
    }
    
    //Development Section ------------------------------------------------------------------------------------------------------
    public void checkDuplicates(){
        articles.ForEach(article => {
            List<Article> tempArticles = JsonConvert.DeserializeObject<List<Article>>(JsonConvert.SerializeObject(articles));
            tempArticles.ForEach(article2 => {
                if (article2.Name != article.Name){
                    bool match = true;

                    article.Keywords.ForEach(k => {
                        if (article2.Keywords.Contains(k)){
                            article2.Keywords.Remove(k);
                        } else {
                            match = false;
                        }
                    });
                    if (article2.Keywords.Count > 0) {
                        match = false;
                    }
                    if (article.Shape != article2.Shape) {match = false;}
                    if (article.ToShape != article2.ToShape) {match = false;}
                    if (article.Surface != article2.Surface) {match = false;}
                    if (match) {
                        Console.WriteLine(article.Name + " matches " + article2.Name);
                    }
                };
            });
            // article.Keywords.ForEach(k => {
            //    if (article.Keywords.Where(k2 => k == k2).Count() > 1) {
            //        Console.WriteLine(article.Name + " contains " + k + " multiple Times");
            //    } 
            // });
        });
    }
    public void checkKeywords(){
        articles.ForEach(article => {
            if (GetArticles(article.Keywords.ToArray()).Where(a => a.Keywords.Count == article.Keywords.Count).Count() > 1 ) {
                Console.WriteLine(article.Name + ": More than one found article with keywords: " + string.Join(", ", article.Keywords) + "\nFound Articles: " + string.Join(", ", GetArticles(article.Keywords.ToArray()).Where(a => a.Keywords.Count == article.Keywords.Count).Select(a => a.Name).ToArray()));
            }
        });
        List<string> Keywords = File.ReadAllLines("C:/Users/tgu/OneDrive - This AG/Dokumente/Privat/AutoAlteration/src/Configuration/Keywords.txt").ToList();
        Keywords.ForEach(k => {
            int count = 0;
            foreach (Article article in articles)
            {
                if (article.Keywords.Contains(k))
                {
                    count++;
                }
            }
            Console.WriteLine($"'{k}' count: {count}");
        });
        articles.ForEach(article => {
            article.Keywords.ForEach(keyword => {
                if (Keywords.Contains(keyword)) {
                    Keywords.RemoveAll(k => k == keyword);
                }
            });
        });
        Console.WriteLine("Keywords left: " + string.Join(", ", Keywords));
    }

    public void changeKeywords(string[] removeKeyword, string[] addKeyword) {
        articles.ForEach(article => {
            article.Keywords.AddRange(addKeyword);
            removeKeyword.ToList().ForEach(k => {
                if (!article.Keywords.Remove(k)){
                    Console.WriteLine(article.Name + ": Keyword " + k + " not found");
                }
            });
        });
    }
    public void print() {
        articles.ForEach(article => {
            Console.WriteLine(article.Name + ": " + string.Join(", ", article.Keywords));
        });
    }
}