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

    public void Multiply(RotationMatrix other)
    {
        // Multiply the current rotation matrix by the other rotation matrix
        rotationMatrix = rotationMatrix * other.rotationMatrix;
    }
    public static Vec3 RotateRelative(Vec3 rotation, Vec3 byRotation){
        double yaw = rotation.X;
        double pitch = rotation.Y;//probably inverted
        double roll = rotation.Z;//same

        // Rotation matrix for yaw (Z-axis)
        Matrix<double> rotationZ = DenseMatrix.OfArray(new double[,] {
            { Math.Cos(yaw), -Math.Sin(yaw), 0 , 0},
            { Math.Sin(yaw), Math.Cos(yaw), 0 , 0},
            { 0, 0, 1 , 0},
            { 0, 0, 0 , 1}
        });

        // Rotation matrix for pitch (Y-axis)
        Matrix<double> rotationY = DenseMatrix.OfArray(new double[,] {
            { Math.Cos(pitch), 0, Math.Sin(pitch) ,0},
            { 0, 1, 0 ,0},
            { -Math.Sin(pitch), 0, Math.Cos(pitch) ,0},
            { 0, 0, 0 , 1}
        });

        // Rotation matrix for roll (X-axis)
        Matrix<double> rotationX = DenseMatrix.OfArray(new double[,] {
            { 1, 0, 0 ,0},
            { 0, Math.Cos(roll), -Math.Sin(roll) ,0},
            { 0, Math.Sin(roll), Math.Cos(roll) ,0},
            { 0, 0, 0 , 1}
        });
        double byYaw = byRotation.X;
        double byPitch = byRotation.Y;//probably inverted
        double byRoll = byRotation.Z;//same

        // Rotation matrix for byYaw (Z-axis)
        Matrix<double> byRotationZ = DenseMatrix.OfArray(new double[,] {
            { Math.Cos(byYaw), -Math.Sin(byYaw), 0 ,0},
            { Math.Sin(byYaw), Math.Cos(byYaw), 0 ,0},
            { 0, 0, 1 ,0},
            { 0, 0, 0 , 1}
        });

        // Rotation matrix for byPitch (Y-axis)
        Matrix<double> byRotationY = DenseMatrix.OfArray(new double[,] {
            { Math.Cos(byPitch), 0, Math.Sin(byPitch) ,0},
            { 0, 1, 0 ,0},
            { -Math.Sin(byPitch), 0, Math.Cos(byPitch) ,0},
            { 0, 0, 0 , 1}
        });

        // Rotation matrix for byRoll (X-axis)
        Matrix<double> byRotationX = DenseMatrix.OfArray(new double[,] {
            { 1, 0, 0 ,0},
            { 0, Math.Cos(byRoll), -Math.Sin(byRoll) ,0},
            { 0, Math.Sin(byRoll), Math.Cos(byRoll) ,0},
            { 0, 0, 0 , 1}
        });

        // Matrix<double> rotationMatrix = byRotationX * byRotationY * byRotationZ * rotationX * rotationY * rotationZ;//probably XYZ
        Matrix<double> rotationMatrix = rotationZ * rotationY * rotationX * byRotationZ * byRotationY * byRotationX;

        double extractedYaw = Math.Atan2(rotationMatrix[1, 0], rotationMatrix[0, 0]);
        double extractedPitch = Math.Asin(-rotationMatrix[2, 0]);
        double extractedRoll = Math.Atan2(rotationMatrix[2, 1], rotationMatrix[2, 2]);
        return new Vec3((float)extractedYaw, (float)extractedPitch, (float)extractedRoll);
    }
}