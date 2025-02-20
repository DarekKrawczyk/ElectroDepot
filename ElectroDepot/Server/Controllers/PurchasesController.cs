using ElectroDepotClassLibrary.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Context;
using Server.ExtensionMethods;
using Server.Models;

namespace Server.Controllers
{
    [Route("ElectroDepot/[controller]")]
    [ApiController]
    public class PurchasesController : CustomControllerBase
    {
        public PurchasesController(DatabaseContext context) : base(context)
        {
        }
        #region Create
        /// <summary>
        /// POST: ElectroDepot/Purchases/Create
        /// </summary>
        /// <param name="createPurchaseDTO"></param>
        /// <returns></returns>
        [HttpPost("Create")]
        public async Task<ActionResult<PurchaseDTO>> CreatePurchase(CreatePurchaseDTO createPurchaseDTO)
        {
            User? userExists = await _context.Users.FindAsync(createPurchaseDTO.UserID);
            if (userExists == null)
            {
                return NotFound(new { title = "User not found", status = 404, message = $"User with given id: '{createPurchaseDTO.UserID}' doesn't exist" });
            }

            Supplier? supplierExists = await _context.Suppliers.FindAsync(createPurchaseDTO.SupplierID);
            if (supplierExists == null)
            {
                return NotFound(new { title = "Supplier not found", status = 404, message = $"Supplier with given id: '{createPurchaseDTO.SupplierID}' doesn't exist" });
            }

            Purchase purchase = createPurchaseDTO.ToPurchase();

            _context.Purchases.Add(purchase);
            await _context.SaveChangesAsync();

            return Ok(purchase.ToPurchaseDTO());
        }
        #endregion
        #region Read
        /// <summary>
        /// GET: ElectroDepot/Purchases/GetAll
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<PurchaseDTO>>> GetAllPurchases()
        {
            return Ok(await _context.Purchases.Select(x => x.ToPurchaseDTO()).ToListAsync());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetByID/{ID}")]
        public async Task<ActionResult<PurchaseDTO>> GetPurchaseByID(int ID)
        {
            Purchase? purchase = await _context.Purchases.FindAsync(ID);

            if (purchase == null)
            {
                return NotFound(new { title = "Not found", code = "404" });
            }

            return Ok(purchase.ToPurchaseDTO());
        }

        /// <summary>
        /// List of 12 elements. Last element is money spend this month, first element is money spend 11 months ago
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpGet("GetSpendingsForLastYearFromUser/{ID}")]
        public async Task<ActionResult<IEnumerable<double>>> GetSpendingsForLastYearFromUser(int ID)
        {
            User foundUser = await _context.Users.FindAsync(ID);

            if (foundUser == null)
            {
                return NotFound();
            }

            IEnumerable<Purchase> purchases = await (from purchase in _context.Purchases
                                                     join users in _context.Users
                                                     on purchase.UserID equals users.UserID
                                                     where users.UserID == foundUser.UserID &&
                                                     purchase.PurchasedDate >= DateTime.Now.AddYears(-1)
                                                     select new Purchase()
                                                     {
                                                         PurchaseID = purchase.PurchaseID,
                                                         User = purchase.User,
                                                         UserID = purchase.UserID,
                                                         Supplier = purchase.Supplier,
                                                         SupplierID = purchase.SupplierID,
                                                         PurchasedDate = purchase.PurchasedDate,
                                                         TotalPrice = purchase.TotalPrice,
                                                     }).ToListAsync();

            IGrouping<DateTime, Purchase>[] groupedByMonth = purchases.GroupBy(x => x.PurchasedDate).ToArray();

            IEnumerable<(DateTime, double)> prices = groupedByMonth.Select(x => (x.Key, x.Sum(y => y.TotalPrice)));

            DateTime now = DateTime.Now;
            DateTime startMonth = new DateTime(now.Year, now.Month, 1).AddMonths(-11);

            // Create a dictionary from the provided prices for quick lookup
            var priceDict = prices
                .GroupBy(p => new DateTime(p.Item1.Year, p.Item1.Month, 1)) // Group by month
                .ToDictionary(g => g.Key, g => g.Sum(x => x.Item2)); // Sum values if multiple entries exist in the same month

            // Create a list to hold the filled results
            var result = new List<(DateTime, double)>();

            // Iterate from startMonth to the current month
            for (int i = 0; i < 12; i++)
            {
                DateTime month = startMonth.AddMonths(i);
                if (priceDict.TryGetValue(month, out double value))
                {
                    result.Add((month, value));
                }
                else
                {
                    // If the month is missing, add a dummy value
                    result.Add((month, 0.0));
                }
            }

            IEnumerable<double> lastYearSpendings = result.Select(x => x.Item2);

            return Ok(lastYearSpendings);
        }

