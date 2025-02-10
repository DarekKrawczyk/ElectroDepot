using ElectroDepotClassLibrary.DTOs;
using ElectroDepotClassLibrary.Models;
using Xunit.Abstractions;

namespace ElectroDepotClassLibraryTests.TestsOperations
{
    public class SelectCalls : BaseDataProviderTest
    {

        public SelectCalls(ITestOutputHelper output) : base(output)
        {
        }
        #region Users
        [Fact]
        public async Task Get_All_Users()
        {
            try
            {
                string[][] users = new string[][]
                {
                    new string[] { "jacek.jaworek", "jacek.jaworek@gmail.com" },
                    new string[] { "agnieszka.nakowicz", "agnieszka.nakowicz@gmail.com" },
                    new string[] { "hanna.kozłowska", "hanna.kozłowska@gmail.com" },
                    new string[] { "piotr.wisniewski", "piotr.wisniewski@gmail.com" },
                    new string[] { "jarosław.paduch", "jarosław.paduch@gmail.com" },
                    new string[] { "andrzej.kowalski", "andrzej.kowalski@gmail.com" },
                    new string[] { "oleg.ontemijczuk", "oleg.ontemijczuk@gmail.com" },
                    new string[] { "ewa.maj", "ewa.maj@gmail.com" },
                    new string[] { "grzegorz.baron", "grzegorz.baron@gmail.com" },
                    new string[] { "joanna.pawlowska", "joanna.pawlowska@gmail.com" }
                };
                IEnumerable<User> allUsersFromDB = await UserDP.GetAllUsers();

                foreach (User user in allUsersFromDB)
                {
                    Console.WriteLine(user.ToString());
                    string[] dbUserData = new string[] {user.Name, user.Email };
                    if (!users.Any(x=> x[0] == dbUserData[0] && x[1] == dbUserData[1]))
                    {
                        Assert.Fail($"User '{user.Name}' not found!");
                    }
                }
            }
            catch(Exception exception)
            {
                Assert.Fail(exception.Message);
            }
        }

        [Fact]
        public async Task Get_All_Components_From_User()
        {
            try
            {
                int[] CompsIDsOfJacekJaworek = new int[] { 77, 2, 5, 38, 82, 67, 9 };

                User user = await UserDP.GetUserByUsername("jacek.jaworek");
                IEnumerable<Component> componentsOfUser = await ComponentDP.GetAllUserComponent(user.ID);
                foreach(Component component in componentsOfUser)
                {
                    // Bias is Used to match with ids from TestingData.md
                    Console.WriteLine($"UserID: '{user.ID - Utility.UserIDBias}', ComponentID: '{component.ID - Utility.ComponentIDBias}'");
                }
                int[] dbIDs = componentsOfUser.Select(x => x.ID - Utility.ComponentIDBias).ToArray();

                Assert.True(CompsIDsOfJacekJaworek.OrderBy(x => x).SequenceEqual(dbIDs.OrderBy(x => x)));
            }
            catch (Exception exception)
            {
                Assert.Fail(exception.Message);
            }
        }

        [Fact]
        public async Task Get_All_Projects_From_User()
        {
            try
            {

                int[] ProjectsIDsOfJacekJaworek = new int[] { 0, 4 };

                User user = await UserDP.GetUserByUsername("jacek.jaworek");
                IEnumerable<Project> projectsOfUser = await ProjectDP.GetAllProjectOfUser(user);
                foreach (Project project in projectsOfUser)
                {
                    // Bias is used to match with ids from TestingData.md
                    Console.WriteLine($"UserID: '{user.ID - Utility.UserIDBias}', Project: '{project.Name}'");
                }
                int[] dbIDs = projectsOfUser.Select(x => x.ID - Utility.ProjectIDBias).ToArray();

                Assert.True(ProjectsIDsOfJacekJaworek.OrderBy(x => x).SequenceEqual(dbIDs.OrderBy(x => x)));
            }
            catch (Exception exception)
            {
                Assert.Fail(exception.Message);
            }
        }

