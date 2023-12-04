using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projet.Data;
using Projet.Models;
using System.Security.Claims;

namespace Projet.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ShoppingCartController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ShoppingCart
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var shoppingCart = await _context.ShoppingCarts
                .Include(s => s.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(s => s.UserId == userId);

            if (shoppingCart == null && userId!=null)
            {
                return RedirectToAction(nameof(Create));
            }

            return View(shoppingCart);
        }

        // GET: ShoppingCart/Create
        public IActionResult Create()
        {
            
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var shoppingCart = new ShoppingCart
            {
                UserId = userId,
                User = _context.Users.FirstOrDefault(u => u.Id == userId),
                Items = new List<CartItem>()
            };
            _context.ShoppingCarts.Add(shoppingCart);
            _context.SaveChanges();

            return View(shoppingCart);
        }

        // POST: ShoppingCart/AddToCart/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToCart(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var shoppingCart = await _context.ShoppingCarts
                .Include(s => s.Items)
                .FirstOrDefaultAsync(s => s.UserId == userId);

            if (shoppingCart == null)
            {
                return RedirectToAction(nameof(Create));
            }

            var cartItem = shoppingCart.Items.FirstOrDefault(i => i.ProductId == id);

            if (cartItem != null)
            {
                cartItem.Quantity++;
            }
            else
            {
                cartItem = new CartItem
                {
                    ProductId = id,
                    Quantity = 1,
                    Product = product
                };
                shoppingCart.Items.Add(cartItem);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: ShoppingCart/RemoveFromCart/5
        public async Task<IActionResult> RemoveFromCart(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cartItem = await _context.CartItems.FindAsync(id);

            if (cartItem == null)
            {
                return NotFound();
            }

            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        // GET: ShoppingCart/DecreaseQuantity/5
        public async Task<IActionResult> DecreaseQuantity(int id)
        {
            var cartItem = await _context.CartItems.FindAsync(id);

            if (cartItem == null)
            {
                return NotFound();
            }

            if (cartItem.Quantity > 1)
            {
                cartItem.Quantity--;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: ShoppingCart/IncreaseQuantity/5
        public async Task<IActionResult> IncreaseQuantity(int id)
        {
            var cartItem = await _context.CartItems.FindAsync(id);

            if (cartItem == null)
            {
                return NotFound();
            }

            cartItem.Quantity++;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }
}
