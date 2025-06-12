#ifndef BoardConfig_h
#define BoardConfig_h

#include <Arduino.h>

struct BoardConfig {
  // General Section
  const char* hostname;

  // Wifi Section
  const char* wifi_name;
  const char* wifi_password;
  int max_connections;

  // OTA SECTION
  const char* password_ota;
  bool ota_auth;
  int ota_port;
  
  // NTP Section
  const char* ntp_server_1;
  const char* ntp_server_2; 
  const char* ntp_server_3;
  long timezone;
  byte daysavetime;
};

#endif