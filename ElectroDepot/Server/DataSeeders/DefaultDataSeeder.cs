using Server.Models;
using Server.Context;
using Microsoft.EntityFrameworkCore;
using ElectroDepotClassLibrary.Services;
using Server.Services;

public static class DefaultDataSeeder
{
    public static async Task SeedDataAsync(DatabaseContext context, ServerConfigService serverConfigService)
    {
        ImageStorageService ISS = ImageStorageService.CreateService();
        ISS.Initialize(serverConfigService.StoragePath);

        await context.Database.MigrateAsync();
        await context.ProjectComponents.ExecuteDeleteAsync();
        await context.PurchaseItems.ExecuteDeleteAsync();
        await context.OwnsComponent.ExecuteDeleteAsync();
        await context.Projects.ExecuteDeleteAsync();
        await context.Purchases.ExecuteDeleteAsync();
        await context.Components.ExecuteDeleteAsync();
        await context.Users.ExecuteDeleteAsync();
        await context.Suppliers.ExecuteDeleteAsync();
        await context.Categories.ExecuteDeleteAsync();
        await context.PredefinedImage.ExecuteDeleteAsync();
        await context.SaveChangesAsync();

        List<User> users = new List<User>()
        {
            // Password 'admin123'
            new User { Username = "Administrator", Password = "AQAAAAIAAYagAAAAELHVeAroaaRtW3+U5ucNcws+bWr6NFwk+Of3LeBU5tG3HSzctxU8wWRcyPNnfMbY0g==", Email = "admin@gmail.com", Name = "Administrator" },
        };

        context.Users.AddRange(users);
        await context.SaveChangesAsync();

        string assetsPath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Assests");

        //string assetsPath = "D:\\Repo\\ElectronDepot\\ElectroDepot\\Server\\Assests\\";

        string categoriesImagesPath = Path.Combine(assetsPath, "Categories");
        //string categoriesImagesPath = assetsPath + "Categories\\";
        string predefinedImagesPath = Path.Combine(assetsPath, "PredefinedImages");
        //string predefinedImagesPath = assetsPath + "PredefinedImages\\";
        string defaultComponentImagePath = Path.Combine(predefinedImagesPath, "1.png");
        //string defaultComponentImagePath = predefinedImagesPath + "1.png";

        List<Category> categories = new List<Category>()
            {
                new Category { Name = "Czujnik ciśnienia", Description = "Mierzy ciśnienie", Image = File.ReadAllBytes(Path.Combine(categoriesImagesPath, "Pressure.png"))},
                new Category { Name = "Czujnik czystości powietrza", Description = "Mierzy poziom czystości powietrza", Image = File.ReadAllBytes(Path.Combine(categoriesImagesPath, "AirQuality.png")) },
                new Category { Name = "Czujnik gestów", Description = "Wykrywa gesty", Image = File.ReadAllBytes(Path.Combine(categoriesImagesPath, "Gesture.png")) },
                new Category { Name = "Czujnik krańcowe", Description = "Wykrywa osiągnięcie krańca ruchu", Image = File.ReadAllBytes(Path.Combine(categoriesImagesPath, "LimitSensor.png")) },
                new Category { Name = "Czujnik gazów", Description = "Mierzy stężenie gazów", Image = File.ReadAllBytes(Path.Combine(categoriesImagesPath,"GasSensor.png")) },
                new Category { Name = "Czujnik magnetyczne", Description = "Wykrywa pole magnetyczne", Image = File.ReadAllBytes(Path.Combine(categoriesImagesPath, "MagneticSensor.png")) },
                new Category { Name = "Czujnik odbiciowe", Description = "Wykrywa obiekty przez odbicie światła", Image = File.ReadAllBytes(Path.Combine(categoriesImagesPath, "ReflectiveSensor.png")) },
                new Category { Name = "Czujnik odległości", Description = "Mierzy odległość", Image = File.ReadAllBytes(Path.Combine(categoriesImagesPath, "Distance.png")) },
                new Category { Name = "Czujnik temperatury", Description = "Mierzy temperaturę", Image = File.ReadAllBytes(Path.Combine(categoriesImagesPath, "Temperature.png")) },
                new Category { Name = "Czujnik wilgotności", Description = "Mierzy wilgotność", Image = File.ReadAllBytes(Path.Combine(categoriesImagesPath, "Humidity.png")) },
                new Category { Name = "Enkodery", Description = "Mierzą pozycję kątową lub liniową", Image = File.ReadAllBytes(Path.Combine(categoriesImagesPath, "Encoder.png")) },
                new Category { Name = "Fotorezystory", Description = "Zmieniają opór w zależności od światła", Image = File.ReadAllBytes(Path.Combine(categoriesImagesPath, "Photoresistor.png")) },
                new Category { Name = "Fototranzystory", Description = "Wykrywają światło za pomocą tranzystora", Image = File.ReadAllBytes(Path.Combine(categoriesImagesPath, "Phototransistor.png")) },
                new Category { Name = "Odbiornik podczerwieni", Description = "Odbiera sygnały podczerwieni", Image = File.ReadAllBytes(Path.Combine(categoriesImagesPath, "Infrared.png")) },
                new Category { Name = "Akcelerometry", Description = "Mierzy przyspieszenie", Image = File.ReadAllBytes(Path.Combine(categoriesImagesPath, "Accelerometer.png")) },
                new Category { Name = "Czujnik hallotronowe", Description = "Wykrywa pole magnetyczne za pomocą efektu Halla", Image = File.ReadAllBytes(Path.Combine(categoriesImagesPath, "Hall.png")) },
                new Category { Name = "Mikrokontroler", Description = "Programowo sterowany układ elektroniczny", Image = File.ReadAllBytes(Path.Combine(categoriesImagesPath, "Microcontroller.png")) }
            };

        context.Categories.AddRange(categories);
        await context.SaveChangesAsync();

        List<Supplier> suppliers = new List<Supplier>()
        {
            new Supplier { Name = "Other", Website = "https://www.google.com/", Image = File.ReadAllBytes(Path.Combine(assetsPath, "OtherIcon.png")) },
            new Supplier { Name = "DigiKey", Website = "https://www.digikey.pl/", Image = File.ReadAllBytes(Path.Combine(assetsPath, "DigiKeyIcon.png")) },
            new Supplier { Name = "Botland", Website = "https://botland.com.pl/", Image = File.ReadAllBytes(Path.Combine(assetsPath, "BotlandIcon.png")) },
            new Supplier { Name = "Mouser", Website = "https://www.mouser.com/", Image = File.ReadAllBytes(Path.Combine(assetsPath, "MouserIcon.png")) },
            new Supplier { Name = "Kamami", Website = "https://kamami.pl/", Image = File.ReadAllBytes(Path.Combine(assetsPath, "KamamiIcon.png")) },
            new Supplier { Name = "M-Salamon", Website = "https://msalamon.pl/", Image = File.ReadAllBytes(Path.Combine(assetsPath, "MsalamonIcon.png")) },
            new Supplier { Name = "Allegro", Website = "https://allegro.pl/", Image = File.ReadAllBytes(Path.Combine(assetsPath, "AllegroIcon.png")) },
            new Supplier { Name = "AliExpress", Website = "https://pl.aliexpress.com/", Image = File.ReadAllBytes(Path.Combine(assetsPath, "AliexpressIcon.png")) },
        };
        context.Suppliers.AddRange(suppliers);
        await context.SaveChangesAsync();

        List<Component> components = new List<Component>
            {
                // Czujniki ciśnienia
                new Component
                {
                    Name = "BMP180",
                    Manufacturer = "Bosch",
                    ShortDescription = "Barometric pressure sensor",
                    LongDescription = "High-precision barometric pressure sensor suitable for weather and altimeter applications.",
                    ImageURI = ISS.InsertComponentImage(File.ReadAllBytes(defaultComponentImagePath)),
                    DatasheetLink = "",
                    CategoryID = categories[0].CategoryID
                },
                new Component
                {
                    Name = "MPX5700AP",
                    Manufacturer = "NXP",
                    ShortDescription = "Absolute pressure sensor",
                    LongDescription = "Accurate absolute pressure sensor ideal for automotive and industrial applications.",
                    ImageURI = ISS.InsertComponentImage(File.ReadAllBytes(defaultComponentImagePath)),
                    DatasheetLink = "",
                    CategoryID = categories[0].CategoryID
                },
                new Component
                {
                    Name = "MPX5010",
                    Manufacturer = "NXP",
                    ShortDescription = "Differential pressure sensor with a range of 0-10 kPa",
                    LongDescription = "Reliable differential pressure sensor for low-pressure measurements in HVAC and medical devices.",
                    ImageURI = ISS.InsertComponentImage(File.ReadAllBytes(defaultComponentImagePath)),
                    DatasheetLink = "",
                    CategoryID = categories[0].CategoryID
                },
                new Component
                {
                    Name = "BMP280",
                    Manufacturer = "Bosch",
                    ShortDescription = "Atmospheric pressure sensor and altimeter",
                    LongDescription = "Versatile atmospheric pressure sensor with built-in altimeter functionality, perfect for wearable and IoT devices.",
                    ImageURI = ISS.InsertComponentImage(File.ReadAllBytes(defaultComponentImagePath)),
                    DatasheetLink = "",
                    CategoryID = categories[0].CategoryID
                },
                new Component
                {
                    Name = "APC301",
                    Manufacturer = "Amphenol",
                    ShortDescription = "Analog industrial pressure sensor",
                    LongDescription = "Durable analog industrial pressure sensor designed for heavy-duty and high-accuracy applications.",
                    ImageURI = ISS.InsertComponentImage(File.ReadAllBytes(defaultComponentImagePath)),
                    DatasheetLink = "",
                    CategoryID = categories[0].CategoryID
                },

                // Air Quality Sensors
                new Component
                {
                    Name = "MQ-135",
                    Manufacturer = "Winsen",
                    ShortDescription = "Air quality sensor",
                    LongDescription = "Sensitive air quality sensor ideal for detecting various gases including NH3, NOx, alcohol, and benzene.",
                    ImageURI = ISS.InsertComponentImage(File.ReadAllBytes(defaultComponentImagePath)),
                    DatasheetLink = "",
                    CategoryID = categories[1].CategoryID
                },
                new Component
                {
                    Name = "CCS811",
                    Manufacturer = "AMS",
                    ShortDescription = "Organic gas and CO2 sensor",
                    LongDescription = "Low-power digital gas sensor for indoor air quality monitoring, capable of detecting VOCs and CO2.",
                    ImageURI = ISS.InsertComponentImage(File.ReadAllBytes(defaultComponentImagePath)),
                    DatasheetLink = "",
                    CategoryID = categories[1].CategoryID
                },
                new Component
                {
                    Name = "MQ-7",
                    Manufacturer = "Winsen",
                    ShortDescription = "Gas sensor for CO detection",
                    LongDescription = "Carbon monoxide sensor designed for safety and industrial applications.",
                    ImageURI = ISS.InsertComponentImage(File.ReadAllBytes(defaultComponentImagePath)),
                    DatasheetLink = "",
                    CategoryID = categories[1].CategoryID
                },
                new Component
                {
                    Name = "AQS-1",
                    Manufacturer = "Air Quality Sensor",
                    ShortDescription = "Air quality sensor for pollutants and CO2",
                    LongDescription = "Comprehensive air quality sensor capable of monitoring particulate matter and CO2 levels.",
                    ImageURI = ISS.InsertComponentImage(File.ReadAllBytes(defaultComponentImagePath)),
                    DatasheetLink = "",
                    CategoryID = categories[1].CategoryID
                },

                // Gesture Sensors
                new Component
                {
                    Name = "APDS-9960",
                    Manufacturer = "Broadcom",
                    ShortDescription = "Gesture, light, and color sensor",
                    LongDescription = "Integrated sensor for detecting gestures, ambient light, proximity, and RGB color.",
                    ImageURI = ISS.InsertComponentImage(File.ReadAllBytes(defaultComponentImagePath)),
                    DatasheetLink = "",
                    CategoryID = categories[2].CategoryID
                },
                new Component
                {
                    Name = "AS3935",
                    Manufacturer = "ams AG",
                    ShortDescription = "Gesture and storm detection sensor",
                    LongDescription = "Innovative sensor capable of gesture recognition and lightning detection.",
                    ImageURI = ISS.InsertComponentImage(File.ReadAllBytes(defaultComponentImagePath)),
                    DatasheetLink = "",
                    CategoryID = categories[2].CategoryID
                },
                new Component
                {
                    Name = "MTGesture",
                    Manufacturer = "Microchip",
                    ShortDescription = "3D gesture sensor with remote control functionality",
                    LongDescription = "Advanced sensor for 3D gesture recognition and remote control interaction.",
                    ImageURI = ISS.InsertComponentImage(File.ReadAllBytes(defaultComponentImagePath)),
                    DatasheetLink = "",
                    CategoryID = categories[2].CategoryID
                },
                new Component
                {
                    Name = "ADPD188GG",
                    Manufacturer = "Analog Devices",
                    ShortDescription = "Optical sensor for gesture detection and health monitoring",
                    LongDescription = "High-performance optical sensor designed for gesture recognition and biometric health tracking.",
                    ImageURI = ISS.InsertComponentImage(File.ReadAllBytes(defaultComponentImagePath)),
                    DatasheetLink = "",
                    CategoryID = categories[2].CategoryID
                },

                // Limit Sensors
                new Component
                {
                    Name = "Limit Switch KW12-3",
                    Manufacturer = "Omron",
                    ShortDescription = "Mechanical limit sensor",
                    LongDescription = "Compact and durable mechanical limit switch for industrial applications.",
                    ImageURI = ISS.InsertComponentImage(File.ReadAllBytes(defaultComponentImagePath)),
                    DatasheetLink = "",
                    CategoryID = categories[3].CategoryID
                },
                new Component
                {
                    Name = "ME-8108",
                    Manufacturer = "Heschen",
                    ShortDescription = "Industrial limit switch with roller lever",
                    LongDescription = "Industrial-grade limit switch with a durable roller lever mechanism.",
                    ImageURI = ISS.InsertComponentImage(File.ReadAllBytes(defaultComponentImagePath)),
                    DatasheetLink = "",
                    CategoryID = categories[3].CategoryID
                },
                new Component
                {
                    Name = "Omron D4MC",
                    Manufacturer = "Omron",
                    ShortDescription = "High-durability micro limit switch",
                    LongDescription = "Reliable and long-lasting micro limit switch for various applications.",
                    ImageURI = ISS.InsertComponentImage(File.ReadAllBytes(defaultComponentImagePath)),
                    DatasheetLink = "",
                    CategoryID = categories[3].CategoryID
                },
                new Component
                {
                    Name = "V-156-1C25",
                    Manufacturer = "Honeywell",
                    ShortDescription = "Limit switch with roller lever",
                    LongDescription = "Versatile limit switch featuring a sturdy roller lever for industrial settings.",
                    ImageURI = ISS.InsertComponentImage(File.ReadAllBytes(defaultComponentImagePath)),
                    DatasheetLink = "",
                    CategoryID = categories[3].CategoryID
                },
                new Component
                {
                    Name = "MLC-S-130L",
                    Manufacturer = "Honeywell",
                    ShortDescription = "Compact limit sensor with short arm",
                    LongDescription = "Compact and efficient limit sensor with a short actuation arm for space-constrained applications.",
                    ImageURI = ISS.InsertComponentImage(File.ReadAllBytes(defaultComponentImagePath)),
                    DatasheetLink = "",
                    CategoryID = categories[3].CategoryID
                },

                // Gas Sensors
                new Component
                {
                    Name = "MQ-2",
                    Manufacturer = "Hanwei",
                    ShortDescription = "Combustible gas and smoke sensor",
                    LongDescription = "Versatile sensor designed for detecting combustible gases and smoke, suitable for safety applications.",
                    ImageURI = ISS.InsertComponentImage(File.ReadAllBytes(defaultComponentImagePath)),
                    DatasheetLink = "",
                    CategoryID = categories[4].CategoryID
                },
                new Component
                {
                    Name = "MH-Z19B",
                    Manufacturer = "Winsen",
                    ShortDescription = "Carbon dioxide (CO2) sensor",
                    LongDescription = "High-precision CO2 sensor ideal for indoor air quality monitoring.",
                    ImageURI = ISS.InsertComponentImage(File.ReadAllBytes(defaultComponentImagePath)),
                    DatasheetLink = "",
                    CategoryID = categories[4].CategoryID
                },
                new Component
                {
                    Name = "MQ-3",
                    Manufacturer = "Hanwei Electronics",
                    ShortDescription = "Alcohol sensor for detecting alcohol vapors",
                    LongDescription = "Reliable alcohol sensor used in breathalyzers and safety devices.",
                    ImageURI = ISS.InsertComponentImage(File.ReadAllBytes(defaultComponentImagePath)),
                    DatasheetLink = "",
                    CategoryID = categories[4].CategoryID
                },
                new Component
                {
                    Name = "ZP07-O3",
                    Manufacturer = "Winsen",
                    ShortDescription = "Ozone (O3) sensor for air quality measurement",
                    LongDescription = "Precise ozone sensor designed to monitor air quality and detect O3 levels.",
                    ImageURI = ISS.InsertComponentImage(File.ReadAllBytes(defaultComponentImagePath)),
                    DatasheetLink = "",
                    CategoryID = categories[4].CategoryID
                },

                // Magnetic Sensors
                new Component
                {
                    Name = "A3144",
                    Manufacturer = "Allegro",
                    ShortDescription = "Hall sensor for magnetic field detection",
                    LongDescription = "Robust Hall effect sensor for detecting magnetic fields in various applications.",
                    ImageURI = ISS.InsertComponentImage(File.ReadAllBytes(defaultComponentImagePath)),
                    DatasheetLink = "",
                    CategoryID = categories[5].CategoryID
                },
                new Component
                {
                    Name = "TLE493D",
                    Manufacturer = "Infineon",
                    ShortDescription = "Three-axis Hall sensor for magnetic field measurement",
                    LongDescription = "High-precision 3-axis Hall effect sensor suitable for advanced magnetic sensing applications.",
                    ImageURI = ISS.InsertComponentImage(File.ReadAllBytes(defaultComponentImagePath)),
                    DatasheetLink = "",
                    CategoryID = categories[5].CategoryID
                },
                new Component
                {
                    Name = "LIS3MDL",
                    Manufacturer = "STMicroelectronics",
                    ShortDescription = "3-axis magnetic sensor for measuring magnetic fields",
                    LongDescription = "Advanced 3-axis magnetic sensor for compasses, navigation, and industrial applications.",
                    ImageURI = ISS.InsertComponentImage(File.ReadAllBytes(defaultComponentImagePath)),
                    DatasheetLink = "",
                    CategoryID = categories[5].CategoryID
                },
                new Component
                {
                    Name = "HMC5883L",
                    Manufacturer = "Honeywell",
                    ShortDescription = "3-axis compass sensor using magnetic technology",
                    LongDescription = "Reliable compass sensor leveraging magnetic sensing technology for precise navigation.",
                    ImageURI = ISS.InsertComponentImage(File.ReadAllBytes(defaultComponentImagePath)),
                    DatasheetLink = "",
                    CategoryID = categories[5].CategoryID
                },
                new Component
                {
                    Name = "MMT-3D",
                    Manufacturer = "Microchip",
                    ShortDescription = "Hall sensor for measuring magnetic fields in three dimensions",
                    LongDescription = "High-performance Hall sensor for 3D magnetic field measurement, perfect for industrial use.",
                    ImageURI = ISS.InsertComponentImage(File.ReadAllBytes(defaultComponentImagePath)),
                    DatasheetLink = "",
                    CategoryID = categories[5].CategoryID
                },

                // Reflective Sensors
                new Component
                {
                    Name = "TCRT5000",
                    Manufacturer = "Vishay",
                    ShortDescription = "Reflective optical sensor",
                    LongDescription = "Compact reflective sensor ideal for object detection and line-following robots.",
                    ImageURI = ISS.InsertComponentImage(File.ReadAllBytes(defaultComponentImagePath)),
                    DatasheetLink = "",
                    CategoryID = categories[6].CategoryID
                },
                new Component
                {
                    Name = "QRD1114",
                    Manufacturer = "Fairchild",
                    ShortDescription = "Reflective optical sensor for object detection",
                    LongDescription = "Reliable reflective sensor designed for detecting objects at close range.",
                    ImageURI = ISS.InsertComponentImage(File.ReadAllBytes(defaultComponentImagePath)),
                    DatasheetLink = "",
                    CategoryID = categories[6].CategoryID
                },
                new Component
                {
                    Name = "TSOP4838",
                    Manufacturer = "Vishay",
                    ShortDescription = "Reflective optical sensor, IR receiver",
                    LongDescription = "Infrared reflective sensor designed for remote control and object detection applications.",
                    ImageURI = ISS.InsertComponentImage(File.ReadAllBytes(defaultComponentImagePath)),
                    DatasheetLink = "",
                    CategoryID = categories[6].CategoryID
                },
                new Component
                {
                    Name = "GP2Y0A21YK0F",
                    Manufacturer = "Sharp",
                    ShortDescription = "Reflective distance sensor with analog output",
                    LongDescription = "Analog distance sensor ideal for obstacle avoidance and proximity detection.",
                    ImageURI = ISS.InsertComponentImage(File.ReadAllBytes(defaultComponentImagePath)),
                    DatasheetLink = "",
                    CategoryID = categories[6].CategoryID
                },
                new Component
                {
                    Name = "TCS3200",
                    Manufacturer = "Texas Instruments",
                    ShortDescription = "Reflective color sensor with light detection",
                    LongDescription = "Color sensor capable of detecting RGB color and light intensity, ideal for sorting and robotics.",
                    ImageURI = ISS.InsertComponentImage(File.ReadAllBytes(defaultComponentImagePath)),
                    DatasheetLink = "",
                    CategoryID = categories[6].CategoryID
                },

                // Distance Sensors
                new Component
                {
                    Name = "HC-SR04",
                    Manufacturer = "Elecfreaks",
                    ShortDescription = "Ultrasonic distance sensor",
                    LongDescription = "Reliable ultrasonic sensor for distance measurement in robotics and automation applications.",
                    ImageURI = ISS.InsertComponentImage(File.ReadAllBytes(defaultComponentImagePath)),
                    DatasheetLink = "",
                    CategoryID = categories[7].CategoryID
                },
                new Component
                {
                    Name = "VL53L0X",
                    Manufacturer = "STMicroelectronics",
                    ShortDescription = "Time-of-flight distance sensor",
                    LongDescription = "Compact sensor using time-of-flight technology for precise distance measurement.",
                    ImageURI = ISS.InsertComponentImage(File.ReadAllBytes(defaultComponentImagePath)),
                    DatasheetLink = "",
                    CategoryID = categories[7].CategoryID
                },
                new Component
                {
                    Name = "SRF05",
                    Manufacturer = "SRF",
                    ShortDescription = "Ultrasonic distance sensor with up to 4 meters range",
                    LongDescription = "Versatile ultrasonic sensor for detecting objects and measuring distances up to 4 meters.",
                    ImageURI = ISS.InsertComponentImage(File.ReadAllBytes(defaultComponentImagePath)),
                    DatasheetLink = "",
                    CategoryID = categories[7].CategoryID
                },
                new Component
                {
                    Name = "GP2Y0A02YK0F",
                    Manufacturer = "Sharp",
                    ShortDescription = "Analog output distance sensor up to 2 meters",
                    LongDescription = "Sharp sensor with analog output, suitable for distance measurement up to 2 meters.",
                    ImageURI = ISS.InsertComponentImage(File.ReadAllBytes(defaultComponentImagePath)),
                    DatasheetLink = "",
                    CategoryID = categories[7].CategoryID
                },
                new Component
                {
                    Name = "LIDAR-Lite v3",
                    Manufacturer = "Garmin",
                    ShortDescription = "LIDAR sensor for accurate distance measurement",
                    LongDescription = "High-precision LIDAR sensor designed for mapping, navigation, and rangefinding applications.",
                    ImageURI = ISS.InsertComponentImage(File.ReadAllBytes(defaultComponentImagePath)),
                    DatasheetLink = "",
                    CategoryID = categories[7].CategoryID
                },
                new Component
                {
                    Name = "ToF Rangefinder",
                    Manufacturer = "Benewake",
                    ShortDescription = "Time-of-Flight distance sensor, high accuracy and fast measurement",
                    LongDescription = "Advanced ToF sensor with high accuracy and speed, ideal for robotics and automation.",
                    ImageURI = ISS.InsertComponentImage(File.ReadAllBytes(defaultComponentImagePath)),
                    DatasheetLink = "",
                    CategoryID = categories[7].CategoryID
                },

                // Temperature Sensors
                new Component
                {
                    Name = "DS18B20",
                    Manufacturer = "Maxim Integrated",
                    ShortDescription = "Digital temperature sensor",
                    LongDescription = "Precise digital temperature sensor with one-wire communication.",
                    ImageURI = ISS.InsertComponentImage(File.ReadAllBytes(defaultComponentImagePath)),
                    DatasheetLink = "",
                    CategoryID = categories[8].CategoryID
                },
                new Component
                {
                    Name = "LM35",
                    Manufacturer = "Texas Instruments",
                    ShortDescription = "Analog temperature sensor",
                    LongDescription = "Reliable analog temperature sensor suitable for environmental and industrial monitoring.",
                    ImageURI = ISS.InsertComponentImage(File.ReadAllBytes(defaultComponentImagePath)),
                    DatasheetLink = "",
                    CategoryID = categories[8].CategoryID
                },
                new Component
                {
                    Name = "BME280",
                    Manufacturer = "Bosch",
                    ShortDescription = "Temperature, humidity, and atmospheric pressure sensor",
                    LongDescription = "All-in-one sensor for environmental monitoring, measuring temperature, humidity, and pressure.",
                    ImageURI = ISS.InsertComponentImage(File.ReadAllBytes(defaultComponentImagePath)),
                    DatasheetLink = "",
                    CategoryID = categories[8].CategoryID
                },
                new Component
                {
                    Name = "TMP36",
                    Manufacturer = "Analog Devices",
                    ShortDescription = "High accuracy analog temperature sensor",
                    LongDescription = "Accurate and easy-to-use analog temperature sensor for precision measurements.",
                    ImageURI = ISS.InsertComponentImage(File.ReadAllBytes(defaultComponentImagePath)),
                    DatasheetLink = "",
                    CategoryID = categories[8].CategoryID
                },
                new Component
                {
                    Name = "HDC1080",
                    Manufacturer = "Texas Instruments",
                    ShortDescription = "Low-power digital temperature and humidity sensor",
                    LongDescription = "Efficient sensor for temperature and humidity measurement in energy-conscious designs.",
                    ImageURI = ISS.InsertComponentImage(File.ReadAllBytes(defaultComponentImagePath)),
                    DatasheetLink = "",
                    CategoryID = categories[8].CategoryID
                },

                // Humidity Sensors
                new Component
                {
                    Name = "DHT22",
                    Manufacturer = "AOSONG",
                    ShortDescription = "Digital humidity and temperature sensor",
                    LongDescription = "Precise digital sensor for measuring humidity and temperature in various environments.",
                    ImageURI = ISS.InsertComponentImage(File.ReadAllBytes(defaultComponentImagePath)),
                    DatasheetLink = "",
                    CategoryID = categories[9].CategoryID
                },
                new Component
                {
                    Name = "AM2301",
                    Manufacturer = "AOSONG",
                    ShortDescription = "Digital humidity and temperature sensor",
                    LongDescription = "Compact sensor for accurate humidity and temperature measurements.",
                    ImageURI = ISS.InsertComponentImage(File.ReadAllBytes(defaultComponentImagePath)),
                    DatasheetLink = "",
                    CategoryID = categories[9].CategoryID
                },
                new Component
                {
                    Name = "SHT31",
                    Manufacturer = "Sensirion",
                    ShortDescription = "High-precision digital humidity and temperature sensor",
                    LongDescription = "Advanced sensor offering precise humidity and temperature readings for critical applications.",
                    ImageURI = ISS.InsertComponentImage(File.ReadAllBytes(defaultComponentImagePath)),
                    DatasheetLink = "",
                    CategoryID = categories[9].CategoryID
                },
                new Component
                {
                    Name = "SHTC3",
                    Manufacturer = "Sensirion",
                    ShortDescription = "Miniature digital humidity and temperature sensor",
                    LongDescription = "Compact and efficient sensor for humidity and temperature monitoring in space-constrained applications.",
                    ImageURI = ISS.InsertComponentImage(File.ReadAllBytes(defaultComponentImagePath)),
                    DatasheetLink = "",
                    CategoryID = categories[9].CategoryID
                },

                // Encoders
                new Component
                {
                    Name = "KY-040",
                    Manufacturer = "Keyes",
                    ShortDescription = "Rotary encoder for angular position detection",
                    LongDescription = "Durable rotary encoder used in navigation, adjustment, and control systems.",
                    ImageURI = ISS.InsertComponentImage(File.ReadAllBytes(defaultComponentImagePath)),
                    DatasheetLink = "",
                    CategoryID = categories[10].CategoryID
                },
                new Component
                {
                    Name = "EC11",
                    Manufacturer = "Bourns",
                    ShortDescription = "Rotary encoder with button, ideal for adjustments and navigation",
                    LongDescription = "Compact encoder with an integrated button, perfect for user interface applications.",
                    ImageURI = ISS.InsertComponentImage(File.ReadAllBytes(defaultComponentImagePath)),
                    DatasheetLink = "",
                    CategoryID = categories[10].CategoryID
                },
                new Component
                {
                    Name = "Bourns PEC11R",
                    Manufacturer = "Bourns",
                    ShortDescription = "Silent rotary encoder with multiple outputs, ideal for audio projects",
                    LongDescription = "Smooth and silent rotary encoder designed for audio and multimedia applications.",
                    ImageURI = ISS.InsertComponentImage(File.ReadAllBytes(defaultComponentImagePath)),
                    DatasheetLink = "",
                    CategoryID = categories[10].CategoryID
                },
                new Component
                {
                    Name = "AMT102",
                    Manufacturer = "CUI Devices",
                    ShortDescription = "Incremental rotary encoder with digital output, high precision",
                    LongDescription = "High-resolution rotary encoder suitable for industrial and robotics applications.",
                    ImageURI = ISS.InsertComponentImage(File.ReadAllBytes(defaultComponentImagePath)),
                    DatasheetLink = "",
                    CategoryID = categories[10].CategoryID
                },

                // Photoresistors
                new Component
                {
                    Name = "GL5528",
                    Manufacturer = "GLT",
                    ShortDescription = "Photoresistor responsive to light levels",
                    LongDescription = "A compact and reliable photoresistor ideal for ambient light detection and automation systems.",
                    ImageURI = ISS.InsertComponentImage(File.ReadAllBytes(defaultComponentImagePath)),
                    DatasheetLink = "",
                    CategoryID = categories[11].CategoryID
                },
                new Component
                {
                    Name = "GL5537",
                    Manufacturer = "GLT",
                    ShortDescription = "Photoresistor for automation applications",
                    LongDescription = "Photoresistor designed for accurate light detection, commonly used in automatic systems.",
                    ImageURI = ISS.InsertComponentImage(File.ReadAllBytes(defaultComponentImagePath)),
                    DatasheetLink = "",
                    CategoryID = categories[11].CategoryID
                },
                new Component
                {
                    Name = "LDR",
                    Manufacturer = "General",
                    ShortDescription = "High sensitivity photoresistor for DIY applications",
                    LongDescription = "Versatile light-dependent resistor (LDR) ideal for custom and DIY electronics projects.",
                    ImageURI = ISS.InsertComponentImage(File.ReadAllBytes(defaultComponentImagePath)),
                    DatasheetLink = "",
                    CategoryID = categories[11].CategoryID
                },
                new Component
                {
                    Name = "TSL2591",
                    Manufacturer = "Adafruit",
                    ShortDescription = "High-sensitivity digital light sensor with I2C interface",
                    LongDescription = "Highly precise light sensor with a wide dynamic range, perfect for ambient light measurement.",
                    ImageURI = ISS.InsertComponentImage(File.ReadAllBytes(defaultComponentImagePath)),
                    DatasheetLink = "",
                    CategoryID = categories[11].CategoryID
                },
                new Component
                {
                    Name = "VEML6075",
                    Manufacturer = "Vishay",
                    ShortDescription = "UV photodiode for UV radiation level measurement",
                    LongDescription = "Advanced UV sensor for accurate detection of ultraviolet radiation levels.",
                    ImageURI = ISS.InsertComponentImage(File.ReadAllBytes(defaultComponentImagePath)),
                    DatasheetLink = "",
                    CategoryID = categories[11].CategoryID
                },

                // Phototransistors
                new Component
                {
                    Name = "PT333-3C",
                    Manufacturer = "Everlight",
                    ShortDescription = "Phototransistor sensitive to visible light",
                    LongDescription = "Reliable phototransistor for detecting visible light, ideal for optical sensing applications.",
                    ImageURI = ISS.InsertComponentImage(File.ReadAllBytes(defaultComponentImagePath)),
                    DatasheetLink = "",
                    CategoryID = categories[12].CategoryID
                },
                new Component
                {
                    Name = "LPT-80",
                    Manufacturer = "Lite-On",
                    ShortDescription = "Phototransistor ideal for object detection",
                    LongDescription = "High-sensitivity phototransistor suitable for object detection and optoelectronic systems.",
                    ImageURI = ISS.InsertComponentImage(File.ReadAllBytes(defaultComponentImagePath)),
                    DatasheetLink = "",
                    CategoryID = categories[12].CategoryID
                },
                new Component
                {
                    Name = "HPT-20",
                    Manufacturer = "Everlight",
                    ShortDescription = "High-sensitivity phototransistor for optoelectronic applications",
                    LongDescription = "Advanced phototransistor designed for optoelectronic devices and high-precision light detection.",
                    ImageURI = ISS.InsertComponentImage(File.ReadAllBytes(defaultComponentImagePath)),
                    DatasheetLink = "",
                    CategoryID = categories[12].CategoryID
                },
                new Component
                {
                    Name = "PT203-3C",
                    Manufacturer = "Everlight",
                    ShortDescription = "General-purpose phototransistor for visible light detection",
                    LongDescription = "General-purpose phototransistor ideal for light-sensitive electronics.",
                    ImageURI = ISS.InsertComponentImage(File.ReadAllBytes(defaultComponentImagePath)),
                    DatasheetLink = "",
                    CategoryID = categories[12].CategoryID
                },

                // Infrared Receivers
                new Component
                {
                    Name = "TSOP38238",
                    Manufacturer = "Vishay",
                    ShortDescription = "Infrared receiver for IR signal decoding",
                    LongDescription = "Efficient IR receiver for remote control applications and signal decoding at 38 kHz.",
                    ImageURI = ISS.InsertComponentImage(File.ReadAllBytes(defaultComponentImagePath)),
                    DatasheetLink = "",
                    CategoryID = categories[13].CategoryID
                },
                new Component
                {
                    Name = "TSOP38240",
                    Manufacturer = "Vishay",
                    ShortDescription = "40 kHz infrared signal receiver",
                    LongDescription = "IR receiver optimized for high accuracy signal decoding at 40 kHz frequency.",
                    ImageURI = ISS.InsertComponentImage(File.ReadAllBytes(defaultComponentImagePath)),
                    DatasheetLink = "",
                    CategoryID = categories[13].CategoryID
                },
                new Component
                {
                    Name = "VS1838B",
                    Manufacturer = "Vishay",
                    ShortDescription = "High sensitivity IR receiver with low power consumption",
                    LongDescription = "Compact and power-efficient IR receiver for remote control and sensing applications.",
                    ImageURI = ISS.InsertComponentImage(File.ReadAllBytes(defaultComponentImagePath)),
                    DatasheetLink = "",
                    CategoryID = categories[13].CategoryID
                },
                new Component
                {
                    Name = "GP1UX511QS",
                    Manufacturer = "Sharp",
                    ShortDescription = "Infrared receiver with long range and narrow detection angle",
                    LongDescription = "High-performance IR receiver for applications requiring precise angle detection.",
                    ImageURI = ISS.InsertComponentImage(File.ReadAllBytes(defaultComponentImagePath)),
                    DatasheetLink = "",
                    CategoryID = categories[13].CategoryID
                },
                new Component
                {
                    Name = "RPR-220",
                    Manufacturer = "Rohm",
                    ShortDescription = "Infrared receiver for various IR applications",
                    LongDescription = "Versatile IR receiver ideal for remote sensing and IR communication systems.",
                    ImageURI = ISS.InsertComponentImage(File.ReadAllBytes(defaultComponentImagePath)),
                    DatasheetLink = "",
                    CategoryID = categories[13].CategoryID
                },

                // Accelerometers
                new Component
                {
                    Name = "ADXL345",
                    Manufacturer = "Analog Devices",
                    ShortDescription = "Three-axis accelerometer",
                    LongDescription = "Compact and accurate accelerometer suitable for motion detection and tilt measurement.",
                    ImageURI = ISS.InsertComponentImage(File.ReadAllBytes(defaultComponentImagePath)),
                    DatasheetLink = "",
                    CategoryID = categories[14].CategoryID
                },
                new Component
                {
                    Name = "LIS3DH",
                    Manufacturer = "STMicroelectronics",
                    ShortDescription = "Three-axis accelerometer with I2C/SPI digital output",
                    LongDescription = "High-performance accelerometer with digital output for precise motion sensing.",
                    ImageURI = ISS.InsertComponentImage(File.ReadAllBytes(defaultComponentImagePath)),
                    DatasheetLink = "",
                    CategoryID = categories[14].CategoryID
                },
                new Component
                {
                    Name = "BMA220",
                    Manufacturer = "Bosch",
                    ShortDescription = "Miniature three-axis accelerometer with low power consumption",
                    LongDescription = "Low-power accelerometer for compact and energy-efficient designs.",
                    ImageURI = ISS.InsertComponentImage(File.ReadAllBytes(defaultComponentImagePath)),
                    DatasheetLink = "",
                    CategoryID = categories[14].CategoryID
                },
                new Component
                {
                    Name = "MPU6050",
                    Manufacturer = "InvenSense",
                    ShortDescription = "3-axis accelerometer and 3-axis gyroscope in one chip",
                    LongDescription = "Integrated motion sensor for precise measurement of acceleration and rotational velocity.",
                    ImageURI = ISS.InsertComponentImage(File.ReadAllBytes(defaultComponentImagePath)),
                    DatasheetLink = "",
                    CategoryID = categories[14].CategoryID
                },
                new Component
                {
                    Name = "ADXL362",
                    Manufacturer = "Analog Devices",
                    ShortDescription = "Ultra-low power three-axis accelerometer with motion detection",
                    LongDescription = "Energy-efficient accelerometer with advanced motion detection features.",
                    ImageURI = ISS.InsertComponentImage(File.ReadAllBytes(defaultComponentImagePath)),
                    DatasheetLink = "",
                    CategoryID = categories[14].CategoryID
                },

                // Hall Effect Sensors
                new Component
                {
                    Name = "DRV5053",
                    Manufacturer = "Texas Instruments",
                    ShortDescription = "Linear Hall effect sensor for magnetic field measurement",
                    LongDescription = "A high-accuracy Hall effect sensor for precise and linear measurement of magnetic fields.",
                    ImageURI = ISS.InsertComponentImage(File.ReadAllBytes(defaultComponentImagePath)),
                    DatasheetLink = "",
                    CategoryID = categories[15].CategoryID
                },
                new Component
                {
                    Name = "A3141",
                    Manufacturer = "Allegro",
                    ShortDescription = "Hall sensor with analog output for magnetic field measurement",
                    LongDescription = "Analog-output Hall sensor ideal for detecting and measuring magnetic fields.",
                    ImageURI = ISS.InsertComponentImage(File.ReadAllBytes(defaultComponentImagePath)),
                    DatasheetLink = "",
                    CategoryID = categories[15].CategoryID
                },
                new Component
                {
                    Name = "HMC5883L",
                    Manufacturer = "Honeywell",
                    ShortDescription = "Three-axis Hall sensor for magnetic field measurement",
                    LongDescription = "Compact and versatile three-axis magnetometer for applications requiring precise magnetic field sensing.",
                    ImageURI = ISS.InsertComponentImage(File.ReadAllBytes(defaultComponentImagePath)),
                    DatasheetLink = "",
                    CategoryID = categories[15].CategoryID
                },
                new Component
                {
                    Name = "TLE493D-W2B6",
                    Manufacturer = "Infineon",
                    ShortDescription = "Low-power Hall sensor for magnetic field measurement",
                    LongDescription = "Energy-efficient Hall sensor with advanced capabilities for magnetic field measurement.",
                    ImageURI = ISS.InsertComponentImage(File.ReadAllBytes(defaultComponentImagePath)),
                    DatasheetLink = "",
                    CategoryID = categories[15].CategoryID
                },
                new Component
                {
                    Name = "DRV5055",
                    Manufacturer = "Texas Instruments",
                    ShortDescription = "Linear Hall sensor with analog output for magnetic field measurement",
                    LongDescription = "High-sensitivity Hall effect sensor with linear analog output, suitable for various magnetic sensing applications.",
                    ImageURI = ISS.InsertComponentImage(File.ReadAllBytes(defaultComponentImagePath)),
                    DatasheetLink = "",
                    CategoryID = categories[15].CategoryID
                },

                // Microcontrollers
                new Component
                {
                    Name = "ATmega328P",
                    Manufacturer = "Microchip",
                    ShortDescription = "8-bit AVR microcontroller used in Arduino platforms",
                    LongDescription = "A versatile 8-bit microcontroller, popular for use in Arduino Uno and other development platforms.",
                    ImageURI = ISS.InsertComponentImage(File.ReadAllBytes(defaultComponentImagePath)),
                    DatasheetLink = "",
                    CategoryID = categories[16].CategoryID
                },
                new Component
                {
                    Name = "STM32F103C8T6",
                    Manufacturer = "STMicroelectronics",
                    ShortDescription = "32-bit microcontroller with ARM Cortex-M3 core",
                    LongDescription = "A high-performance 32-bit microcontroller for development in the STM32 ecosystem, known for its speed and reliability.",
                    ImageURI = ISS.InsertComponentImage(File.ReadAllBytes(defaultComponentImagePath)),
                    DatasheetLink = "",
                    CategoryID = categories[16].CategoryID
                },
                new Component
                {
                    Name = "ESP8266",
                    Manufacturer = "Espressif",
                    ShortDescription = "Low-power WiFi microcontroller, commonly used in IoT",
                    LongDescription = "Compact microcontroller with built-in WiFi capabilities, ideal for IoT and smart home applications.",
                    ImageURI = ISS.InsertComponentImage(File.ReadAllBytes(defaultComponentImagePath)),
                    DatasheetLink = "",
                    CategoryID = categories[16].CategoryID
                },
                new Component
                {
                    Name = "ESP32",
                    Manufacturer = "Espressif",
                    ShortDescription = "Dual-core microcontroller with built-in WiFi and Bluetooth",
                    LongDescription = "Powerful microcontroller featuring dual cores, WiFi, and Bluetooth for robust IoT applications.",
                    ImageURI = ISS.InsertComponentImage(File.ReadAllBytes(defaultComponentImagePath)),
                    DatasheetLink = "",
                    CategoryID = categories[16].CategoryID
                },
                new Component
                {
                    Name = "PIC16F877A",
                    Manufacturer = "Microchip",
                    ShortDescription = "8-bit microcontroller from the PIC family",
                    LongDescription = "Reliable and widely used 8-bit microcontroller suitable for embedded applications.",
                    ImageURI = ISS.InsertComponentImage(File.ReadAllBytes(defaultComponentImagePath)),
                    DatasheetLink = "",
                    CategoryID = categories[16].CategoryID
                },
                new Component
                {
                    Name = "ATtiny85",
                    Manufacturer = "Microchip",
                    ShortDescription = "Small AVR microcontroller ideal for compact projects",
                    LongDescription = "A miniature microcontroller offering versatility and simplicity for space-constrained designs.",
                    ImageURI = ISS.InsertComponentImage(File.ReadAllBytes(defaultComponentImagePath)),
                    DatasheetLink = "",
                    CategoryID = categories[16].CategoryID
                },
                new Component
                {
                    Name = "RP2040",
                    Manufacturer = "Raspberry Pi Foundation",
                    ShortDescription = "Dual-core ARM Cortex-M0+ microcontroller used in Raspberry Pi Pico",
                    LongDescription = "High-performance microcontroller for versatile applications, designed by the Raspberry Pi Foundation.",
                    ImageURI = ISS.InsertComponentImage(File.ReadAllBytes(defaultComponentImagePath)),
                    DatasheetLink = "",
                    CategoryID = categories[16].CategoryID
                },
                new Component
                {
                    Name = "MSP430G2553",
                    Manufacturer = "Texas Instruments",
                    ShortDescription = "16-bit microcontroller with ultra-low power consumption",
                    LongDescription = "Efficient and low-power 16-bit microcontroller, ideal for portable and battery-operated devices.",
                    ImageURI = ISS.InsertComponentImage(File.ReadAllBytes(defaultComponentImagePath)),
                    DatasheetLink = "",
                    CategoryID = categories[16].CategoryID
                },
                new Component
                {
                    Name = "nRF52840",
                    Manufacturer = "Nordic Semiconductor",
                    ShortDescription = "Microcontroller with built-in BLE and NFC for IoT",
                    LongDescription = "Advanced microcontroller with integrated Bluetooth Low Energy (BLE) and NFC capabilities for IoT solutions.",
                    ImageURI = ISS.InsertComponentImage(File.ReadAllBytes(defaultComponentImagePath)),
                    DatasheetLink = "",
                    CategoryID = categories[16].CategoryID
                },
                new Component
                {
                    Name = "SAMD21",
                    Manufacturer = "Microchip",
                    ShortDescription = "32-bit microcontroller with ARM Cortex-M0+ core",
                    LongDescription = "Modern microcontroller with low-power design, popular in Arduino Zero and other maker boards.",
                    ImageURI = ISS.InsertComponentImage(File.ReadAllBytes(defaultComponentImagePath)),
                    DatasheetLink = "",
                    CategoryID = categories[16].CategoryID
                },
            };
        context.Components.AddRange(components);
        await context.SaveChangesAsync();

        List<PredefinedImage> predefinedImages = new List<PredefinedImage>()
            {
                new PredefinedImage(){ Name = "IC: Default", Category = "SMD", Image = File.ReadAllBytes(Path.Combine(predefinedImagesPath, "1.png")) },
                new PredefinedImage(){ Name = "IC: ", Category = "TH", Image = File.ReadAllBytes(Path.Combine(predefinedImagesPath, "2.png")) },
                new PredefinedImage(){ Name = "7 segment display", Category = "TH", Image = File.ReadAllBytes(Path.Combine(predefinedImagesPath, "3.png")) },
                new PredefinedImage(){ Name = "Tactile switch", Category = "SMD", Image = File.ReadAllBytes(Path.Combine(predefinedImagesPath, "4.png")) },
                new PredefinedImage(){ Name = "Inductor", Category = "SMD", Image = File.ReadAllBytes(Path.Combine(predefinedImagesPath, "5.png")) },
                new PredefinedImage(){ Name = "IC: 3 pin", Category = "SMD", Image = File.ReadAllBytes(Path.Combine(predefinedImagesPath, "6.png")) },
                new PredefinedImage(){ Name = "IC: 3 pin", Category = "TH", Image = File.ReadAllBytes(Path.Combine(predefinedImagesPath, "7.png")) },
                new PredefinedImage(){ Name = "LED", Category = "TH", Image = File.ReadAllBytes(Path.Combine(predefinedImagesPath, "8.png")) },
                new PredefinedImage(){ Name = "LCD Display", Category = "Board", Image = File.ReadAllBytes(Path.Combine(predefinedImagesPath, "9.png")) },
                new PredefinedImage(){ Name = "Development board", Category = "Board", Image = File.ReadAllBytes(Path.Combine(predefinedImagesPath, "10.png")) },
                new PredefinedImage(){ Name = "Resistor", Category = "SMD", Image = File.ReadAllBytes(Path.Combine(predefinedImagesPath, "11.png")) },
                new PredefinedImage(){ Name = "IC: 16 pin", Category = "QFN", Image = File.ReadAllBytes(Path.Combine(predefinedImagesPath, "12.png")) },
                new PredefinedImage(){ Name = "LED Green", Category = "TH", Image = File.ReadAllBytes(Path.Combine(predefinedImagesPath, "13.png")) },
                new PredefinedImage(){ Name = "Diode", Category = "SMD", Image = File.ReadAllBytes(Path.Combine(predefinedImagesPath, "14.png")) },
                new PredefinedImage(){ Name = "GPIO Header", Category = "TH", Image = File.ReadAllBytes(Path.Combine(predefinedImagesPath, "15.png")) },
                new PredefinedImage(){ Name = "LCD Display", Category = "Board", Image = File.ReadAllBytes(Path.Combine(predefinedImagesPath, "16.png")) },
                new PredefinedImage(){ Name = "Capacitor", Category = "SMD", Image = File.ReadAllBytes(Path.Combine(predefinedImagesPath, "17.png")) },
                new PredefinedImage(){ Name = "Module", Category = "Board", Image = File.ReadAllBytes(Path.Combine(predefinedImagesPath, "18.png")) },
                new PredefinedImage(){ Name = "IC: 8 pin", Category = "SMD", Image = File.ReadAllBytes(Path.Combine(predefinedImagesPath, "19.png")) },
                new PredefinedImage(){ Name = "GPIO Header", Category = "TH", Image = File.ReadAllBytes(Path.Combine(predefinedImagesPath, "20.png")) },
                new PredefinedImage(){ Name = "Arduino UNO", Category = "Board", Image = File.ReadAllBytes(Path.Combine(predefinedImagesPath, "21.png")) },
                new PredefinedImage(){ Name = "Relay", Category = "TH", Image = File.ReadAllBytes(Path.Combine(predefinedImagesPath, "22.png")) },
                new PredefinedImage(){ Name = "IC: 30 pin", Category = "SMD", Image = File.ReadAllBytes(Path.Combine(predefinedImagesPath, "23.png")) },
                new PredefinedImage(){ Name = "IC: 16 pin", Category = "TH", Image = File.ReadAllBytes(Path.Combine(predefinedImagesPath, "24.png")) },
                new PredefinedImage(){ Name = "Diode", Category = "TH", Image = File.ReadAllBytes(Path.Combine(predefinedImagesPath, "25.png")) },
                new PredefinedImage(){ Name = "GPIO Header", Category = "TH", Image = File.ReadAllBytes(Path.Combine(predefinedImagesPath, "26.png")) },
            };
        context.PredefinedImage.AddRange(predefinedImages);
        await context.SaveChangesAsync();
    }
}