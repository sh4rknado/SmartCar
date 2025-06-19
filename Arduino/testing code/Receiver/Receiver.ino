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
