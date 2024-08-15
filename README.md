# Airdrop Manager
The all-in-one airdrop manager for Unturned servers.

## Features
* **Airdrop Interval:** Automatically send airdrops at a configurable interval.
* **Airdrop Spawns:** Set up airdrop spawns with custom airdrop configurations.
* **Airdrop Commands:** Order airdrops at random spawn, specific spawn, or your current position.
* **Mass Airdrop:** Order a mass airdrop at all spawns.
* **Airdrop Grenade:** Configure airdrops to be ordered by throwing a specific grenade.
* **Customizable Messages:** Customize all messages sent by the plugin.
* **Customizable Airdrops:** Set up custom airdrops with custom items and weights.
* **Rich Text Formatting:** Use rich text formatting in messages.

## Commands
* **/airdrop** - Order an airdrop at random spawn.
* **/airdrop \<spawn\> [speed]** - Order an airdrop at a specific spawn with optional speed.
* **/airdrop random [speed]** - Order an airdrop at a random spawn with optional speed.
* **/airdrophere** - Order an airdrop at your current position.
* **/airdrophere \<airdrop\> [speed]** - Order an airdrop at your current position with optional speed.
* **/airdrophere random [speed]** - Order an airdrop at your current position with optional speed.
* **/massairdrop** - Order a mass airdrop at all spawns.
* **/massairdrop [speed]** - Order a mass airdrop at all spawns with optional speed.
* **/setairdropspawn \<name\> [airdrop]** - Set the airdrop spawn at your current position.
* **/whenairdrop** - Check the time until the next airdrop.
* **/rocket reload airdropmanager** - Reload the AirdropManager configuration.

## Permissions
All permissions are the same as the command names. Here are additional permissions:
```xml
<!-- Allows the player to specify spawn and speed. -->
<Permission Cooldown="0">airdrop.full</Permission>
<!-- Allows the player to specify airdrop and speed. -->
<Permission Cooldown="0">airdrophere.full</Permission>
<!-- Allows the player to specify speed. -->
<Permission Cooldown="0">massairdrop.full</Permission>
```

## FAQ
### How to use airdrop grenades?
In `Airdrops.{Map}.xml` file add `<Grenade />` tag, like so:
```xml
<?xml version="1.0" encoding="utf-8"?>
<AirdropsConfiguration xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <Airdrops>
    <Airdrop Id="541" Name="Washington_Carepackage_America">
      <Grenade Id="263" Name="Green Smoke"  />
      <Items>
        <Item Id="123" Name="Ranger Magazine" Weight="150" />
        <Item Id="1449" Name="Scalar Magazine" Weight="100" />
        ...

```
### How to change the size of storage of one specific airdrop?
In `Airdrops.{Map}.xml` file add `<Storage />` tag, like so:
```xml
<?xml version="1.0" encoding="utf-8"?>
<AirdropsConfiguration xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <Airdrops>
    <Airdrop Id="541" Name="Washington_Carepackage_America">
      <Storage Width="8" Height="10" />
      <Items>
        <Item Id="123" Name="Ranger Magazine" Weight="150" />
        <Item Id="1449" Name="Scalar Magazine" Weight="100" />
        ...
```
You can also change the barricade id of the storage by adding the `BarricadeId` attribute to the `<Storage />` tag.
```xml
<Storage BarricadeId="366" Name="Maple Crate" Width="4" Height="4" />
```
### How to change the effect of one specific airdrop?
In `Airdrops.{Map}.xml` file add `<LandedEffectGuid />` tag, like so:
```xml
<?xml version="1.0" encoding="utf-8"?>
<AirdropsConfiguration xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <Airdrops>
    <Airdrop Id="541" Name="Washington_Carepackage_America">
      <LandedEffectGuid>61d63a016a6448ffb6e4432a4c6a6ee1</LandedEffectGuid>
      <Items>
        <Item Id="123" Name="Ranger Magazine" Weight="150" />
        <Item Id="1449" Name="Scalar Magazine" Weight="100" />
        ...
```

