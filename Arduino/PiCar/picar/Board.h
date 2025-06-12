#ifndef Board_h
#define Board_h

#include <ESP8266WiFi.h>
#include <WiFiUdp.h>
#include <RemoteDebug.h>
#include "FirmwareManager.h"
#include "FileManager.h"
#include "BoardConfig.h"

class Board {
  private:
    RemoteDebug* _debug;
    FirmwareManager* _firmware;
    FileManager* _fileManager;
    BoardConfig _config;

  public:
    Board();
    BoardConfig GetConfiguration();
    RemoteDebug* GetRemoteDebug();
    void Setup();
    void SetupNTP();
    void SetupWifi();
    void Loop();
    void SaveBoardConfiguration(const char* configurationPath, FileManager* fileManager, const BoardConfig& config);
    void ApplyDefaultConfiguration(const char* configurationPath, FileManager* fileManager, BoardConfig& config);
    void ReadBoardConfiguration(const char* configurationPath, FileManager* fileManager, BoardConfig& config);
    
};

#endif


