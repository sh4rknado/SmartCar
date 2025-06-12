#ifndef FirmwareManager_h
#define FirmwareManager_h

#include <ESP8266mDNS.h>
#include <ArduinoOTA.h>
#include <RemoteDebug.h>
#include "FileManager.h"

class FirmwareManager {
  private:
    RemoteDebug* _debug;
    FileManager* _fileManager;

  public:
    FirmwareManager(RemoteDebug* debug, FileManager* fileManager);
    void SetupFirmware(const char* hostname, int port, bool auth, const char* password);
    void CheckFirmwareUpdate();
    void OnUpdateStart();
    void OnUpdateEnd();
    void OnUpdateProgress(unsigned int progress, unsigned int total);
    void OnUpdateError(ota_error_t error);
  
  private:
    bool IsValidPort(int port);
    bool StringIsNullOrEmpty(const char* str);

};

#endif