## Configuration
### AirdropManager.configuration.xml
```xml
<?xml version="1.0" encoding="utf-8"?>
<AirdropManager3Configuration xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <Debug>false</Debug>
  <MessageColor>yellow</MessageColor>
  <EnableAirdropInterval>true</EnableAirdropInterval>
  <AirdropIntervalMin>2400</AirdropIntervalMin>
  <AirdropIntervalMax>3600</AirdropIntervalMax>
  <AirdropIntervalMinPlayers>5</AirdropIntervalMinPlayers>
  <DefaultAirdropSpeed>128</DefaultAirdropSpeed>
  <DefaultAirdropStorageBarricadeId>1374</DefaultAirdropStorageBarricadeId>
  <DefaultAirdropStorageWidth>7</DefaultAirdropStorageWidth>
  <DefaultAirdropStorageHeight>7</DefaultAirdropStorageHeight>
  <DefaultLandedEffectGuid>2c17fbd0f0ce49aeb3bc4637b68809a2</DefaultLandedEffectGuid>
  <Broadcasts DefaultIconUrl="https://i.imgur.com/kRIfsOg.png">
    <AirdropCommand Message="Airdrop is on the way to [[b]]{spawn}![[/b]]" Enabled="true" />
    <AirdropHereCommand Message="[[b]]{player}[[/b]] ordered an airdrop at their position!" Enabled="true" />
    <MassAirdropCommand Message="[[b]]Mass airdrop is on the way![[/b]]" Enabled="true" />
    <AirdropGrenade Message="[[b]]{player}[[/b]] threw an airdrop grenade!" Enabled="true" />
    <AirdropInterval Message="Airdrop is on the way to [[b]]{spawn}![[/b]]" Enabled="true" />
    <AirdropIntervalMinPlayers Message="Airdrop skipped: less than [[b]]{min_players}[[/b]] players online." Enabled="true" />
  </Broadcasts>
</AirdropManager3Configuration>
```

### Airdrops.Washington.xml
<?xml version="1.0" encoding="utf-8"?>
<AirdropsConfiguration xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <Airdrops>
    <Airdrop Id="541" Name="Washington_Carepackage_America">
      <Items>
        <Item Id="123" Name="Ranger Magazine" Weight="150" />
        <Item Id="1449" Name="Scalar Magazine" Weight="100" />
        <Item Id="1368" Name="Vonya Magazine" Weight="100" />
        <Item Id="1366" Name="Vonya" Weight="100" />
        <Item Id="1360" Name="Teklowvka" Weight="100" />
        <Item Id="1481" Name="Empire" Weight="100" />
        <Item Id="1241" Name="Demolition Charge" Weight="100" />
        <Item Id="1381" Name="Calling Card Magazine" Weight="100" />
        <Item Id="1369" Name="Bulldog" Weight="100" />
        <Item Id="1447" Name="Scalar" Weight="100" />
        <Item Id="1361" Name="Teklowvka Magazine" Weight="100" />
        <Item Id="1371" Name="Bulldog Magazine" Weight="100" />
        <Item Id="1379" Name="Calling Card" Weight="100" />
        <Item Id="1483" Name="Empire Magazine" Weight="100" />
        <Item Id="1375" Name="Fusilaut" Weight="50" />
        <Item Id="1484" Name="Devil's Bane" Weight="50" />
        <Item Id="1488" Name="Swissgewehr" Weight="50" />
        <Item Id="1362" Name="Augewehr" Weight="50" />
        <Item Id="1490" Name="Swissgewehr Magazine" Weight="50" />
        <Item Id="6" Name="Military Magazine" Weight="50" />
        <Item Id="363" Name="Maplestrike" Weight="50" />
        <Item Id="1485" Name="Devil's Bane Magazine" Weight="50" />
        <Item Id="1377" Name="Nightraider" Weight="50" />
        <Item Id="1240" Name="Detonator" Weight="40" />
        <Item Id="1384" Name="Ekho Magazine" Weight="25" />
        <Item Id="1382" Name="Ekho" Weight="25" />
        <Item Id="20" Name="Timberwolf Magazine" Weight="25" />
        <Item Id="18" Name="Timberwolf" Weight="25" />
        <Item Id="1364" Name="Hell's Fury" Weight="10" />
        <Item Id="1365" Name="Hell's Fury Drum" Weight="10" />
      </Items>
    </Airdrop>
  </Airdrops>
