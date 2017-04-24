const int led = 13;

//Motor A (Left)
const int PWMA = 3; //Speed control 
const int AIN1 = 6; //Direction
const int AIN2 = 7; //Direction

//Motor B (Right)
const int PWMB = 5; //Speed control
const int BIN1 = 10; //Direction
const int BIN2 = 11; //Direction

const int STBY = 4; //standby

const int JoyH = 0;
const int JoyV = 1;
//const int JoyS = 3;

long Xinv, Y;

int V, W; // V = R+L; W = R-L;

int velL, velR;

// for motorMove func
boolean dir1;
boolean dir2;

void setup() {
  // H-Bridge
  pinMode(STBY, OUTPUT);
  
  pinMode(PWMA, OUTPUT);
  pinMode(AIN1, OUTPUT);
  pinMode(AIN2, OUTPUT);
  
  pinMode(PWMB, OUTPUT);
  pinMode(BIN1, OUTPUT);
  pinMode(BIN2, OUTPUT);
  
  //DEBUG LED
  pinMode(led, OUTPUT);
  digitalWrite(led, LOW);
  
  pinMode(JoyH, INPUT);
  pinMode(JoyV, INPUT);
  //pinMode(JoyS, INPUT);

  Serial.begin(115200);
}

void moveForward(){
  moveLR(255,255);
}

void moveBackward(){
  moveLR(-255,-255);
}

void moveLeft(){
  moveLR(0,255);
}

void moveRight(){
  moveLR(255,0);
}


void motorMove(int motor, int speed, int direction){
//Move specific motor at speed and direction
//motor: 0 for A 1 for B
//speed: 0 is off, and 255 is full speed
//direction: 0 clockwise, 1 counter-clockwise

  digitalWrite(STBY, HIGH); //disable standby

  dir1 = LOW;
  dir2 = HIGH;

  if(direction == 1){
    dir1 = HIGH;
    dir2 = LOW;
  }

  if(motor == 0){
    digitalWrite(AIN1, dir1);
    digitalWrite(AIN2, dir2);
    analogWrite(PWMA, speed);
  }else{
    digitalWrite(BIN1, dir1);
    digitalWrite(BIN2, dir2);
    analogWrite(PWMB, speed);
  }
}

//recieves (-256,-256) <= (velL,velR) <= (256,256)
void moveLR(int velL, int velR){
  motorMove(1, velL>=0 ? velL : -velL, velL>0 ? 0 : 1);
  motorMove(0, velR>=0 ? velR : -velR, velR>0 ? 0 : 1);
}

void stop(){
//enable standby  
  digitalWrite(STBY, LOW); 
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
  
  moveLR(velL, velR);
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
