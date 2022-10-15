using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using catstore.Data;
using Microsoft.EntityFrameworkCore;
using System.Dynamic;
using catstore.Models;
//using catstore.DTO;

/// <summary>
/// Aka Carrito
/// </summary>
namespace catstore.Controllers
{

    public class ProformaController : Controller
    {
        private readonly ILogger<ProformaController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager; 


        public ProformaController(ApplicationDbContext context,
            ILogger<ProformaController> logger,
            UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var userIDSession = _userManager.GetUserName(User);
            //SELECT * FROM Proforma p,Producto pr WHERE p.productId=pr.Id And p.UserId=? And p.status='PENDIENTE' 

            var items = from o in _context.DataPRO select o;
            items = items.Include(p => p.Producto).
                    Where(w => w.userID.Equals(userIDSession) &&
                     w.Status.Equals("PENDIENTE"));
            var itemsCarrito = items.ToList();
            //Fila1 1234, Shampo; Precio, Cantidad
            //Fila2 12345, Shampo3; Precio, Cantidad
            var total = itemsCarrito.Sum(c => c.Cantidad * c.Precio);

            //MEMORIA
            dynamic model = new ExpandoObject();
            model.montoTotal = total;
            model.elementosCarrito = itemsCarrito;

            //Carrito carrito = new Carrito();
            //carrito.total = total;
            //carrito.itemsCarrito = itemsCarrito;

            //return View(carrito);
            return View(model);
        }

       public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proforma = await _context.DataPRO.FindAsync(id);
            if (proforma == null)
            {
                return NotFound();
            }
            return View(proforma);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,Cantidad,Precio,userID")] PRO proforma)
        {
            if (id != proforma.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(proforma);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.DataPRO.Any(e => e.id == id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(proforma);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proforma = await _context.DataPRO.FindAsync(id);
            _context.DataPRO.Remove(proforma);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}