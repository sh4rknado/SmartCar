#ifndef SmartCar_h
#define SmartCar_h

#include <Wire.h>
#include <Adafruit_PWMServoDriver.h>

class SmartCar
{
    private:
        const int SERVO_MIN = 150;          // This is the 'minimum' pulse length count (out of 4096) 
        const int SERVO_MAX = 600;          // This is the 'maximum' pulse length count (out of 4096)
        const int SERVO_FREQ = 50;          // Analog servos run at ~50 Hz updates
        const int MIN_PULSE_LENGTH = 1000;  // Minimum pulse length in µs
        const int MAX_PULSE_LENGTH = 2000;  // Maximum pulse length in µs
        Adafruit_PWMServoDriver* _pwm;
        float _currentDirection;
        float _currentCameraX;
        float _currentCameraY;
        int _cam_channel_x;
        int _cam_channel_y;
        int _direction_channel;

    private:
        void SetAngle(float angle, int channel);
        bool CheckI2C(byte address);
        void InitializePCA9685();
        void ArmMotor();

    public:
        SmartCar SmartCar(int cam_channel_x, int  cam_channel_y, int direction_channel);
        void Setup();
        void SetDirection(float angle);
        void SetCameraX(float angle);
        void SetCameraY(float angle);
        float GetDirection();
        float GetCameraX();
        float GetCameraY();
}

#endif
