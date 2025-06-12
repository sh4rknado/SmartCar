#ifndef FileManager_h
#define FileManager_h

#include <FS.h>
#include <LittleFS.h>
#include <RemoteDebug.h>
#include <ArduinoJson.h>

class FileManager {
  private:
    RemoteDebug* _debug;
    bool isMount;

  public:
    FileManager(RemoteDebug* debug);
    void UnMount();
    bool Mount();
    void Format();
    void ListDir(const char* dirname);
    void PrintFile(const char* path);
    bool Exists(const char* path);
    bool OpenFileWrite(const char* path, File& file);
    void WriteFile(const char* path, const char* message);
    void AppendFile(const char* path, const char* message);
    void RenameFile(const char* path1, const char* path2);
    bool DeleteFile(const char* path);
    bool ReadFile(const char* path, File& file);
    bool ReadJson(const char* configPath, JsonDocument& doc);
    bool WriteJson(const char* configPath, JsonDocument doc);

};
#endif