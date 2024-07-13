using GBX.NET;

class MoveChain {
    public List<Move> moves = new();
    public MoveChain(){}
    public void Apply(Position position,Article article){
        foreach(Move move in moves){
            switch(move.type){
                case MoveType.Move:
                    position.Move(move.vector);
                    break;
                case MoveType.Rotate:
                    position.Rotate(move.vector);
                    break;
                case MoveType.RotateMid:
                    position.RotateMid(move.vector, article);
                    break;
            }
        }
    }
    public MoveChain Move(Vec3 vector) {
        moves.Add(new Move(MoveType.Move, vector));
        return this;
    }
    
    public MoveChain Rotate(Vec3 vector) {
        moves.Add(new Move(MoveType.Rotate, vector));
        return this;
    }

    public MoveChain RotateMid(Vec3 vector) {
        moves.Add(new Move(MoveType.RotateMid, vector));
        return this;
    }
}
class Move{
    public MoveType type;
    public Vec3 vector;
    public Move(MoveType type, Vec3 vector) {
        this.type = type;
        this.vector = vector;
    }
}

enum MoveType{
    Move,
    Rotate,
    RotateMid
}