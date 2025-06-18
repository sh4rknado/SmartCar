
#include "SmartCar.h"

#define FIRMWARE_VERSION 1010

SmartCar SmartCar::SmartCar(SmartCarConfig config) {
    _config = config;
}

void SmartCar::SetDirection(float angle) { 
    if(angle < _config.servo_angle_min || angle > _config.servo_angle_max)
        return;

    SetAngle(angle, _config.direction_channel); 
    _currentDirection = angle;
}

void SmartCar::SetCameraX(float angle) {
    if(angle < _config.camera_x_angle_min || angle > _config.camera_x_angle_max)
        return;

    SetAngle(angle, _config._cam_channel_x);
    _currentCameraX = angle;
}

void SmartCar::SetCameraY(float angle) { 
    if(angle < _config.camera_y_angle_min || angle > _config.camera_y_angle_max)
        return;

    SetAngle(angle, config._cam_channel_y);
    _currentCameraY = angle;
}

float SmartCar::GetDirection() { return _currentDirection; }
float SmartCar::GetCameraX() { return _currentCameraX; }
float SmartCar::GetCameraY() { return _currentCameraY; }

void SmartCar::SetAngle(float angle, int channel) {
    float pulselength = map(angle, 0, 180, _config.servo_pulse_min, _config.servo_pulse_max);
    _pwm->setPWM(channel, 0, pulselength);
}

void SmartCar::Setup() {
  Wire.begin(_config.sda_pin, _config.scl_pin); // ESP8266 
  while (!CheckI2C(_config.i2c_pca9685_address)); // wait I2C
  InitializePCA9685();
  ArmMotor();
}

void SmartCar::ArmMotor() {
    Serial.println("Arming the motor with MOTOR_STOP_PULSE_LENGTH...");
    pwm.writeMicroseconds(1, _config.motor_pulse_stop);
    delay(7000);
    Serial.println("ESC-Motor Armed !");
}

bool SmartCar::CheckI2C(byte address) {
    Serial.println("Scanning I2C address...");
    Wire.beginTransmission(address);
    byte error = Wire.endTransmission();
    
    if(error == 0)
        Serial.println("I2C found at address 0x40...");

    return error == 0;
    delay(500);
}

void SmartCar::InitializePCA9685() {
    _pwm->begin();
    _pwm->setOscillatorFrequency(27000000);
    _pwm->setPWMFreq(_config.servo_freq);
    delay(10);
}

void SmartCar::StopMotor() {
    Serial.println("Sending stop motor");
    _pwm->writeMicroseconds(_config.motor_channel, _config.motor_pulse_stop);
}

void SmartCar::SetAngle(float angle, int channel) {
    float pulselength = map(angle, 0, 180, _config.servo_pulse_min, _config.servo_pulse_max);
    _pwm->setPWM(channel, 0, pulselength);
}
