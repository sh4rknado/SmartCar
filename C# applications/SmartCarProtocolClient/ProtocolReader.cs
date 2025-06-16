using SmartCarProtocol.data;
using SmartCarProtocol;
using SmartCarProtocol.settings;

namespace SmartCarProtocolClient
{
    public class ProtocolReader: IReadParameters
    { 
        #region Settings

        /// <summary>
        /// Get the Gettings limits x-y of the pan tilt of the camera
        /// </summary>
        /// <param name="minX"></param>
        /// <param name="maxX"></param>
        /// <param name="minY"></param>
        /// <param name="maxY"></param>
        /// <returns></returns>
        public ProtocolMessage GetCameraGettings(float minX, float maxX, float minY, float maxY)
        {
            CameraLimitsData data = new(minX, maxX, minY, maxY);
            return new(MessageType.Settings, ActionType.GetPanTilt, data);
        }

        /// <summary>
        /// Get the direction limit
        /// </summary>
        /// <param name="directionMax"></param>
        /// <param name="directionMin"></param>
        /// <returns></returns>
        public ProtocolMessage GetDirectionGettings(float directionMax, float directionMin)
        {
            DirectionLimits data = new(directionMax, directionMin);
            return new(MessageType.Settings, ActionType.GetDirection, data);
        }

        /// <summary>
        /// Get the motor limit
        /// </summary>
        /// <param name="DirectionMax"></param>
        /// <param name="DirectionMin"></param>
        /// <returns></returns>
        public ProtocolMessage GetMotorGettings(double speedMax, double speedMin)
        {
            MotorLimit data = new(speedMax, speedMin);
            return new(MessageType.Settings, ActionType.GetMotor, data);
        }

        /// <summary>
        /// Get Servo parameters limit
        /// </summary>
        /// <param name="frequencyHzData"></param>
        /// <param name="servoPwmMinData"></param>
        /// <param name="servoPwmMaxData"></param>
        /// <returns></returns>
        public ProtocolMessage GetServoGettings(float frequencyHzData, double servoPwmMinData, double servoPwmMaxData)
        {
            ServoSettings data = new(frequencyHzData, servoPwmMinData, servoPwmMaxData);
            return new(MessageType.Settings, ActionType.GetServos, data);
        }

        #endregion

        #region parameters

        /// <summary>
        /// Get camera position
        /// </summary>
        /// <param name="positionX"></param>
        /// <param name="positionY"></param>
        /// <returns></returns>
        public ProtocolMessage GetCameraPosition(float positionX, float positionY)
        {
            CameraPanTiltData data = new(positionX, positionY);
            return new(MessageType.Command, ActionType.GetPanTilt, data);
        }

        /// <summary>
        /// Get direction
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public ProtocolMessage GetDirection(float direction)
        {
            DirectionControlData data = new(direction);
            return new(MessageType.Command, ActionType.GetDirection, data);
        }

        /// <summary>
        /// Get motor
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public ProtocolMessage GetMotor(MotorDirection direction, float speed)
        {
            MotorControlData data = new(direction, speed);
            return new(MessageType.Command, ActionType.GetMotor, data);
        }

        #endregion
    }
}
