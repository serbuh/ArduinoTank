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

//--------------- Joystick ---------------
const int joyHpin = 0;
const int joyVpin = 1;
//const int JoySpin = 3;
int joyH, joyV;
long Xinv, Y;
int V, W; // V = R+L; W = R-L;
int velL, velR;

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
char BT_Msg[numChars] = {0};
int BT_Dir = 0;
int BT_Vel = 0;

boolean newData = false;


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
  digitalWrite(led, LOW);

  //--------------- Joystick ---------------
  pinMode(joyHpin, INPUT);
  pinMode(joyVpin, INPUT);
  //pinMode(joySpin, INPUT);

  //--------------- Serial: PC <-> Arduino ---------------
  Serial.begin(115200);
  while (!Serial) {
    ; // wait for serial port to connect. Needed for native USB port only
  }
  Serial.println("Arduino -> PC :)");

  //--------------- Serial: Arduino <-> BT ---------------
  BTserial.begin(9600);
  BTserial.println("Arduino -> BT :)");
  BTserial.println("Expecting: <[Msg], [Dir], [Vel]>");
  BTserial.println();
}


//======================== Loop ========================

void loop() {
  read_JoyHV(); //joyH, joyV
  HV_to_LR();   //velL, velR
  print_LR();
  LR_to_Motors(velL, velR);
  
  //BTtoDV();
  
  
  
  
  
  /*
  //BT_to_PC();
  //parse();
  */
  
  
}


//======================== Functions ========================

//--------------- Joystick -> HV ---------------
void read_JoyHV(){
  joyH = analogRead(joyHpin);
  joyV = analogRead(joyVpin);
}


//--------------- HV -> LR ---------------
void HV_to_LR(){
  Xinv = joyH/2-256;
  Y = joyV/2-256;

  Xinv=(Xinv == -256) ? 255 : -Xinv;
  Y=(Y == -256) ? -255 : Y;
  /*
  Serial.print("Xinv: ");
  Serial.println(Xinv);
  Serial.print("Y: ");
  Serial.println(Y);
  */
  V = (256-abs(Xinv))*Y/256 + Y;
  W = (256-abs(Y))*Xinv/256 + Xinv;
  /*
  Serial.print("V: ");
  Serial.println(V);
  Serial.print("W: ");
  Serial.println(W);
  */
  velL = (V-W)/2;
  velR = (V+W)/2;
}

//--------------- LR -> Motors ---------------
//recieves (-256,-256) <= (velL,velR) <= (256,256)
void LR_to_Motors(int velL, int velR){
  if ((L_minThr <= velL) && (velL <= L_maxThr) && (R_minThr <= velR) && (velR <= R_maxThr)){  // Joystick center => Motors standby
    digitalWrite(led, LOW);
    motorsStop();
  } else {                                                                                    // Joystick move => Motors move
    digitalWrite(led, HIGH);
    motorMove(1, velL>=0 ? velL : -velL, velL>0 ? 0 : 1);
    motorMove(0, velR>=0 ? velR : -velR, velR>0 ? 0 : 1);  
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


//======================== BT Functions ========================

//--------------- ApplicationBT -> LR ---------------
void BTtoMDV(){
  recvWithStartEndMarkers();
  if (newData == true) {
    strcpy(tempChars, receivedChars);
            // this temporary copy is necessary to protect the original data
            //   because strtok() used in parseData() replaces the commas with \0
    parseData();
    print_Msg_Dir_Vel();
    newData = false;
  }
}

void recvWithStartEndMarkers() {
    static boolean recvInProgress = false;
    static byte ndx = 0;
    char startMarker = '<';
    char endMarker = '>';
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


void parseData() {      // split the data into its parts

    char * strtokIndx; // this is used by strtok() as an index

    strtokIndx = strtok(tempChars,",");   // get the first part - the string
    strcpy(BT_Msg, strtokIndx);    // copy it to BT_Msg
 
    strtokIndx = strtok(NULL, ",");       // this continues where the previous call left off
    BT_Dir = atoi(strtokIndx);  // convert direction to integer

    strtokIndx = strtok(NULL, ",");
    BT_Vel = atoi(strtokIndx);  // convert velocity to integer

}


void print_Msg_Dir_Vel() {
    Serial.print("Msg: ");
    Serial.println(BT_Msg);
    Serial.print("Dir: ");
    Serial.println(BT_Dir);
    Serial.print("Vel: ");
    Serial.println(BT_Vel);
}

//--------------- Serial echo PC <-> BT ---------------

void PC_to_BT(){
  // PC -> BT
  while (Serial.available() > 0) {
    delay(1);
    if (Serial.available() >0)
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