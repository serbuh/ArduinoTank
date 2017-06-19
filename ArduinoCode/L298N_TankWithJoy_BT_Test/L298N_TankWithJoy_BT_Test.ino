//======================== Init ========================
//--------------- General ---------------
#include<SoftwareSerial.h>
const int led = 13;

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
char joyMsg[numChars] = {'S'};

boolean newData = false;
boolean BT_echo = false;                   //******** PARAMETER ******** echo BT to Serial console (disables parsing)
                                          //use baud 115200 in Serial Console
boolean BT_enabled = true;                //******** PARAMETER ******** BT or Joystick flag
unsigned long previousMillis = 0;         // will store last time BT data was updated
unsigned long currentMillis = 0;
const long BT_timeout = 2000;             // BT timeout (milliseconds). Zero H,V values after timeout.

//--------------- Bridge Servo --------------- 
#include <Servo.h>
const int servoPin = 12;
int servoVal = 0;
bool servo_already_stay = false;
Servo brdgServo;

//======================== Setup ========================

void setup() {
  //--------------- LED ---------------
  pinMode(led, OUTPUT);
  //digitalWrite(led, LOW);

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
  BT_to_MHV();   // read MHV from BT
  M_to_Servo(joyMsg);
}


//======================== Functions ========================
//--------------- Msg -> Bridge Servo ---------------
void M_to_Servo(char* joyMsg) {
  if (joyMsg[0] == 'S') {
    if(servo_already_stay){
      pinMode(servoPin, INPUT); // "switch off" the servo
      //Nothing to do. Already set to 90 degrees
    } else {
      //Serial.println("Stay");
      pinMode(servoPin, OUTPUT); // "switch on" the servo
      brdgServo.write(90);
      servo_already_stay = true;
    }
  } else if (joyMsg[0] == 'U') {
    //Serial.println("Up");
    pinMode(servoPin, OUTPUT); // "switch on" the servo
    brdgServo.write(115);
    servo_already_stay = false;
  } else if (joyMsg[0] == 'D') {
    //Serial.println("Down");
    pinMode(servoPin, OUTPUT); // "switch on" the servo
    brdgServo.write(65);
    servo_already_stay = false;
  }
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
    strcpy(joyMsg, BT_joyMsg);
    newData = false;
  }
  if (currentMillis - previousMillis >= BT_timeout){  // no data recieved for at least BT_timeout ms. Center VH.
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
