using System.Numerics;
using GBX.NET;

class Position {
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
    public Position addPosition(Position position){
        if (position != null){
            addPosition(position.coords, position.pitchYawRoll);
        }
        return this;
    }
    public Position addPosition(Vec3 coords, Vec3 pitchYawRoll){
        this.coords += relativeOffset(this.pitchYawRoll, coords);
        this.pitchYawRoll = addRotation(this.pitchYawRoll, pitchYawRoll);
        return this;
    }
    public Position move(Vec3 coords){
        this.coords += relativeOffset(this.pitchYawRoll, coords);
        return this;
    }
    public Position rotate(Vec3 pitchYawRoll){
        this.pitchYawRoll = addRotation(this.pitchYawRoll, pitchYawRoll);
        return this;
    }
    public Position subtractPosition(Position position){
        subtractPosition(position.coords, position.pitchYawRoll);
        return this;
    }
    public Position subtractPosition(Vec3 coords, Vec3 pitchYawRoll){
        this.pitchYawRoll = addRotation(this.pitchYawRoll, -pitchYawRoll);
        this.coords -= relativeOffset(this.pitchYawRoll, coords);
        return this;
    }

    public static Vec3 relativeOffset(Vec3 pitchYawRoll, Vec3 offset){
        Matrix4x4 rotationMatrix = Matrix4x4.CreateFromYawPitchRoll(pitchYawRoll.X, pitchYawRoll.Y, pitchYawRoll.Z);
        Vector3 offsetV3 = new Vector3(offset.X,offset.Y,offset.Z);
        Vector3 transformedOffset = Vector3.Transform(offsetV3, rotationMatrix);
        return new Vec3(transformedOffset.X, transformedOffset.Y, transformedOffset.Z);
    }

    public static Vec3 addRotation(Vec3 currentRotation, Vec3 addRotation) {
        Matrix4x4 rotationMatrix = Matrix4x4.CreateRotationX(currentRotation.X) *
                                   Matrix4x4.CreateRotationY(currentRotation.Y) *
                                   Matrix4x4.CreateRotationZ(currentRotation.Z);
        Matrix4x4 addMatrix = Matrix4x4.CreateRotationX(addRotation.X) *
                                   Matrix4x4.CreateRotationY(addRotation.Y) *
                                   Matrix4x4.CreateRotationZ(addRotation.Z);

        Matrix4x4 totalMatrix = rotationMatrix * addMatrix;

        Vector3 newRotation = GetEulerAngles(totalMatrix);

        return new Vec3(newRotation.X, newRotation.Y, newRotation.Z);
    }
    public static Vector3 GetEulerAngles(this Matrix4x4 matrix)
    {
        // Extract the pitch, yaw, and roll angles from the rotation matrix
        float pitch = (float)Math.Asin(-matrix.M13);
        float roll = (float)Math.Atan2(matrix.M23, matrix.M33);
        float yaw = (float)Math.Atan2(matrix.M12, matrix.M11);

        return new Vector3(pitch, yaw, roll);
    }
}