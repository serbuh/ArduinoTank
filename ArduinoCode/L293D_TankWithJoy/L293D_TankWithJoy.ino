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

long Xinv, Y;

int V, W; // V = R+L; W = R-L;

int velL, velR;

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

//recieves (-256,-256) <= (velL,velR) <= (256,256)
void motorMove(int velL, int velR){
  analogWrite(LF, velL>0 ?  velL : 0);
  analogWrite(LB, velL<0 ? -velL : 0);
  analogWrite(RF, velR>0 ?  velR : 0);
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



void JoyToMotor(){
  Xinv = (analogRead(JoyH)/2-256);
  Y = analogRead(JoyV)/2-256;

  Xinv=(Xinv == -256) ? 255 : -Xinv;
  Y=(Y == -256) ? -255 : Y;
  
  Serial.print("Xinv: ");
  Serial.println(Xinv);
  Serial.print("Y: ");
  Serial.println(Y);

/*
  Serial.print("abs(Xinv): ");
  Serial.println((abs(Xinv)));
  Serial.print("256-abs(Xinv): ");
  Serial.println((256-abs(Xinv)));
  Serial.print("(256-abs(Xinv)*Y/256): ");
  Serial.println((256-abs(Xinv)*Y/256));
*/

  V = (256-abs(Xinv))*Y/256 + Y;
  W = (256-abs(Y))*Xinv/256 + Xinv;

  Serial.print("V: ");
  Serial.println(V);
  Serial.print("W: ");
  Serial.println(W);

  velL = (V-W)/2;
  velR = (V+W)/2;

  Serial.print("velL: ");
  Serial.println(velL);
  Serial.print("velR: ");
  Serial.println(velR);
  Serial.println();
  
  motorMove(velL, velR);
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

  JoyToMotor();
  /*velL = 255;
  velR = -255;
  //motorMove(velL, velR);
  digitalWrite(LF, HIGH);
  digitalWrite(LB, LOW);
  digitalWrite(RF, LOW);
  digitalWrite(RB, HIGH);
  
  delay(1000);

  velL = -510;
  velR = 510;
  //motorMove(velL, velR);
  analogWrite(LF, 0);
  analogWrite(LB, -velL);
  analogWrite(RF, velR);
  analogWrite(RB, 0);
  
  delay(1000);
  */
}
