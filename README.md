# SmartCar project

## Building SmartCarFirmware (using esp8266)

### Step 1: interfacing the ESC brushless motor

connectining the esc (3 wires using jumpers) to arduino using theses pins:

1.VCC to 5v pin 

2.ground to ground 

3.PWM to D5 - GPIO14 (SCLK) 


    #include <Servo.h>
    // ------------------------------------------------------------------------
    // Customize here pulse lengths as needed
    #define MIN_PULSE_LENGTH 1000 // Minimum pulse length in µs
    #define MAX_PULSE_LENGTH 2000 // Maximum pulse length in µs
    // ---------------------------------------------------------------------------
    Servo mot;
    char data;
    // ---------------------------------------------------------------------------
    
    /**
     * Initialisation routine
     */
    void setup() {
        Serial.begin(9600);
        mot.attach(D5, MIN_PULSE_LENGTH, MAX_PULSE_LENGTH);
        delay(5000);
        //-------------------------------------
        Serial.println("Sending minimum throttle");
        mot.writeMicroseconds(MIN_PULSE_LENGTH);
        delay(2500);
        //-------------------------------------
        Serial.println("Sending maximum throttle");
        mot.writeMicroseconds(MAX_PULSE_LENGTH);
        //-------------------------------------
    }
    
    void loop()
    {
        delay(2000);
        test();
    }
    
    void test()
    {
        Serial.print("Pulse length = 1500");        
        mot.writeMicroseconds(1500);
        delay(5000);
        
        Serial.print("Pulse length = 1000");        
        mot.writeMicroseconds(1000);
        delay(5000);
    
        Serial.print("Pulse length = 700");        
        mot.writeMicroseconds(700);
        delay(5000);
    }


### Step 2: Testing I2C with PCA9685 and ESP8266 D1 MINI

A. Connect the power DC 7/11 V to LM5675
B. Connect the external alimentation to PCA9685.
C. Connect the 5V/ground
D. connect the D2 to SDA and D1 to SCL
C. Scan and try to detect the address 0x40 (I found 2 address 0x40 and 0x70)
    
    // --------------------------------------
    // i2c_scanner
    //
    // Version 1
    //    This program (or code that looks like it)
    //    can be found in many places.
    //    For example on the Arduino.cc forum.
    //    The original author is not know.
    // Version 2, Juni 2012, Using Arduino 1.0.1
    //     Adapted to be as simple as possible by Arduino.cc user Krodal
    // Version 3, Feb 26  2013
    //    V3 by louarnold
    // Version 4, March 3, 2013, Using Arduino 1.0.3
    //    by Arduino.cc user Krodal.
    //    Changes by louarnold removed.
    //    Scanning addresses changed from 0...127 to 1...119,
    //    according to the i2c scanner by Nick Gammon
    //    http://www.gammon.com.au/forum/?id=10896
    // Version 5, March 28, 2013
    //    As version 4, but address scans now to 127.
    //    A sensor seems to use address 120.
    // Version 6, November 27, 2015.
    //    Added waiting for the Leonardo serial communication.
    // 
    //
    // This sketch tests the standard 7-bit addresses
    // Devices with higher bit address might not be seen properly.
    //
    
    #include <Wire.h>
    
    
    void setup()
    {
      //ZWire.begin(); // Arduino
      Wire.begin(D2, D1); // ESP8266
    
      Serial.begin(9600);
      while (!Serial);             // Leonardo: wait for serial monitor
      Serial.println("\nI2C Scanner");
    }
    
    
    void loop()
    {
      byte error, address;
      int nDevices;
    
      Serial.println("Scanning...");
    
      nDevices = 0;
      for(address = 1; address < 127; address++ ) 
      {
        // The i2c_scanner uses the return value of
        // the Write.endTransmisstion to see if
        // a device did acknowledge to the address.
        Wire.beginTransmission(address);
        error = Wire.endTransmission();
    
        if (error == 0)
        {
          Serial.print("I2C device found at address 0x");
          if (address<16) 
            Serial.print("0");
          Serial.print(address,HEX);
          Serial.println("  !");
    
          nDevices++;
        }
        else if (error==4) 
        {
          Serial.print("Unknown error at address 0x");
          if (address<16) 
            Serial.print("0");
          Serial.println(address,HEX);
        }    
      }
      if (nDevices == 0)
        Serial.println("No I2C devices found\n");
      else
        Serial.println("done\n");
    
      delay(5000);           // wait 5 seconds for next scan
    }


