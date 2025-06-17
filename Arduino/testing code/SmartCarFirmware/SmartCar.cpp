
#include "SmartCar.h"

#define FIRMWARE_VERSION 1000

SmartCar SmartCar::SmartCar(int cam_channel_x, int  cam_channel_y, int direction_channel){
    _cam_channel_x = cam_channel_x;
    _cam_channel_y = cam_channel_y;
    _direction_channel = direction_channel;
}

void  SmartCar::SetDirection(float angle) { 
    SetAngle(angle, _cam_channel_x); 
    _currentDirection = angle;
}

void  SmartCar::SetCameraX(float angle) {
    SetAngle(angle, _cam_channel_x);
    _currentCameraX = angle;
}

void  SmartCar::SetCameraY(float angle) { 
    SetAngle(angle, _cam_channel_y);
    _currentCameraY = angle;
}

float SmartCar::GetDirection() { return _currentDirection; }
float SmartCar::GetCameraX() { return _currentCameraX; }
float SmartCar::GetCameraY() { return _currentCameraY; }

void SmartCar::SetAngle(float angle, int channel) 
{
    float pulselength = map(angle, 0, 180, SERVOMIN, SERVOMAX);
    _pwm->setPWM(channel, 0, pulselength);
}

void SmartCar::setup() {
  Wire.begin(D2, D1); // ESP8266 
  //   Serial.begin(9600);
  //   while (!Serial); // wait serial
  while (!CheckI2C(0x40)); // wait I2C
  InitializePCA9685();
  ArmMotor();
}

void SmartCar::ArmMotor() {
    delay(5000);
    //-------------------------------------
    Serial.println("Sending minimum throttle to ESC...");
    _pwm->writeMicroseconds(1,MIN_PULSE_LENGTH);
    delay(2500);
    //-------------------------------------
    Serial.println("Sending maximum throttle to ESC...");
    _pwm->writeMicroseconds(1,MAX_PULSE_LENGTH);
    //-------------------------------------
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
    _pwm->setPWMFreq(SERVO_FREQ);  // Analog servos run at ~50 Hz updates
    delay(10);
}

void loop() { 
    Serial.println("Sending minimum throttle");
    _pwm->writeMicroseconds(1, 1610);
    delay(1000);

    Serial.println("Sending stop motor");
    _pwm->writeMicroseconds(1, 1600);
    delay(1000);
}

void SmartCar::SetAngle(float angle, int channel) {
    float pulselength = map(angle, 0, 180, SERVO_MIN, SERVO_MAX);
    _pwm->setPWM(channel, 0, pulselength);
}
