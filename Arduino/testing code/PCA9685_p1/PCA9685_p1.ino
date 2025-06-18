#include <Wire.h>
#include <Adafruit_PWMServoDriver.h>

Adafruit_PWMServoDriver pwm = Adafruit_PWMServoDriver();

#define SERVOMIN  150 // This is the 'minimum' pulse length count (out of 4096)
#define SERVOMAX  600 // This is the 'maximum' pulse length count (out of 4096)
#define USMIN  600 // This is the rounded 'minimum' microsecond length based on the minimum pulse of 150
#define USMAX  2400 // This is the rounded 'maximum' microsecond length based on the maximum pulse of 600
#define SERVO_FREQ 50 // Analog servos run at ~50 Hz updates

void setup()
{
  Wire.begin(D2, D1); // ESP8266 
  Serial.begin(9600);
  while (!Serial); // wait serial
  while (!CheckI2C(0x40)); // wait I2C
  InitializePCA9685();
}

bool CheckI2C(byte address) {
    Serial.println("Scanning...");
    Wire.beginTransmission(address);
    byte error = Wire.endTransmission();
    
    if(error == 0)
        Serial.println("I2C found at address 0x40...");

    return error == 0;
    delay(500);
}

void InitializePCA9685() {
    pwm.begin();
    pwm.setOscillatorFrequency(27000000);
    pwm.setPWMFreq(SERVO_FREQ);  // Analog servos run at ~50 Hz updates
    delay(10);
}

void loop() { 
    SetAngle(10.0, 0);
    delay(5000);
    SetAngle(90.0, 0);
    delay(5000);
    SetAngle(130.0, 0);
    delay(5000);
    SetAngle(90.0, 0);
}

void SetAngle(float angle, int channel) {
    float pulselength = map(angle, 0, 180, SERVOMIN, SERVOMAX);
    pwm.setPWM(channel, 0, pulselength);
}