### Step 3: Interfacing the Servo

A. connect servo motor to channel 0.


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
        SetAngle(160.0, 0);
        delay(5000);
        SetAngle(90.0, 0);
    }
    
    void SetAngle(float angle, int channel) {
        float pulselength = map(angle, 0, 180, SERVOMIN, SERVOMAX);
        pwm.setPWM(channel, 0, pulselength);
    }

### step 4 interfacing servo and motor bldc with ESC inside PCA9685

#include <Wire.h>
#include <Adafruit_PWMServoDriver.h>

Adafruit_PWMServoDriver pwm = Adafruit_PWMServoDriver();

#define SERVOMIN  150 // This is the 'minimum' pulse length count (out of 4096)
#define SERVOMAX  600 // This is the 'maximum' pulse length count (out of 4096)
#define USMIN  600    // This is the rounded 'minimum' microsecond length based on the minimum pulse of 150
#define USMAX  2400   // This is the rounded 'maximum' microsecond length based on the maximum pulse of 600
#define SERVO_FREQ 50 // Analog servos run at ~50 Hz updates
#define MOTOR_STOP_PULSE_LENGTH 1500 // Stop pulse length in µs

void setup()
{
  Wire.begin(D2, D1); // ESP8266 
  Serial.begin(9600);
  while (!Serial); // wait serial
  while (!CheckI2C(0x40)); // wait I2C
  InitializePCA9685();
  ArmMotor();
}

void ArmMotor() {
    //-------------------------------------
    Serial.println("Arming the motor with MOTOR_STOP_PULSE_LENGTH ");
    pwm.writeMicroseconds(1, MOTOR_STOP_PULSE_LENGTH);
    delay(7000);
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
    delay(1000);
    SetAngle(90.0, 0);
    delay(1000);
    SetAngle(130.0, 0);
    delay(1000);
    SetAngle(90.0, 0);

    Serial.println("Sending minimum throttle - 1589");
    pwm.writeMicroseconds(1, 1589);
    delay(1000);
    
    Serial.println("Sending minimum throttle - 1500");
    pwm.writeMicroseconds(1, MOTOR_STOP_PULSE_LENGTH);
    delay(1000);
}

void SetAngle(float angle, int channel) {
    float pulselength = map(angle, 0, 180, SERVOMIN, SERVOMAX);
    pwm.setPWM(channel, 0, pulselength);
}


### Step5: using radiolinks receiver to forward informations into PCA9685

void setup() {
  pinMode(D0, INPUT);
  pinMode(D5, INPUT);
  pinMode(D6, INPUT);
  pinMode(D7, INPUT);
  Serial.begin(9600);
}

void loop() {
  int pwm_0 = pulseIn(D0, HIGH);
  int pwm_1 = pulseIn(D5, HIGH);
  int pwm_2 = pulseIn(D6, HIGH);
  int pwm_3 = pulseIn(D7, HIGH);

  Serial.print("PWM 0 : ");
  Serial.println(pwm_0);

  Serial.print("PWM 1 : ");
  Serial.println(pwm_1);

  Serial.print("PWM 2 : ");
  Serial.println(pwm_2);

  Serial.print("PWM 3 : ");
  Serial.println(pwm_3);

  delay(500);
}



## Documentation

### Mount the car (motor and chassis)

Full build : https://www.youtube.com/watch?v=5jaY4WWStfw&t=1752s

Full build (with upgrade): https://www.youtube.com/watch?v=5jaY4WWStfw&t=1752s

gear ratio explaination : https://youtu.be/xz_7wudsCxA?si=6EfOzw9CyRQAZsRG

tt02 gear ratio 1 : https://youtu.be/sZeNW9iXfhA?si=DJAV-P7mNhNqOBhw

tt02 gear ratio 2 (140km/h) (french speed run)
https://youtu.be/VqwL-JDO_k4?si=ZbUx-5rV4UbFxDs0

