const int led = 13;
const int HBEn1 = 2;
const int HBEn2 = 3;

//Right motor
const int HBIn1 = 5;
const int HBIn2 = 6;
//Left motor
const int HBIn3 = HBIn1 + 5;
const int HBIn4 = HBIn2 + 5;

typedef enum {LEFT, RIGHT} Side;

//const int JoyHorzPin = 0;
//const int JoyVertPin = 1;
//const int JoySelPin = 3;

int velocity;


void setup() {
  // set all the other pins you're using as outputs:
  pinMode(HBEn1, OUTPUT);
  pinMode(HBEn2, OUTPUT);
  pinMode(HBIn1, OUTPUT);
  pinMode(HBIn2, OUTPUT);
  pinMode(HBIn3, OUTPUT);
  pinMode(HBIn4, OUTPUT);
  digitalWrite(HBEn1, HIGH);
  digitalWrite(HBEn2, HIGH);
  
  //DEBUG LED
  pinMode(led, OUTPUT);
  digitalWrite(led, LOW);
  
  //pinMode(JoySelPin, INPUT);

}

void motorMove(Side side, int velocity, boolean reverse){
  analogWrite(HBIn1+side*5, velocity * !reverse);
  analogWrite(HBIn2+side*5, velocity * reverse);
}

void holdPosition(){
  motorMove(LEFT,0,false);
  motorMove(RIGHT,0,false);
}

void moveForward(){
  digitalWrite(HBIn1, 0);
  digitalWrite(HBIn2, 0);
  digitalWrite(HBIn3, 0);
  digitalWrite(HBIn4, 0);
}

void moveBackward(){
  motorMove(LEFT,255,true);
  motorMove(RIGHT,255,true);
}

void rotateLeft(){
  motorMove(LEFT,0,false);
  motorMove(RIGHT,255,false);
}

void rotateRight(){
  motorMove(LEFT,255,false);
  motorMove(RIGHT,0,false);
}

void moveLeft(){
  motorMove(LEFT,128,false);
  motorMove(RIGHT,255,false);
}

void moveRight(){
  motorMove(LEFT,255,false);
  motorMove(RIGHT,128,false);
}

void loop() {
  
  digitalWrite(led, HIGH);
  analogWrite(HBIn1, 255);
  analogWrite(HBIn2, 0);
  analogWrite(HBIn3, 255);
  analogWrite(HBIn4, 0);
  
  delay(3000);

  digitalWrite(led, LOW);
  analogWrite(HBIn1, 0);
  analogWrite(HBIn2, 255);
  analogWrite(HBIn3, 0);
  analogWrite(HBIn4, 255);
    
  delay(3000);

  /*
  Serial.print("Switch:  ");
  Serial.print(digitalRead(JoySelPin));
  Serial.print("\n");
  Serial.print("X-axis: ");
  Serial.print(analogRead(JoyHorzPin));
  Serial.print("\n");
  Serial.print("Y-axis: ");
  Serial.println(analogRead(JoyVertPin));
  Serial.print("\n\n");
  delay(500);
  */
}
