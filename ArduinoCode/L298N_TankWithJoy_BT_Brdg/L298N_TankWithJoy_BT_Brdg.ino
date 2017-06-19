//======================== Init ========================
//--------------- General ---------------
#include<SoftwareSerial.h>
const int led = 13;

//--------------- Motors ---------------
//Motor A (Left)
const int PWMA = 3; //Speed control 
const int AIN1 = 6; //Direction
const int AIN2 = 7; //Direction

//Motor B (Right)
const int PWMB = 5; //Speed control
const int BIN1 = 10; //Direction
const int BIN2 = 11; //Direction

const int STBY = 4; //standby

//--------------- Motor Move Func ---------------
boolean dir1;
boolean dir2;
const int L_minThr = -10;
const int L_maxThr = 10;
const int R_minThr = -10;
const int R_maxThr = 10;

//--------------- BT ---------------
const int BT_rx = 8;
const int BT_tx = 9;
char BT_data = 0;
SoftwareSerial BTserial(BT_rx, BT_tx);

//--------------- BT Parse ---------------
const byte numChars = 32;
char receivedChars[numChars];
char tempChars[numChars];        // temporary array for use when parsing

// variables to hold the parsed data
char BT_joyMsg[numChars] = {'S'};
int BT_joyH = 512;
int BT_joyV = 512;

boolean newData = false;
boolean BT_echo = false;                   //******** PARAMETER ******** echo BT to Serial console (disables parsing)
                                          //use baud 115200 in Serial Console
boolean BT_enabled = true;                //******** PARAMETER ******** BT or Joystick flag
unsigned long previousMillis = 0;         // will store last time BT data was updated
unsigned long currentMillis = 0;
const long BT_timeout = 2000;             // BT timeout (milliseconds). Zero H,V values after timeout.

//--------------- Joystick ---------------
const int joyHpin = 0;
const int joyVpin = 1;
//const int JoySpin = 3;
int joyH = 512, joyV = 512;
char joyMsg[numChars] = {'S'};
long Xinv, X, Y;
int V, W; // V = R+L; W = R-L;
int velL, velR;

//--------------- Bridge Servo --------------- 
#include <Servo.h>
const int servoPin = 12;
int servoVal = 0;
bool servo_already_stay = false;
Servo brdgServo;

//======================== Setup ========================

void setup() {
  //--------------- H-Bridge ---------------
  pinMode(STBY, OUTPUT);
  
  pinMode(PWMA, OUTPUT);
  pinMode(AIN1, OUTPUT);
  pinMode(AIN2, OUTPUT);
  
  pinMode(PWMB, OUTPUT);
  pinMode(BIN1, OUTPUT);
  pinMode(BIN2, OUTPUT);
  
  //--------------- LED ---------------
  pinMode(led, OUTPUT);
  //digitalWrite(led, LOW);

  //--------------- Joystick ---------------
  pinMode(joyHpin, INPUT);
  pinMode(joyVpin, INPUT);
  //pinMode(joySpin, INPUT);

  //--------------- Bridge Servo --------------- 
  brdgServo.attach(servoPin);

  //--------------- Serial: PC <-> Arduino ---------------
  Serial.begin(115200);
  while (!Serial) {
    ; // wait for serial port to connect. Needed for native USB port only
  }
  Serial.println("Arduino -> PC :)");

  //--------------- Serial: Arduino <-> BT ---------------
  BTserial.begin(9600);
  BTserial.println("Arduino -> BT :)");
  BTserial.println("Expecting: <[Msg], [Hor], [Ver]>");
  BTserial.println();
}


//======================== Loop ========================

void loop() {
  if(BT_echo){ // echo BT msgs for debug purposes
    BT_to_PC();
    PC_to_BT(); 
  }
  else{
    if(BT_enabled){ 
      BT_to_MHV();   // read MHV from BT
    } else {        
      read_JoyHV(); // read HV from joystick
      strcpy(joyMsg, "S");
    }               // joyH, joyV, joyMsg  
    
    //print_HV();
    //print_MHV();
    
    HV_to_LR();     //velL, velR
  
    //print_LR();
  
    LR_to_Motors(velL, velR);

    M_to_Servo(joyMsg);
  }
}


//======================== Functions ========================
//--------------- Joystick -> HV ---------------
void read_JoyHV(){
  joyH = analogRead(joyHpin);
  joyV = analogRead(joyVpin);
}


//--------------- HV -> LR ---------------
// HV: (0-1023) => LR: (-255,255)
void HV_to_LR(){  
  //Xinv: (0-1023) -> (-255,...,0,1,..,256)
  //   X: (0-1023) -> (-256,..,-1,0,..,255)
  //   Y: (0-1023) -> (-255,...,0,1,..,256)
  Xinv = joyH/2-255;
  X = -Xinv;
  Y = joyV/2-255;
  
  //X: (-256,..,-1,0,..,255) -> (-255,..,0,0,..255)
  //Y: (-255,...,0,1,..,256) -> (-255,..,0,0,..255)
  X = (X < 0) ? (X + 1) : X;
  Y = (Y > 0) ? (Y - 1) : Y;
  
  V = (255-abs(X))*Y/255 + Y;
  W = (255-abs(Y))*X/255 + X;

  //vel: (-255,..,0,0,..255)
  velL = (V-W)/2;
  velR = (V+W)/2;
}


