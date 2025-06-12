#include "core_esp8266_features.h"
#include "Board.h"

Board::Board() {
  _debug = new RemoteDebug();  
  _fileManager = new FileManager(_debug);
  _firmware = new FirmwareManager(_debug, _fileManager);
}

// #################################################### < SETTER/GETTER REGION > ###########################################################

BoardConfig Board::GetConfiguration() { return _config; }

RemoteDebug* Board::GetRemoteDebug() { return _debug; }

// #################################################### < SETUP REGION > ###########################################################

void Board::Setup() {
  // mount file system
  _fileManager->Mount(); 
  
  // read config
  ReadBoardConfiguration("/config.json", _fileManager, _config);
  
  // setup wifi configuration
  SetupWifi();
  
  // setup telnet debug
  Serial.println("setup remote debug...");
  _debug->begin(_config.hostname, RemoteDebug::ANY);

  // setup firmware OTA
  Serial.println("setup firmware...");
  _firmware->SetupFirmware(_config.hostname, _config.ota_port, _config.ota_auth, _config.password_ota);
}

void Board::SetupNTP() {
  configTime(3600 * _config.timezone, _config.daysavetime * 3600,  _config.ntp_server_1,  _config.ntp_server_2,  _config.ntp_server_3);
  struct tm tmstruct;
  delay(2000);
  tmstruct.tm_year = 0;
  getLocalTime(&tmstruct, 1000);
  _debug->printf("\nNow is : %d-%02d-%02d %02d:%02d:%02d\n", (tmstruct.tm_year) + 1900, (tmstruct.tm_mon) + 1, tmstruct.tm_mday, tmstruct.tm_hour, tmstruct.tm_min, tmstruct.tm_sec);
}

void Board::SetupWifi() {
  WiFi.mode(WIFI_STA);  
  WiFi.begin(_config.wifi_name, _config.wifi_password);

 // Configures static IP address
  if (!WiFi.config(IPAddress(10, 0, 0, 100), IPAddress(10, 0, 0, 1), IPAddress(255, 255, 255, 0), IPAddress(10, 0, 0, 1), IPAddress(10, 0, 0, 1))) {
    Serial.println("STA Failed to configure");
  }

  Serial.print("Connecting to WiFi");

	while (WiFi.status() != WL_CONNECTED) {
		Serial.printf(".");
    delay(500);
	}

	Serial.print(" Connected !\n");
	Serial.printf("IP: %s\n", WiFi.localIP().toString().c_str());
  Serial.printf("Netmask: %s\n", WiFi.subnetMask().toString().c_str());
  Serial.printf("Gateway: %s\n", WiFi.gatewayIP().toString().c_str());
  Serial.printf("DNS: %s\n", WiFi.dnsIP().toString().c_str());
}

// #################################################### < LOOP Handle REGION > ###########################################################

void Board::Loop() { 
  _firmware->CheckFirmwareUpdate();  //handle OTA and update MDNS discovery
  _debug->handle();                  //handle debug
}

// #################################################### < CONFIGURATION > ###########################################################

void Board::ReadBoardConfiguration(const char* configurationPath, FileManager* fileManager, BoardConfig& config) {
  JsonDocument doc; // Allocate a temporary JsonDocument
  
  fileManager->DeleteFile(configurationPath);

  if(!fileManager->Exists(configurationPath)) {
    ApplyDefaultConfiguration(configurationPath, fileManager, config);
  }
  else if(fileManager->ReadJson(configurationPath, doc)) {
    
    Serial.println("Configuration found, read configuration...");

    //general section
    config.hostname = doc["hostname"] | "esp8266";
    
    //wifi section
    config.wifi_name = doc["wifi_name"] | "SmartCar";
    config.wifi_password = doc["wifi_password"] | "zerocool";

    // OTA Section
    config.password_ota = doc["password_ota"] | "SmartCar";
    config.ota_port = doc["ota_port"] | 8266;
    config.ota_auth = doc["ota_auth"] | true;
    
    // NTP Section
    config.ntp_server_1 = doc["ntp_server_1"] | "10.0.1.1";
    config.ntp_server_2 = doc["ntp_server_2"] | "10.0.1.1";
    config.ntp_server_3 = doc["ntp_server_3"] | "10.0.1.1";
    config.timezone = doc["timezone"] | 0;
    config.daysavetime = doc["daysavetime"] | 1;

    // debug json serialize
    // serializeJson(doc, Serial);
  }
}

void Board::ApplyDefaultConfiguration(const char* configurationPath, FileManager* fileManager, BoardConfig& config) {
    
    Serial.println("Configuration not found, default configuration will be used");

    //general section
    config.hostname = "esp8266";

    //wifi section
    config.wifi_name = "SmartCar";
    config.wifi_password = "zerocool";

    // OTA Section
    config.password_ota = "SmartCar";
    config.ota_port = 8266;
    config.ota_auth = true;

    // NTP Section
    config.ntp_server_1 = "10.0.0.1";
    config.ntp_server_2 = "10.0.0.1";
    config.ntp_server_3 = "10.0.0.1";
    config.timezone = 0;
    config.daysavetime = 1;

    SaveBoardConfiguration(configurationPath, fileManager, config);
}

void Board::SaveBoardConfiguration(const char* configurationPath, FileManager* fileManager, const BoardConfig& config) {
  JsonDocument doc;

  // General Section
  doc["hostname"] = config.hostname;
  
  // Wifi Section
  doc["wifi_name"] = config.wifi_name;
  doc["wifi_password"] = config.wifi_password;

  // OTA SECTION
  doc["password_ota"] = config.password_ota;
  doc["ota_auth"] = config.ota_auth;
  doc["ota_port"] = config.ota_port;

  // NTP Section
  doc["ntp_server_1"] = config.ntp_server_1;
  doc["ntp_server_2"] = config.ntp_server_2;
  doc["ntp_server_3"] = config.ntp_server_3;
  doc["timezone"] = config.timezone;
  doc["daysavetime"] = config.daysavetime;

  Serial.println("Write default configuration...");
  fileManager->WriteJson(configurationPath, doc);
  Serial.println("save configuration completed");
}

