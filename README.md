[![Version](https://img.shields.io/github/release/RestoreMonarchyPlugins/AirdropManager.svg)](https://github.com/RestoreMonarchyPlugins/AirdropManager/releases) ![Discord](https://discordapp.com/api/guilds/520355060312440853/widget.png)
# AirdropManager 2.1 - RocketMod4 Plugin

### Configuration

```xml
<?xml version="1.0" encoding="utf-8"?>
<AirdropManagerConfiguration xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <UseDefaultSpawns>true</UseDefaultSpawns>
  <UseDefaultAirdrops>false</UseDefaultAirdrops>
  <AirdropMessageIcon>https://i.imgur.com/JCDmlqI.png</AirdropMessageIcon>
  <AirdropInterval>3600</AirdropInterval>
  <Airdrops>
    <Airdrop AirdropId="1005">
      <Items>
        <AirdropItem ItemId="363" Chance="10" />
        <AirdropItem ItemId="17" Chance="20" />
      </Items>
    </Airdrop>
  </Airdrops>
  <AirdropSpawns>
  </AirdropSpawns>
  <BlacklistedAirdrops />
  <MessageColor>yellow</MessageColor>
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
