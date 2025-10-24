
using GBX.NET;

public abstract class Move(Vec3 vector)
{
    public Vec3 vector = vector;

    public abstract void Apply(Position position, Article article);
}

public class Offset(Vec3 vector): Move(vector){
    public override void Apply(Position position, Article article){
        position.Move(vector);
    }
}

public class Rotate(Vec3 vector): Move(vector){
    public override void Apply(Position position, Article? article = null)
    {
        position.Rotate(vector);
    }
}

public class RotateMid(Vec3 vector): Move(vector){
    public override void Apply(Position position, Article article)
    {
        position.Move(new Vec3(article.Width * 16, article.Height * 4, article.Length * 16));
        position.Rotate(vector);
        position.Move(new Vec3(-article.Width * 16, -article.Height * 4, -article.Length * 16));
    }
}

public class RotateCenter(Vec3 vector): Move(vector){
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