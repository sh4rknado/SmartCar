#ifndef SmartCarConfig_h
#define SmartCarConfig_h

#include "Arduino.h"

struct SmartCarConfig
{
    int cam_channel_x;          // default 3
    int cam_channel_y;          // default 2
    int direction_channel;      // default 0 
    int motor_channel;          // default 1
    int sda_pin;                // D2 SDA pin 4 esp8266
    int scl_pin;                // D1 SCL pin 5 esp8266
    int servo_freq;             // 50 Analog servos run at ~50 Hz updates
    int servo_pulse_min;        // 150 This is the 'minimum' pulse length count (out of 4096) 
    int servo_pulse_max;        // 600 This is the 'maximum' pulse length count (out of 4096)
    int motor_pulse_stop;       // 1500 default BRUSHLESS_PULSE_STOP
    int direction_angle_max;    // 130 degree
    int direction_angle_min;    // 10 degree
    int camera_x_angle_min;     //
    int camera_x_angle_max;     //
    int camera_y_angle_min;     //
    int camera_y_angle_max;     //
    byte i2c_pca9685_address;   // default address I2C 0x40
};

#endif