        /// <summary>
        /// GET: ElectroDepot/Purchases/GetAll/{ID}
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet("GetPurchaseItemByID/{ID}")]
        public async Task<ActionResult<IEnumerable<PurchaseItemDTO>>> GetPurchaseItem(int ID)
        {
            Purchase? foundPurchase = await _context.Purchases.FindAsync(ID);

            if (foundPurchase == null)
            {
                return NotFound();
            }

            List<PurchaseItemDTO> foundPurchaseItems = await _context.PurchaseItems.Where(x => x.PurchaseID == ID).Select(x=>x.ToPurchaseItemDTO()).ToListAsync();

            return foundPurchaseItems;
        }

        /// <summary>
        /// GET: ElectroDepot/Purchases/GetAllByUserID/{ID}
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet("GetAllByUserID/{ID}")]
        public async Task<ActionResult<IEnumerable<PurchaseDTO>>> GetAllByUserID(int ID)
        {
            User? foundUser = await _context.Users.FindAsync(ID);

            if (foundUser == null)
            {
                return NotFound();
            }

            IEnumerable<PurchaseDTO> purchasesOfUser = await _context.Purchases.Where(x => x.UserID == ID).Select(x => x.ToPurchaseDTO()).ToListAsync();

            return Ok(purchasesOfUser);
        }

        /// <summary>
        /// GET: ElectroDepot/Purchases/GetAllBySupplierID/{ID}
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetAllBySupplierID/{ID}")]
        public async Task<ActionResult<IEnumerable<PurchaseDTO>>> GetAllBySupplierID(int ID)
        {
            Supplier? foundSupplier = await _context.Suppliers.FindAsync(ID);

            if (foundSupplier == null)
            {
                return NotFound(new { title = "Not Found", code = "404", message = $"Supplier with ID:{ID} doesn't exsit" });
            }

            IEnumerable<PurchaseDTO> purchasesOfUser = await _context.Purchases.Where(x => x.SupplierID == ID).Select(x => x.ToPurchaseDTO()).ToListAsync();

            return Ok(purchasesOfUser);
        }
        #endregion
        #region Update
        /// <summary>
        /// POST: ElectroDepot/Purchases/Update/{ID}
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="updatePurchaseDTO"></param>
        /// <returns></returns>
        [HttpPut("Update/{ID}")]
        public async Task<IActionResult> UpdatePurchase(int ID, UpdatePurchaseDTO updatePurchaseDTO)
        {
            Purchase? purchase = await _context.Purchases.FindAsync(ID);

            if(purchase == null)
            {
                NotFound();
            }

            purchase.TotalPrice = updatePurchaseDTO.TotalPrice;
            purchase.PurchasedDate = updatePurchaseDTO.PurchaseDate;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PurchaseExists(ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok();
        }
        #endregion
        #region Delete
        /// <summary>
        /// DELETE: ElectroDepot/Purchases/Delete/{ID}
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpDelete("Delete/{ID}")]
        public async Task<IActionResult> DeletePurchase(int ID)
        {
            Purchase? purchase = await _context.Purchases.FindAsync(ID);
            if (purchase == null)
            {
                return NotFound(new { title = "Not Found", code = "404" });
            }

            _context.Purchases.Remove(purchase);
            await _context.SaveChangesAsync();

            return Ok();
        }
        #endregion
        private bool PurchaseExists(int id)
        {
            return _context.Purchases.Any(e => e.PurchaseID == id);
        }
    }
}
