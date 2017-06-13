#include <Servo.h> 

const int led = 13;
const int servoPin = 2;
const int potPin = 2;
int potVal = 0;
int servoVal = 0;

int potRead;

Servo myServo;

void setup() {
  pinMode(led, OUTPUT);
  myServo.attach(servoPin);

  Serial.begin(115200);
}

void loop() {
  // (0 - 1023)
  potVal =analogRead(potPin);
  // (0 - 180)
  servoVal = map(potVal, 0, 1023, 0, 180);
  Serial.print("potVal: ");
  Serial.println(potVal);
  Serial.print("servoVal: ");
  Serial.println(servoVal);
  
  myServo.write(servoVal);
  digitalWrite(led, HIGH);
}
