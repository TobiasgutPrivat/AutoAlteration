
using GBX.NET;

public class Move(Vec3 vector)
{
    public Vec3 vector = vector;

    public virtual void Apply(Position position, Article article){
        position.RelativeOffset(vector);
    }
}

public class Rotate(Vec3 vector): Move(vector){
    public override void Apply(Position position, Article article)
    {
        position.AddRotation(vector);
    }
}

public class RotateMid(Vec3 vector): Move(vector){
    public override void Apply(Position position, Article article)
    {
        position.RelativeOffset(new Vec3(article.Width * 16, article.Height * 4, article.Length * 16));
        position.AddRotation(vector);
        position.RelativeOffset(new Vec3(-article.Width * 16, -article.Height * 4, -article.Length * 16));
    }
}

public class RotateCenter(Vec3 vector): Move(vector){
    public override void Apply(Position position, Article article){
        Vec3 Offset = new(768 - position.coords.X, 120 - position.coords.Y, 768 - position.coords.Z);
        Vec3 rotation = position.pitchYawRoll;
        position.Rotate(-rotation);
        position.RelativeOffset(Offset);
        position.AddRotation(vector);
        position.RelativeOffset(-Offset);
        position.Rotate(rotation);
    }
}