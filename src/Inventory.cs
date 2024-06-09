using Newtonsoft.Json;
class Inventory {
    public List<Article> articles = new List<Article>();

    public Inventory() {}
    public Inventory(List<Article> articles) {this.articles = articles;}
    
    public Inventory select(string keywordFilter) =>
        new Inventory(GetArticles(keywordFilter));
    public Inventory select(BlockType blockType) =>
        new Inventory(articles.Where(a => a.type == blockType).ToList());

    public Inventory selectString(string nameString) =>
        new Inventory(articles.Where(a => a.name.Contains(nameString)).ToList());

    public string[] names() =>
        articles.Select(a => a.name).ToArray();

    public Article GetArticle(string name) =>
        articles.Where(a => a.name == name).First();

    public List<Article> GetArticles(string keywordFilter) =>
        articles.Where(a => a.match(keywordFilter)).ToList();

    public List<Article> GetArticles(string[] keywords) =>
        articles.Where(a => keywords.All(k => a.keywords.Contains(k))).ToList();

    public Article alignArticle(Article article) {
        List<Article> temparticles = articles.Where(a => article.keywords.All(k => a.keywords.Contains(k)) && a.keywords.Count == article.keywords.Count && article.surface == a.surface && article.shape == a.shape && article.toShape == a.toShape).ToList();
        if (temparticles.Count > 1) {
            Console.WriteLine("More than one found article with keywords: " + string.Join(", ", string.Join(", ", article.keywords),article.surface,article.shape,article.toShape) + "\nFound Articles: " + string.Join(", ", temparticles.Select(a => a.name).ToArray()));
            return null;
        } else if (temparticles.Count == 1) {
            return temparticles.First();
        }
        return null;
    }

    public KeywordEdit add(string Keyword) =>
        edit().add(Keyword);
    public KeywordEdit add(string[] Keyword) =>
        edit().add(Keyword);

    public KeywordEdit remove(string Keyword) =>
        edit().remove(Keyword);
    public KeywordEdit remove(string[] Keyword) =>
        edit().remove(Keyword);

    public KeywordEdit surface(string Keyword) =>
        edit().surface(Keyword);

    public KeywordEdit shape(string Keyword) =>
        edit().shape(Keyword);

    public KeywordEdit toShape(string Keyword) =>
        edit().toShape(Keyword);
    public KeywordEdit changePosition(Position position) =>
        edit().changePosition(position);
    public KeywordEdit posCorrection(PosCorection posCorection) =>
        edit().posCorrection(posCorection);

    public KeywordEdit edit(){
        List<Article> articleClone = articles.Select(a => a.CloneArticle()).ToList();//JsonConvert.DeserializeObject<List<Article>>(JsonConvert.SerializeObject(articles))
        return new KeywordEdit(articleClone);
    }

    public bool hasArticle(string name) {
        return articles.Any(article => article.name == name);
    }


    //Development Section ------------------------------------------------------------------------------------------------------
    public Inventory addArticles(List<Article> newArticles) {
        if (!Alteration.devMode){
            Console.WriteLine("Adding Articles only available in devMode");
        } else {
            articles.AddRange(newArticles);
        }
        return this;
    }
    public Inventory addArticles(KeywordEdit keywordEdit) {
        if (!Alteration.devMode){
            Console.WriteLine("Adding Articles only available in devMode");
        } else {
            articles.AddRange(keywordEdit.articles);
        }
        return this;
    }
    public KeywordEdit editOriginal(){
        if (!Alteration.devMode){
            Console.WriteLine("Edit Original only available in devMode");
            return null;
        } else {
            articles.ForEach(x => x.cacheFilter.Clear());
            return new KeywordEdit(articles);
        }
    }

    public void checkDuplicates(){
        articles.ForEach(article => {
            List<Article> tempArticles = articles.Select(a => a.CloneArticle()).ToList();
            tempArticles.ForEach(article2 => {
                if (article2.name != article.name){
                    bool match = true;

                    article.keywords.ForEach(k => {
                        if (article2.keywords.Contains(k)){
                            article2.keywords.Remove(k);
                        } else {
                            match = false;
                        }
                    });
                    if (article2.keywords.Count > 0) {
                        match = false;
                    }
                    if (article.shape != article2.shape) {match = false;}
                    if (article.toShape != article2.toShape) {match = false;}
                    if (article.surface != article2.surface) {match = false;}
                    if (match) {
                        Console.WriteLine(article.name + " matches " + article2.name);
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
    // public void checkKeywords(){
    //     articles.ForEach(article => {
    //         if (GetArticles(article.Keywords.ToArray()).Where(a => a.Keywords.Count == article.Keywords.Count).Count() > 1 ) {
    //             Console.WriteLine(article.Name + ": More than one found article with keywords: " + string.Join(", ", article.Keywords) + "\nFound Articles: " + string.Join(", ", GetArticles(article.Keywords.ToArray()).Where(a => a.Keywords.Count == article.Keywords.Count).Select(a => a.Name).ToArray()));
    //         }
    //     });
    //     List<string> Keywords = File.ReadAllLines("C:/Users/tgu/OneDrive - This AG/Dokumente/Privat/AutoAlteration/src/Configuration/Keywords.txt").ToList();
    //     Keywords.ForEach(k => {
    //         int count = 0;
    //         foreach (Article article in articles)
    //         {
    //             if (article.Keywords.Contains(k))
    //             {
    //                 count++;
    //             }
    //         }
    //         Console.WriteLine($"'{k}' count: {count}");
    //     });
    //     articles.ForEach(article => {
    //         article.Keywords.ForEach(keyword => {
    //             if (Keywords.Contains(keyword)) {
    //                 Keywords.RemoveAll(k => k == keyword);
    //             }
    //         });
    //     });
    //     Console.WriteLine("Unused Keywords: " + string.Join(", ", Keywords));
    // }

    public Inventory print() {
        articles.ForEach(article => {
            Console.WriteLine(article.name + ": " + string.Join(", ", string.Join(", ", article.keywords),article.surface,article.shape,article.toShape,article.posCorection));
        });
        return this;
    }
}