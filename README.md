## Vehicles

---

## Rakenduse kirjeldus

Projekt on C#-keeles kirjutatud ja WPF-il põhinev töölauarakendus. Rakendus võimaldab kasutajal lisada ja kustutada sõidukeid, neid valida, määrata vahemaa ja sooritada toetatud toiminguid (sõita/ujuda), vaadates samal ajal praegust läbisõitu ja toimingute ajalugu

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
--------

## Kuidas programmiga töötada

- Sisestage sõiduki mark ja mudel 

- Valige sõiduki tüüp

- Vajutage nuppu **Lisa sõiduk**

- Sisestage vahemaa väljale **Vahemaa**

- Vajutage nuppu **Liiguta**, **Sõida** või **Uju**, sõltuvalt valitud sõidukitüübist

- Seejärel ilmub kasutajaliidesele sõiduk ja sellega seotud teave

- Sõiduki tabelist eemaldamiseks valige see ja vajutage nuppu **Eemalda sõiduk**

---

## Veastsenaariumid ja teated

### Valed sisendandmed:

<img width="979" height="635" alt="Знімок екрана 2026-09-29 194020" src="https://github.com/user-attachments/assets/fd05412a-03dc-494b-a771-853b780668f3" />

<img width="980" height="632" alt="image" src="https://github.com/user-attachments/assets/90355150-1474-42ca-ab9e-44dfd5b5715d" />

<img width="986" height="633" alt="image" src="https://github.com/user-attachments/assets/27c4ada4-f066-4806-b761-0e02b25dce3e" />

---
## Näide täidetud tabelist

<img width="983" height="639" alt="image" src="https://github.com/user-attachments/assets/1b8a72a1-4cbd-40d9-ab76-480070aff76b" />

<img width="982" height="634" alt="image" src="https://github.com/user-attachments/assets/c14caa50-a7cb-43eb-8220-e2ebf9d613c2" />

### Tabel pärast **Amphi 600** eemaldamist

<img width="982" height="638" alt="image" src="https://github.com/user-attachments/assets/25a3a2bf-41e5-4725-ba59-ddb03e0cf9ae" />


