using SmartCarProtocol;
using SmartCarProtocol.data;

namespace SmartCarProtocolClient
{
    public interface ISetParameters
    {
        #region Settings

        /// <summary>
        /// Définit les limites de réglage x-y du pan et du tilt de la caméra.
        /// </summary>
        /// <param name="minX">Limite minimale en X.</param>
        /// <param name="maxX">Limite maximale en X.</param>
        /// <param name="minY">Limite minimale en Y.</param>
        /// <param name="maxY">Limite maximale en Y.</param>
        /// <returns>Un objet ProtocolMessage  indiquant le succès ou l'échec de l'opération.</returns>
        public ProtocolMessage SetCameraSettings(double minX, double maxX, double minY, double maxY);

        /// <summary>
        /// Définit la limite de direction.
        /// </summary>
        /// <param name="directionMax">Limite maximale de direction.</param>
        /// <param name="directionMin">Limite minimale de direction.</param>
        /// <returns>Un objet ProtocolMessage  indiquant le succès ou l'échec de l'opération.</returns>
        public ProtocolMessage SetDirectionSettings(double directionMax, double directionMin);

        /// <summary>
        /// Définit la limite du moteur (vitesse).
        /// </summary>
        /// <param name="speedMax">Vitesse maximale du moteur.</param>
        /// <param name="speedMin">Vitesse minimale du moteur.</param>
        /// <returns>Un objet ProtocolMessage  indiquant le succès ou l'échec de l'opération.</returns>
        public ProtocolMessage SetMotorSettings(double speedMax, double speedMin);

        /// <summary>
        /// Définit les paramètres limites du servo.
        /// </summary>
        /// <param name="frequencyHzData">Fréquence en Hz.</param>
        /// <param name="servoPwmMinData">Valeur PWM minimale du servo.</param>
        /// <param name="servoPwmMaxData">Valeur PWM maximale du servo.</param>
        /// <returns>Un objet ProtocolMessage  indiquant le succès ou l'échec de l'opération.</returns>
        public ProtocolMessage SetServoSettings(double frequencyHzData, double servoPwmMinData, double servoPwmMaxData);

        #endregion

        #region Parameters

        /// <summary>
        /// Définit la position de la caméra.
        /// </summary>
        /// <param name="positionX">Position en X de la caméra.</param>
        /// <param name="positionY">Position en Y de la caméra.</param>
        /// <returns>Un objet ProtocolMessage  indiquant le succès ou l'échec de l'opération.</returns>
        public ProtocolMessage SetCameraPosition(double positionX, double positionY);

        /// <summary>
        /// Définit la direction.
        /// </summary>
        /// <param name="direction">Valeur de la direction.</param>
        /// <returns>Un objet ProtocolMessage  indiquant le succès ou l'échec de l'opération.</returns>
        public ProtocolMessage SetDirection(double direction);

        /// <summary>
        /// Définit l'état du moteur.
        /// </summary>
        /// <param name="direction">Direction du moteur (enum MotorDirection).</param>
        /// <param name="speed">Vitesse du moteur.</param>
        /// <returns>Un objet ProtocolMessage  indiquant le succès ou l'échec de l'opération.</returns>
        public ProtocolMessage SetMotor(MotorDirection direction, double speed);

        #endregion
    }
}