#include <Arduino.h>
#include <WiFi.h>
#include <WebServer.h>
#include <ArduinoJson.h>
#include <FreeRTOS.h>
//#include <Adafruit_BME280.h>
//#include <Adafruit_Sensor.h>
//#include <Adafruit_NeoPixel.h>
#include <HTTPClient.h>

//#include <heltec.h>
// pass 35473531
//const char *SSID = "your_wifi-ssid";
//const char *PWD = "your_wifi_password";
//const char *SSID = "sc-199d 2.4";
//const char *PWD = "0142439647";
const char *SSID = "LERIN-OFICINA";
const char *PWD = "13873942";

//const char *SSID = "Conectividad Cordoba";
//const char *PWD = "";
#define NUM_OF_LEDS 9
#define PIN 4
// Web server running on port 80
WebServer server(80);
 
// Sensor
//Adafruit_BME280 bme;
// Neopixel LEDs strip
//Adafruit_NeoPixel pixels(NUM_OF_LEDS, PIN, NEO_GRB + NEO_KHZ800);
// JSON data buffer
StaticJsonDocument<250> jsonDocument;
char buffer[250];
 
// env variable
int temperature;
int temperature1;
int temp_ant=0;
int volume;
int volume1;
int vol_ant=0;
float pressure;

#define REDLED 34
#define GREENLED 33
#define SW1 19
#define SW2 20

const char* serverUrlt = "http://192.168.123.45:5087/agregar";
const char* serverUrlv = "http://192.168.123.45:5087/agregar1";
const char* apiKey = "tu_api_key";

void enviarJSONaAPIt(int temp) {
  // Iniciar la conexión HTTP
  HTTPClient http;
  http.begin(serverUrlt);
  // Crear un objeto JSON
  StaticJsonDocument<200> jsonDoc;
  jsonDoc["Temp"] = temp; // Aquí puedes poner los datos que quieras enviar en formato JSON
  // Convertir el objeto JSON en una cadena
  String jsonString;
  serializeJson(jsonDoc, jsonString);
  // Configurar las cabeceras HTTP
  http.addHeader("Content-Type", "application/json");
  //http.addHeader("X-API-Key", apiKey);
  // Enviar la solicitud POST con el JSON
  int httpResponseCode = http.POST(jsonString);
  // Verificar la respuesta
  if (httpResponseCode > 0) {
    Serial.print("Respuesta del servidor: ");
    Serial.println(http.getString());
  } else {
    Serial.print("Error en la solicitud HTTP: ");
    Serial.println(httpResponseCode);
  }
  // Cerrar la conexión HTTP
  http.end();
}

void enviarJSONaAPIv(int vol) {
  // Iniciar la conexión HTTP
  HTTPClient http;
  http.begin(serverUrlv);
  // Crear un objeto JSON
  StaticJsonDocument<200> jsonDoc;
  jsonDoc["Vol"] = vol; // Aquí puedes poner los datos que quieras enviar en formato JSON
  // Convertir el objeto JSON en una cadena
  String jsonString;
  serializeJson(jsonDoc, jsonString);
  // Configurar las cabeceras HTTP
  http.addHeader("Content-Type", "application/json");
  //http.addHeader("X-API-Key", apiKey);
  // Enviar la solicitud POST con el JSON
  int httpResponseCode = http.POST(jsonString);
  // Verificar la respuesta
  if (httpResponseCode > 0) {
    Serial.print("Respuesta del servidor: ");
    Serial.println(http.getString());
  } else {
    Serial.print("Error en la solicitud HTTP: ");
    Serial.println(httpResponseCode);
  }
  // Cerrar la conexión HTTP
  http.end();
}

void connectToWiFi() {
  Serial.print("Connecting to ");
  Serial.println(SSID);
  WiFi.begin(SSID, PWD);
  while (WiFi.status() != WL_CONNECTED) {
    Serial.print(".");
    delay(500);
    // we can even make the ESP32 to sleep
  }
  Serial.print("Connected. IP: ");
  Serial.println(WiFi.localIP());
}
 
void create_json(char *tag, float value, char *unit) { 
  jsonDocument.clear(); 
  jsonDocument["type"] = tag;
  jsonDocument["value"] = value;
  jsonDocument["unit"] = unit;
  serializeJson(jsonDocument, buffer);
  Serial.println("Buffer:");
  Serial.println(buffer);  
}
 
