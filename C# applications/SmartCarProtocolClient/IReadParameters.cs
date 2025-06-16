using SmartCarProtocol;
using SmartCarProtocol.data;

namespace SmartCarProtocolClient
{
    public interface IReadParameters
    {
        #region Settings

        /// <summary>
        /// Obtient les limites de réglage x-y du pan et du tilt de la caméra.
        /// </summary>
        /// <param name="minX">Limite minimale en X.</param>
        /// <param name="maxX">Limite maximale en X.</param>
        /// <param name="minY">Limite minimale en Y.</param>
        /// <param name="maxY">Limite maximale en Y.</param>
        /// <returns>Un objet ProtocolMessage indiquant le succès ou l'échec de l'opération.</returns>
        public ProtocolMessage GetCameraGettings(float minX, float maxX, float minY, float maxY);

        /// <summary>
        /// Obtient les limites de direction.
        /// </summary>
        /// <param name="directionMax">Limite maximale de direction.</param>
        /// <param name="directionMin">Limite minimale de direction.</param>
        /// <returns>Un objet ProtocolMessage indiquant le succès ou l'échec de l'opération.</returns>
        public ProtocolMessage GetDirectionGettings(float directionMax, float directionMin);

        /// <summary>
        /// Obtient les limites du moteur (vitesse).
        /// </summary>
        /// <param name="speedMax">Vitesse maximale du moteur.</param>
        /// <param name="speedMin">Vitesse minimale du moteur.</param>
        /// <returns>Un objet ProtocolMessage indiquant le succès ou l'échec de l'opération.</returns>
        public ProtocolMessage GetMotorGettings(double speedMax, double speedMin);

        /// <summary>
        /// Obtient les paramètres limites du servo.
        /// </summary>
        /// <param name="frequencyHzData">Fréquence en Hz.</param>
        /// <param name="servoPwmMinData">Valeur PWM minimale du servo.</param>
        /// <param name="servoPwmMaxData">Valeur PWM maximale du servo.</param>
        /// <returns>Un objet ProtocolMessage indiquant le succès ou l'échec de l'opération.</returns>
        public ProtocolMessage GetServoGettings(float frequencyHzData, double servoPwmMinData, double servoPwmMaxData);

        #endregion

        #region Parameters

        /// <summary>
        /// Obtient la position de la caméra.
        /// </summary>
        /// <param name="positionX">Position en X de la caméra.</param>
        /// <param name="positionY">Position en Y de la caméra.</param>
        /// <returns>Un objet ProtocolMessage indiquant le succès ou l'échec de l'opération.</returns>
        public ProtocolMessage GetCameraPosition(float positionX, float positionY);

        /// <summary>
        /// Obtient la direction.
        /// </summary>
        /// <param name="direction">Valeur de la direction.</param>
        /// <returns>Un objet ProtocolMessage indiquant le succès ou l'échec de l'opération.</returns>
        public ProtocolMessage GetDirection(float direction);

        /// <summary>
        /// Obtient l'état du moteur.
        /// </summary>
        /// <param name="direction">Direction du moteur (enum MotorDirection).</param>
        /// <param name="speed">Vitesse du moteur.</param>
        /// <returns>Un objet ProtocolMessage indiquant le succès ou l'échec de l'opération.</returns>
        public ProtocolMessage GetMotor(MotorDirection direction, float speed);

        #endregion
    }
}