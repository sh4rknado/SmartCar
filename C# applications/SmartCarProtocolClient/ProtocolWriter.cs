using SmartCarProtocol.data;
using SmartCarProtocol.settings;
using SmartCarProtocol;

namespace SmartCarProtocolClient
{
    public class ProtocolWriter: ISetParameters
    {
        #region Settings

        /// <summary>
        /// Set the settings limits x-y of the pan tilt of the camera
        /// </summary>
        /// <param name="minX"></param>
        /// <param name="maxX"></param>
        /// <param name="minY"></param>
        /// <param name="maxY"></param>
        /// <returns></returns>
        public ProtocolMessage SetCameraSettings(double minX, double maxX, double minY, double maxY)
        {
            CameraLimitsData data = new(minX, maxX, minY, maxY);
            return new(MessageType.Settings, ActionType.SetPanTilt, data);
        }

        /// <summary>
        /// Set the direction limit
        /// </summary>
        /// <param name="directionMax"></param>
        /// <param name="directionMin"></param>
        /// <returns></returns>
        public ProtocolMessage SetDirectionSettings(double directionMax, double directionMin)
        {
            DirectionLimits data = new(directionMax, directionMin);
            return new(MessageType.Settings, ActionType.SetDirection, data);
        }

        /// <summary>
        /// Set the motor limit
        /// </summary>
        /// <param name="DirectionMax"></param>
        /// <param name="DirectionMin"></param>
        /// <returns></returns>
        public ProtocolMessage SetMotorSettings(double speedMax, double speedMin)
        {
            MotorLimit data = new(speedMax, speedMin);
            return new(MessageType.Settings, ActionType.SetMotor, data);
        }

        /// <summary>
        /// Set Servo parameters limit
        /// </summary>
        /// <param name="frequencyHzData"></param>
        /// <param name="servoPwmMinData"></param>
        /// <param name="servoPwmMaxData"></param>
        /// <returns></returns>
        public ProtocolMessage SetServoSettings(double frequencyHzData, double servoPwmMinData, double servoPwmMaxData)
        {
            ServoSettings data = new(frequencyHzData, servoPwmMinData, servoPwmMaxData);
            return new(MessageType.Settings, ActionType.SetServos, data);
        }

        #endregion

        #region parameters

        /// <summary>
        /// Set camera position
        /// </summary>
        /// <param name="positionX"></param>
        /// <param name="positionY"></param>
        /// <returns></returns>
        public ProtocolMessage SetCameraPosition(double positionX, double positionY)
        {
            CameraPanTiltData data = new(positionX, positionY);
            return new(MessageType.Command, ActionType.SetPanTilt, data);
        }

        /// <summary>
        /// Set direction
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public ProtocolMessage SetDirection(double direction)
        {
            DirectionControlData data = new(direction);
            return new(MessageType.Command, ActionType.SetDirection, data);
        }

        /// <summary>
        /// Set motor
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public ProtocolMessage SetMotor(MotorDirection direction, double speed)
        {
            MotorControlData data = new(direction, speed);
            return new(MessageType.Command, ActionType.SetMotor, data);
        }

        #endregion
    }
}
