#include "SmartCar.h"

SmartCar::SmartCar(RemoteDebug* debug) {
  _debug = debug;
  _pwm = new Adafruit_PWMServoDriver();
}

void SmartCar::Setup() {
  Serial.println("setup PWM...");
  _pwm->begin();
  _pwm->setOscillatorFrequency(27000000);
  _pwm->setPWMFreq(SERVO_FREQ);  // Analog servos run at ~50 Hz updates
}

int SmartCar::GetPtzX() { return _pwm->getPWM(channel_cam_x); }
void SmartCar::SetPtzX(int val) {
  if(val <= CAMERA_MAX_X || val >= CAMERA_MIN_X)
    _pwm->setPWM(channel_cam_x, 0, val);
}

int SmartCar::GetPtzY() { return _pwm->getPWM(channel_cam_y); }
void SmartCar::SetPtzY(int val) {
  if(val <= CAMERA_MAX_Y || val >= CAMERA_MIN_Y)
    _pwm->setPWM(channel_cam_y, 0, val);
}

int SmartCar::GetDirection() { return  _pwm->getPWM(channel_direction); }
void SmartCar::SetDirection(int val) { 
    if(val <= DIRECTION_MAX || val >= DIRECTION_MIN)
      _pwm->setPWM(channel_direction, 0, val); 
}

int SmartCar::GetMotor() { return 0; }
void SmartCar::SetMotor(int val) { }

int SmartCar::GetGyro() { return 0; }

void SmartCar::Stop() {
  SetMotor(0);
  SetDirection(0);
}

void SmartCar::SetAllSensors(Sensor& config) {
  SetMotor(config.motor);
  SetDirection(config.direction);
  SetPtzX(config.cam_x);
  SetPtzY(config.cam_y);
}

bool SmartCar::DesirializeJsonSensors(String json, Sensor& config) {
  // La taille du buffer JSON est cruciale ! Utilisez l'assistant ArduinoJson
  // https://arduinojson.org/v6/assistant/
  const size_t capacity = JSON_OBJECT_SIZE(4) + 46;
  StaticJsonDocument<capacity> doc;
  DeserializationError error = deserializeJson(doc, json);
 
  if(error) {
    Serial.println("unable to decode the frame, the connection will be closed");
    Stop();
    return false;
  }
 
  config.motor = doc["motor"];
  config.direction = doc["direction"];
  config.cam_x = doc["cam_x"];
  config.cam_y = doc["cam_y"];

  return true;
}

