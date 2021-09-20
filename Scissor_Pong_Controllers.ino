int potPin1 = A0;    // select the input pins for the potentiometers
int potPin2 = A1;

int sensorValue1 = 0;  // variables to store the values coming from the potentiometers
int sensorValue2 = 0;

void setup() {
  Serial.begin(9600);
}

void loop() {
  // read the value from the potentiometer:
  sensorValue1 = analogRead(potPin1);   
  sensorValue2 = analogRead(potPin2);  

  Serial.flush();
  Serial.print("p1: ");
  Serial.println(sensorValue1);

  Serial.flush();
  Serial.print("p2: ");
  Serial.println(sensorValue2);
  delay(100);            
}
