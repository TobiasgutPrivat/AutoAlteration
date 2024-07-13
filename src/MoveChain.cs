using GBX.NET;

class MoveChain {
    public List<Move> moves = new();
    public MoveChain(){}
    public MoveChain Clone() {
        MoveChain clone = new();
        clone.moves.AddRange(moves);
        return clone;
    }
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
    public void Subtract(Position position,Article article){
        moves.Reverse();
        foreach(Move move in moves){
            switch(move.type){
                case MoveType.Move:
                    position.Move(-move.vector);
                    break;
                case MoveType.Rotate:
                    position.Rotate(-move.vector);//TODO order of angles might be wrong
                    break;
                case MoveType.RotateMid:
                    position.RotateMid(-move.vector, article);
                    break;
            }
        }
        moves.Reverse();
    }
    public MoveChain AddChain(MoveChain moveChain) {
        moves.AddRange(moveChain.moves);
        return this;
    }
    public MoveChain Move(float x, float y, float z) =>
        Move(new Vec3(x,y,z));

    public MoveChain Move(Vec3 vector) {
        moves.Add(new Move(MoveType.Move, vector));
        return this;
    }
    
    public MoveChain Rotate(float x, float y, float z) =>
        Rotate(new Vec3(x,y,z));

    public MoveChain Rotate(Vec3 vector) {
        moves.Add(new Move(MoveType.Rotate, vector));
        return this;
    }

    public MoveChain RotateMid(float x, float y, float z) =>
        RotateMid(new Vec3(x,y,z));

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