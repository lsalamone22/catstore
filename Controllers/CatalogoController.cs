using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using catstore.Models;
using catstore.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;


namespace catstore.Controllers
{
    
    public class CatalogoController : Controller
    {
        private readonly ILogger<CatalogoController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public CatalogoController(ApplicationDbContext context,
                ILogger<CatalogoController> logger,
                UserManager<IdentityUser> userManager)
        {
            _logger = logger;
             _context = context;
             _userManager= userManager;
        }

         public async Task<IActionResult> IndexAsync(string? searchString)
        {
            var productos = from o in _context.DataProductos select o;
            if(!String.IsNullOrEmpty(searchString)){
                productos = productos.Where(s => s.Name.Contains(searchString));
            }

            return View(await productos.ToListAsync());
            
        }

        public async Task<IActionResult> Details(int? id)
        {
                Producto objProducto = await _context.DataProductos.FindAsync(id);
                if (objProducto == null){
                    return NotFound();
                }

                return View(objProducto);

        }

         public async Task<IActionResult> Add(int? id){
            var userID = _userManager.GetUserName(User); //sesion
            if(userID == null){
                ViewData["Message"] = "Por favor debe loguearse antes de agregar un producto";
                List<Producto> productos = new List<Producto>();
                return  View("Index",productos);
            }else{
                var producto = await _context.DataProductos.FindAsync(id);

                PRO proforma = new PRO();
                proforma.Producto = producto;
                proforma.Precio = producto.Precio; //precio del producto en ese momento
                proforma.Cantidad = 1;
                proforma.userID = userID;
                _context.Add(proforma);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }

        
    }
}