        [Fact]
        public async Task Get_Component_Amount_From_User()
        {
            try
            {
                int[][] OwnedComponentCountOfJacekJaworek = new int[][] 
                { 
                    new int[] { 77, 1 },
                    new int[] { 2, 1 },
                    new int[] { 5, 2 },
                    new int[] { 38, 1 },
                    new int[] { 82, 2 },
                    new int[] { 67, 2 },
                    new int[] { 9, 2 },
                };

                User user = await UserDP.GetUserByUsername("jacek.jaworek");
                Assert.NotNull(user);

                IEnumerable<OwnsComponent> OwnedComponentsOfUser = await OwnsComponentDP.GetAllOwnsComponentsFromUser(user);
                Assert.NotNull(OwnedComponentsOfUser);

                foreach (OwnsComponent ownedComponent in OwnedComponentsOfUser)
                {
                    // Bias is used to match with ids from TestingData.md
                    Console.WriteLine($"UserID: '{user.ID - Utility.UserIDBias}', ComponentID: '{ownedComponent.ComponentID - Utility.ComponentIDBias}', Quantity: '{ownedComponent.Quantity}'");

                    int[] dbIDs = new int[] { ownedComponent.ComponentID - Utility.ComponentIDBias, ownedComponent.Quantity };

                    Assert.True(OwnedComponentCountOfJacekJaworek.Any(y => y.OrderBy(x => x).SequenceEqual(dbIDs.OrderBy(x => x))));
                }

                IEnumerable<Component> ComponentsOfUser = await ComponentDP.GetAllUserComponent(user.ID);
                Assert.NotNull(ComponentsOfUser);

                Component FirstComponent = ComponentsOfUser.FirstOrDefault();
                Assert.NotNull(FirstComponent);

                OwnsComponent componentOfComponentAndUser = await OwnsComponentDP.GetOwnComponentsFromUser(user, FirstComponent);
                Assert.NotNull(componentOfComponentAndUser);

                Console.WriteLine($"Found: Owner:'{user.ID - Utility.UserIDBias}', ComponentID:'{componentOfComponentAndUser.ComponentID - Utility.ComponentIDBias}', Quantity:'{componentOfComponentAndUser.Quantity}'");

            }
            catch (Exception exception)
            {
                Assert.Fail(exception.Message);
            }
        }

        [Fact]
        public async Task Get_All_Components_From_Project()
        {
            try
            {
                int[] ComponentsIDsFromProject0 = new int[] { 77, 2, 5, 38 };

                int projectID = Utility.ProjectIDBias;

                Project project = await ProjectDP.GetProjectByID(projectID); 
                Assert.NotNull(project);

                IEnumerable<Component> ComponentsOfProject = await ProjectDP.GetAllComponentsFromProject(project);
                Assert.NotNull(ComponentsOfProject);
                Assert.NotEmpty(ComponentsOfProject);

                int[] ids = ComponentsOfProject.Select(x => Math.Abs(Utility.ComponentIDBias - x.ID)).ToArray();

                bool isSame = ComponentsIDsFromProject0.SequenceEqual(ids);
                Assert.True(isSame);

                foreach (Component ownedComponent in ComponentsOfProject)
                {
                    Console.WriteLine(ownedComponent.ToString());
                }
            }
            catch (Exception exception)
            {
                Assert.Fail(exception.Message);
            }
        }

        [Fact]
        public async Task Get_All_Purchases_From_User()
        {
            try
            {
                // DATA DEF
                string username = "agnieszka.nakowicz";
                int[] purchaseIDS = new int[] { 1, 2 };
                //

                User user = await UserDP.GetUserByUsername(username);
                Assert.NotNull(user);

                IEnumerable<Purchase> purchases = await PurchaseDP.GetAllPurchasesFromUser(user);
                Assert.NotNull(purchases);
                Assert.NotEmpty(purchases);

                int[] purIDS = purchases.Select(x=> Math.Abs(Utility.PurchaseIDBias - x.ID)).ToArray();
                bool isSame = purIDS.SequenceEqual(purchaseIDS);
                Assert.True(isSame);

                foreach (Purchase purchase in purchases)
                {
                    Console.WriteLine(purchase.ToString());
                }
            }
            catch (Exception exception)
            {
                Assert.Fail(exception.Message);
            }
        }

