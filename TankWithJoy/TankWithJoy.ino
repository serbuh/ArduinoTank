const int led = 13;
const int HBEn1 = 2;
const int HBEn2 = 3;

//Left motor
const int LF = 5;
const int LB = 6;
//Right motor
const int RF = LF + 5;
const int RB = LB + 5;

typedef enum {LEFT, RIGHT} Side;

const int JoyH = 0;
const int JoyV = 1;
//const int JoyS = 3;

int Xval;
int Yval;


void setup() {
  // set all the other pins you're using as outputs:
  pinMode(HBEn1, OUTPUT);
  pinMode(HBEn2, OUTPUT);
  pinMode(LF, OUTPUT);
  pinMode(LB, OUTPUT);
  pinMode(RF, OUTPUT);
  pinMode(RB, OUTPUT);
  digitalWrite(HBEn1, HIGH);
  digitalWrite(HBEn2, HIGH);
  
  //DEBUG LED
  pinMode(led, OUTPUT);
  digitalWrite(led, LOW);
  
  pinMode(JoyH, INPUT);
  pinMode(JoyV, INPUT);
  //pinMode(JoyS, INPUT);

  Serial.begin(115200);
}

//recieves (-255,-255) <= (velL,velR) <= (255,255)
void motorMove(int velL, int velR){
  analogWrite(LF, velL>0 ? velL : 0);
  analogWrite(LB, velL<0? -velL : 0);
  analogWrite(RF, velR>0 ? velR : 0);
  analogWrite(RB, velR<0 ? -velR : 0);
}


void moveForward(){
  motorMove(255,255);
}

void moveBackward(){
  motorMove(-255,-255);
}

void moveLeft(){
  motorMove(200,255);
}

void moveRight(){
  motorMove(255,200);
}


void loop() {
/*
  digitalWrite(led, HIGH);
  moveForward();
  
  delay(3000);
  digitalWrite(led, LOW);
  moveBackward();
  
  delay(3000);
  moveRight();

  delay(3000);
  moveLeft();
  
  delay(3000);
*/
  
  /*
  Serial.print("Switch:  ");
  Serial.print(digitalRead(JoyS));
  Serial.print("\n");
  */

  Xval = analogRead(JoyH)/2-255;
  Yval = analogRead(JoyV)/2-255;
  
  Serial.print("X: ");
  Serial.print(Xval);
  Serial.print("\n");
  Serial.print("Y: ");
  Serial.println(Yval);
  Serial.print("\n\n");
  motorMove(-Xval, Yval);
  delay(500);
  
}
