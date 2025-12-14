# Barbie Webshop

A **Barbie Webshop** egy full stack webáruház alkalmazás,  
amely Barbie termékek böngészését, kosárkezelést és a rendelések leadását teszi lehetővé.

A projekt célja egy **átlátható, rétegelt backend architektúra** és egy  
**letisztult, reszponzív frontend** megvalósítása HTML, CSS és JavaScript segítségével.

---

## Fő funkciók

- Termékek listázása és kategória szerinti szűrése
- Kosárba helyezés készletellenőrzéssel
- Kosár kezelése (mennyiség módosítás, törlés)
- Felhasználói regisztráció és bejelentkezés
- Rendelés leadása
- Reszponzív felhasználói felület

---

## Használt technológiák

### Frontend
- **HTML5** – struktúra
- **CSS3** – megjelenés
- **JavaScript** - interakciók, API hívások

### Backend
- **C# / ASP.NET**
- **Entity Framework Core (EF Core)**
- **Repository pattern**
- **REST alapú kommunikáció**

### Adatbázis
- **PostgreSQL**

### Dokumentáció
- **UML diagramok**

---

## Fő funkciók

- Termékek listázása és kategória szerinti szűrése
- Kosár kezelése (hozzáadás, módosítás, törlés)
- Felhasználói regisztráció és bejelentkezés
- Rendelés leadása
- Készletellenőrzés vásárlás előtt

---

## Architektúra

Az alkalmazás **rétegelt architektúrát** használ:

- **Controller réteg**
  - HTTP kérések kezelése
  - Input validálás
  - Service réteg meghívása

- **Service réteg**
  - Üzleti logika
  - Készletellenőrzés
  - Számítások (összeg, mennyiség)

- **Repository réteg**
  - Adatbázis műveletek
  - CRUD műveletek entitásokra

---

## Fő modulok

### User
- Regisztráció
- Bejelentkezés
- Felhasználói adatok kezelése

### Product
- Termékek listázása
- Kategória szerinti szűrés
- Készlet lekérdezés

### Cart
- Kosár lekérdezése
- Termék hozzáadása
- Mennyiség módosítása
- Kosár ürítése

### Order
- Rendelés létrehozása
- Rendelések lekérdezése felhasználónként

---

## Folyamatleírás (példa)

### Termék kosárba helyezése

1. Felhasználó kosárba tesz egy terméket
2. Controller továbbítja a kérést a Service rétegnek
3. Service lekéri a terméket a ProductRepository-ból
4. Ellenőrzi a készletet
5. Ha van elegendő készlet:
   - Kosár frissítése
   - Sikeres válasz
6. Ha nincs elegendő készlet:
   - Hibaválasz

 A teljes folyamat a **sequence diagramon** részletesen megtekinthető.

---

## Adatmodell

Az adatbázis az alábbi fő entitásokat tartalmazza:

- User
- Product
- Cart
- CartItem
- Favorite
- Order
- OrderItem

Az entitások közti kapcsolatok az **UML diagramon** láthatók.


