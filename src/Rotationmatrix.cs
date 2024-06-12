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
        double yaw = rotation.X;
        double pitch = rotation.Y;
        double roll = rotation.X;

        //Trackmania does Yaw, then Roll, then Pitch (All Clockwise)

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

        // Rotation matrix for pitch (Y-axis)
        Matrix<double> rotationY = DenseMatrix.OfArray(new double[,] {
            { Math.Cos(roll), 0, Math.Sin(roll) ,0},
            { 0, 1, 0 ,0},
            { -Math.Sin(roll), 0, Math.Cos(roll) ,0},
            { 0, 0, 0 , 1}
        });
        rotationMatrix *= rotationY;
        printMatrix(rotationMatrix);

        // Rotation matrix for roll (X-axis)
        Matrix<double> rotationX = DenseMatrix.OfArray(new double[,] {
            { 1, 0, 0 ,0},
            { 0, Math.Cos(pitch), -Math.Sin(pitch) ,0},
            { 0, Math.Sin(pitch), Math.Cos(pitch) ,0},
            { 0, 0, 0 , 1}
        });
        rotationMatrix *= rotationX;
        printMatrix(rotationMatrix);

        double byYaw = byRotation.X;
        double byPitch = byRotation.Y;
        double byRoll = byRotation.X;

        // Rotation matrix for byYaw (Z-axis)
        Matrix<double> byRotationZ = DenseMatrix.OfArray(new double[,] {
            { Math.Cos(byYaw), -Math.Sin(byYaw), 0 ,0},
            { Math.Sin(byYaw), Math.Cos(byYaw), 0 ,0},
            { 0, 0, 1 ,0},
            { 0, 0, 0 , 1}
        });
        rotationMatrix *= byRotationZ;
        printMatrix(rotationMatrix);

        // Rotation matrix for byPitch (Y-axis)
        Matrix<double> byRotationY = DenseMatrix.OfArray(new double[,] {
            { Math.Cos(byRoll), 0, Math.Sin(byRoll) ,0},
            { 0, 1, 0 ,0},
            { -Math.Sin(byRoll), 0, Math.Cos(byRoll) ,0},
            { 0, 0, 0 , 1}
        });
        rotationMatrix *= byRotationY;
        printMatrix(rotationMatrix);

        // Rotation matrix for byRoll (X-axis)
        Matrix<double> byRotationX = DenseMatrix.OfArray(new double[,] {
            { 1, 0, 0 ,0},
            { 0, Math.Cos(byPitch), -Math.Sin(byPitch) ,0},
            { 0, Math.Sin(byPitch), Math.Cos(byPitch) ,0},
            { 0, 0, 0 , 1}
        });
        rotationMatrix *= byRotationX;
        printMatrix(rotationMatrix);

        // double extractedYaw = Math.Atan2(rotationMatrix[1, 0], rotationMatrix[0, 0]);
        // double extractedRoll = -Math.Asin(rotationMatrix[2, 0]);
        // double extractedPitch = Math.Atan2(rotationMatrix[2, 1], rotationMatrix[2, 2]);
        //Generated
        double extractedYaw;
        double extractedPitch;
        double extractedRoll;

        if (rotationMatrix[1, 2] < 1)
        {
            if (rotationMatrix[1, 2] > -1)
            {
                extractedPitch = Math.Asin(rotationMatrix[1, 2]);
                extractedRoll = Math.Atan2(-rotationMatrix[0, 2], rotationMatrix[2, 2]);
                extractedYaw = Math.Atan2(-rotationMatrix[1, 0], rotationMatrix[1, 1]);
            }
            else // rotationMatrix[1, 2] == -1
            {
                extractedPitch = -Math.PI / 2;
                extractedRoll = -Math.Atan2(rotationMatrix[0, 1], rotationMatrix[0, 0]);
                extractedYaw = 0;
            }
        }
        else // rotationMatrix[1, 2] == 1
        {
            extractedPitch = Math.PI / 2;
            extractedRoll = Math.Atan2(rotationMatrix[0, 1], rotationMatrix[0, 0]);
            extractedYaw = 0;
        }

        return new Vec3((float)extractedYaw, (float)extractedPitch, (float)extractedRoll);
    }

    private static void printMatrix(Matrix<double> rotationMatrix) {
        Console.WriteLine(rotationMatrix);
        double extractedYaw = Math.Atan2(rotationMatrix[1, 0], rotationMatrix[0, 0]);
        double extractedPitch = -Math.Asin(rotationMatrix[2, 0]);
        double extractedRoll = Math.Atan2(rotationMatrix[2, 1], rotationMatrix[2, 2]);
        Console.WriteLine($"Yaw: {extractedYaw}, Pitch: {extractedPitch}, Roll: {extractedRoll}");
    }
}