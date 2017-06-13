#include<SoftwareSerial.h>
const int led = 13;
//BT
const int BT_rx = 8;
const int BT_tx = 9;
char BT_data = 0;
SoftwareSerial BTserial(BT_rx, BT_tx);

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

}

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

void loop() {
  BT_to_PC();

  PC_to_BT();  
}