tt02 gear ratio 3: (200km/h) :
https://youtu.be/H_VMgpEubjk?si=jDgYzwlfr2YrgA6w

### explaination about electronics 

Brushless ESC : https://youtu.be/yiD5nCfmbV0?si=4gDMlUopPC1GDRFV

servo motor:
https://youtu.be/6sWHo6GXQTc?si=A46Mng9aF-uA64HG

## Hardware part:
| Item              | Price    | Link |
| :---------------- | :------: | ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------: |
chassis TT02-R (carbon-alu) | 120€ |  https://fr.aliexpress.com/item/1005007610264496.html?spm=a2g0o.productlist.main.12.76a7TJbKTJbKoo&algo_pvid=63f3f2d3-7832-4c45-9358-9b56eefea020&algo_exp_id=63f3f2d3-7832-4c45-9358-9b56eefea020-11&pdp_ext_f=%7B%22order%22%3A%2238%22%2C%22eval%22%3A%221%22%7D&pdp_npi=4%40dis%21EUR%21329.36%21108.69%21%21%212664.72%21879.36%21%402103867617500937454246013e371a%2112000041496358494%21sea%21BE%213294809779%21X&curPageLogUid=ZWm2PIl0acht&utparam-url=scene%3Asearch%7Cquery_from%3A
motor SurpassHobby (60A)+card | 33€ | https://fr.aliexpress.com/item/1005006742987156.html?spm=a2g0o.productlist.main.2.4ac16a93vRI7ax&algo_pvid=ae2e9803-f77a-4600-9eb1-5af927e05599&aem_p4p_detail=202506161012533678679071203120000042013&algo_exp_id=ae2e9803-f77a-4600-9eb1-5af927e05599-1&pdp_ext_f=%7B%22order%22%3A%221755%22%2C%22eval%22%3A%221%22%7D&pdp_npi=4%40dis%21EUR%2197.82%2127.39%21%21%21791.42%21221.60%21%40211b876e17500939734093277e5acd%2112000038163192471%21sea%21BE%213294809779%21X&curPageLogUid=SaZgQMfJ4yLL&utparam-url=scene%3Asearch%7Cquery_from%3A&search_p4p_id=202506161012533678679071203120000042013_1
Servo MG996R | 15€ | https://fr.aliexpress.com/item/1005007395940830.html?spm=a2g0o.productlist.main.1.e8db1248nHliAb&algo_pvid=c6ef145c-b931-4659-b939-4924d33599bd&algo_exp_id=c6ef145c-b931-4659-b939-4924d33599bd-0&pdp_ext_f=%7B%22order%22%3A%22579%22%2C%22eval%22%3A%221%22%7D&pdp_npi=4%40dis%21EUR%2157.11%2115.99%21%21%21462.06%21129.37%21%40211b81a317500940457661297e4a8a%2112000040576263131%21sea%21BE%213294809779%21X&curPageLogUid=zNX3LP8QvdrS&utparam-url=scene%3Asearch%7Cquery_from%3A
Radio RC6GS V3 R7FG | 56€ | https://fr.aliexpress.com/item/1005005190722412.html?spm=a2g0o.productlist.main.6.44afWD9lWD9luq&algo_pvid=57644be9-5321-4dba-a2b3-ed7f71073b1d&algo_exp_id=57644be9-5321-4dba-a2b3-ed7f71073b1d-5&pdp_ext_f=%7B%22order%22%3A%22875%22%2C%22eval%22%3A%221%22%7D&pdp_npi=4%40dis%21EUR%21163.53%2145.79%21%21%211323.09%21370.47%21%40211b61bb17500941465907656e5366%2112000032058485923%21sea%21BE%213294809779%21X&curPageLogUid=Cs8yVya946Mv&utparam-url=scene%3Asearch%7Cquery_from%3A


