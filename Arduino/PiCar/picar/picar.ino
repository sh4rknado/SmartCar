#include "Board.h"
#include "SmartCar.h"
#include <Wire.h>
#include <PCA9685.h>
#include "sensors.h"

Board* board;
SmartCar* smartCar;
int port = 8080;
WiFiServer server(port);
Sensor sensor;

void setup() {
  Serial.begin(115200);
  while (!Serial && millis() < 5000);
  delay(300);
  
  //board setup
  board = new Board();
  board->Setup();
  
  smartCar = new SmartCar(board->GetRemoteDebug());
  smartCar->Setup();

  // Démarrage du serveur TCP
  server.begin();
  Serial.print("Server started on port: ");
  Serial.println(port);
}

void loop() {
  board->Loop();

  WiFiClient client = server.available(); // Vérifie si un client est connecté

  if (client) {
    Serial.println("Client connected.");
    unsigned long clientReadTimeout = millis() + 5000; // 5 secondes de timeout

    while (client.connected() && client.available()) {
      String json = client.readStringUntil('\n');
      Serial.println("Received json frame" + json);
      Sensor config;
      if(smartCar->DesirializeJsonSensors(json, config))
      {        
        Serial.println("Set sensors values...");
        smartCar->SetAllSensors(config);
      }
      else
      {
        Serial.println("Could not decode the frame => failsafe mode stop the car");
        smartCar->Stop();
      }
    }

    Serial.println("Client disconnected => failsafe mode stop the car");
    smartCar->Stop();
  }
  else
  {
    //Serial.println("No client connected => failsafe mode stop the car");
    smartCar->Stop();
  }
}
