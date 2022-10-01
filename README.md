# busylight-client

## how to build
``dotnet publish -r win-x64 -c Release -p:PublishSingleFile=true -p:PublishTrimmed=true``




| Name         | Default value |                                                             Allowed value                                                             |
| ------------ | ------------- | :-----------------------------------------------------------------------------------------------------------------------------------: |
| Location     |               |                                                                   *                                                                   |
| SignalR_Uri  |               |                                                                   *                                                                   |
| ApiKey       |               |                                                                   *                                                                   |
| KeyName      | ApiKey        |                                                                   *                                                                   |
| Ring_Tune    | OpenOffice    | "OpenOffice", "Quiet", "Funky", "FairyTale", "KuandoTrain", "TelephoneNordic", "TelephoneOriginal", "TelephonePickMeUp", "IM1", "IM2" |
| Ring_Color   | Red           |                                                "Red", "Green", "Blue", "Yellow", "Off"                                                |
| Ring_Time    | 5000          |                                                                                                                                       |
| Ring_Volume  | 100           |                                                                                                                                       |
| Idle_Color   | Off           |                                                   Same options as for `Ring_Color`                                                    |
| Custom_Sound |               |                           File path to the sound you want to play, it has only been tested with an mp3 file                           |


Example of a appsettings.json:
```json
{
  "AppSettings": {
    "Location": "Brande",
    "SignalR_Uri": "http://localhost:50625/BusyHub",
    "Ring_Time": 5500,
    "Ring_Tune": "OpenOffice",
    "Ring_Color": "Red",
    "KeyName": "ApiKey",
    "ApiKey": "some-key",
    "Idle_Color": "Off",
    "Custom_Sound": "C:\\sounds\\surprise.mp3"
  }
}
```
