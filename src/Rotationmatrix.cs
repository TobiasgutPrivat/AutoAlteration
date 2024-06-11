using GBX.NET;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

class RotationMatrix {
    public Matrix<double> rotationMatrix;

    public RotationMatrix(Vec3 rotation) {
        double yaw = rotation.X;
        double pitch = rotation.Y;//probably inverted
        double roll = rotation.Z;//same

        // Rotation matrix for yaw (Z-axis)
        Matrix<double> rotationZ = DenseMatrix.OfArray(new double[,] {
            { Math.Cos(yaw), -Math.Sin(yaw), 0 },
            { Math.Sin(yaw), Math.Cos(yaw), 0 },
            { 0, 0, 1 }
        });

        // Rotation matrix for pitch (Y-axis)
        Matrix<double> rotationY = DenseMatrix.OfArray(new double[,] {
            { Math.Cos(pitch), 0, Math.Sin(pitch) },
            { 0, 1, 0 },
            { -Math.Sin(pitch), 0, Math.Cos(pitch) }
        });

        // Rotation matrix for roll (X-axis)
        Matrix<double> rotationX = DenseMatrix.OfArray(new double[,] {
            { 1, 0, 0 },
            { 0, Math.Cos(roll), -Math.Sin(roll) },
            { 0, Math.Sin(roll), Math.Cos(roll) }
        });

        // Combined rotation matrix (Z * Y * X)
        rotationMatrix = rotationZ * rotationY * rotationX;//probably XYZ

    }
    public void print(){
        Console.WriteLine("Rotation Matrix based on Euler angles:");
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                Console.Write($"{rotationMatrix[i, j]:F8} ");
            }
            Console.WriteLine();
        }
    }

    public Vec3 GetEulerAngles()
    {
        double extractedYaw = Math.Atan2(rotationMatrix[1, 0], rotationMatrix[0, 0]);
        double extractedPitch = Math.Asin(-rotationMatrix[2, 0]);
        double extractedRoll = Math.Atan2(rotationMatrix[2, 1], rotationMatrix[2, 2]);
        return new Vec3((float)extractedYaw, (float)extractedPitch, (float)extractedRoll);//probably pitch and rollinverted
    }

    public static Vec3 RotateRelative(Vec3 rotation, Vec3 byRotation){
        double yaw = -rotation.X;
        double roll = rotation.Z;
        double pitch = -rotation.Y;

        Console.WriteLine("################\nStart\n####################");
        // Rotation matrix for yaw (Z-axis)
        Matrix<double> rotationZ = DenseMatrix.OfArray(new double[,] {
            { Math.Cos(yaw), -Math.Sin(yaw), 0 , 0},
            { Math.Sin(yaw), Math.Cos(yaw), 0 , 0},
            { 0, 0, 1 , 0},
            { 0, 0, 0 , 1}
        });
        Matrix<double> rotationMatrix = rotationZ;
        printMatrix(rotationMatrix);

        // Rotation matrix for roll (X-axis)
        Matrix<double> rotationX = DenseMatrix.OfArray(new double[,] {
            { 1, 0, 0 ,0},
            { 0, Math.Cos(roll), -Math.Sin(roll) ,0},
            { 0, Math.Sin(roll), Math.Cos(roll) ,0},
            { 0, 0, 0 , 1}
        });
        rotationMatrix *= rotationX;
        printMatrix(rotationMatrix);

        // Rotation matrix for pitch (Y-axis)
        Matrix<double> rotationY = DenseMatrix.OfArray(new double[,] {
            { Math.Cos(pitch), 0, Math.Sin(pitch) ,0},
            { 0, 1, 0 ,0},
            { -Math.Sin(pitch), 0, Math.Cos(pitch) ,0},
            { 0, 0, 0 , 1}
        });
        rotationMatrix *= rotationY;
        printMatrix(rotationMatrix);

        double byYaw = -byRotation.X;
        double byRoll = byRotation.Z;
        double byPitch = -byRotation.Y;

        // Rotation matrix for byYaw (Z-axis)
        Matrix<double> byRotationZ = DenseMatrix.OfArray(new double[,] {
            { Math.Cos(byYaw), -Math.Sin(byYaw), 0 ,0},
            { Math.Sin(byYaw), Math.Cos(byYaw), 0 ,0},
            { 0, 0, 1 ,0},
            { 0, 0, 0 , 1}
        });
        rotationMatrix *= byRotationZ;
        printMatrix(rotationMatrix);

        // Rotation matrix for byRoll (X-axis)
        Matrix<double> byRotationX = DenseMatrix.OfArray(new double[,] {
            { 1, 0, 0 ,0},
            { 0, Math.Cos(byRoll), -Math.Sin(byRoll) ,0},
            { 0, Math.Sin(byRoll), Math.Cos(byRoll) ,0},
            { 0, 0, 0 , 1}
        });
        rotationMatrix *= byRotationX;
        printMatrix(rotationMatrix);

        // Rotation matrix for byPitch (Y-axis)
        Matrix<double> byRotationY = DenseMatrix.OfArray(new double[,] {
            { Math.Cos(byPitch), 0, Math.Sin(byPitch) ,0},
            { 0, 1, 0 ,0},
            { -Math.Sin(byPitch), 0, Math.Cos(byPitch) ,0},
            { 0, 0, 0 , 1}
        });
        rotationMatrix *= byRotationY;
        printMatrix(rotationMatrix);

        double extractedYaw = Math.Atan2(rotationMatrix[1, 0], rotationMatrix[0, 0]);
        double extractedPitch = -Math.Asin(rotationMatrix[2, 0]);
        double extractedRoll = Math.Atan2(rotationMatrix[2, 1], rotationMatrix[2, 2]);
        return new Vec3(-(float)extractedYaw, -(float)extractedPitch, (float)extractedRoll);
    }

    // public static Vec3 DebugRotateRelative(double yaw,double pitch,double roll,double byYaw,double byPitch,double byRoll){
    //     //!!!Rotationmatrix working
    //     //!!!Issue with euler angles reading
    //     //system like: https://danceswithcode.net/engineeringnotes/rotations_in_3d/demo3D/rotations_in_3d_tool.html
    //     //but top/bottom is mirrord (-yaw,-pitch,roll) -> clockwise

    //     // Rotation matrix for yaw (Z-axis)
    //     Matrix<double> rotationZ = DenseMatrix.OfArray(new double[,] {
    //         { Math.Cos(yaw), -Math.Sin(yaw), 0 , 0},
    //         { Math.Sin(yaw), Math.Cos(yaw), 0 , 0},
    //         { 0, 0, 1 , 0},
    //         { 0, 0, 0 , 1}
    //     });
    //     Matrix<double> rotationMatrix = rotationZ;
    //     printMatrix(rotationMatrix);

    //     // Rotation matrix for pitch (Y-axis)
    //     Matrix<double> rotationY = DenseMatrix.OfArray(new double[,] {
    //         { Math.Cos(pitch), 0, Math.Sin(pitch) ,0},
    //         { 0, 1, 0 ,0},
    //         { -Math.Sin(pitch), 0, Math.Cos(pitch) ,0},
    //         { 0, 0, 0 , 1}
    //     });
    //     rotationMatrix *= rotationY;
    //     printMatrix(rotationMatrix);

    //     // Rotation matrix for roll (X-axis)
    //     Matrix<double> rotationX = DenseMatrix.OfArray(new double[,] {
    //         { 1, 0, 0 ,0},
    //         { 0, Math.Cos(roll), -Math.Sin(roll) ,0},
    //         { 0, Math.Sin(roll), Math.Cos(roll) ,0},
    //         { 0, 0, 0 , 1}
    //     });
    //     rotationMatrix *= rotationX;
    //     printMatrix(rotationMatrix);

    //     // Rotation matrix for byYaw (Z-axis)
    //     Matrix<double> byRotationZ = DenseMatrix.OfArray(new double[,] {
    //         { Math.Cos(byYaw), -Math.Sin(byYaw), 0 ,0},
    //         { Math.Sin(byYaw), Math.Cos(byYaw), 0 ,0},
    //         { 0, 0, 1 ,0},
    //         { 0, 0, 0 , 1}
    //     });
    //     rotationMatrix *= byRotationZ;
    //     printMatrix(rotationMatrix);

    //     // Rotation matrix for byPitch (Y-axis)
    //     Matrix<double> byRotationY = DenseMatrix.OfArray(new double[,] {
    //         { Math.Cos(byPitch), 0, Math.Sin(byPitch) ,0},
    //         { 0, 1, 0 ,0},
    //         { -Math.Sin(byPitch), 0, Math.Cos(byPitch) ,0},
    //         { 0, 0, 0 , 1}
    //     });
    //     rotationMatrix *= byRotationY;
    //     printMatrix(rotationMatrix);

    //     // Rotation matrix for byRoll (X-axis)
    //     Matrix<double> byRotationX = DenseMatrix.OfArray(new double[,] {
    //         { 1, 0, 0 ,0},
    //         { 0, Math.Cos(byRoll), -Math.Sin(byRoll) ,0},
    //         { 0, Math.Sin(byRoll), Math.Cos(byRoll) ,0},
    //         { 0, 0, 0 , 1}
    //     });
    //     rotationMatrix *= byRotationX;
    //     printMatrix(rotationMatrix);

    //     double extractedYaw = Math.Atan2(rotationMatrix[1, 0], rotationMatrix[0, 0]);
    //     double extractedPitch = -Math.Asin(rotationMatrix[2, 0]);
    //     double extractedRoll = Math.Atan2(rotationMatrix[2, 1], rotationMatrix[2, 2]);
    //     return new Vec3((float)extractedYaw, (float)extractedPitch, (float)extractedRoll);
    // }

    private static void printMatrix(Matrix<double> rotationMatrix) {
        Console.WriteLine(rotationMatrix);
        double extractedYaw = Math.Atan2(rotationMatrix[1, 0], rotationMatrix[0, 0]);
        double extractedPitch = -Math.Asin(rotationMatrix[2, 0]);
        double extractedRoll = Math.Atan2(rotationMatrix[2, 1], rotationMatrix[2, 2]);
        Console.WriteLine($"Yaw: {extractedYaw}, Pitch: {extractedPitch}, Roll: {extractedRoll}");
    }
}