# AirdropManager - Unturned Plugin
* Plugin is available in both RocketMod 4 and 5 versions
* Allows you to create airdrops with custom loot
* Allows you to create targets for airdrops or you can use default
* Allows you to set up ChatMaster like message (with size, colors, style and icon)
* Allows you to blacklist unwanted airdrops by id

## Commands
* /airdrop - Calls in an airdrop
* /whenairdrop - Shows time left to next airdrop
* /setairdrop \<airdrop id\> - Saves you current position as airdrop target

## Configuration

**RocketMod 4**
```xml
<?xml version="1.0" encoding="utf-8"?>
<AirdropManagerConfiguration xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <UseDefaultSpawns>true</UseDefaultSpawns>
  <UseDefaultAirdrops>false</UseDefaultAirdrops>
  <AirdropMessage>{size=17}{color=magenta}{i}Airdrop{/i} is coming!{/color}{/size}</AirdropMessage>
  <AirdropMessageIcon>https://i.imgur.com/JCDmlqI.png</AirdropMessageIcon>
  <AirdropInterval>3600</AirdropInterval>
  <Airdrops>
    <Airdrop>
      <AirdropId>1005</AirdropId>
      <Items>
        <AirdropItem>
          <ItemId>363</ItemId>
          <Chance>10</Chance>
        </AirdropItem>
        <AirdropItem>
          <ItemId>17</ItemId>
          <Chance>20</Chance>
        </AirdropItem>
      </Items>
    </Airdrop>
  </Airdrops>
  <AirdropSpawns>
    <AirdropSpawn>
      <AirdropId>1005</AirdropId>
      <Position>
        <X>1</X>
        <Y>1</Y>
        <Z>1</Z>
      </Position>
    </AirdropSpawn>
  </AirdropSpawns>
  <BlacklistedAirdrops />
</AirdropManagerConfiguration>
```
**RocketMod 5**
```yml
UseDefaultSpawns: true
UseDefaultAirdrops: false
AirdropMessage: <size=17><color=magenta><i>Airdrop</i> is coming!</color></size>
AirdropMessageIcon: https://i.imgur.com/JCDmlqI.png
AirdropInterval: 3600
Airdrops:
- AirdropId: 1005
  Items:
  - ItemId: 363
    Chance: 10
  - ItemId: 17
    Chance: 20
AirdropSpawns:
- AirdropId: 1005
  Position:
    X: 1
    Y: 1
    Z: 1
BlacklistedAirdrops: []
```

## Translation

**RocketMod 4**
```xml
<?xml version="1.0" encoding="utf-8"?>
<Translations xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <Translation Id="Next_Airdrop" Value="Next airdrop will be in {0}" />
</Translations>
```

**RocketMod 5**
```yml
NextAirdrop: Next airdrop will be in {0}!
```