//--------------- LR -> Motors ---------------
// (-255,-255) <= (velL,velR) <= (255,255)
void LR_to_Motors(int velL, int velR){
  //if ((L_minThr <= velL) && (velL <= L_maxThr) && (R_minThr <= velR) && (velR <= R_maxThr)){  // Joystick center => Motors standby
  //  digitalWrite(led, LOW);
  //  motorsStop();
  //} else {                                                                                    // Joystick move => Motors move
    digitalWrite(led, HIGH);
    motorMove(1, velL>=0 ? velL : -velL, velL>0 ? 0 : 1);
    motorMove(0, velR>=0 ? velR : -velR, velR>0 ? 0 : 1);  
  //}
}


//--------------- Msg -> Bridge Servo ---------------
void M_to_Servo(char* joyMsg) {
  if (joyMsg[0] == 'S') {
    if(servo_already_stay){
      pinMode(servoPin, INPUT); // "switch off" the servo
      //Nothing to do. Already set to 90 degrees
    } else {
      Serial.println("Stay");
      pinMode(servoPin, OUTPUT); // "switch on" the servo
      brdgServo.write(90);
      servo_already_stay = true;
    }
  } else if (joyMsg[0] == 'U') {
    //Serial.println("Up");
    pinMode(servoPin, OUTPUT); // "switch on" the servo
    brdgServo.write(100);
    servo_already_stay = false;
  } else if (joyMsg[0] == 'D') {
    Serial.println("Down");
    pinMode(servoPin, OUTPUT); // "switch on" the servo
    brdgServo.write(80);
    servo_already_stay = false;
  }
}


//--------------- Print HV ---------------
void print_HV(){
  Serial.print("joyH: ");
  Serial.println(joyH);
  Serial.print("joyV: ");
  Serial.println(joyV);
  Serial.println();
}


//--------------- Print LR ---------------
void print_LR(){
  Serial.print("velL: ");
  Serial.println(velL);
  Serial.print("velR: ");
  Serial.println(velR);
  Serial.println();
}


//--------------- Move Motors ---------------
void motorsStop(){
//enable standby  
  digitalWrite(STBY, LOW); 
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


//--------------- Motors Debug ---------------
// Simple debug functions
void moveForward(){
  LR_to_Motors(255,255);
}

void moveBackward(){
  LR_to_Motors(-255,-255);
}

void moveLeft(){
  LR_to_Motors(0,255);
}

void moveRight(){
  LR_to_Motors(255,0);
}

void fullRight(){
  LR_to_Motors(-255,255);
}


//======================== BT Functions ========================
//--------------- BT -> MHV ---------------
void BT_to_MHV(){
  recvWithStartEndMarkers();
  currentMillis = millis();
  if (newData == true) {
    previousMillis = currentMillis; 
    strcpy(tempChars, receivedChars);
            // this temporary copy is necessary to protect the original data
            //   because strtok() used in parseData() replaces the commas with \0
    parseData_to_MHV();
    //print_MHV();
    joyH = constrain(BT_joyH, 0, 1023);
    joyV = constrain(BT_joyV, 0, 1023);
    strcpy(joyMsg, BT_joyMsg);
    newData = false;
  }
  if (currentMillis - previousMillis >= BT_timeout){  // no data recieved for at least BT_timeout ms. Center VH.
    joyH = 512;
    joyV = 512;
    strcpy(joyMsg, "S");
  }
}


void recvWithStartEndMarkers() {
    static boolean recvInProgress = false;
    static byte ndx = 0;
    char startMarker = '*';
    char endMarker = '#';
    char rc;

    while (BTserial.available() > 0 && newData == false) {
        rc = BTserial.read();

        if (recvInProgress == true) {
            if (rc != endMarker) {
                receivedChars[ndx] = rc;
                ndx++;
                if (ndx >= numChars) {
                    ndx = numChars - 1;
                }
            }
            else {
                receivedChars[ndx] = '\0'; // terminate the string
                recvInProgress = false;
                ndx = 0;
                newData = true;
            }
        }

        else if (rc == startMarker) {
            recvInProgress = true;
        }
    }
}


void parseData_to_MHV() {      // split the data into its parts

    char * strtokIndx; // this is used by strtok() as an index

    strtokIndx = strtok(tempChars,",");   // get the first part - the string
    strcpy(BT_joyMsg, strtokIndx);    // copy it to BT_joyMsg
 
    strtokIndx = strtok(NULL, ",");       // this continues where the previous call left off
    BT_joyH = atoi(strtokIndx);  // convert joystick horizontal to integer

    strtokIndx = strtok(NULL, ",");
    BT_joyV = atoi(strtokIndx);  // convert joystick vertical to integer

}


void print_MHV() {
    Serial.print("Msg: ");
    Serial.println(BT_joyMsg);
    Serial.print("joyH: ");
    Serial.println(BT_joyH);
    Serial.print("joyV: ");
    Serial.println(BT_joyV);
}

//--------------- Serial echo PC <-> BT ---------------

void PC_to_BT(){
  // PC -> BT
  while (Serial.available() > 0) {
    delay(1);
    if (Serial.available() > 0)
    {
      Serial.println(Serial.available());
      BT_data = Serial.read();
      Serial.write(BT_data);
      BTserial.write(BT_data);
    }
  }
}

void BT_to_PC(){
  while (BTserial.available() > 0) {
    BT_data = BTserial.read();
    Serial.write(BT_data);
    // send this POS further
  }
}