void add_json_object(char *tag, float value, char *unit) {
  JsonObject obj = jsonDocument.createNestedObject();
  obj["type"] = tag;
  obj["value"] = value;
  obj["unit"] = unit; 
}

void read_sensor_data(void * parameter) {
   for (;;) {
     //temperature = analogRead(2);
     //volume = 20; // analogRead(37); //bme.readvolume();
     pressure = 30; //bme.readPressure() / 100;
     Serial.println("Read sensor data");
     // delay the task
     vTaskDelay(60000 / portTICK_PERIOD_MS);
   }
}
 
void getTemperature() {
  Serial.println("Get temperature");
  temperature = analogRead(2);
  create_json("temperature", temperature, "°C");
  server.sendHeader("Access-Control-Allow-Origin", "*");
  server.send(200, "application/json", buffer);
}
 
void getVolume() {
  Serial.println("Get volume");
  volume = analogRead(3);
  create_json("volume", volume, "%");
  server.sendHeader("Access-Control-Allow-Origin", "*");
  server.send(200, "application/json", buffer);
}
 
void getPressure() {
  Serial.println("Get pressure");
  create_json("pressure", pressure, "mBar");
  server.send(200, "application/json", buffer);
}
 
void getEnv() {
  Serial.println("Get env");
  jsonDocument.clear();
  add_json_object("temperature", temperature, "°C");
  add_json_object("volume", volume, "%");
  add_json_object("pressure", pressure, "mBar");
  serializeJson(jsonDocument, buffer);
  server.send(200, "application/json", buffer);
}

void handleOptions() {
  server.sendHeader("Access-Control-Allow-Origin", "*");
  server.sendHeader("Access-Control-Allow-Methods", "POST");
  server.sendHeader("Access-Control-Allow-Headers", "Content-Type");
  server.send(200);
}

void handlePost() {
  if (server.hasArg("plain") == false) {
    //handle error here
  }

  String body = server.arg("plain");
  Serial.println(body);
  deserializeJson(jsonDocument, body);
  // Get RGB components
  int red = jsonDocument["red"];
  int green = jsonDocument["green"];
  int blue = jsonDocument["blue"];

  Serial.print("Red: ");
  Serial.print(red);
  if (red==1){
    digitalWrite(REDLED,true);
  }
  else
    digitalWrite(REDLED,false);
  if (green==1){
    digitalWrite(GREENLED,true);
  }
  else
    digitalWrite(GREENLED,false);
  //pixels.fill(pixels.Color(red, green, blue));
  delay(30);
  //pixels.show();
  // Respond to the client
  server.sendHeader("Access-Control-Allow-Origin", "*");
  server.send(200, "application/json", "{}");
}
 
// setup API resources
void setup_routing() { 
  server.on("/temperature", getTemperature);
  server.on("/pressure", getPressure);
  server.on("/volume", getVolume);
  server.on("/env", getEnv);
  server.on("/led", HTTP_POST, handlePost);
  server.on("/led", HTTP_OPTIONS, handleOptions);
  server.sendHeader("Access-Control-Allow-Origin", "*");
  // start server
  server.begin();
}

void setup_task() {
  xTaskCreate(
    read_sensor_data,    
    "Read sensor data",   // Name of the task (for debugging)
    1000,            // Stack size (bytes)
    NULL,            // Parameter to pass
    1,               // Task priority
    NULL             // Task handle
  );
}
 
void setup() {
   Serial.begin(9600);
  // Sensor setup
  //if (!bme.begin(0x76)) {
  //  Serial.println("Problem connecting to BME280");
  //}
  connectToWiFi();
  setup_task();
  setup_routing();  
  // Initialize Neopixel
  //pixels.begin();
  //adcAttachPin(1);
  //analogSetClockDiv(255); // 1338mS 
  pinMode(REDLED,OUTPUT);
  digitalWrite(REDLED,HIGH);
  pinMode(GREENLED,OUTPUT);
  digitalWrite(GREENLED,HIGH);
  delay(100);
  digitalWrite(REDLED,LOW);
  digitalWrite(GREENLED,LOW);
  pinMode(SW1,PULLUP);
  pinMode(SW2,PULLUP);  
}
 
void loop() {
  server.handleClient();
  temperature1=analogRead(2);
  if (!(temperature1 == temp_ant)){
    enviarJSONaAPIt(temperature1);
    temp_ant=temperature1;
  }

  volume1=analogRead(3); 
  if (!(volume1 == vol_ant)){
    enviarJSONaAPIv(volume1);
    vol_ant=volume1;
  }

  delay(1500);

}