using ElectroDepotClassLibrary.Services;
using ElectroDepotClassLibrary.Utility;
using Microsoft.EntityFrameworkCore;
using Server.Context;
using Server.Models;

public static class TestingDataSeeder
{
    public static async Task SeedDataAsync(DatabaseContext context)
    {
        await context.ProjectComponents.ExecuteDeleteAsync();
        await context.PurchaseItems.ExecuteDeleteAsync();
        await context.OwnsComponent.ExecuteDeleteAsync();

        await context.Projects.ExecuteDeleteAsync();
        await context.Purchases.ExecuteDeleteAsync();
        await context.Components.ExecuteDeleteAsync();

        await context.Users.ExecuteDeleteAsync();
        await context.Suppliers.ExecuteDeleteAsync();
        await context.Categories.ExecuteDeleteAsync();

        await context.SaveChangesAsync();

        if (!context.Users.Any())
        {
            List<User> users = new List<User>
            {
                new User { Username = "jacek.jaworek", Password = "FindMeIfYouCan123", Email = "jacek.jaworek@gmail.com" },
                new User { Username = "agnieszka.nakowicz", Password = "mocSensora789", Email = "agnieszka.nakowicz@gmail.com" },
                new User { Username = "hanna.kozłowska", Password = "IoTAndBeyond456", Email = "hanna.kozłowska@gmail.com" },
                new User { Username = "piotr.wisniewski", Password = "powerSupplyGuru", Email = "piotr.wisniewski@gmail.com" },
                new User { Username = "jarosław.paduch", Password = "TylkoAVR420", Email = "jarosław.paduch@gmail.com" },
                new User { Username = "andrzej.kowalski", Password = "PiMasters2023", Email = "andrzej.kowalski@gmail.com" },
                new User { Username = "oleg.ontemijczuk", Password = "tylkoWindowsEmbedded", Email = "oleg.ontemijczuk@gmail.com" },
                new User { Username = "ewa.maj", Password = "SecureNetworking45", Email = "ewa.maj@gmail.com" },
                new User { Username = "grzegorz.baron", Password = "digitalSeamanProcessing", Email = "grzegorz.baron@gmail.com" },
                new User { Username = "joanna.pawlowska", Password = "SmartHomeLife55", Email = "joanna.pawlowska@gmail.com" }
            };

            context.Users.AddRange(users);
            await context.SaveChangesAsync();

            string fullPath = "D:\\Repo\\ElectronDepot\\ElectroDepot\\Server\\Assests\\";
            string categoriesFullPath = fullPath + "Categories\\";
            List<Category> categories = new List<Category>
            {
                new Category { Name = "Czujnik ciśnienia", Description = "Mierzy ciśnienie", Image = File.ReadAllBytes(categoriesFullPath + "Pressure.png")},
                new Category { Name = "Czujnik czystości powietrza", Description = "Mierzy poziom czystości powietrza", Image = File.ReadAllBytes(categoriesFullPath + "AirQuality.png") },
                new Category { Name = "Czujnik gestów", Description = "Wykrywa gesty", Image = File.ReadAllBytes(categoriesFullPath + "Gesture.png") },
                new Category { Name = "Czujnik krańcowe", Description = "Wykrywa osiągnięcie krańca ruchu", Image = File.ReadAllBytes(categoriesFullPath + "LimitSensor.png") },
                new Category { Name = "Czujnik gazów", Description = "Mierzy stężenie gazów", Image = File.ReadAllBytes(categoriesFullPath + "GasSensor.png") },
                new Category { Name = "Czujnik magnetyczne", Description = "Wykrywa pole magnetyczne", Image = File.ReadAllBytes(categoriesFullPath + "MagneticSensor.png") },
                new Category { Name = "Czujnik odbiciowe", Description = "Wykrywa obiekty przez odbicie światła", Image = File.ReadAllBytes(categoriesFullPath + "ReflectiveSensor.png") },
                new Category { Name = "Czujnik odległości", Description = "Mierzy odległość", Image = File.ReadAllBytes(categoriesFullPath + "Distance.png") },
                new Category { Name = "Czujnik temperatury", Description = "Mierzy temperaturę", Image = File.ReadAllBytes(categoriesFullPath + "Temperature.png") },
                new Category { Name = "Czujnik wilgotności", Description = "Mierzy wilgotność", Image = File.ReadAllBytes(categoriesFullPath + "Humidity.png") },
                new Category { Name = "Enkodery", Description = "Mierzą pozycję kątową lub liniową", Image = File.ReadAllBytes(categoriesFullPath + "Encoder.png") },
                new Category { Name = "Fotorezystory", Description = "Zmieniają opór w zależności od światła", Image = File.ReadAllBytes(categoriesFullPath + "Photoresistor.png") },
                new Category { Name = "Fototranzystory", Description = "Wykrywają światło za pomocą tranzystora", Image = File.ReadAllBytes(categoriesFullPath + "Phototransistor.png") },
                new Category { Name = "Odbiornik podczerwieni", Description = "Odbiera sygnały podczerwieni", Image = File.ReadAllBytes(categoriesFullPath + "Infrared.png") },
                new Category { Name = "Akcelerometry", Description = "Mierzy przyspieszenie", Image = File.ReadAllBytes(categoriesFullPath + "Accelerometer.png") },
                new Category { Name = "Czujnik hallotronowe", Description = "Wykrywa pole magnetyczne za pomocą efektu Halla", Image = File.ReadAllBytes(categoriesFullPath + "Hall.png") },
                new Category { Name = "Mikrokontroler", Description = "Programowo sterowany układ elektroniczny", Image = File.ReadAllBytes(categoriesFullPath + "Microcontroller.png") }
            };

            context.Categories.AddRange(categories);
            await context.SaveChangesAsync();

            // Seed Suppliers
            List<Supplier> suppliers = new List<Supplier>
            {
                new Supplier { Name = "DigiKey", Website = "https://www.digikey.pl/", Image = File.ReadAllBytes(fullPath + "DigiKeyIcon.png") },
                new Supplier { Name = "Botland", Website = "https://botland.com.pl/", Image = File.ReadAllBytes(fullPath + "BotlandIcon.png") },
                new Supplier { Name = "Mouser", Website = "https://www.mouser.com/", Image = File.ReadAllBytes(fullPath + "MouserIcon.png") },
                new Supplier { Name = "Kamami", Website = "https://kamami.pl/", Image = File.ReadAllBytes(fullPath + "KamamiIcon.png") },
                new Supplier { Name = "M-Salamon", Website = "https://msalamon.pl/", Image = File.ReadAllBytes(fullPath + "MSalamonIcon.png") },
                new Supplier { Name = "Allegro", Website = "https://allegro.pl/", Image = File.ReadAllBytes(fullPath + "AllegroIcon.png") },
                new Supplier { Name = "AliExpress", Website = "https://pl.aliexpress.com/", Image = File.ReadAllBytes(fullPath + "AliexpressIcon.png") },
            };
            context.Suppliers.AddRange(suppliers);
            await context.SaveChangesAsync();

            List<Component> components = new List<Component>
            {
                // Czujniki ciśnienia
                new Component { Name = "BMP180", Manufacturer = "Bosch", ShortDescription   = "Czujnik ciśnienia barometrycznego", CategoryID = categories[0].CategoryID },
                new Component { Name = "MPX5700AP", Manufacturer = "NXP", ShortDescription  = "Czujnik ciśnienia absolutnego", CategoryID = categories[0].CategoryID },
                new Component { Name = "MPX5010", Manufacturer = "NXP", ShortDescription  = "Czujnik ciśnienia różnicowego o zakresie 0-10 kPa", CategoryID = categories[0].CategoryID },
                new Component { Name = "BMP280", Manufacturer = "Bosch", ShortDescription  = "Czujnik ciśnienia atmosferycznego i wysokościomierz", CategoryID = categories[0].CategoryID },
                new Component { Name = "APC301", Manufacturer = "Amphenol", ShortDescription  = "Analogowy, przemysłowy czujnik ciśnienia", CategoryID = categories[0].CategoryID },

                // Czujniki czystości powietrza
                new Component { Name = "MQ-135", Manufacturer = "Winsen", ShortDescription  = "Czujnik jakości powietrza", CategoryID = categories[1].CategoryID },
                new Component { Name = "CCS811", Manufacturer = "AMS", ShortDescription  = "Czujnik gazów organicznych i CO2", CategoryID = categories[1].CategoryID },
                new Component { Name = "MQ-7", Manufacturer = "Winsen", ShortDescription  = "Czujnik gazu do detekcji CO", CategoryID = categories[1].CategoryID },
                new Component { Name = "AQS-1", Manufacturer = "Air Quality Sensor", ShortDescription  = "Czujnik jakości powietrza do pomiaru zanieczyszczeń i CO2", CategoryID = categories[1].CategoryID },

                // Czujniki gestów
                new Component { Name = "APDS-9960", Manufacturer = "Broadcom", ShortDescription  = "Czujnik gestów, światła i koloru", CategoryID = categories[2].CategoryID },
                new Component { Name = "AS3935", Manufacturer = "ams AG", ShortDescription  = "Czujnik gestów oraz detekcji burzy", CategoryID = categories[2].CategoryID },
                new Component { Name = "MTGesture", Manufacturer = "Microchip", ShortDescription  = "Czujnik gestów 3D z funkcją zdalnego sterowania", CategoryID = categories[2].CategoryID },
                new Component { Name = "ADPD188GG", Manufacturer = "Analog Devices", ShortDescription  = "Czujnik optyczny do detekcji gestów i monitorowania zdrowia", CategoryID = categories[2].CategoryID },

                // Czujniki krańcowe
                new Component { Name = "Limit Switch KW12-3", Manufacturer = "Omron", ShortDescription = "Mechaniczny czujnik krańcowy", CategoryID = categories[3].CategoryID },
                new Component { Name = "ME-8108", Manufacturer = "Heschen", ShortDescription  = "Przemysłowy czujnik krańcowy z dźwignią rolkową", CategoryID = categories[3].CategoryID },
                new Component { Name = "Omron D4MC", Manufacturer = "Omron", ShortDescription  = "Mikroprzełącznik krańcowy z wysoką trwałością", CategoryID = categories[3].CategoryID },
                new Component { Name = "V-156-1C25", Manufacturer = "Honeywell", ShortDescription = "Przełącznik krańcowy z dźwignią rolkową", CategoryID = categories[3].CategoryID },
                new Component { Name = "MLC-S-130L", Manufacturer = "Honeywell", ShortDescription  = "Kompaktowy krańcowy czujnik z krótkim ramieniem", CategoryID = categories[3].CategoryID },

                // Czujniki gazów
                new Component { Name = "MQ-2", Manufacturer = "Hanwei", ShortDescription  = "Czujnik gazów palnych i dymu", CategoryID = categories[4].CategoryID },
                new Component { Name = "MH-Z19B", Manufacturer = "Winsen", ShortDescription  = "Czujnik dwutlenku węgla (CO2)", CategoryID = categories[4].CategoryID },
                new Component { Name = "MQ-3", Manufacturer = "Hanwei Electronics", ShortDescription  = "Czujnik alkoholu do wykrywania oparów alkoholu", CategoryID = categories[4].CategoryID },
                new Component { Name = "ZP07-O3", Manufacturer = "Winsen", ShortDescription  = "Czujnik ozonu (O3) do pomiaru jakości powietrza", CategoryID = categories[4].CategoryID },

                // Czujniki magnetyczne
                new Component { Name = "A3144", Manufacturer = "Allegro", ShortDescription  = "Czujnik Halla do wykrywania pola magnetycznego", CategoryID = categories[5].CategoryID },
                new Component { Name = "TLE493D", Manufacturer = "Infineon", ShortDescription  = "Czujnik Halla z trzema osiami do pomiaru pola magnetycznego", CategoryID = categories[5].CategoryID },
                new Component { Name = "LIS3MDL", Manufacturer = "STMicroelectronics", ShortDescription  = "Czujnik magnetyczny 3-osiowy z możliwością pomiaru pól magnetycznych", CategoryID = categories[5].CategoryID },
                new Component { Name = "HMC5883L", Manufacturer = "Honeywell", ShortDescription  = "Czujnik kompasu 3-osiowego, który wykorzystuje technologię magnetyczną", CategoryID = categories[5].CategoryID },
                new Component { Name = "MMT-3D", Manufacturer = "Microchip", ShortDescription  = "Czujnik Halla do pomiaru pola magnetycznego w trzech wymiarach", CategoryID = categories[5].CategoryID },

                // Czujniki odbiciowe
                new Component { Name = "TCRT5000", Manufacturer = "Vishay", ShortDescription  = "Odbiciowy czujnik optyczny", CategoryID = categories[6].CategoryID },
                new Component { Name = "QRD1114", Manufacturer = "Fairchild", ShortDescription  = "Odbiciowy czujnik optyczny do detekcji obiektów", CategoryID = categories[6].CategoryID },
                new Component { Name = "TSOP4838", Manufacturer = "Vishay", ShortDescription  = "Odbiciowy czujnik optyczny, odbiornik IR", CategoryID = categories[6].CategoryID },
                new Component { Name = "GP2Y0A21YK0F", Manufacturer = "Sharp", ShortDescription  = "Odbiciowy czujnik odległości z analogowym wyjściem", CategoryID = categories[6].CategoryID },
                new Component { Name = "TCS3200", Manufacturer = "Texas Instruments", ShortDescription  = "Odbiciowy czujnik koloru z detekcją światła", CategoryID = categories[6].CategoryID },

                // Czujniki odległości
                new Component { Name = "HC-SR04", Manufacturer = "Elecfreaks", ShortDescription  = "Ultradźwiękowy czujnik odległości", CategoryID = categories[7].CategoryID },
                new Component { Name = "VL53L0X", Manufacturer = "STMicroelectronics", ShortDescription  = "Czujnik odległości oparty na pomiarze czasu przelotu", CategoryID = categories[7].CategoryID },
                new Component { Name = "SRF05", Manufacturer = "SRF", ShortDescription  = "Ultradźwiękowy czujnik odległości z pomiarem do 4 metrów", CategoryID = categories[7].CategoryID },
                new Component { Name = "GP2Y0A02YK0F", Manufacturer = "Sharp", ShortDescription  = "Czujnik odległości z analogowym wyjściem do 2 metrów", CategoryID = categories[7].CategoryID },
                new Component { Name = "LIDAR-Lite v3", Manufacturer = "Garmin", ShortDescription  = "Czujnik LIDAR do dokładnego pomiaru odległości", CategoryID = categories[7].CategoryID },
                new Component { Name = "ToF Rangefinder", Manufacturer = "Benewake", ShortDescription  = "Czujnik odległości Time-of-Flight, wysoka dokładność i szybki pomiar", CategoryID = categories[7].CategoryID },

                // Czujniki temperatury
                new Component { Name = "DS18B20", Manufacturer = "Maxim Integrated", ShortDescription  = "Cyfrowy czujnik temperatury", CategoryID = categories[8].CategoryID },
                new Component { Name = "LM35", Manufacturer = "Texas Instruments", ShortDescription  = "Analogowy czujnik temperatury", CategoryID = categories[8].CategoryID },
                new Component { Name = "BME280", Manufacturer = "Bosch", ShortDescription = "Czujnik temperatury, wilgotności i ciśnienia atmosferycznego", CategoryID = categories[8].CategoryID },
                new Component { Name = "TMP36", Manufacturer = "Analog Devices", ShortDescription  = "Analogowy czujnik temperatury o wysokiej dokładności", CategoryID = categories[8].CategoryID },
                new Component { Name = "HDC1080", Manufacturer = "Texas Instruments", ShortDescription  = "Cyfrowy czujnik temperatury i wilgotności, niskopoborowy", CategoryID = categories[8].CategoryID },

                // Czujniki wilgotności
                new Component { Name = "DHT22", Manufacturer = "AOSONG", ShortDescription  = "Cyfrowy czujnik wilgotności i temperatury", CategoryID = categories[9].CategoryID },
                new Component { Name = "AM2301", Manufacturer = "AOSONG", ShortDescription  = "Cyfrowy czujnik wilgotności i temperatury", CategoryID = categories[9].CategoryID },
                new Component { Name = "SHT31", Manufacturer = "Sensirion", ShortDescription  = "Cyfrowy czujnik wilgotności i temperatury o wysokiej precyzji", CategoryID = categories[9].CategoryID },
                new Component { Name = "SHTC3", Manufacturer = "Sensirion", ShortDescription  = "Miniaturowy cyfrowy czujnik wilgotności i temperatury", CategoryID = categories[9].CategoryID },

                // Enkodery
                new Component { Name = "KY-040", Manufacturer = "Keyes", ShortDescription  = "Obrotowy enkoder do wykrywania położenia kątowego", CategoryID = categories[10].CategoryID },
                new Component { Name = "EC11", Manufacturer = "Bourns", ShortDescription  = "Obrotowy enkoder z przyciskiem, idealny do regulacji i nawigacji", CategoryID = categories[10].CategoryID },
                new Component { Name = "Bourns PEC11R", Manufacturer = "Bourns", ShortDescription  = "Ciche, rotacyjne enkodery z wieloma wyjściami, idealne do projektów audio", CategoryID = categories[10].CategoryID },
                new Component { Name = "AMT102", Manufacturer = "CUI Devices", ShortDescription  = "Inkrementalny enkoder obrotowy z wyjściem cyfrowym, wysokiej precyzji", CategoryID = categories[10].CategoryID },

                // Fotorezystory
                new Component { Name = "GL5528", Manufacturer = "GLT", ShortDescription  = "Fotorezystor reagujący na poziom światła", CategoryID = categories[11].CategoryID },
                new Component { Name = "GL5537", Manufacturer = "GLT", ShortDescription  = "Fotorezystor reagujący na światło, stosowany w automatyce", CategoryID = categories[11].CategoryID },
                new Component { Name = "LDR", Manufacturer = "General", ShortDescription  = "Czujnik światła, fotorezystor o dużej czułości, używany w aplikacjach DIY", CategoryID = categories[11].CategoryID },
                new Component { Name = "TSL2591", Manufacturer = "Adafruit", ShortDescription  = "Cyfrowy czujnik światła o wysokiej czułości z interfejsem I2C", CategoryID = categories[11].CategoryID },
                new Component { Name = "VEML6075", Manufacturer = "Vishay", ShortDescription  = "Fotorezystor UV do pomiaru poziomu promieniowania ultrafioletowego", CategoryID = categories[11].CategoryID },

                // Fototranzystory
                new Component { Name = "PT333-3C", Manufacturer = "Everlight", ShortDescription  = "Fototranzystor czuły na światło widzialne", CategoryID = categories[12].CategoryID },
                new Component { Name = "LPT-80", Manufacturer = "Lite-On", ShortDescription  = "Fototranzystor czuły na światło widzialne, idealny do detekcji obiektów", CategoryID = categories[12].CategoryID },
                new Component { Name = "HPT-20", Manufacturer = "Everlight", ShortDescription  = "Fototranzystor o wysokiej czułości, przeznaczony do zastosowań optoelektronicznych", CategoryID = categories[12].CategoryID },
                new Component { Name = "PT203-3C", Manufacturer = "Everlight", ShortDescription  = "Fototranzystor do zastosowań ogólnych, czuły na światło widzialne", CategoryID = categories[12].CategoryID },

                // Odbiorniki podczerwieni
                new Component { Name = "TSOP38238", Manufacturer = "Vishay", ShortDescription  = "Odbiornik podczerwieni do dekodowania sygnałów IR", CategoryID = categories[13].CategoryID },
                new Component { Name = "TSOP38240", Manufacturer = "Vishay", ShortDescription  = "Odbiornik podczerwieni do dekodowania sygnałów IR w zakresie 40 kHz", CategoryID = categories[13].CategoryID },
                new Component { Name = "VS1838B", Manufacturer = "Vishay", ShortDescription  = "Odbiornik podczerwieni o dużej czułości i niskim poborze mocy", CategoryID = categories[13].CategoryID },
                new Component { Name = "GP1UX511QS", Manufacturer = "Sharp", ShortDescription  = "Odbiornik podczerwieni z dużym zasięgiem i wąskim kątem detekcji", CategoryID = categories[13].CategoryID },
                new Component { Name = "RPR-220", Manufacturer = "Rohm", ShortDescription  = "Odbiornik podczerwieni, przeznaczony do różnych zastosowań IR", CategoryID = categories[13].CategoryID },

                // Akcelerometry
                new Component { Name = "ADXL345", Manufacturer = "Analog Devices", ShortDescription  = "Trójosiowy akcelerometr", CategoryID = categories[14].CategoryID },
                new Component { Name = "LIS3DH", Manufacturer = "STMicroelectronics", ShortDescription  = "Trójosiowy akcelerometr z cyfrowym wyjściem I2C/SPI", CategoryID = categories[14].CategoryID },
                new Component { Name = "BMA220", Manufacturer = "Bosch", ShortDescription  = "Miniaturowy trójosiowy akcelerometr z niskim poborem mocy", CategoryID = categories[14].CategoryID },
                new Component { Name = "MPU6050", Manufacturer = "InvenSense", ShortDescription  = "Akcelerometr 3-osiowy z żyroskopem 3-osiowym w jednym chipie", CategoryID = categories[14].CategoryID },
                new Component { Name = "ADXL362", Manufacturer = "Analog Devices", ShortDescription  = "Trójosiowy akcelerometr z ultra-niskim poborem mocy i funkcją detekcji ruchu", CategoryID = categories[14].CategoryID },

                // Czujniki hallotronowe
                new Component { Name = "DRV5053", Manufacturer = "Texas Instruments", ShortDescription  = "Czujnik Halla, liniowy pomiar pola magnetycznego", CategoryID = categories[15].CategoryID },
                new Component { Name = "A3141", Manufacturer = "Allegro", ShortDescription  = "Czujnik Halla do pomiaru pola magnetycznego z analogowym wyjściem", CategoryID = categories[15].CategoryID },
                new Component { Name = "HMC5883L", Manufacturer = "Honeywell", ShortDescription  = "Czujnik 3-osiowy Halla do pomiaru pola magnetycznego", CategoryID = categories[15].CategoryID },
                new Component { Name = "TLE493D-W2B6", Manufacturer = "Infineon", ShortDescription  = "Czujnik Halla o niskim poborze mocy z możliwością pomiaru pól magnetycznych", CategoryID = categories[15].CategoryID },
                new Component { Name = "DRV5055", Manufacturer = "Texas Instruments", ShortDescription  = "Czujnik Halla, liniowy pomiar pola magnetycznego z wyjściem analogowym", CategoryID = categories[15].CategoryID },

                // Mikrokontrolery
                new Component { Name = "ATmega328P", Manufacturer = "Microchip", ShortDescription  = "8-bitowy mikrokontroler AVR używany w platformach Arduino", CategoryID = categories[16].CategoryID },
                new Component { Name = "STM32F103C8T6", Manufacturer = "STMicroelectronics", ShortDescription  = "32-bitowy mikrokontroler z rdzeniem ARM Cortex-M3, popularny w rozwoju STM32", CategoryID = categories[16].CategoryID },
                new Component { Name = "ESP8266", Manufacturer = "Espressif", ShortDescription  = "Mikrokontroler WiFi o niskiej mocy, często używany w IoT", CategoryID = categories[16].CategoryID },
                new Component { Name = "ESP32", Manufacturer = "Espressif", ShortDescription  = "Mikrokontroler z dwurdzeniowym procesorem i wbudowanym WiFi i Bluetooth", CategoryID = categories[16].CategoryID },
                new Component { Name = "PIC16F877A", Manufacturer = "Microchip", ShortDescription  = "Mikrokontroler 8-bitowy z rodziny PIC, popularny w aplikacjach embedded", CategoryID = categories[16].CategoryID },
                new Component { Name = "ATtiny85", Manufacturer = "Microchip", ShortDescription  = "Mały mikrokontroler AVR, idealny do projektów o ograniczonej przestrzeni", CategoryID = categories[16].CategoryID },
                new Component { Name = "RP2040", Manufacturer = "Raspberry Pi Foundation", ShortDescription  = "Mikrokontroler z dwurdzeniowym procesorem ARM Cortex-M0+, stosowany w Raspberry Pi Pico", CategoryID = categories[16].CategoryID },
                new Component { Name = "MSP430G2553", Manufacturer = "Texas Instruments", ShortDescription  = "16-bitowy mikrokontroler z ultraniskim poborem mocy", CategoryID = categories[16].CategoryID },
                new Component { Name = "nRF52840", Manufacturer = "Nordic Semiconductor", ShortDescription  = "Mikrokontroler z wbudowanym Bluetooth Low Energy (BLE) i NFC, stosowany w aplikacjach IoT", CategoryID = categories[16].CategoryID },
                new Component { Name = "SAMD21", Manufacturer = "Microchip", ShortDescription  = "Mikrokontroler 32-bitowy z rdzeniem ARM Cortex-M0+, używany w Arduino Zero", CategoryID = categories[16].CategoryID }
            };
            context.Components.AddRange(components);
            await context.SaveChangesAsync();

            List<Purchase> purchases = new List<Purchase>
            {
                new Purchase { UserID = users[0].UserID, SupplierID = suppliers[0].SupplierID, PurchasedDate = new DateTime(2024, 10, 14, 0, 0, 0), TotalPrice = 150.50 },
                new Purchase { UserID = users[1].UserID, SupplierID = suppliers[1].SupplierID, PurchasedDate = new DateTime(2024, 10, 15, 0, 0, 0), TotalPrice = 320.75 },
                new Purchase { UserID = users[1].UserID, SupplierID = suppliers[3].SupplierID, PurchasedDate = new DateTime(2024, 10, 15, 0, 0, 0), TotalPrice = 320.75 },
                new Purchase { UserID = users[2].UserID, SupplierID = suppliers[2].SupplierID, PurchasedDate = new DateTime(2024, 10, 16, 0, 0, 0), TotalPrice = 210.40 },
                new Purchase { UserID = users[3].UserID, SupplierID = suppliers[3].SupplierID, PurchasedDate = new DateTime(2024, 10, 17, 0, 0, 0), TotalPrice = 275.60 },
                new Purchase { UserID = users[4].UserID, SupplierID = suppliers[2].SupplierID, PurchasedDate = new DateTime(2024, 10, 18, 0, 0, 0), TotalPrice = 140.00 },
                new Purchase { UserID = users[4].UserID, SupplierID = suppliers[3].SupplierID, PurchasedDate = new DateTime(2024, 10, 18, 0, 0, 0), TotalPrice = 140.00 },
                new Purchase { UserID = users[5].UserID, SupplierID = suppliers[1].SupplierID, PurchasedDate = new DateTime(2024, 10, 19, 0, 0, 0), TotalPrice = 330.00 },
                new Purchase { UserID = users[6].UserID, SupplierID = suppliers[2].SupplierID, PurchasedDate = new DateTime(2024, 10, 20, 0, 0, 0), TotalPrice = 190.75 },
                new Purchase { UserID = users[6].UserID, SupplierID = suppliers[2].SupplierID, PurchasedDate = new DateTime(2024, 10, 20, 0, 0, 0), TotalPrice = 190.75 },
                new Purchase { UserID = users[7].UserID, SupplierID = suppliers[3].SupplierID, PurchasedDate = new DateTime(2024, 10, 21, 0, 0, 0), TotalPrice = 285.20 },
                new Purchase { UserID = users[8].UserID, SupplierID = suppliers[0].SupplierID, PurchasedDate = new DateTime(2024, 10, 22, 0, 0, 0), TotalPrice = 165.80 },
                new Purchase { UserID = users[8].UserID, SupplierID = suppliers[1].SupplierID, PurchasedDate = new DateTime(2024, 10, 22, 0, 0, 0), TotalPrice = 165.80 },
                new Purchase { UserID = users[8].UserID, SupplierID = suppliers[2].SupplierID, PurchasedDate = new DateTime(2024, 10, 22, 0, 0, 0), TotalPrice = 165.80 },
                new Purchase { UserID = users[9].UserID, SupplierID = suppliers[1].SupplierID, PurchasedDate = new DateTime(2024, 10, 23, 0, 0, 0), TotalPrice = 210.50 },
                new Purchase { UserID = users[9].UserID, SupplierID = suppliers[2].SupplierID, PurchasedDate = new DateTime(2024, 10, 23, 0, 0, 0), TotalPrice = 210.50 }
            };
            context.Purchases.AddRange(purchases);
            await context.SaveChangesAsync();

            List<PurchaseItem> purchaseItems = new List<PurchaseItem>
            {
                new PurchaseItem { PurchaseID = purchases[0].PurchaseID, ComponentID = components[77].ComponentID, Quantity = 1, PricePerUnit = 8.50 },
                new PurchaseItem { PurchaseID = purchases[0].PurchaseID, ComponentID = components[2].ComponentID, Quantity = 1, PricePerUnit = 7.50 },
                new PurchaseItem { PurchaseID = purchases[0].PurchaseID, ComponentID = components[5].ComponentID, Quantity = 2, PricePerUnit = 4.50 },
                new PurchaseItem { PurchaseID = purchases[0].PurchaseID, ComponentID = components[38].ComponentID, Quantity = 1, PricePerUnit = 2.50 },
                new PurchaseItem { PurchaseID = purchases[0].PurchaseID, ComponentID = components[82].ComponentID, Quantity = 2, PricePerUnit = 27.50 },
                new PurchaseItem { PurchaseID = purchases[0].PurchaseID, ComponentID = components[67].ComponentID, Quantity = 2, PricePerUnit = 7.50 },
                new PurchaseItem { PurchaseID = purchases[0].PurchaseID, ComponentID = components[9].ComponentID, Quantity = 2, PricePerUnit = 5.50 },
                new PurchaseItem { PurchaseID = purchases[1].PurchaseID, ComponentID = components[80].ComponentID, Quantity = 1, PricePerUnit = 12.75 },
                new PurchaseItem { PurchaseID = purchases[1].PurchaseID, ComponentID = components[50].ComponentID, Quantity = 1, PricePerUnit = 8.75 },
                new PurchaseItem { PurchaseID = purchases[1].PurchaseID, ComponentID = components[41].ComponentID, Quantity = 1, PricePerUnit = 0.75 },
                new PurchaseItem { PurchaseID = purchases[1].PurchaseID, ComponentID = components[21].ComponentID, Quantity = 1, PricePerUnit = 8.75 },
                new PurchaseItem { PurchaseID = purchases[1].PurchaseID, ComponentID = components[7].ComponentID, Quantity = 2, PricePerUnit = 6.75 },
                new PurchaseItem { PurchaseID = purchases[1].PurchaseID, ComponentID = components[1].ComponentID, Quantity = 2, PricePerUnit = 2.75 },
                new PurchaseItem { PurchaseID = purchases[1].PurchaseID, ComponentID = components[74].ComponentID, Quantity = 2, PricePerUnit = 5.75 },
                new PurchaseItem { PurchaseID = purchases[1].PurchaseID, ComponentID = components[21].ComponentID, Quantity = 2, PricePerUnit = 1.75 },
                new PurchaseItem { PurchaseID = purchases[1].PurchaseID, ComponentID = components[4].ComponentID, Quantity = 2, PricePerUnit = 9.75 },
                new PurchaseItem { PurchaseID = purchases[1].PurchaseID, ComponentID = components[3].ComponentID, Quantity = 2, PricePerUnit = 7.75 },
                new PurchaseItem { PurchaseID = purchases[2].PurchaseID, ComponentID = components[75].ComponentID, Quantity = 2, PricePerUnit = 8.40 },
                new PurchaseItem { PurchaseID = purchases[2].PurchaseID, ComponentID = components[47].ComponentID, Quantity = 2, PricePerUnit = 4.40 },
                new PurchaseItem { PurchaseID = purchases[2].PurchaseID, ComponentID = components[13].ComponentID, Quantity = 2, PricePerUnit = 4.40 },
                new PurchaseItem { PurchaseID = purchases[2].PurchaseID, ComponentID = components[0].ComponentID, Quantity = 2, PricePerUnit = 2.40 },
                new PurchaseItem { PurchaseID = purchases[2].PurchaseID, ComponentID = components[22].ComponentID, Quantity = 2, PricePerUnit = 6.40 },
                new PurchaseItem { PurchaseID = purchases[2].PurchaseID, ComponentID = components[79].ComponentID, Quantity = 2, PricePerUnit = 8.40 },
                new PurchaseItem { PurchaseID = purchases[2].PurchaseID, ComponentID = components[69].ComponentID, Quantity = 2, PricePerUnit = 14.40 },
                new PurchaseItem { PurchaseID = purchases[2].PurchaseID, ComponentID = components[62].ComponentID, Quantity = 2, PricePerUnit = 16.40 },
                new PurchaseItem { PurchaseID = purchases[2].PurchaseID, ComponentID = components[60].ComponentID, Quantity = 2, PricePerUnit = 2.40 },
                new PurchaseItem { PurchaseID = purchases[2].PurchaseID, ComponentID = components[3].ComponentID, Quantity = 2, PricePerUnit = 9.40 },
                new PurchaseItem { PurchaseID = purchases[3].PurchaseID, ComponentID = components[83].ComponentID, Quantity = 2, PricePerUnit = 8.60 },
                new PurchaseItem { PurchaseID = purchases[3].PurchaseID, ComponentID = components[59].ComponentID, Quantity = 2, PricePerUnit = 3.60 },
                new PurchaseItem { PurchaseID = purchases[3].PurchaseID, ComponentID = components[46].ComponentID, Quantity = 2, PricePerUnit = 6.60 },
                new PurchaseItem { PurchaseID = purchases[3].PurchaseID, ComponentID = components[24].ComponentID, Quantity = 2, PricePerUnit = 1.60 },
                new PurchaseItem { PurchaseID = purchases[3].PurchaseID, ComponentID = components[81].ComponentID, Quantity = 2, PricePerUnit = 13.60 },
                new PurchaseItem { PurchaseID = purchases[3].PurchaseID, ComponentID = components[43].ComponentID, Quantity = 2, PricePerUnit = 2.60 },
                new PurchaseItem { PurchaseID = purchases[3].PurchaseID, ComponentID = components[40].ComponentID, Quantity = 2, PricePerUnit = 10.60 },
                new PurchaseItem { PurchaseID = purchases[3].PurchaseID, ComponentID = components[19].ComponentID, Quantity = 2, PricePerUnit = 9.60 },
                new PurchaseItem { PurchaseID = purchases[4].PurchaseID, ComponentID = components[83].ComponentID, Quantity = 2, PricePerUnit = 7.00 },
                new PurchaseItem { PurchaseID = purchases[4].PurchaseID, ComponentID = components[55].ComponentID, Quantity = 2, PricePerUnit = 16.00 },
                new PurchaseItem { PurchaseID = purchases[4].PurchaseID, ComponentID = components[56].ComponentID, Quantity = 1, PricePerUnit = 12.00 },
                new PurchaseItem { PurchaseID = purchases[5].PurchaseID, ComponentID = components[75].ComponentID, Quantity = 1, PricePerUnit = 9.00 },
                new PurchaseItem { PurchaseID = purchases[5].PurchaseID, ComponentID = components[23].ComponentID, Quantity = 1, PricePerUnit = 7.00 },
                new PurchaseItem { PurchaseID = purchases[5].PurchaseID, ComponentID = components[64].ComponentID, Quantity = 1, PricePerUnit = 2.00 },
                new PurchaseItem { PurchaseID = purchases[6].PurchaseID, ComponentID = components[70].ComponentID, Quantity = 1, PricePerUnit = 1.75 },
                new PurchaseItem { PurchaseID = purchases[6].PurchaseID, ComponentID = components[71].ComponentID, Quantity = 1, PricePerUnit = 3.75 },
                new PurchaseItem { PurchaseID = purchases[6].PurchaseID, ComponentID = components[80].ComponentID, Quantity = 1, PricePerUnit = 34.75 },
                new PurchaseItem { PurchaseID = purchases[7].PurchaseID, ComponentID = components[80].ComponentID, Quantity = 1, PricePerUnit = 7.20 },
                new PurchaseItem { PurchaseID = purchases[7].PurchaseID, ComponentID = components[56].ComponentID, Quantity = 1, PricePerUnit = 5.20 },
                new PurchaseItem { PurchaseID = purchases[7].PurchaseID, ComponentID = components[36].ComponentID, Quantity = 1, PricePerUnit = 9.20 },
                new PurchaseItem { PurchaseID = purchases[7].PurchaseID, ComponentID = components[67].ComponentID, Quantity = 1, PricePerUnit = 7.20 },
                new PurchaseItem { PurchaseID = purchases[8].PurchaseID, ComponentID = components[81].ComponentID, Quantity = 1, PricePerUnit = 14.80 },
                new PurchaseItem { PurchaseID = purchases[8].PurchaseID, ComponentID = components[47].ComponentID, Quantity = 1, PricePerUnit = 4.80 },
                new PurchaseItem { PurchaseID = purchases[9].PurchaseID, ComponentID = components[78].ComponentID, Quantity = 1, PricePerUnit = 7.50 },
                new PurchaseItem { PurchaseID = purchases[9].PurchaseID, ComponentID = components[45].ComponentID, Quantity = 1, PricePerUnit = 5.50 },
                new PurchaseItem { PurchaseID = purchases[9].PurchaseID, ComponentID = components[46].ComponentID, Quantity = 1, PricePerUnit = 14.50 }
            };
            context.PurchaseItems.AddRange(purchaseItems);
            await context.SaveChangesAsync();

            ImageStorageService ISS = new ImageStorageService(AppDomain.CurrentDomain.BaseDirectory);
            ISS.Initialize();

            byte[] image = File.ReadAllBytes("D:\\Repo\\ElectronDepot\\ElectroDepot\\ElectroDepotClassLibraryTests\\Assets\\image2.png");
            List<string> imageIDS = new List<string>();
            for(int i = 0; i < 14; i++)
            {
                imageIDS.Add(ISS.InsertProjectImage(image));
            }

            List<Project> projects = new List<Project>
            {
                new Project { UserID = users[0].UserID, ImageURI = imageIDS[0], Name = "Smart Home System", Description = "System automatyzacji domu", CreatedAt = DateTime.Now },
                new Project { UserID = users[1].UserID, ImageURI = imageIDS[1], Name = "Weather Station", Description = "Monitorowanie pogody", CreatedAt = DateTime.Now },
                new Project { UserID = users[2].UserID, ImageURI = imageIDS[2], Name = "Automated Irrigation System", Description = "System do automatycznego nawadniania ogrodu", CreatedAt = DateTime.Now },
                new Project { UserID = users[3].UserID, ImageURI = imageIDS[3], Name = "Home Security System", Description = "System monitorowania bezpieczeństwa domu", CreatedAt = DateTime.Now },
                new Project { UserID = users[0].UserID, ImageURI = imageIDS[4], Name = "Fitness Tracker", Description = "Aplikacja do monitorowania aktywności fizycznej", CreatedAt = DateTime.Now },
                new Project { UserID = users[1].UserID, ImageURI = imageIDS[5], Name = "Air Quality Monitor", Description = "System monitorowania jakości powietrza", CreatedAt = DateTime.Now },
                new Project { UserID = users[2].UserID, ImageURI = imageIDS[6], Name = "Industrial Automation", Description = "Automatyzacja procesów przemysłowych", CreatedAt = DateTime.Now },
                new Project { UserID = users[3].UserID, ImageURI = imageIDS[7], Name = "Plant Monitoring System", Description = "System monitorowania wilgotności gleby i zdrowia roślin", CreatedAt = DateTime.Now },
                new Project { UserID = users[4].UserID, ImageURI = imageIDS[8], Name = "Smart Lighting System", Description = "Automatyczne oświetlenie zależne od warunków otoczenia", CreatedAt = DateTime.Now },
                new Project { UserID = users[5].UserID, ImageURI = imageIDS[9], Name = "Remote Health Monitoring", Description = "Zdalny system monitorowania parametrów zdrowotnych", CreatedAt = DateTime.Now },
                new Project { UserID = users[6].UserID, ImageURI = imageIDS[10], Name = "Energy Management System", Description = "Monitorowanie zużycia energii elektrycznej w domu", CreatedAt = DateTime.Now },
                new Project { UserID = users[7].UserID, ImageURI = imageIDS[11], Name = "Robot Navigation", Description = "System nawigacji dla robota autonomicznego", CreatedAt = DateTime.Now },
                new Project { UserID = users[8].UserID, ImageURI = imageIDS[12], Name = "Voice Controlled Assistant", Description = "Asystent głosowy do sterowania urządzeniami w domu", CreatedAt = DateTime.Now },
                new Project { UserID = users[9].UserID, ImageURI = imageIDS[13], Name = "Smart Thermostat", Description = "Inteligentny system zarządzania temperaturą w domu", CreatedAt = DateTime.Now }
            };
            context.Projects.AddRange(projects);
            await context.SaveChangesAsync();

            List<ProjectComponent> projectComponents = new List<ProjectComponent>
            {
                new ProjectComponent { ProjectID = projects[0].ProjectID, ComponentID = components[77].ComponentID, Quantity = 1 },
                new ProjectComponent { ProjectID = projects[0].ProjectID, ComponentID = components[2].ComponentID, Quantity = 1 },
                new ProjectComponent { ProjectID = projects[0].ProjectID, ComponentID = components[5].ComponentID, Quantity = 2 },
                new ProjectComponent { ProjectID = projects[0].ProjectID, ComponentID = components[38].ComponentID, Quantity = 1 },
                new ProjectComponent { ProjectID = projects[1].ProjectID, ComponentID = components[80].ComponentID, Quantity = 1 },
                new ProjectComponent { ProjectID = projects[1].ProjectID, ComponentID = components[50].ComponentID, Quantity = 1 },
                new ProjectComponent { ProjectID = projects[1].ProjectID, ComponentID = components[41].ComponentID, Quantity = 1 },
                new ProjectComponent { ProjectID = projects[1].ProjectID, ComponentID = components[21].ComponentID, Quantity = 1 },
                new ProjectComponent { ProjectID = projects[1].ProjectID, ComponentID = components[7].ComponentID, Quantity = 1 },
                new ProjectComponent { ProjectID = projects[1].ProjectID, ComponentID = components[1].ComponentID, Quantity = 2 },
                new ProjectComponent { ProjectID = projects[2].ProjectID, ComponentID = components[75].ComponentID, Quantity = 2 },
                new ProjectComponent { ProjectID = projects[2].ProjectID, ComponentID = components[47].ComponentID, Quantity = 2 },
                new ProjectComponent { ProjectID = projects[2].ProjectID, ComponentID = components[13].ComponentID, Quantity = 2 },
                new ProjectComponent { ProjectID = projects[2].ProjectID, ComponentID = components[0].ComponentID, Quantity = 2 },
                new ProjectComponent { ProjectID = projects[2].ProjectID, ComponentID = components[22].ComponentID, Quantity = 2 },
                new ProjectComponent { ProjectID = projects[3].ProjectID, ComponentID = components[83].ComponentID, Quantity = 2 },
                new ProjectComponent { ProjectID = projects[3].ProjectID, ComponentID = components[59].ComponentID, Quantity = 2 },
                new ProjectComponent { ProjectID = projects[3].ProjectID, ComponentID = components[46].ComponentID, Quantity = 2 },
                new ProjectComponent { ProjectID = projects[3].ProjectID, ComponentID = components[24].ComponentID, Quantity = 2 },
                new ProjectComponent { ProjectID = projects[4].ProjectID, ComponentID = components[82].ComponentID, Quantity = 2 },
                new ProjectComponent { ProjectID = projects[4].ProjectID, ComponentID = components[67].ComponentID, Quantity = 2 },
                new ProjectComponent { ProjectID = projects[4].ProjectID, ComponentID = components[9].ComponentID, Quantity = 2 },
                new ProjectComponent { ProjectID = projects[5].ProjectID, ComponentID = components[74].ComponentID, Quantity = 2 },
                new ProjectComponent { ProjectID = projects[5].ProjectID, ComponentID = components[21].ComponentID, Quantity = 2 },
                new ProjectComponent { ProjectID = projects[5].ProjectID, ComponentID = components[4].ComponentID, Quantity = 2 },
                new ProjectComponent { ProjectID = projects[5].ProjectID, ComponentID = components[3].ComponentID, Quantity = 2 },
                new ProjectComponent { ProjectID = projects[6].ProjectID, ComponentID = components[79].ComponentID, Quantity = 2 },
                new ProjectComponent { ProjectID = projects[6].ProjectID, ComponentID = components[69].ComponentID, Quantity = 2 },
                new ProjectComponent { ProjectID = projects[6].ProjectID, ComponentID = components[62].ComponentID, Quantity = 2 },
                new ProjectComponent { ProjectID = projects[6].ProjectID, ComponentID = components[60].ComponentID, Quantity = 2 },
                new ProjectComponent { ProjectID = projects[6].ProjectID, ComponentID = components[3].ComponentID, Quantity = 2 },
                new ProjectComponent { ProjectID = projects[7].ProjectID, ComponentID = components[81].ComponentID, Quantity = 2 },
                new ProjectComponent { ProjectID = projects[7].ProjectID, ComponentID = components[43].ComponentID, Quantity = 2 },
                new ProjectComponent { ProjectID = projects[7].ProjectID, ComponentID = components[40].ComponentID, Quantity = 2 },
                new ProjectComponent { ProjectID = projects[7].ProjectID, ComponentID = components[19].ComponentID, Quantity = 2 },
                new ProjectComponent { ProjectID = projects[8].ProjectID, ComponentID = components[83].ComponentID, Quantity = 2 },
                new ProjectComponent { ProjectID = projects[8].ProjectID, ComponentID = components[55].ComponentID, Quantity = 2 },
                new ProjectComponent { ProjectID = projects[8].ProjectID, ComponentID = components[56].ComponentID, Quantity = 1 },
                new ProjectComponent { ProjectID = projects[9].ProjectID, ComponentID = components[75].ComponentID, Quantity = 1 },
                new ProjectComponent { ProjectID = projects[9].ProjectID, ComponentID = components[23].ComponentID, Quantity = 1 },
                new ProjectComponent { ProjectID = projects[9].ProjectID, ComponentID = components[64].ComponentID, Quantity = 1 },
                new ProjectComponent { ProjectID = projects[10].ProjectID, ComponentID = components[70].ComponentID, Quantity = 1 },
                new ProjectComponent { ProjectID = projects[10].ProjectID, ComponentID = components[71].ComponentID, Quantity = 1 },
                new ProjectComponent { ProjectID = projects[10].ProjectID, ComponentID = components[80].ComponentID, Quantity = 1 },
                new ProjectComponent { ProjectID = projects[11].ProjectID, ComponentID = components[80].ComponentID, Quantity = 1 },
                new ProjectComponent { ProjectID = projects[11].ProjectID, ComponentID = components[56].ComponentID, Quantity = 1 },
                new ProjectComponent { ProjectID = projects[11].ProjectID, ComponentID = components[36].ComponentID, Quantity = 1 },
                new ProjectComponent { ProjectID = projects[11].ProjectID, ComponentID = components[67].ComponentID, Quantity = 1 },
                new ProjectComponent { ProjectID = projects[12].ProjectID, ComponentID = components[81].ComponentID, Quantity = 1 },
                new ProjectComponent { ProjectID = projects[12].ProjectID, ComponentID = components[47].ComponentID, Quantity = 1 },
                new ProjectComponent { ProjectID = projects[13].ProjectID, ComponentID = components[78].ComponentID, Quantity = 1 },
                new ProjectComponent { ProjectID = projects[13].ProjectID, ComponentID = components[45].ComponentID, Quantity = 1 },
                new ProjectComponent { ProjectID = projects[13].ProjectID, ComponentID = components[46].ComponentID, Quantity = 1 }
            };
            context.ProjectComponents.AddRange(projectComponents);
            await context.SaveChangesAsync();

            List<OwnsComponent> ownsComponents = new List<OwnsComponent>
            {
                new OwnsComponent { UserID = users[0].UserID, ComponentID = components[77].ComponentID, Quantity = 1 }, 
                new OwnsComponent { UserID = users[0].UserID, ComponentID = components[2].ComponentID, Quantity = 1 },   
                new OwnsComponent { UserID = users[0].UserID, ComponentID = components[5].ComponentID, Quantity = 2 },   
                new OwnsComponent { UserID = users[0].UserID, ComponentID = components[38].ComponentID, Quantity = 1 },  
                new OwnsComponent { UserID = users[0].UserID, ComponentID = components[82].ComponentID, Quantity = 2 },
                new OwnsComponent { UserID = users[0].UserID, ComponentID = components[67].ComponentID, Quantity = 2 },   
                new OwnsComponent { UserID = users[0].UserID, ComponentID = components[9].ComponentID, Quantity = 2 },    
                new OwnsComponent { UserID = users[1].UserID, ComponentID = components[80].ComponentID, Quantity = 1 },   
                new OwnsComponent { UserID = users[1].UserID, ComponentID = components[50].ComponentID, Quantity = 1 },   
                new OwnsComponent { UserID = users[1].UserID, ComponentID = components[41].ComponentID, Quantity = 1 },   
                new OwnsComponent { UserID = users[1].UserID, ComponentID = components[21].ComponentID, Quantity = 1 },   
                new OwnsComponent { UserID = users[1].UserID, ComponentID = components[7].ComponentID, Quantity = 2 },    
                new OwnsComponent { UserID = users[1].UserID, ComponentID = components[1].ComponentID, Quantity = 2 },    
                new OwnsComponent { UserID = users[1].UserID, ComponentID = components[74].ComponentID, Quantity = 2 },   
                new OwnsComponent { UserID = users[1].UserID, ComponentID = components[21].ComponentID, Quantity = 2 },    
                new OwnsComponent { UserID = users[1].UserID, ComponentID = components[4].ComponentID, Quantity = 2 },     
                new OwnsComponent { UserID = users[1].UserID, ComponentID = components[3].ComponentID, Quantity = 2 },    
                new OwnsComponent { UserID = users[2].UserID, ComponentID = components[75].ComponentID, Quantity = 2 },   
                new OwnsComponent { UserID = users[2].UserID, ComponentID = components[47].ComponentID, Quantity = 2 },   
                new OwnsComponent { UserID = users[2].UserID, ComponentID = components[13].ComponentID, Quantity = 2 },   
                new OwnsComponent { UserID = users[2].UserID, ComponentID = components[0].ComponentID, Quantity = 2 },    
                new OwnsComponent { UserID = users[2].UserID, ComponentID = components[22].ComponentID, Quantity = 2 },  
                new OwnsComponent { UserID = users[2].UserID, ComponentID = components[79].ComponentID, Quantity = 2 },  
                new OwnsComponent { UserID = users[2].UserID, ComponentID = components[69].ComponentID, Quantity = 2 },     
                new OwnsComponent { UserID = users[2].UserID, ComponentID = components[62].ComponentID, Quantity = 2 },     
                new OwnsComponent { UserID = users[2].UserID, ComponentID = components[60].ComponentID, Quantity = 2 },    
                new OwnsComponent { UserID = users[2].UserID, ComponentID = components[3].ComponentID, Quantity = 2 },      
                new OwnsComponent { UserID = users[3].UserID, ComponentID = components[83].ComponentID, Quantity = 2 },     
                new OwnsComponent { UserID = users[3].UserID, ComponentID = components[59].ComponentID, Quantity = 2 },     
                new OwnsComponent { UserID = users[3].UserID, ComponentID = components[46].ComponentID, Quantity = 2 },    
                new OwnsComponent { UserID = users[3].UserID, ComponentID = components[24].ComponentID, Quantity = 2 },     
                new OwnsComponent { UserID = users[3].UserID, ComponentID = components[81].ComponentID, Quantity = 2 },     
                new OwnsComponent { UserID = users[3].UserID, ComponentID = components[43].ComponentID, Quantity = 2 },    
                new OwnsComponent { UserID = users[3].UserID, ComponentID = components[40].ComponentID, Quantity = 2 },     
                new OwnsComponent { UserID = users[3].UserID, ComponentID = components[19].ComponentID, Quantity = 2 },   
                new OwnsComponent { UserID = users[4].UserID, ComponentID = components[83].ComponentID, Quantity = 2 },     
                new OwnsComponent { UserID = users[4].UserID, ComponentID = components[55].ComponentID, Quantity = 2 },     
                new OwnsComponent { UserID = users[4].UserID, ComponentID = components[56].ComponentID, Quantity = 1 },   
                new OwnsComponent { UserID = users[5].UserID, ComponentID = components[75].ComponentID, Quantity = 1 },     
                new OwnsComponent { UserID = users[5].UserID, ComponentID = components[23].ComponentID, Quantity = 1 },   
                new OwnsComponent { UserID = users[5].UserID, ComponentID = components[64].ComponentID, Quantity = 1 },    
                new OwnsComponent { UserID = users[6].UserID, ComponentID = components[70].ComponentID, Quantity = 1 },     
                new OwnsComponent { UserID = users[6].UserID, ComponentID = components[71].ComponentID, Quantity = 1 },    
                new OwnsComponent { UserID = users[6].UserID, ComponentID = components[80].ComponentID, Quantity = 1 },   
                new OwnsComponent { UserID = users[7].UserID, ComponentID = components[80].ComponentID, Quantity = 1 },    
                new OwnsComponent { UserID = users[7].UserID, ComponentID = components[56].ComponentID, Quantity = 1 },    
                new OwnsComponent { UserID = users[7].UserID, ComponentID = components[36].ComponentID, Quantity = 1 },    
                new OwnsComponent { UserID = users[7].UserID, ComponentID = components[67].ComponentID, Quantity = 1 },    
                new OwnsComponent { UserID = users[8].UserID, ComponentID = components[81].ComponentID, Quantity = 1 },     
                new OwnsComponent { UserID = users[8].UserID, ComponentID = components[47].ComponentID, Quantity = 1 },    
                new OwnsComponent { UserID = users[9].UserID, ComponentID = components[78].ComponentID, Quantity = 1 },    
                new OwnsComponent { UserID = users[9].UserID, ComponentID = components[45].ComponentID, Quantity = 1 },    
                new OwnsComponent { UserID = users[9].UserID, ComponentID = components[46].ComponentID, Quantity = 1 }
            };
            context.OwnsComponent.AddRange(ownsComponents);
            await context.SaveChangesAsync();
        }
    }
}
