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
        coords += new Vec3(transformedOffset.X, transformedOffset.Y, transformedOffset.Z);
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
        Matrix<double> rotationMatrix = createNadeoMatrix(rotation);
        Matrix<double> byrotationMatrix = createNadeoMatrix(byRotation);
                  
        rotationMatrix = rotationMatrix * byrotationMatrix;
        printMatrix(rotationMatrix);

        double extractedYaw = Math.Atan2(rotationMatrix[1, 0], rotationMatrix[0, 0]);
        double extractedPitch = -Math.Asin(rotationMatrix[2, 0]);
        double extractedRoll = Math.Atan2(rotationMatrix[2, 1], rotationMatrix[2, 2]);
        return new Vec3((float)extractedYaw, (float)extractedPitch, (float)extractedRoll);
    }

    private static Matrix<double> createNadeoMatrix(Vec3 rotation) {
        double Pitch = rotation.Y;
        double Roll = rotation.Z;
        double Yaw = rotation.X;

        // Rotation matrix for Yaw (Z-axis)
        Matrix<double> RotationZ = DenseMatrix.OfArray(new double[,] {
            { Math.Cos(Yaw), -Math.Sin(Yaw), 0 ,0},
            { Math.Sin(Yaw), Math.Cos(Yaw), 0 ,0},
            { 0, 0, 1 ,0},
            { 0, 0, 0 , 1}
        });
        Matrix<double> rotationMatrix = RotationZ;
        printMatrix(rotationMatrix);

        // Rotation matrix for Roll (X-axis)
        Matrix<double> RotationX = DenseMatrix.OfArray(new double[,] {
            { 1, 0, 0 ,0},
            { 0, Math.Cos(Roll), -Math.Sin(Roll) ,0},
            { 0, Math.Sin(Roll), Math.Cos(Roll) ,0},
            { 0, 0, 0 , 1}
        });
        rotationMatrix *= RotationX;
        printMatrix(rotationMatrix);

        // Rotation matrix for Pitch (Y-axis)
        Matrix<double> RotationY = DenseMatrix.OfArray(new double[,] {
            { Math.Cos(Pitch), 0, Math.Sin(Pitch) ,0},
            { 0, 1, 0 ,0},
            { -Math.Sin(Pitch), 0, Math.Cos(Pitch) ,0},
            { 0, 0, 0 , 1}
        });
        rotationMatrix *= RotationY;
        printMatrix(rotationMatrix);
        return rotationMatrix;
    }
    
    private static void printMatrix(Matrix<double> rotationMatrix) {
        Console.WriteLine(rotationMatrix);
        double extractedYaw = Math.Atan2(rotationMatrix[1, 0], rotationMatrix[0, 0]);
        double extractedPitch = -Math.Asin(rotationMatrix[2, 0]);
        double extractedRoll = Math.Atan2(rotationMatrix[2, 1], rotationMatrix[2, 2]);
        Console.WriteLine($"Yaw: {extractedYaw}, Roll: {extractedRoll}, Pitch: {extractedPitch}");
        Console.WriteLine("-------------------------------------------------------------------------");
    }

    public static Vec3 DebugRotateRelative(Vec3 rotation, Vec3 byRotation){
        double yaw = rotation.X;
        double roll = rotation.Z;
        double pitch = rotation.Y;

        //Trackmania does Yaw, then Roll, then Pitch (All Clockwise)

        Console.WriteLine("################\nStart\n####################");
        // Rotation matrix for yaw (Z-axis)
        Matrix<double> rotationZ = DenseMatrix.OfArray(new double[,] {
            { Math.Cos(yaw), -Math.Sin(yaw), 0 , 0},
            { Math.Sin(yaw), Math.Cos(yaw), 0 , 0},
            { 0, 0, 1 , 0},
            { 0, 0, 0 , 1}
        });
        Console.WriteLine("Yaw: " + yaw);
        Matrix<double> rotationMatrix = rotationZ;
        printMatrix(rotationMatrix);

        // Rotation matrix for roll (X-axis)
        Matrix<double> rotationX = DenseMatrix.OfArray(new double[,] {
            { 1, 0, 0 ,0},
            { 0, Math.Cos(roll), -Math.Sin(roll) ,0},
            { 0, Math.Sin(roll), Math.Cos(roll) ,0},
            { 0, 0, 0 , 1}
        });
        Console.WriteLine("Roll: " + roll);
        rotationMatrix *= rotationX;
        printMatrix(rotationMatrix);

        // Rotation matrix for pitch (Y-axis)
        Matrix<double> rotationY = DenseMatrix.OfArray(new double[,] {
            { Math.Cos(pitch), 0, Math.Sin(pitch) ,0},
            { 0, 1, 0 ,0},
            { -Math.Sin(pitch), 0, Math.Cos(pitch) ,0},
            { 0, 0, 0 , 1}
        });
        Console.WriteLine("Pitch: " + pitch);
        rotationMatrix *= rotationY;
        printMatrix(rotationMatrix);

        double byYaw = byRotation.X;
        double byRoll = byRotation.Z;
        double byPitch = byRotation.Y;

        // Rotation matrix for byYaw (Z-axis)
        Matrix<double> byRotationZ = DenseMatrix.OfArray(new double[,] {
            { Math.Cos(byYaw), -Math.Sin(byYaw), 0 ,0},
            { Math.Sin(byYaw), Math.Cos(byYaw), 0 ,0},
            { 0, 0, 1 ,0},
            { 0, 0, 0 , 1}
        });
        Console.WriteLine("byYaw: " + byYaw);
        rotationMatrix *= byRotationZ;
        printMatrix(rotationMatrix);

        // Rotation matrix for byRoll (X-axis)
        Matrix<double> byRotationX = DenseMatrix.OfArray(new double[,] {
            { 1, 0, 0 ,0},
            { 0, Math.Cos(byRoll), -Math.Sin(byRoll) ,0},
            { 0, Math.Sin(byRoll), Math.Cos(byRoll) ,0},
            { 0, 0, 0 , 1}
        });
        Console.WriteLine("byRoll: " + byRoll);
        rotationMatrix *= byRotationX;
        printMatrix(rotationMatrix);

        // Rotation matrix for byPitch (Y-axis)
        Matrix<double> byRotationY = DenseMatrix.OfArray(new double[,] {
            { Math.Cos(byPitch), 0, Math.Sin(byPitch) ,0},
            { 0, 1, 0 ,0},
            { -Math.Sin(byPitch), 0, Math.Cos(byPitch) ,0},
            { 0, 0, 0 , 1}
        });
        Console.WriteLine("byPitch: " + byPitch);
        rotationMatrix *= byRotationY;
        printMatrix(rotationMatrix);

        double extractedYaw = Math.Atan2(rotationMatrix[1, 0], rotationMatrix[0, 0]);
        double extractedPitch = -Math.Asin(rotationMatrix[2, 0]);
        double extractedRoll = Math.Atan2(rotationMatrix[2, 1], rotationMatrix[2, 2]);
        return new Vec3((float)extractedYaw, (float)extractedPitch, (float)extractedRoll);
    }
}