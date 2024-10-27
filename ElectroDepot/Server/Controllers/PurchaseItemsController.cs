﻿using ElectroDepotClassLibrary.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Context;
using Server.ExtensionMethods;
using Server.Models;

namespace Server.Controllers
{
    [Route("ElectroDepot/[controller]")]
    [ApiController]
    public class PurchaseItemsController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public PurchaseItemsController(DatabaseContext context)
        {
            _context = context;
        }
        #region Create
        /// <summary>
        /// POST: ElectroDepot/PurchaseItems/Create
        /// </summary>
        /// <param name="createPurchaseItemDTO"></param>
        /// <returns></returns>
        [HttpPost("Create")]
        public async Task<ActionResult<PurchaseItem>> CreatePurchaseItem(CreatePurchaseItemDTO createPurchaseItemDTO)
        {
            // Check if 'Purchase' and 'Component' exists
            Purchase? purchase = await _context.Purchases.FindAsync(createPurchaseItemDTO.PurchaseID);

            if (purchase == null)
            {
                return NotFound(new { title = "Not Found", status = 404, message = $"Purchase with ID:{createPurchaseItemDTO.PurchaseID} doesn't exist" });
            }

            Component? component = await _context.Components.FindAsync(createPurchaseItemDTO.ComponentID);

            if (purchase == null)
            {
                return NotFound(new { title = "Not Found", status = 404, message = $"Component with ID:{createPurchaseItemDTO.ComponentID} doesn't exist" });
            }

            PurchaseItem newPurchaseItem = createPurchaseItemDTO.ToPurchase();

            _context.PurchaseItems.Add(newPurchaseItem);
            await _context.SaveChangesAsync();

            return Ok(newPurchaseItem.ToPurchaseItemDTO());
        }
        #endregion
        #region Read
        /// <summary>
        /// GET: ElectroDepot/PurchaseItems/GetAll
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<PurchaseItemDTO>>> GetAllPurchaseItems()
        {
            return Ok(await _context.PurchaseItems.Select(x=>x.ToPurchaseItemDTO()).ToListAsync());
        }

        /// <summary>
        /// GET: ElectroDepot/PurchaseItems/GetByID/{id}
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetByID/{id}")]
        public async Task<ActionResult<IEnumerable<PurchaseItem>>> GetPurchaseItemByID(int id)
        {
            PurchaseItem? foundPurchase = await _context.PurchaseItems.FindAsync(id);

            if (foundPurchase == null)
            {
                return NotFound(new { title = "Not Found", code = "404", message = $"PurchaseItem with ID:{id} doesn't exsit" });
            }

            return Ok(foundPurchase);
        }
        #endregion
        #region Update
        /// <summary>
        /// POST: ElectroDepot/PurchaseItems/Update/{ID}
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updatePurchaseItemDTO"></param>
        /// <returns></returns>
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdatePurchase(int id, UpdatePurchaseItemDTO updatePurchaseItemDTO)
        {
            PurchaseItem? purchaseItem = await _context.PurchaseItems.FindAsync(id);

            if (purchaseItem == null)
            {
                NotFound();
            }

            purchaseItem.Quantity = updatePurchaseItemDTO.Quantity;
            purchaseItem.PricePerUnit = updatePurchaseItemDTO.PricePerUnit;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PurchaseItemExists(id))
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
        /// DELETE: ElectroDepot/PurchaseItems/Delete/{id}
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeletePurchaseItem(int id)
        {
            PurchaseItem? purchaseItem = await _context.PurchaseItems.FindAsync(id);
            if (purchaseItem == null)
            {
                return NotFound(new { title = "Not Found", code = "404", message = $"PurchasItem with ID:{id} doesn't exist" });
            }

            _context.PurchaseItems.Remove(purchaseItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        #endregion
        private bool PurchaseItemExists(int id)
        {
            return _context.PurchaseItems.Any(e => e.PurchaseItemID == id);
        }
    }
}
