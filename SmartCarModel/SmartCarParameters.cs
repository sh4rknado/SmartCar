namespace SmartCarModel
{
    public class SmartCarParameters
    {
        public SmartCarParameters(double cameraX, double cameraY, double direction, double motor)
        {
            CameraX = cameraX;
            CameraY = cameraY;
            Direction = direction;
            Motor = motor;
        }

        public double CameraX { get; set; }
        public double CameraY { get; set; }
        public double Direction { get; set; }
        public double Motor { get; set; }
    }
}