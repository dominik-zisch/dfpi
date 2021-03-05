#include "COBS.h"
#include "Crc16.h"

#include <Wire.h>
#include <DFRobot_QMC5883.h>

#define BAUDRATE 115200             //Data Speed
#define PACKET_SIZE 512            // Input Packet size
#define HEADER_SIZE 6
#define SENSOR_INTERVAL 100           // interval between sensor messages [milliseconds]

typedef union                       // Used for float to byte conversion
{
  float number;
  uint8_t bytes[4];
} FLOATUNION_t;

// Packet structure:
//
// <lenlo> <lenhi> <crc16ln> <crc16hi> <cmd> <dataType> [data]
//
// <lenlo>    Packet length low
// <lenhi>    Packet length high
// <crclo>    CRC16 checksum low
// <crchi>    CRC16 checksum high
// <cmd>      Command byte
// <dataType> Data type
// [data]     Data bytes
//
//
// Data Type Identifiers:
//
//  01 - ByteArray
//  02 - bool
//  03 - int
//  04 - float
//  05 - string
//  06 - int2
//  07 - int3
//  08 - float2
//  09 - float3
//  10 - float4

//============================================================================//
//========================================================// Serial Declarations

int bytesReceived = 0;                        // number of bytes received
const uint8_t packetMarker = (uint8_t) 0x00;  // packet marker
uint8_t inputBuffer[PACKET_SIZE];             // buffer to store input
Crc16 crc;                                    // CRC algorithm for checksum


//============================================================================//
//========================================================// Sensor Declarations

DFRobot_QMC5883 compass;
float headingRaw = 0;
unsigned long sensorTimer = 0;


//============================================================================//
//===========================================================// Sensor functions

//--------------------------------------------------------//
//-------------------// compassInit - initialize the compass
void compassInit() {

  while (!compass.begin()) {
    Serial.println("Could not find sensor");
    delay(500);
  }
  
  if (compass.isHMC()) {
    Serial.println("Initialize HMC5883");
    compass.setRange(HMC5883L_RANGE_1_3GA);
    compass.setMeasurementMode(HMC5883L_CONTINOUS);
    compass.setDataRate(HMC5883L_DATARATE_15HZ);
    compass.setSamples(HMC5883L_SAMPLES_8);
  } else if (compass.isQMC()) {
    Serial.println("Initialize QMC5883");
    compass.setRange(QMC5883_RANGE_2GA);
    compass.setMeasurementMode(QMC5883_CONTINOUS); 
    compass.setDataRate(QMC5883_DATARATE_50HZ);
    compass.setSamples(QMC5883_SAMPLES_8);
  }
}


//--------------------------------------------------------//
//------------------------// compassRead - get compass value
void compassRead() {
  // Set declination angle on your location and fix heading
  // You can find your declination on: http://magnetic-declination.com/
  // (+) Positive or (-) for negative
  // For Bytom / Poland declination angle is 4'26E (positive)
  // Formula: (deg + (min / 60.0)) / (180 / PI);
  float declinationAngle = (4.0 + (26.0 / 60.0)) / (180 / PI);
  compass.setDeclinationAngle(declinationAngle);
  Vector mag = compass.readRaw();
  compass.getHeadingDegrees();

  headingRaw = mag.HeadingDegress;
}


//============================================================================//
//========================================================// Lifecycle functions

//--------------------------------------------------------//
//------------------------------// setup - called at startup
void setup()
{
  // put your setup code here, to run once:
  Serial.begin(BAUDRATE);
  pinMode(LED_BUILTIN, OUTPUT);

  // Sensor init
  compassInit();
}


//--------------------------------------------------------//
//--------------------------------// loop - called in a loop
void loop()
{
  // Sensor loop
  if (millis() > sensorTimer) {
    sensorTimer += SENSOR_INTERVAL;
    compassRead();
   
    sendFloat(1, headingRaw);
    sendFloat3(2, 0, headingRaw, 0);
  }
}


//============================================================================//
//===========================================================// Serial functions

//--------------------------------------------------------//
//---------------------------------------------// Send Float
void sendFloat(int cmd, float f)
{
  uint8_t buffer[4];
  copyFloatToBuf(f, buffer, 0);
  sendPacket(cmd, 4, buffer, 4);
}

//--------------------------------------------------------//
//------------------------------------------// Send 3 Floats
void sendFloat3(int cmd, float f1, float f2, float f3)
{
  uint8_t buffer[12];
  copyFloatToBuf(f1, buffer, 0);
  copyFloatToBuf(f2, buffer, 4);
  copyFloatToBuf(f3, buffer, 8);
  sendPacket(cmd, 9, buffer, 12);
}

//--------------------------------------------------------//
//-------------------------------------// Send data to Unity
void sendPacket(int cmd, int dataType, uint8_t* buffer, size_t len)
{
  int checksum = crc.computeChecksum(buffer, len);
  uint8_t serialPacket[len + HEADER_SIZE];

  serialPacket[0] = (uint8_t) (len & 0xff);
  serialPacket[1] = (uint8_t) ((len >> 8) & 0xff);
  serialPacket[2] = (uint8_t) (checksum & 0xff);
  serialPacket[3] = (uint8_t) ((checksum >> 8) & 0xff);
  serialPacket[4] = (uint8_t) cmd;
  serialPacket[5] = (uint8_t) dataType;

  memcpy(serialPacket + HEADER_SIZE * sizeof(uint8_t), buffer, len * sizeof(uint8_t));

  uint8_t _encodeBuffer[COBS::getEncodedBufferSize(len + HEADER_SIZE)];

  size_t numEncoded = COBS::encode(serialPacket, len + HEADER_SIZE, _encodeBuffer);

  Serial.write(_encodeBuffer, numEncoded);
  Serial.write(packetMarker);
}



//============================================================================//
//======================================================// Utilities and Helpers

//--------------------------------------------------------//
//-----------------------------------// Copy float to buffer
void copyFloatToBuf(float val, uint8_t* buffer, int offset)
{
  byte* b = (byte*) &val;
  memcpy(buffer + offset, b, 4);
}
