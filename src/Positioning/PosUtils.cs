using GBX.NET;

public class PosUtils{
    public const float PI = (float)Math.PI;

    public static MoveChain Move(float x, float y, float z) =>
        Move(new Vec3(x,y,z));

    public static MoveChain Move(Vec3 vector) {
        MoveChain moveChain = new();
        moveChain.Move(vector);
        return moveChain;
    }

    public static MoveChain Rotate(float x, float y, float z) =>
        Rotate(new Vec3(x,y,z));

    public static MoveChain Rotate(Vec3 vector) {
        MoveChain moveChain = new();
        moveChain.Rotate(vector);
        return moveChain;
    }

    public static MoveChain RotateMid(float x, float y, float z) =>
        RotateMid(new Vec3(x,y,z));

    public static MoveChain RotateMid(Vec3 vector) {
        MoveChain moveChain = new();
        moveChain.RotateMid(vector);
        return moveChain;
    }
    public static MoveChain RotateCenter(float x, float y, float z) =>
        RotateCenter(new Vec3(x,y,z));

    public static MoveChain RotateCenter(Vec3 vector) {
        MoveChain moveChain = new();
        moveChain.RotateCenter(vector);
        return moveChain;
    } 
}