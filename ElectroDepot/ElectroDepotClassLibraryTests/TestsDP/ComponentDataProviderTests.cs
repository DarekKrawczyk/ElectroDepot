using ElectroDepotClassLibrary.DTOs;
using ElectroDepotClassLibrary.Models;
using Xunit.Abstractions;

namespace ElectroDepotClassLibraryTests.Tests
{
    public class ComponentDataProviderTests : BaseDataProviderTest
    {
        public ComponentDataProviderTests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task GetAll()
        {
            try
            {
                IEnumerable<Component> components = await ComponentDP.GetAllComponents();
                Assert.NotNull(components);
                foreach (Component component in components)
                {
                    Console.WriteLine($"{component.ToString()}");
                }
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
        [Fact]
        public async Task Create()
        {
            try
            {
                // Find category with 'Temperature gauge'
                Category category = await CategoryDP.GetCategoryByName("Temperature gauge");

                if (category == null)
                {
                    Category gauge = new Category(id: 0, name: "Temperature gauge", description: "Measures temperature", image: new byte[] { });
                    bool created = await CategoryDP.CreateCategory(gauge);
                    Assert.True(created);
                    category = await CategoryDP.GetCategoryByName("Temperature gauge");
                    Assert.NotNull(category);
                }

                Component component = new Component(id: 0, categoryID: category.ID, category: null, name: "LM35",
                    manufacturer: "Texas Instruments", shortDescription: "Analog temperature gauge", longDescription: "long desc",
                    datasheetLink: "google.com", new byte[0]{ });

                var obj = await ComponentDP.CreateComponent(component);
                Assert.True(obj != null);
                Console.WriteLine(component.ToString());
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
        [Fact]
        public async Task Delete()
        {
            try
            {
                IEnumerable<Component> components = await ComponentDP.GetAllComponents();
                Assert.NotNull(components);
                Assert.True(components.Count() > 0);

                int countBefore = components.Count();

                Component last = components.Last();

                await ComponentDP.DeleteComponent(last.ID);

                components = await ComponentDP.GetAllComponents();
                Assert.NotNull(components);
                Assert.True(components.Count() > 0);

                int countAfter = components.Count();

                Assert.True(countBefore == countAfter + 1);
                foreach (Component component in components)
                {
                    Console.WriteLine(component.ToString());
                }
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
        [Fact]
        public async Task CreateWithCategory()
        {
            try
            {
                Category delFirst = await CategoryDP.GetCategoryByName("Silniczek");
                // Delete category
                if (delFirst != null)
                {
                    bool wasDefFirst = await CategoryDP.DeleteCategory(delFirst.ID);
                    Assert.True(wasDefFirst);
                }

                // Create category
                Category category = new Category(id: 0, name: "Silniczek", description: "Fajnie kreci", image: new byte[] { });
                bool wasCreated = await CategoryDP.CreateCategory(category);
                Assert.True(wasCreated);

                // Find
                Category foundCategory = await CategoryDP.GetCategoryByName(category.Name);
                Assert.NotNull(foundCategory);

                // Delete component if exists
                Component delCompFirst = await ComponentDP.GetComponentByName("Silniczek");
                if (delCompFirst != null)
                {
                    bool wasdelCompFirst = await ComponentDP.DeleteComponent(delCompFirst);
                    Assert.True(wasdelCompFirst);
                }

                // Create component with category
                Component component = new Component(id: 0, categoryID: foundCategory.ID, category:null, name: "Silniczek", 
                    manufacturer: "Texus fujara", shortDescription: "Ale kręci", longDescription: "long desc",
                    datasheetLink: "google.com", new byte[0] { });
                var obj = await ComponentDP.CreateComponent(component);
                Assert.True(obj != null);

                // Find create component
                Component createdComponent = await ComponentDP.GetComponentByName(component.Name);
                Assert.NotNull(createdComponent);

                Console.WriteLine(createdComponent.ToString());
                Console.WriteLine(foundCategory.ToString());

                // Delete component
                bool wasComponentDeleted = await ComponentDP.DeleteComponent(createdComponent.ID);
                Assert.True(wasComponentDeleted);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
        [Fact]
        public async Task Update()
        {
            try
            {
                // Find
                Component foundComponent = await ComponentDP.GetComponentByID(1);
                Assert.NotNull(foundComponent);

                // Update
                Component editedCategoryDTO = new Component(id: foundComponent.ID, categoryID: foundComponent.CategoryID, category: null, 
                    name: foundComponent.Name, manufacturer: foundComponent.Manufacturer, shortDescription: "Edited component",
                    longDescription: "long desc", datasheetLink: "google.com", new byte[0] { });
                bool wasUpdated = await ComponentDP.UpdateComponent(editedCategoryDTO);
                Assert.True(wasUpdated);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
    }
}

