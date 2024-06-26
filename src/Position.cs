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
    public Position clone(){
        return new Position(coords, pitchYawRoll);
    }
    public void relativeOffset(Vec3 offset){
        double Yaw = pitchYawRoll.X;
        double Roll = pitchYawRoll.Z;
        double Pitch = pitchYawRoll.Y;

        // Original vector coordinates
        double x = offset.X;
        double y = offset.Y;
        double z = offset.Z;

        // Rotate around Z-axis (Yaw)
        double x1 = x * Math.Cos(Yaw) - y * Math.Sin(Yaw);
        double y1 = x * Math.Sin(Yaw) + y * Math.Cos(Yaw);
        double z1 = z;

        // Rotate around X-axis (Roll)
        double x2 = x1;
        double y2 = y1 * Math.Cos(Roll) - z1 * Math.Sin(Roll);
        double z2 = y1 * Math.Sin(Roll) + z1 * Math.Cos(Roll);

        // Rotate around Y-axis (Pitch)
        double x3 = x2 * Math.Cos(Pitch) + z2 * Math.Sin(Pitch);
        double y3 = y2;
        double z3 = -x2 * Math.Sin(Pitch) + z2 * Math.Cos(Pitch);

        coords += new Vec3((float)x3, (float)y3, (float)z3);
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