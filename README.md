## AirdropManager

### Features
* Create airdrops with custom loot
* Disable or override default airdrop spawns
* Set custom airdrop spawn positions
* Optionally set a custom airdrop speed and barricade

### Commands
* **/whenairdrop** - Displays time left to the next scheduled airdrop
* **/airdrop** - Calls in airdrop at the random spawn position
* **/massairdrop** - Calls in airdrop at every possible spawn positions
* **/setairdrop** *\<airdropId\> [name]* - Set the airdrop spawn at your current position

### FAQ
**Why does Airdrop spawn empty?**
Most common reason is one or more of the items you have configured in the airdrop no longer exist on the server, because You uninstalled the original workshop file from the server or they just never existed.

### Configuration

```xml
<?xml version="1.0" encoding="utf-8"?>
<AirdropManagerConfiguration xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <MessageColor>yellow</MessageColor>
  <UseDefaultSpawns>true</UseDefaultSpawns>
  <UseDefaultAirdrops>false</UseDefaultAirdrops>
  <AirdropMessageIcon>https://i.imgur.com/JCDmlqI.png</AirdropMessageIcon>
  <AirdropInterval>3600</AirdropInterval>
  <AirdropBarricadeId>0</AirdropBarricadeId>
  <AirdropSpeed>128</AirdropSpeed>
  <Airdrops>
    <Airdrop AirdropId="13623" Name="Military">
      <Items>
        <AirdropItem ItemId="363" Chance="10" Name="Maplestrike" />
        <AirdropItem ItemId="17" Chance="20" Name="Military Drum" />
      </Items>
    </Airdrop>
  </Airdrops>
  <AirdropSpawns>
    <AirdropSpawn AirdropId="13623" Name="Middle of the map">
      <Position X="0" Y="0" Z="0" />
    </AirdropSpawn>
  </AirdropSpawns>
  <BlacklistedAirdrops />
</AirdropManagerConfiguration>
```

### Translation

```xml
<?xml version="1.0" encoding="utf-8"?>
<Translations xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <Translation Id="NextAirdrop" Value="Next airdrop will be in {0}" />
  <Translation Id="SuccessAirdrop" Value="Successfully called in airdrop!" />
  <Translation Id="SuccessMassAirdrop" Value="Successfully called in mass airdrop!" />
  <Translation Id="Airdrop" Value="{size=17}{color=magenta}{i}Airdrop{/i} is coming!{/color}{/size}" />
  <Translation Id="MassAirdrop" Value="{size=17}{color=magenta}{i}Mass Airdrop{/i} is coming!{/color}{/size}" />
  <Translation Id="SetAirdropFormat" Value="Format: /setairdrop &lt;AirdropID&gt;" />
  <Translation Id="SetAirdropSuccess" Value="Successfully set an airdrop spawn at your position!" />
  <Translation Id="SetAirdropInvalid" Value="Invalid AirdropID" />
</Translations>
```
