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

namespace catstore.Controllers
{
    
    public class CatalogoController : Controller
    {
        private readonly ILogger<CatalogoController> _logger;
        private readonly ApplicationDbContext _context;
        public CatalogoController(ILogger<CatalogoController> logger,ApplicationDbContext context)
        {
            _logger = logger;
             _context = context;
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
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}