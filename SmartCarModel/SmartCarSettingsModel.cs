namespace SmartCarModel
{
    public class SmartCarSettingsModel
    {
        public SmartCarSettingsModel()
        {
            SmartCarIP = "10.0.0.100";
            SmartCarPort = 8080;

            ServoFreq = 50; // Analog servos run at ~50 Hz updates
            
            CameraXMax = 600;
            CameraXMin = 150;
            CameraYMax = 600;
            CameraYMin = 150;

            DirectionMin = 150;
            DirectionMax = 600;

            ServoPwmMin = 600; // This is the rounded 'minimum' microsecond length based on the minimum pulse of 150
            ServoPwmMax = 2400;  // This is the rounded 'maximum' microsecond length based on the maximum pulse of 600
        }

        public string SmartCarIP { get; set; }
        public int SmartCarPort { get; set; }
        public string SmartCarCamera { get; set; }
        public double ServoFreq { get; set; }
        public double CameraXMin { get; set; }
        public double CameraXMax { get; set; }
        public double CameraYMin { get; set; }
        public double CameraYMax { get; set; }
        public double DirectionMin { get; set; }
        public double DirectionMax { get; set; }
        public double MotorMin { get; set; }
        public double MotorMax { get; set; }
        public double ServoPwmMin { get; set; }
        public double ServoPwmMax { get; set; }
        public Controllers SelectedController { get; set; }
    }
}
