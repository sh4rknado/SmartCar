#include "FirmwareManager.h"

FirmwareManager::FirmwareManager(RemoteDebug* debug, FileManager* fileManager) {
  _debug = debug;
  _fileManager = fileManager;
}

void FirmwareManager::SetupFirmware(const char* hostname, int port, bool auth, const char* password) {
  
  // Port defaults to 8266
  if (IsValidPort(port)) {
    ArduinoOTA.setPort(port);
  } else {
    ArduinoOTA.setPort(8266);
  }

  // Hostname defaults to esp8266-[ChipID]
  if (StringIsNullOrEmpty(hostname)) {
    ArduinoOTA.setHostname(hostname);
  } else {
    ArduinoOTA.setHostname("esp8266");
  }

  if (auth) {
    ArduinoOTA.setPassword(password);
  }

  ArduinoOTA.onStart([this]() { this->OnUpdateStart(); });
  ArduinoOTA.onEnd([this]() { this->OnUpdateEnd(); });
  ArduinoOTA.onProgress([this](unsigned int progress, unsigned int total) { this->OnUpdateProgress(progress, total); });
  ArduinoOTA.onError([this](ota_error_t error) { this->OnUpdateError(error); });
  ArduinoOTA.begin();
}

void FirmwareManager::CheckFirmwareUpdate() { ArduinoOTA.handle(); }

void FirmwareManager::OnUpdateStart() {
  if (ArduinoOTA.getCommand() == U_FLASH) {
    _debug->println("Start updating sketch");
  } else {  // U_FS
    _fileManager->UnMount();
    _debug->println("Start updating U_FS");
  }
}

void FirmwareManager::OnUpdateEnd() {
 _debug->println("\nEnd");
 // remount file system
 _fileManager->Mount();
}

void FirmwareManager::OnUpdateProgress(unsigned int progress, unsigned int total) {
  float percentage = (progress / (total / 100));
  _debug->println("Progress: " + String(percentage) + " %"); 
}

void FirmwareManager::OnUpdateError(ota_error_t error) {
 switch (error) {
      case OTA_AUTH_ERROR:
      _debug->println("Auth Failed");
        break;
      case OTA_BEGIN_ERROR:
       _debug->println("Begin Failed");
        break;
      case OTA_CONNECT_ERROR:
       _debug->println("connect Failed");
        break;
      case OTA_RECEIVE_ERROR:
       _debug->println("Receive Failed");
        break;
      case OTA_END_ERROR:
       _debug->println("End Failed");
        break;
      default:
       _debug->println("Unknow error:" + String(error));
        break;
    }
}

bool FirmwareManager::IsValidPort(int port) {
  return port > 0 && port < 65535;
}

bool FirmwareManager::StringIsNullOrEmpty(const char* str) {
  return str == nullptr || *str == '\0';
}