#ifndef SmartCar_h
#define SmartCar_h

#include <RemoteDebug.h>
#include <Wire.h>
#include <Adafruit_PWMServoDriver.h>
#include "sensors.h"
#include <ArduinoJson.h>

class SmartCar {
  private:
    RemoteDebug* _debug;
    Adafruit_PWMServoDriver* _pwm;     // called this way, it uses the default address 0x40
    int channel_cam_x;
    int channel_cam_y;
    int channel_direction;
    
  public:
    const int SERVO_FREQ = 50;    // Analog servos run at ~50 Hz updates

    const int CAMERA_MIN_X = 150;
    const int CAMERA_MAX_X = 600;
    const int CAMERA_MIN_Y = 150;
    const int CAMERA_MAX_Y = 600;

    const int DIRECTION_MIN = 150;
    const int DIRECTION_MAX = 600;

    const int USMIN = 600;        // This is the rounded 'minimum' microsecond length based on the minimum pulse of 150
    const int USMAX = 2400;       // This is the rounded 'maximum' microsecond length based on the maximum pulse of 600

  public:
    SmartCar(RemoteDebug* debug);
    void Setup();
    int GetMotor();
    void SetMotor(int val);
    int GetDirection();
    void SetDirection(int val);
    int GetPtzX();
    void SetPtzX(int val);
    int GetPtzY();
    void SetPtzY(int val);
    int GetGyro();
    void Stop();
    void SetAllSensors(Sensor& config);
    bool DesirializeJsonSensors(String json, Sensor& config);

  private:
    bool IsValidPort(int port);
    bool StringIsNullOrEmpty(const char* str);

};

#endif