        [Fact]
        public async Task Get_Project_Price()
        {
            try
            {
                // DATA DEF
                int projectID = Utility.ProjectIDBias;
                //

                Project project = await ProjectDP.GetProjectByID(projectID);
                Assert.NotNull(project);

                double projectPrice = await ProjectDP.GetProjectPrice(project);
                Assert.True(projectPrice != -1.0);
                Assert.True(projectPrice == 27.5);

                Console.WriteLine($"Price for '{project.Name}' project is '{projectPrice}'[pln]");
            }
            catch (Exception exception)
            {
                Assert.Fail(exception.Message);
            }
        }

        [Fact]
        public async Task Get_All_Components_From_Purchase()
        {
            try
            {
                // Data definition
                int projectID = Utility.ProjectIDBias;

                Project project = await ProjectDP.GetProjectByID(projectID);
                Assert.NotNull(project);

                double projectPrice = await ProjectDP.GetProjectPrice(project);
                Assert.True(projectPrice != -1.0);
                Assert.True(projectPrice == 27.5);

                Console.WriteLine($"Price for '{project.Name}' project is '{projectPrice}'[pln]");
            }
            catch (Exception exception)
            {
                Assert.Fail(exception.Message);
            }
        }

        [Fact]
        public async Task Get_All_PurchaseItems_From_Purchase()
        {
            try
            {
                // Data definition
                int purchaseID = Utility.PurchaseIDBias;
                int[] IDsArray = new int[] { 77, 2, 5, 38, 82, 67, 9 };
                int[] quantities = new int[] { 1, 1, 2, 1, 2, 2, 2 };

                Purchase purchase = await PurchaseDP.GetPurchaseByID(purchaseID);
                Assert.NotNull(purchase);

                IEnumerable<PurchaseItem> purchaseItems = await PurchaseItemDP.GetAllPurchaseItemsFromPurchase(purchase);
                Assert.NotNull(purchaseItems);
                Assert.NotEmpty(purchaseItems);

                int[] idsFromDB = purchaseItems.Select(x => Math.Abs(Utility.ComponentIDBias - x.ComponentID)).ToArray();
                int[] quantitiesFromDB = purchaseItems.Select(x => x.Quantity).ToArray();

                bool areIDsOk = idsFromDB.SequenceEqual(IDsArray);
                bool areQuantitiesOk = quantitiesFromDB.SequenceEqual(quantities);

                Assert.True(areIDsOk);
                Assert.True(areQuantitiesOk);
            }
            catch (Exception exception)
            {
                Assert.Fail(exception.Message);
            }
        }

        [Fact]
        public async Task Get_Free_To_Use_OwnsComponents()
        {
            try
            {
                // Data definition
                int userID = Utility.UserIDBias;

                User user = await UserDP.GetUserByID(userID);
                Assert.NotNull(user);

                IEnumerable<OwnsComponent> freeToUseComponents = await OwnsComponentDP.GetAllFreeToUseComponentsFromUser(user);
                Assert.NotNull(freeToUseComponents);

                foreach(OwnsComponent freeComponent in freeToUseComponents)
                {
                    Console.WriteLine(freeComponent.ToString());
                }
            }
            catch (Exception exception)
            {
                Assert.Fail(exception.Message);
            }
        }

