using GBX.NET;

class RotationMatrix {
    public double[,] rotationMatrix = new double[3, 3];

    public RotationMatrix(double psi, double theta, double phi)
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
        rotationMatrix = MultiplyMatrices(MultiplyMatrices(Rx, Ry), Rz);
    }
    
    private double[,] MultiplyMatrices(double[,] A, double[,] B)
    {
        int m = A.GetLength(0);
        int n = A.GetLength(1);
        int p = B.GetLength(1);
        double[,] C = new double[m, p];
    
        for (int i = 0; i < m; i++)
        {
            for (int j = 0; j < p; j++)
            {
                for (int k = 0; k < n; k++)
                {
                    C[i, j] += A[i, k] * B[k, j];
                }
            }
        }
    
        return C;
    }

    public Vec3 GetEulerAngles()
    {
        double psi, theta, phi;

        if (rotationMatrix[2, 0] != 1 && rotationMatrix[2, 0] != -1)
        {
            theta = -Math.Asin(rotationMatrix[2, 0]);
            double cosTheta = Math.Cos(theta);
            psi = Math.Atan2(rotationMatrix[2, 1] / cosTheta, rotationMatrix[2, 2] / cosTheta);
            phi = Math.Atan2(rotationMatrix[1, 0] / cosTheta, rotationMatrix[0, 0] / cosTheta);
        }
        else
        {
            phi = 0;
            if (rotationMatrix[2, 0] == -1)
            {
                theta = Math.PI / 2;
                psi = phi + Math.Atan2(rotationMatrix[0, 1], rotationMatrix[0, 2]);
            }
            else
            {
                theta = -Math.PI / 2;
                psi = -phi + Math.Atan2(-rotationMatrix[0, 1], -rotationMatrix[0, 2]);
            }
        }

        return new Vec3((float)psi, (float)theta, (float)phi);
    }
}