public class MoveChain : List<Move> {
    public MoveChain() { }

    public void Apply(Position position,Article article){
        foreach(Move move in this){
            move.Apply(position,article);
        }
    }

    public void Subtract(Position position, Article article)
    {
        Reverse();
        foreach (Move move in this)
        {
            move.vector = -move.vector; //TODO doesnt fully work for rotations
            move.Apply(position, article);
            move.vector = -move.vector;
        }
        Reverse();
    }

    public static implicit operator MoveChain(Move move) => [move];
}