</AirdropsConfiguration>
```

### AirdropSpawns.Washington.xml
```xml
<?xml version="1.0" encoding="utf-8"?>
<AirdropSpawnsConfiguration xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <AirdropSpawns>
    <AirdropSpawn AirdropId="541" Name="Arlington Farm" X="354.934082" Y="38.4007874" Z="-860.104248" />
    <AirdropSpawn AirdropId="541" Name="Paradise Point" X="736.7805" Y="70.1387253" Z="-473.59314" />
    <AirdropSpawn AirdropId="541" Name="Kent Raceway" X="-230.669678" Y="38.4007759" Z="-809.34845" />
    <AirdropSpawn AirdropId="541" Name="Rainbridge Island" X="-142.0918" Y="28.9490013" Z="-308.365356" />
    <AirdropSpawn AirdropId="541" Name="Camano Campground" X="192.535034" Y="29.42662" Z="594.0785" />
    <AirdropSpawn AirdropId="541" Name="Bellevue Golf Course" X="-607.9508" Y="34.15535" Z="691.577" />
    <AirdropSpawn AirdropId="541" Name="Kennewick Farm" X="-690.5533" Y="76.52753" Z="323.4618" />
    <AirdropSpawn AirdropId="541" Name="Kennewick Farm" X="-826.906" Y="93.4762039" Z="56.05786" />
    <AirdropSpawn AirdropId="541" Name="Olympia Military Base" X="-847.740967" Y="83.5868149" Z="-445.6571" />
    <AirdropSpawn AirdropId="541" Name="Everett" X="887.0541" Y="80.42981" Z="108.680908" />
    <AirdropSpawn AirdropId="541" Name="Scorpion-7" X="841.015747" Y="71.11272" Z="809.552" />
    <AirdropSpawn AirdropId="541" Name="Clearwater Campground" X="-79.0050659" Y="48.83967" Z="592.875244" />
    <AirdropSpawn AirdropId="541" Name="Seattle" X="-311.916748" Y="38.40078" Z="105.736816" />
  </AirdropSpawns>
</AirdropSpawnsConfiguration>
```
## Translations
```xml
<?xml version="1.0" encoding="utf-8"?>
<Translations xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <Translation Id="AirdropNoPermission" Value="You don't have permission to specify airdrop spawn." />
  <Translation Id="AirdropHereNoPermission" Value="You don't have permission to specify airdrop." />
  <Translation Id="MassAirdropNoPermission" Value="You don't have permission to specify airdrop speed." />
  <Translation Id="AirdropSpawnNotFound" Value="Airdrop spawn with name [[b]]{0}[[/b]] doesn't exist." />
  <Translation Id="AirdropSpawnNone" Value="There isn't any airdrop spawns." />
  <Translation Id="AirdropNone" Value="There isn't any airdrops." />
  <Translation Id="AirdropSpawned" Value="Successfully called in [[b]]{0}[[/b]] airdrop to [[b]]{1}.[[/b]]" />
  <Translation Id="AirdropNotFound" Value="Airdrop with name or ID [[b]]{0}[[/b]] doesn't exist." />
  <Translation Id="AirdropHereSpawned" Value="Successfully called in [[b]]{0}[[/b]] to your position." />
  <Translation Id="MassAirdropSpawned" Value="Successfully called in [[b]]mass airdrop[[/b]] to all [[b]]{0}[[/b]] spawns." />
  <Translation Id="AirdropSpawnSet" Value="Successfully set airdrop spawn [[b]]{0}.[[/b]]" />
  <Translation Id="AirdropSpawnSetWithAirdrop" Value="Successfully set airdrop spawn [[b]]{0}[[/b]] for [[b]]{1}[[/b]] airdrop." />
  <Translation Id="WhenAirdropTimeLeft" Value="Next airdrop will be in [[b]]{0}.[[/b]]" />
  <Translation Id="WhenAirdropNotPlanned" Value="There isn't any automatic airdrop scheduled." />
  <Translation Id="Day" Value="1 day" />
  <Translation Id="Days" Value="{0} days" />
  <Translation Id="Hour" Value="1 hour" />
  <Translation Id="Hours" Value="{0} hours" />
  <Translation Id="Minute" Value="1 minute" />
  <Translation Id="Minutes" Value="{0} minutes" />
  <Translation Id="Second" Value="1 second" />
  <Translation Id="Seconds" Value="{0} seconds" />
  <Translation Id="Zero" Value="a moment" />
</Translations>
```