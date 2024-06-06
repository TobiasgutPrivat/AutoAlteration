using System;
using GBX.NET;

public class RotationMatrix
{
    public static double[,] CreateRotationMatrix(double psi, double theta, double phi)
    {
        // Rotation matrix around the x-axis
        double[,] Rx = new double[3, 3]
        {
            { 1, 0, 0 },
            { 0, Math.Cos(psi), -Math.Sin(psi) },
            { 0, Math.Sin(psi), Math.Cos(psi) }
        };

        // Rotation matrix around the y-axis
        double[,] Ry = new double[3, 3]
        {
            { Math.Cos(theta), 0, Math.Sin(theta) },
            { 0, 1, 0 },
            { -Math.Sin(theta), 0, Math.Cos(theta) }
        };

        // Rotation matrix around the z-axis
        double[,] Rz = new double[3, 3]
        {
            { Math.Cos(phi), -Math.Sin(phi), 0 },
            { Math.Sin(phi), Math.Cos(phi), 0 },
            { 0, 0, 1 }
        };

        // Resulting rotation matrix R = Rz * Ry * Rx
        double[,] R = MultiplyMatrices(MultiplyMatrices(Rz, Ry), Rx);
        return R;
    }

    public static double[,] MultiplyMatrices(double[,] A, double[,] B)
    {
        int aRows = A.GetLength(0);
        int aCols = A.GetLength(1);
        int bCols = B.GetLength(1);
        double[,] result = new double[aRows, bCols];

        for (int i = 0; i < aRows; i++)
        {
            for (int j = 0; j < bCols; j++)
            {
                for (int k = 0; k < aCols; k++)
                {
                    result[i, j] += A[i, k] * B[k, j];
                }
            }
        }

        return result;
    }

    public static Vec3 GetEulerAngles(double[,] R)
    {
        double psi, theta, phi;

        if (R[2, 0] != 1 && R[2, 0] != -1)
        {
            theta = -Math.Asin(R[2, 0]);
            double cosTheta = Math.Cos(theta);
            psi = Math.Atan2(R[2, 1] / cosTheta, R[2, 2] / cosTheta);
            phi = Math.Atan2(R[1, 0] / cosTheta, R[0, 0] / cosTheta);
        }
        else
        {
            // Gimbal lock occurs
            phi = 0; // You can choose any value for phi
            if (R[2, 0] == -1)
            {
                theta = Math.PI / 2;
                psi = phi + Math.Atan2(R[0, 1], R[0, 2]);
            }
            else
            {
                theta = -Math.PI / 2;
                psi = -phi + Math.Atan2(-R[0, 1], -R[0, 2]);
            }
        }

        return new Vec3((float)psi, (float)theta, (float)phi);
    }

    // public static void Main(double psi,double theta,double phi)
    // {
    //     double[,] rotationMatrix = CreateRotationMatrix(psi, theta, phi);

    //     // Print the rotation matrix
    //     for (int i = 0; i < 3; i++)
    //     {
    //         for (int j = 0; j < 3; j++)
    //         {
    //             Console.Write(rotationMatrix[i, j] + "\t");
    //         }
    //         Console.WriteLine();
    //     }
    // }
}
