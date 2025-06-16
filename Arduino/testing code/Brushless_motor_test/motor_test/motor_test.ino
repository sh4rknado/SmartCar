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
    Serial.print("Pulse length = 1500 - motor stop");        
    mot.writeMicroseconds(1500);
    delay(5000);
    
    Serial.print("Pulse length = 1000");        
    mot.writeMicroseconds(1000);
    delay(5000);

    Serial.print("Pulse length = 700");        
    mot.writeMicroseconds(700);
    delay(5000);
}