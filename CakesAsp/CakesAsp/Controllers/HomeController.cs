using CakesAsp.Areas.Admin.Data;
using CakesAsp.Areas.Admin.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CakesAsp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CakesAspContext _context;

        public HomeController(ILogger<HomeController> logger, CakesAspContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.Keys.Contains("Username"))
            {
                ViewBag.Username = HttpContext.Session.GetString("Username");
            }
            var product = _context.Products.ToList();
            return View(product);
           
        }
        public IActionResult Products()
        {
            var product = _context.Products.ToList();
            return View(product);
        }
        public IActionResult Cart()
        {
            if (HttpContext.Session.Keys.Contains("Username"))
            {
                ViewBag.Username = HttpContext.Session.GetString("Username");
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
            ViewBag.Username = HttpContext.Session.GetString("Username");
            string username = HttpContext.Session.GetString("Username");
            var listCart = _context.Carts.Include(c => c.Account).Include(c => c.Product).Where(c => c.Account.Username == username);

           

            return View(listCart.ToList());
        }
        public async Task<IActionResult> Product_detail(int id)
        {
            ViewBag.Username = HttpContext.Session.GetString("Username");
            var getInf = _context.Products.Where(prd => prd.Id == id);
            return View(await getInf.ToListAsync());
        }
        public IActionResult About()
        {
            return View();
        }
        public IActionResult Blog()
        {
            return View();
        }
        public IActionResult Checkout()
        {
            var c = _context.Carts.Include(a=>a.Product).Include(b=>b.Account).ToList();
            return View(c);
        }
        public IActionResult Contact()
        {
            return View();
        }


        public IActionResult Privacy()
        {
            return View();
        }

       
       
        public IActionResult AddToCart(int id)
        {
            return AddToCart(id, 1);
        }
        [HttpPost]
        public IActionResult AddToCart(int productId, int quantity)
        {

            string username = HttpContext.Session.GetString("Username");
            int accountId = _context.Accounts.FirstOrDefault(a => a.Username == username).Id;
            Cart cart = _context.Carts.FirstOrDefault(c => c.AccountId == accountId && c.ProductId == productId);
            if (cart == null)
            {
                cart = new Cart();
                cart.AccountId = accountId;
                cart.ProductId = productId;
                cart.Quantity = quantity;

                _context.Carts.Add(cart);
            }
            else
            {
                cart.Quantity += quantity;

            }
            _context.SaveChanges();
            return RedirectToAction("Cart","Home");
        }
        public async Task<IActionResult> DeleteSp(int id)
        {
            var cart = await _context.Carts.FindAsync(id);
            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();
            return RedirectToAction("Cart","Home");
        }
      
 
        public async Task<IActionResult> PayCart()
        {
            char[] MangKyTu = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'V', 'W', 'Y', 'Z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

            Random fr = new Random();
            string chuoi = "";
            for (int i = 0; i < 6; i++)
            {
                int t = fr.Next(0, MangKyTu.Length);
                chuoi = chuoi + MangKyTu[t];
            }

            string username = HttpContext.Session.GetString("Username");
            int accountId = _context.Carts.FirstOrDefault(ca => ca.Account.Username == username).AccountId;
            List<Cart> cart = _context.Carts.Where(c => c.AccountId == accountId).ToList();
            int total = _context.Carts.Where(ca => ca.Account.Username == username).Sum(c => c.Quantity * c.Product.Price);

        
            Invoice invoices = new Invoice();
            DateTime now = DateTime.Now;
            invoices.AccountId = accountId;
            invoices.Code = chuoi;
            invoices.OrderDate = now;
            invoices.ShippingAddress = _context.Accounts.FirstOrDefault(ca => ca.Username == username).Address;
            invoices.ShippingPhone = _context.Accounts.FirstOrDefault(ca => ca.Username == username).Phone;
            invoices.ShippingName = _context.Accounts.FirstOrDefault(ca => ca.Username == username).FullName;
            invoices.Total = total;
            invoices.Status = true;
            _context.Invoices.Add(invoices);
            _context.SaveChanges();

            foreach (var item in cart)
            {
                InvoiceDetail invoiceDetails = new InvoiceDetail();
                invoiceDetails.InvoiceId = invoices.Id;
                invoiceDetails.ProductId = item.ProductId;
                invoiceDetails.Quantity = item.Quantity;
                invoiceDetails.Total = _context.Products.FirstOrDefault(pr => pr.Id == item.ProductId).Price * item.Quantity;
                _context.InvoiceDetails.Add(invoiceDetails);
            }
            _context.SaveChanges();

            foreach (var item in cart)
            {
                Cart carts = _context.Carts.Find(item.Id);
                _context.Carts.Remove(carts);
            }
            _context.SaveChanges();
            return RedirectToAction("Cart", "Home");
        }
        public async Task<IActionResult> Login(string Username, string Password)
        {
            Account account = _context.Accounts.Where(a => a.Username == Username && a.Password == Password).FirstOrDefault();
            if (account != null)
            {
                CookieOptions cookieOption = new CookieOptions()
                {
                    Expires = DateTime.Now.AddDays(7)
                };
                //HttpContext.Response.Cookies.Append("Account_ID", account.Account_ID.ToString());
                //HttpContext.Response.Cookies.Append("Username", account.Username.ToString());
                HttpContext.Session.SetInt32("Account_ID", account.Id);
                HttpContext.Session.SetString("Username", account.Username);
                if(account.Type=="1")
                {
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
                
            }
            else
            {
                ViewBag.ErrorMessage = "Đăng nhập thất bại";
                return View();
            }
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("Username");
            return View("Login");
        }

    }
}
