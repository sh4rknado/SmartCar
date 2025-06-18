#ifndef SmartCar_h
#define SmartCar_h

#include <Wire.h>
#include <Adafruit_PWMServoDriver.h>
#include "SmartCarConfig.h"

class SmartCar
{
    private:
        SmartCarConfig& _config;
        Adafruit_PWMServoDriver* _pwm;
        float _currentDirection;
        float _currentCameraX;
        float _currentCameraY;

    private:
        void SetAngle(float angle, int channel);
        bool CheckI2C(byte address);
        void InitializePCA9685();
        void ArmMotor();

    public:
        SmartCar SmartCar(SmartCarConfig config);
        void Setup();
        void SetDirection(float angle);
        void SetCameraX(float angle);
        void SetCameraY(float angle);
        float GetDirection();
        float GetCameraX();
        float GetCameraY();
        void StopMotor();
}

#endif