## Electronic parts
| Item              | Price    | Link |
| :---------------- | :------: | ---: |
raspberry pi 3b+  | 50€      | https://shop.mchobby.be/fr/cartes-meres/1326-raspberry-pi-3-b-plus-de-stock--3232100013261.html?src=raspberrypi |
ESP8266 D1 mini   |  5€      | D1 mini type C https://fr.aliexpress.com/item/1005006018009983.html?spm=a2g0o.productlist.main.1.36a35e1dN388i4&algo_pvid=6f36d003-d340-4944-aac7-bcdfb09d4ee2&algo_exp_id=6f36d003-d340-4944-aac7-bcdfb09d4ee2-0&pdp_ext_f=%7B%22order%22%3A%22975%22%2C%22eval%22%3A%221%22%7D&pdp_npi=4%40dis%21EUR%217.72%212.62%21%21%2162.50%2121.25%21%4021038e1e17500946019538144e9912%2112000035516008090%21sea%21BE%213294809779%21X&curPageLogUid=vGzTPk04ObO7&utparam-url=scene%3Asearch%7Cquery_from%3A 
LM7805            | 2€       | https://fr.aliexpress.com/item/1005006018009983.html?spm=a2g0o.productlist.main.1.36a35e1dN388i4&algo_pvid=6f36d003-d340-4944-aac7-bcdfb09d4ee2&algo_exp_id=6f36d003-d340-4944-aac7-bcdfb09d4ee2-0&pdp_ext_f=%7B%22order%22%3A%22975%22%2C%22eval%22%3A%221%22%7D&pdp_npi=4%40dis%21EUR%217.72%212.62%21%21%2162.50%2121.25%21%4021038e1e17500946019538144e9912%2112000035516008090%21sea%21BE%213294809779%21X&curPageLogUid=vGzTPk04ObO7&utparam-url=scene%3Asearch%7Cquery_from%3A
PCA9685 (pwm driver) | 3€ | https://fr.aliexpress.com/item/1005007463410769.html?spm=a2g0o.productlist.main.1.12fe61f081PoTI&algo_pvid=2e555d53-31d9-4a82-bcd7-1c86b389a75f&algo_exp_id=2e555d53-31d9-4a82-bcd7-1c86b389a75f-0&pdp_ext_f=%7B%22order%22%3A%2212%22%2C%22eval%22%3A%221%22%7D&pdp_npi=4%40dis%21EUR%2112.63%213.41%21%21%21102.22%2127.60%21%40211b61d017500947473232504e5ddc%2112000040858588834%21sea%21BE%213294809779%21X&curPageLogUid=DMC6PQU9vEyV&utparam-url=scene%3Asearch%7Cquery_from%3A
Camera Raspberry pi | 3€ | https://fr.aliexpress.com/item/1005003386791483.html?spm=a2g0o.productlist.main.11.52124185rhdlPS&algo_pvid=a3403bdd-846b-4692-9dfa-2fedbec6b804&algo_exp_id=a3403bdd-846b-4692-9dfa-2fedbec6b804-10&pdp_ext_f=%7B%22order%22%3A%22834%22%2C%22eval%22%3A%221%22%7D&pdp_npi=4%40dis%21EUR%214.20%213.91%21%21%214.73%214.40%21%40210384b217500948590273795e0cef%2112000034511448969%21sea%21BE%213294809779%21X&curPageLogUid=udnkhLCQ3K52&utparam-url=scene%3Asearch%7Cquery_from%3A
Camera Mount ptz kit | 3€ | https://fr.aliexpress.com/item/1005007479053402.html?spm=a2g0o.detail.pcDetailTopMoreOtherSeller.11.31b9b31nb31nZJ&gps-id=pcDetailTopMoreOtherSeller&scm=1007.14452.396806.0&scm_id=1007.14452.396806.0&scm-url=1007.14452.396806.0&pvid=80e05349-4a08-4488-93fc-d987408fbac8&_t=gps-id:pcDetailTopMoreOtherSeller,scm-url:1007.14452.396806.0,pvid:80e05349-4a08-4488-93fc-d987408fbac8,tpp_buckets:668%232846%238114%231999&pdp_ext_f=%7B%22order%22%3A%22233%22%2C%22eval%22%3A%221%22%2C%22sceneId%22%3A%2230050%22%7D&pdp_npi=4%40dis%21EUR%213.36%212.99%21%21%2127.18%2124.19%21%402103892f17500949716868514e1389%2112000040920567014%21rec%21BE%213294809779%21X&utparam-url=scene%3ApcDetailTopMoreOtherSeller%7Cquery_from%3A&search_p4p_id=202506161029317266739719906029798847_10
