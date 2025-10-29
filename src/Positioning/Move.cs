
using System.Runtime.CompilerServices;
using GBX.NET;

public abstract class Move
{
    public Vec3 vector;
    public abstract void Apply(Position position, Article article);
}

public class Offset: Move{
    public Offset(Vec3 vector){
        this.vector = vector;
    }
    public Offset(float x, float y, float z) {
        this.vector = new Vec3(x,y,z);
    }
    public override void Apply(Position position, Article article){
        position.Move(vector);
    }
}

public class Rotate: Move{
    public Rotate(Vec3 vector)
    {
        this.vector = vector;
    }
    public Rotate(float x, float y, float z) {
        this.vector = new Vec3(x,y,z);
    }
    public override void Apply(Position position, Article? article = null)
    {
        position.Rotate(vector);
    }

    public static Rotate operator -(Rotate move)
    {
        return new Rotate(-move.vector);
    }
}

public class RotateMid: Move{
    public RotateMid(Vec3 vector)
    {
        this.vector = vector;
    }
    public RotateMid(float x, float y, float z) {
        this.vector = new Vec3(x,y,z);
    }

    public override void Apply(Position position, Article article)
    {
        position.Move(new Vec3(article.Width * 16, article.Height * 4, article.Length * 16));
        position.Rotate(vector);
        position.Move(new Vec3(-article.Width * 16, -article.Height * 4, -article.Length * 16));
    }
}

public class RotateCenter: Move{
    public RotateCenter(Vec3 vector)
    {
        this.vector = vector;
    }
    public RotateCenter(float x, float y, float z) {
        this.vector = new Vec3(x,y,z);
    }
    public override void Apply(Position position, Article article){
        Vec3 Offset = new(768 - position.coords.X, 120 - position.coords.Y, 768 - position.coords.Z);
        Vec3 rotation = position.pitchYawRoll;
        // position.Rotate(-rotation);
        position.pitchYawRoll = new Vec3(0, 0, 0);//not really clean solution
        position.Move(Offset);
        position.Rotate(vector);
        position.Move(-Offset);
        position.Rotate(rotation);
    }
}