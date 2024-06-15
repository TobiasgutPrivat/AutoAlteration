using System.Numerics;
using GBX.NET;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

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
        relativeOffset(coords);
        addRotation(pitchYawRoll);
        return this;
    }
    public Position move(Vec3 coords){
        relativeOffset(coords);
        return this;
    }
    public Position rotate(Vec3 pitchYawRoll){
        addRotation(pitchYawRoll);
        return this;
    }
    public void relativeOffset(Vec3 offset){
        Matrix4x4 rotationMatrix = Matrix4x4.CreateFromYawPitchRoll(pitchYawRoll.X, pitchYawRoll.Y, pitchYawRoll.Z);
        Vector3 offsetV3 = new Vector3(offset.X,offset.Y,offset.Z);
        Vector3 transformedOffset = Vector3.Transform(offsetV3, rotationMatrix);
        coords += new Vec3((float)transformedOffset[0], (float)transformedOffset[1], (float)transformedOffset[2]);
    }

    public Position subtractPosition(Position position){
        addRotation(-position.pitchYawRoll);
        relativeOffset(-position.coords);
        return this;
    }

    public void addRotation(Vec3 rotation) {
        pitchYawRoll = RotateRelative(pitchYawRoll, rotation);
    }

    public static Vec3 RotateRelative(Vec3 rotation, Vec3 byRotation){
        //Trackmania does like ZXY in https://dugas.ch/transform_viewer/index.html
        Matrix<double> rotationMatrix = createZXYMatrix(rotation);
        Matrix<double> byrotationMatrix = createZXYMatrix(byRotation);
                  
        rotationMatrix *= byrotationMatrix;

        return getEulerZXY(rotationMatrix);
    }

    public static Vec3 getEulerZXY(Matrix<double> rotationMatrix) {
        double extractedRoll = Math.Asin(rotationMatrix[2, 1]);
        double extractedPitch = Math.Atan2(-rotationMatrix[2, 0], rotationMatrix[2, 2]);
        double extractedYaw = Math.Atan2(-rotationMatrix[0, 1], rotationMatrix[1, 1]);
        return new Vec3((float)extractedYaw, (float)extractedPitch, (float)extractedRoll);
    }

    public static Matrix<double> createZXYMatrix(Vec3 rotation) {
        double Yaw = rotation.X;
        double Roll = rotation.Z;
        double Pitch = rotation.Y;

        // Rotation matrix for Yaw (Z-axis)
        Matrix<double> RotationZ = DenseMatrix.OfArray(new double[,] {
            { Math.Cos(Yaw), -Math.Sin(Yaw), 0},
            { Math.Sin(Yaw), Math.Cos(Yaw), 0},
            { 0, 0, 1},
        });
        Matrix<double> rotationMatrix = RotationZ;

        // Rotation matrix for Roll (X-axis)
        Matrix<double> RotationX = DenseMatrix.OfArray(new double[,] {
            { 1, 0, 0},
            { 0, Math.Cos(Roll), -Math.Sin(Roll)},
            { 0, Math.Sin(Roll), Math.Cos(Roll)},
        });
        rotationMatrix *= RotationX;

        // Rotation matrix for Pitch (Y-axis)
        Matrix<double> RotationY = DenseMatrix.OfArray(new double[,] {
            { Math.Cos(Pitch), 0, Math.Sin(Pitch)},
            { 0, 1, 0},
            { -Math.Sin(Pitch), 0, Math.Cos(Pitch)},
        });
        rotationMatrix *= RotationY;
        return rotationMatrix;
    }
}