        [Fact]
        public async Task Get_All_Used_OwnsComponents()
        {
            try
            {
                // Data definition
                int userID = Utility.UserIDBias;

                User user = await UserDP.GetUserByID(userID);
                Assert.NotNull(user);

                IEnumerable<OwnsComponent> freeToUseComponents = await OwnsComponentDP.GetAllUsedComponentsFromUser(user);
                Assert.NotNull(freeToUseComponents);

                foreach (OwnsComponent freeComponent in freeToUseComponents)
                {
                    Console.WriteLine(freeComponent.ToString());
                }
            }
            catch (Exception exception)
            {
                Assert.Fail(exception.Message);
            }
        }

        [Fact]
        public async Task Get_All_OwnsComponents_From_User()
        {
            try
            {
                // Data definition
                int userID = Utility.UserIDBias;
                int[] IDsArray = new int[] { 77, 2, 5, 38, 82, 67, 9 };
                int[] quantities = new int[] { 1, 1, 2, 1, 2, 2, 2 };

                User user = await UserDP.GetUserByID(userID);
                Assert.NotNull(user);

                IEnumerable<OwnsComponent> ownedComponents = await OwnsComponentDP.GetAllOwnsComponentsFromUser(user);
                Assert.NotNull(ownedComponents);
                Assert.NotEmpty(ownedComponents);

                int[] idsFromDB = ownedComponents.Select(x => Math.Abs(Utility.ComponentIDBias - x.ComponentID)).ToArray();
                int[] quantitiesFromDB = ownedComponents.Select(x => x.Quantity).ToArray();

                bool areIDsOk = idsFromDB.SequenceEqual(IDsArray);
                bool areQuantitiesOk = quantitiesFromDB.SequenceEqual(quantities);

                Assert.True(areIDsOk);
                Assert.True(areQuantitiesOk);

                foreach(OwnsComponent ownComp in ownedComponents)
                {
                    Console.WriteLine(ownComp.ToString());
                }
            }
            catch (Exception exception)
            {
                Assert.Fail(exception.Message);
            }
        }

        [Fact]
        public async Task Get_All_Suppliers()
        {
            try
            {
                Supplier[] suppliers = new Supplier[]
                {
                    new Supplier(id: 0, name: "DigiKey", website: "https://www.digikey.pl/", image: new byte[]{ }),
                    new Supplier(id: 1, name: "Botland", website: "https://botland.com.pl/", image: new byte[]{ }),
                    new Supplier(id: 2, name: "Mouser", website: "https://www.mouser.com/", image: new byte[]{ }),
                    new Supplier(id: 3, name: "Kamami", website: "https://kamami.pl/", image: new byte[]{ }),
                    new Supplier(id: 4, name: "M-Salamon", website: "https://msalamon.pl/", image: new byte[]{ }),
                    new Supplier(id: 5, name: "Allegro", website: "https://allegro.pl/", image: new byte[]{ }),
                    new Supplier(id: 6, name: "AliExpress", website: "https://pl.aliexpress.com/", image: new byte[]{ })
                };
                IEnumerable<Supplier> allSuppliers = await SupplierDP.GetAllSuppliers();
                Assert.NotEmpty(allSuppliers);
                Assert.NotNull(allSuppliers);

                foreach (Supplier supplierFromDB in allSuppliers)
                {
                    Console.WriteLine(supplierFromDB.ToString());
                    if (!suppliers.Any(x=>x.Name == supplierFromDB.Name && x.Website == supplierFromDB.Website))
                    {
                        Assert.Fail($"Supplier '{supplierFromDB.Name}' not found!");
                    }
                }
            }
            catch (Exception exception)
            {
                Assert.Fail(exception.Message);
            }
        }

        [Fact]
        public async Task Get_Year_Spendings()
        {
            try
            {
                User user = await UserDP.GetUserByUsername("jacek.jaworek");

                IEnumerable<double> spendings = await PurchaseDP.GetSpendingsForLastYearFromUser(user);
                Assert.NotEmpty(spendings);
                Assert.NotNull(spendings);

                foreach(double spending in spendings)
                {
                    Console.WriteLine(spending.ToString());
                }

            }
            catch (Exception exception)
            {
                Assert.Fail(exception.Message);
            }
        }
        #endregion
    }
}
