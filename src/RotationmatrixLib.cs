
class RotationMatrixLib {
    
public static EulerAnglesLib ToEulerAngles(QuaternionLib q) {
        EulerAnglesLib angles = new EulerAnglesLib();

        // roll (x-axis rotation)
        double sinr_cosp = 2 * (q.w * q.x + q.y * q.z);
        double cosr_cosp = 1 - 2 * (q.x * q.x + q.y * q.y);
        angles.roll = Math.Atan2(sinr_cosp, cosr_cosp);

        // pitch (y-axis rotation)
        double sinp = Math.Sqrt(1 + 2 * (q.w * q.y - q.x * q.z));
        double cosp = Math.Sqrt(1 - 2 * (q.w * q.y - q.x * q.z));
        angles.pitch = 2 * Math.Atan2(sinp, cosp) - Math.PI / 2;

        // yaw (z-axis rotation)
        double siny_cosp = 2 * (q.w * q.z + q.x * q.y);
        double cosy_cosp = 1 - 2 * (q.y * q.y + q.z * q.z);
        angles.yaw = Math.Atan2(siny_cosp, cosy_cosp);

        return angles;
    }

    public static QuaternionLib ToQuaternion(double roll, double pitch, double yaw) // roll (x), pitch (y), yaw (z), angles are in radians
    {
        // Abbreviations for the various angular functions

        double cr = Math.Cos(roll * 0.5);
        double sr = Math.Sin(roll * 0.5);
        double cp = Math.Cos(pitch * 0.5);
        double sp = Math.Sin(pitch * 0.5);
        double cy = Math.Cos(yaw * 0.5);
        double sy = Math.Sin(yaw * 0.5);

        QuaternionLib q = new QuaternionLib();
        q.w = cr * cp * cy + sr * sp * sy;
        q.x = sr * cp * cy - cr * sp * sy;
        q.y = cr * sp * cy + sr * cp * sy;
        q.z = cr * cp * sy - sr * sp * cy;

        return q;
    }
}
class QuaternionLib {
    public double w, x, y, z;
};

class EulerAnglesLib {
    public double roll, pitch, yaw;
};