using System.Numerics;
using GBX.NET;

class Position{
    public static float PI = (float)Math.PI;
    public Vec3 coords;
    public Vec3 pitchYawRoll;

    public Position(){
        this.coords = Vec3.Zero;
        this.pitchYawRoll = Vec3.Zero;
    }
    public Position( Vec3 coords){
        this.coords = coords;
        this.pitchYawRoll = Vec3.Zero;
    }
    public Position( Vec3 coords, Vec3 pitchYawRoll){
        this.coords = coords;
        this.pitchYawRoll = pitchYawRoll;
    }
    public virtual Position addPosition(Position position){
        if (position != null){
            addPosition(position.coords, position.pitchYawRoll);
        }
        return this;
    }
    public virtual Position addPosition(Vec3 coords, Vec3 pitchYawRoll){
        this.coords += relativeOffset(this.pitchYawRoll, coords);
        this.pitchYawRoll += getRelativeRotation(this.pitchYawRoll, pitchYawRoll);
        return this;
    }
    public virtual Position move(Vec3 coords){
        this.coords += relativeOffset(this.pitchYawRoll, coords);
        return this;
    }
    public virtual Position rotate(Vec3 pitchYawRoll){
        this.pitchYawRoll += getRelativeRotation(this.pitchYawRoll, pitchYawRoll);
        return this;
    }
    public virtual Position subtractPosition(Position position){
        subtractPosition(position.coords, position.pitchYawRoll);
        return this;
    }
    public virtual Position subtractPosition(Vec3 coords, Vec3 pitchYawRoll){
        this.pitchYawRoll -= getRelativeRotation(this.pitchYawRoll, pitchYawRoll);
        this.coords -= relativeOffset(this.pitchYawRoll, coords);
        return this;
    }

    public static Vec3 relativeOffset(Vec3 pitchYawRoll, Vec3 offset){
        Matrix4x4 rotationMatrix = Matrix4x4.CreateFromYawPitchRoll(pitchYawRoll.X, pitchYawRoll.Y, pitchYawRoll.Z);
        Vector3 offsetV3 = new Vector3(offset.X,offset.Y,offset.Z);
        Vector3 transformedOffset = Vector3.Transform(offsetV3, rotationMatrix);
        return new Vec3(transformedOffset.X, transformedOffset.Y, transformedOffset.Z);
    }

    public static Vec3 getRelativeRotation(Vec3 currentRotation, Vec3 targetRotation) {
        Matrix4x4 currentRotationMatrix = Matrix4x4.CreateFromYawPitchRoll(currentRotation.X, currentRotation.Y, currentRotation.Z);
        Matrix4x4 inverseRotationMatrix = Matrix4x4.Transpose(currentRotationMatrix);

        Vector3 targetRotationV3 = new Vector3(targetRotation.X, targetRotation.Y, targetRotation.Z);
        Vector3 relativeRotationV3 = Vector3.Transform(targetRotationV3, inverseRotationMatrix);

        return new Vec3(relativeRotationV3.X, relativeRotationV3.Y, relativeRotationV3.Z);
    }
}