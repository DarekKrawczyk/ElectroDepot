using Microsoft.EntityFrameworkCore;
using Server.Context;
using Server.Models;

public static class BasicDataSeeder
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
            // Seed Users
            var users = new List<User>
            {
                new User { Username = "jacek.jaworek", Password = "FindMeIfYouCan123", Email = "jacek.jaworek@gmail.com" },
                new User { Username = "anna.kowalska", Password = "SecurePass123", Email = "anna.kowalska@example.com" }
            };
            context.Users.AddRange(users);
            await context.SaveChangesAsync();

            // Seed Categories
            var categories = new List<Category>
            {
                new Category { Name = "Czujnik temperatury", Description = "Mierzy temperaturę" },
                new Category { Name = "Czujnik wilgotności", Description = "Mierzy wilgotność" },
                new Category { Name = "Czujnik światła", Description = "Wykrywa poziom oświetlenia" }
            };
            context.Categories.AddRange(categories);
            await context.SaveChangesAsync();

            // Seed Suppliers
            var suppliers = new List<Supplier>
            {
                new Supplier { Name = "Supplier1", Website = "https://supplier1.com" },
                new Supplier { Name = "Supplier2", Website = "https://supplier2.com" }
            };
            context.Suppliers.AddRange(suppliers);
            await context.SaveChangesAsync();

            // Seed Components
            var components = new List<Component>
            {
                new Component { Name = "Czujnik temperatury DS18B20", Manufacturer = "Dallas", ShortDescription = "Dokładny czujnik temperatury", CategoryID = categories[0].CategoryID },
                new Component { Name = "Czujnik wilgotności DHT22", Manufacturer = "AOSONG", ShortDescription = "Pomiar wilgotności i temperatury", CategoryID = categories[1].CategoryID },
                new Component { Name = "Fotorezystor LDR", Manufacturer = "ResistorCo", ShortDescription = "Oświetlenie na podstawie światła", CategoryID = categories[2].CategoryID }
            };
            context.Components.AddRange(components);
            await context.SaveChangesAsync();

            // Seed Purchases
            var purchases = new List<Purchase>
            {
                new Purchase { UserID = users[0].UserID, SupplierID = suppliers[0].SupplierID, PurchasedDate = DateTime.Now, TotalPrice = 100.50 },
                new Purchase { UserID = users[1].UserID, SupplierID = suppliers[1].SupplierID, PurchasedDate = DateTime.Now, TotalPrice = 150.75 }
            };
            context.Purchases.AddRange(purchases);
            await context.SaveChangesAsync();

            // Seed PurchaseItems
            var purchaseItems = new List<PurchaseItem>
            {
                new PurchaseItem { PurchaseID = purchases[0].PurchaseID, ComponentID = components[0].ComponentID, Quantity = 10, PricePerUnit = 5.05 },
                new PurchaseItem { PurchaseID = purchases[1].PurchaseID, ComponentID = components[1].ComponentID, Quantity = 5, PricePerUnit = 10.15 }
            };
            context.PurchaseItems.AddRange(purchaseItems);
            await context.SaveChangesAsync();

            // Seed Projects
            var projects = new List<Project>
            {
                new Project { UserID = users[0].UserID, Name = "Smart Home System", Description = "System automatyzacji domu", CreatedAt = DateTime.Now },
                new Project { UserID = users[1].UserID, Name = "Weather Station", Description = "Monitorowanie pogody", CreatedAt = DateTime.Now }
            };
            context.Projects.AddRange(projects);
            await context.SaveChangesAsync();

            // Seed ProjectComponents
            var projectComponents = new List<ProjectComponent>
            {
                new ProjectComponent { ProjectID = projects[0].ProjectID, ComponentID = components[0].ComponentID, Quantity = 2 },
                new ProjectComponent { ProjectID = projects[1].ProjectID, ComponentID = components[1].ComponentID, Quantity = 1 }
            };
            context.ProjectComponents.AddRange(projectComponents);
            await context.SaveChangesAsync();

            // Seed OwnsComponents (user's own components)
            var ownsComponents = new List<OwnsComponent>
            {
                new OwnsComponent { UserID = users[0].UserID, ComponentID = components[0].ComponentID, Quantity = 3 },
                new OwnsComponent { UserID = users[1].UserID, ComponentID = components[1].ComponentID, Quantity = 1 }
            };
            context.OwnsComponent.AddRange(ownsComponents);
            await context.SaveChangesAsync();
        }
    }
}
