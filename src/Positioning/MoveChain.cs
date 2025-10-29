using GBX.NET;

public class MoveChain : List<Move> {
    public MoveChain() { }
    
    public MoveChain Clone() {
        MoveChain clone = new();
        clone.AddRange(this);
        return clone;
    }

    public void Apply(Position position,Article article){
        foreach(Move move in this){
            move.Apply(position,article);
        }
    }
    public void Subtract(Position position,Article article){
        this.Reverse();
        foreach(Move move in this){
            move.vector = -move.vector; //TODO doesnt fully work for rotations
            move.Apply(position,article);
            move.vector = -move.vector;
        }
        this.Reverse();
    }
    
    public MoveChain AddChain(MoveChain moveChain) {
        this.AddRange(moveChain);
        return this;
    }
    
    #region Constructors
    public MoveChain Move(float x, float y, float z) =>
        Move(new Vec3(x,y,z));

    public MoveChain Move(Vec3 vector) {
        this.Add(new Offset(vector));
        return this;
    }
    
    public MoveChain Rotate(float x, float y, float z) =>
        Rotate(new Vec3(x,y,z));

    public MoveChain Rotate(Vec3 vector) {
        this.Add(new Rotate(vector));
        return this;
    }

    public MoveChain RotateMid(float x, float y, float z) =>
        RotateMid(new Vec3(x,y,z));

    public MoveChain RotateMid(Vec3 vector) {
        this.Add(new RotateMid( vector));
        return this;
    }
    public MoveChain RotateCenter(float x, float y, float z) =>
        RotateCenter(new Vec3(x,y,z));

    public MoveChain RotateCenter(Vec3 vector) {
        this.Add(new RotateCenter(vector));
        return this;
    }
    #endregion
}