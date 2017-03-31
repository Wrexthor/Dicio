# Dicio
#### Runs powershell scripts targeting multiple hosts at once. Can search for machines using Active Directory or DNS and save/load lists of machines to files.

![alt text](https://github.com/Wrexthor/Dicio/blob/master/dicio_screenshot_01.png?raw=true "Dicio Screenshot")

Instructions

1. Download zip and extract somewhere
2. Open extracted folder, go to DeusDicio\DeusDicio\bin\Debug
3. Run DeusDicio.exe

![alt text](https://github.com/Wrexthor/Dicio/blob/master/dicio_animation.gif?raw=true "Dicio Instruction")

The only things needed to run the program is the DeusDicio.exe and a folder named Scripts in the same location.

Custom scripts can be added either directly to the Scripts folder or using the Load button in the program (this will copy the script into Scripts folder).
Custom scripts need to have a array paramter for the computer names, otherwise they can be formated in any way desirable. Any of the scripts in the scripts folder can be used as a template or changed. They all use powershell jobs to run in parallel to reduce runtime when targeting large amounts of hosts.


