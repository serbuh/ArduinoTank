#include<SoftwareSerial.h>
const int led = 13;
//BT
const int BT_rx = 8;
const int BT_tx = 9;
char BT_data = 0;
SoftwareSerial BTserial(BT_rx, BT_tx);

//======================== BT Parse ========================

const byte numChars = 32;
char receivedChars[numChars];
char tempChars[numChars];        // temporary array for use when parsing

// variables to hold the parsed data
char messageFromPC[numChars] = {0};
int integerDirFromPC = 0;
int integerVelFromPC = 0;

boolean newData = false;

//======================== Setup ========================

void setup() {

  //DEBUG LED
  pinMode(led, OUTPUT);
  digitalWrite(led, LOW);

  //PC <-> Arduino
  Serial.begin(115200);
  while (!Serial) {
    ; // wait for serial port to connect. Needed for native USB port only
  }
  digitalWrite(led, HIGH);

  Serial.println("Arduino -> PC :)");
    
  //Arduino <-> BT
  BTserial.begin(9600);
  BTserial.println("Arduino -> BT :)");
  BTserial.println("Expecting: <[MSG], [Dir], [Vel]>");
  BTserial.println();

}

//======================== Loop ========================

void loop() {
  recvWithStartEndMarkers();
  if (newData == true) {
    strcpy(tempChars, receivedChars);
            // this temporary copy is necessary to protect the original data
            //   because strtok() used in parseData() replaces the commas with \0
    parseData();
    showParsedData();
    newData = false;
  }
  //BT_to_PC();
  //parse();
}

//======================== Functions ========================

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
    strcpy(messageFromPC, strtokIndx);    // copy it to messageFromPC
 
    strtokIndx = strtok(NULL, ",");       // this continues where the previous call left off
    integerDirFromPC = atoi(strtokIndx);  // convert direction to integer

    strtokIndx = strtok(NULL, ",");
    integerVelFromPC = atoi(strtokIndx);  // convert velocity to integer

}


void showParsedData() {
    Serial.print("MSG: ");
    Serial.println(messageFromPC);
    Serial.print("Dir: ");
    Serial.println(integerDirFromPC);
    Serial.print("Vel: ");
    Serial.println(integerVelFromPC);
}
