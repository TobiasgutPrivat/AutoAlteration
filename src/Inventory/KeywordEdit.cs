using GBX.NET.Engines.Game;

public class KeywordEdit {
    public Dictionary<Article,Article?> articles = []; //Keys: cloned articles, Values: aligned articles (from org inventory)

    public KeywordEdit(List<Article> articles) {
        articles.ForEach(a => this.articles.Add(a,null));
    }

    public KeywordEdit(Dictionary<Article,Article?> articles) {
        this.articles = articles;
    }

    #region Edit
    public KeywordEdit AddKeyword(SList<string> keywords) {
        articles.Keys.ToList().ForEach(a => keywords.ToList().ForEach(k => a.Keywords.Add(k)));
        return this;
    }
    
    public KeywordEdit RemoveKeyword(SList<string> keywords) {
        articles.Keys.ToList().ForEach(a => keywords.ToList().ForEach(k => a.Keywords.Remove(k)));
        return this;
    }

    public KeywordEdit AddToShape(SList<string> toShape) {
        articles.Keys.ToList().ForEach(a => toShape.ToList().ForEach(s => a.ToShapes.Add(s)));
        return this;
    }
    public KeywordEdit RemoveToShape(SList<string> toShape) {
        articles.Keys.ToList().ForEach(a => toShape.ToList().ForEach(s => a.ToShapes.Remove(s)));
        return this;
    }

    public KeywordEdit SetChain(MoveChain moveChain) {
        articles.Keys.ToList().ForEach(a => a.MoveChain = moveChain.Clone());
        return this;
    }

    public KeywordEdit AddChain(MoveChain moveChain) {
        articles.Keys.ToList().ForEach(a => a.MoveChain.AddChain(moveChain));
        return this;
    }

    public KeywordEdit Width(int width) {
        articles.Keys.ToList().ForEach(a => a.Width = width);
       
        return this;
    }
    public KeywordEdit Length(int length) {
        articles.Keys.ToList().ForEach(a => a.Length = length);
        return this;
    }
    #endregion

    public void PlaceRelative(Map map,MoveChain ?moveChain = null, Predicate<CGameCtnBlock>? blockCondition = null){
        Align();
        articles.ToList().ForEach( a => {
            if (a.Value != null) {
                map.PlaceRelative(a.Key, a.Value, moveChain, blockCondition);
            }
        });
    }

    public void Replace(Map map,MoveChain ?moveChain = null, Predicate<CGameCtnBlock>? blockCondition = null){
        Align();
        articles.ToList().ForEach( a => {
            if (a.Value != null) {
                map.Replace(a.Key, a.Value, moveChain, blockCondition);
            }
        });
    }

    public void ReplaceWithRandom(Map map,List<string> addKeywords,MoveChain ?moveChain = null){
        articles.Keys.ToList().ForEach( a => {
            map.ReplaceWithRandom(a, Alteration.inventory.AlignMultiple(a, addKeywords), moveChain);
        });
    }

    public void PlaceRelativeWithRandom(Map map,List<string> addKeywords,MoveChain ?moveChain = null){
        articles.Keys.ToList().ForEach( a => {
            map.PlaceRelativeWithRandom(a, Alteration.inventory.AlignMultiple(a, addKeywords), moveChain);
        });
    }

    public KeywordEdit Align() {
        articles.ToList().ForEach( a => {
            articles[a.Key] = Alteration.inventory.AlignArticle(a.Key);
        });
        //only keep aligned articles
        articles = articles.Where(a => a.Value != null).ToDictionary(a => a.Key, a => a.Value);
        return this;
    }

    public Inventory getEdited() {
        return new Inventory(articles.Keys.ToList());
    }

    public Inventory getAligned() {
        return new Inventory(articles.Values.Where(a => a != null).ToList() as List<Article>);
    }

    public Inventory getOriginal() {
        return new Inventory(articles.Keys.Select(a => Alteration.inventory.GetArticle(a.Name)).ToList());
    }

    public KeywordEdit Add(KeywordEdit other) {
        Dictionary<Article,Article?> newEdit = new();
        articles.ToList().ForEach(a => newEdit.Add(a.Key,a.Value));
        other.articles.ToList().ForEach(a => {
            if (articles.TryGetValue(a.Key, out Article? value)) {
                if (value != null) {
                    Console.WriteLine("Article already aligned");
                }
                articles[a.Key] = a.Value;
            } else {
                articles.Add(a.Key,a.Value);
            }
        });
        return new KeywordEdit(newEdit);
    }

    public KeywordEdit Print() {
        articles.ToList().ForEach(article => {
            Console.WriteLine(article.Key.Name + ": " + article.Key.KeywordString() + 
            ((article.Value is not null) ? ("\nMatches: " + article.Value?.Name + ": " + article.Value?.KeywordString()) : "")
            );
        });
        return this;
    }
}