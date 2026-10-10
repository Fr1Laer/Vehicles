## Vehicles

---

## Rakenduse kirjeldus

Projekt on C#-keeles kirjutatud ja WPF-il põhinev töölauarakendus. Rakendus võimaldab kasutajal lisada ja kustutada sõidukeid, neid valida, määrata vahemaa ja sooritada toetatud toiminguid (sõita/ujuda), vaadates samal ajal praegust läbisõitu ja toimingute ajalugu. Sõidukid jaotatakse automaatselt garaažidesse vastavalt nende tüübile. Kui garaaži lisatakse uus sõiduk, vilguvad teiste sõidukite esitulede tuled. Sama juhtub ka siis, kui sõiduk garaažist eemaldatakse

---

## Tehniline ülevaade

`Vehicles.Core` - mudelid,  loogika, liidesed, valideerimine ja `Resources.resx`

`Vehicles.WpfApp` - kasutajaliides, andmete sisestamine, sündmuste töötlemine ja kuvamine

---
## OOP ja muud funktsionaalsused

- **Abstraktsioon:** `Vehicle` - abstraktne baasklass

- **Pärimine:** `Car`, `Boat`, `AmphibiousCar` pärivad klassist `Vehicle`

- **Polümorfism:** `Move(double km)` on pärijates erinevalt realiseeritud

- **Kapseldamine:** `Odometer`-il on avalik `get` ja `protected set`, väli `_odometer` on `private`

- **Liidesed:** `IDriveable` ja `ISwimmable`  

- **Kogum:** kõik sõidukid on salvestatud ühes `ObservableCollection<Vehicle>`-is

- **Garaažid:** Autogaraaž, Paadigaraaž
--------

## Kuidas programmiga töötada

- Sisestage sõiduki mark ja mudel 

- Valige sõiduki tüüp

- Vajutage nuppu **Lisa sõiduk**

- Sisestage vahemaa väljale **Vahemaa**

- Vajutage nuppu **Liiguta**, **Sõida** või **Uju**, sõltuvalt valitud sõidukitüübist

- Seejärel lisatakse sõiduk automaatselt garaaži, sõltuvalt sõidukitüübist

- Sõiduki tabelist eemaldamiseks valige see ja vajutage nuppu **Eemalda sõiduk**

---

## Töö näide

<img width="1077" height="694" alt="image" src="https://github.com/user-attachments/assets/8eb49041-fcfe-4e0f-a2d3-be514172e37f" />



