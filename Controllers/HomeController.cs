using Dashboard.Data;
using Dashboard.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Diagnostics;

namespace Dashboard.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public HomeController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }


        [Authorize]
        public IActionResult Index()
        {
            //ÚÔÇä ÇÙåÑ ÇáßæÑíäÊ íæÒÑ 
            var username = HttpContext.User.Identity.Name??null;// get user data



            //CookieOptions cookie = new CookieOptions(); // storage info in cookie//create cookie
            //cookie.Expires = DateTime.Now.AddMinutes(50); //set time long
            //Response.Cookies.Append("userdata", username, cookie); //userdata take info username//store user data in my cookie

            HttpContext.Session.SetString("userdata",username);


            ViewBag.Username = username;    
           
            
            return View();
        }

        //functions for Products table
        public IActionResult AddNewitems()
        {
            //ViewBag.Username = Request.Cookies["userdata"];//get user data from my cookie


            ViewBag.Username = HttpContext.Session.GetString("userdata");

            var products = _context.products.ToList();//ÓØÑ ÚãáíÉ ÇáÞÑÇÁÉ íÌáÈ ÇáÈíÇäÇÊ Read 
            ViewBag.Products = products;
            return View(products);
        }

        public IActionResult CreateProducts(Products products)
        {

            if (ModelState.IsValid)
            {
                _context.products.Add(products); // Productsåæ ÇáßÇÆä ÍÞ ÇáßáÇÓ Product //ÊÎÒíä ÇáÈíÇäÇÊ Ýí ÇáßáÇÓ
                _context.SaveChanges(); //ÍÝÙ ÇáÈíÇäÇÊ Ýí ÇáÏÇÊÇÈíÓ
                TempData["Add"] = "ÊãÊ ÇáÅÖÇÝÉ ÈäÌÇÍ";
                return RedirectToAction("AddNewitems");//ÚÔÇä ÇÐÇ ÖÛØÊ Úáì ÇáÒÑ íÎáíäí ÇÊã ÈäÝÓ ÇáÕÝÍÉ
            }



            TempData["Add"] = "áã ÊÊã ÇáÅÖÇÝÉ íÑÌì ÇáÊÃßÏ ãä ÇáãÏÎáÇÊ";


            var product = _context.products.ToList();
            return View("AddNewitems", product);

            //_context.Add(products);
            // _context.SaveChanges();
            //return RedirectToAction("AddNewitems");
        }


        [HttpPost]
        public JsonResult Delete(int record)
        {
            // i will search on a record that's equal to p then productdel and save it on it
            var productdel = _context.products.SingleOrDefault(p => p.Id == record); //search on record
            // if productdel found delete it
            if (productdel != null)
            {
                _context.products.Remove(productdel);
                _context.SaveChanges();//save the new changes
                TempData["del"] = true;
            }
            else
            {
                TempData["del"] = false;
            }

            return Json("AddNewitems");
        }

        public IActionResult Update(Products Product)
        {
            if (ModelState.IsValid)
            {
                _context.products.Update(Product);
                _context.SaveChanges();
            }

            return RedirectToAction("AddNewitems");
        }

        //ãåãÊåÇ ÊÓÊÞÈá ÑÞã ÇáÓÌá æÊÑæÍ ÊÈÍË Úä ÇáÑÞã ÇáãØÇÈÞ
        public IActionResult Edit(int record)
        {
            var Product = _context.products.SingleOrDefault(x => x.Id == record);

            return View(Product);
        }


        //functions for Damage Products table
        public IActionResult DamageProducts()
        {
            ViewBag.Username = HttpContext.Session.GetString("userdata");

            var products = _context.products.ToList();  

            //join 3 tables
            var dmgproducts = _context.damagedProducts.Join(
                
                _context.products,

                dmg => dmg.ProductId,
                products=> products.Id,

                (dmg, products) => new
                {
                    dmg,
                    products

                }
                ).Join(
                  _context.productDetails,
                  joindata => joindata.dmg.ProductId,
                  other =>other.ProductId,

                  (joindata, other) => new
                  {
                      id = joindata.dmg.Id,
                      qty = joindata.dmg.Qty,   
                      name = joindata.products.Name,

                      color = other.Color
                  }

                ).ToList();


            ViewBag.products = products;
            ViewBag.damagedProducts = dmgproducts;
            return View();

        }

        //method to add the damage products

        public IActionResult CreateDmg(DamageProducts dmg)
        {
            _context.Add(dmg);
            _context.SaveChanges();

            return RedirectToAction("DamageProducts");
        }

        [HttpPost]
        public JsonResult DeleteDmg(int record)
        {
            // i will search on a record that's equal to p then productdel and save it on it
            var dmgdel = _context.damagedProducts.SingleOrDefault(p => p.Id == record); //search on record
            // if productdel found delete it
            if (dmgdel != null)
            {
                _context.damagedProducts.Remove(dmgdel);
                _context.SaveChanges();//save the new changes
                TempData["delDmg"] = true;
            }
            else
            {
                TempData["delDmg"] = false;
            }

            return Json("DamageProducts");
        }

        public IActionResult UpdateDmg(DamageProducts damagedProducts)
        {
            if (ModelState.IsValid)
            {
                _context.damagedProducts.Update(damagedProducts);
                _context.SaveChanges();
            }

            return RedirectToAction("DamageProducts");
        }

        //ãåãÊåÇ ÊÓÊÞÈá ÑÞã ÇáÓÌá æÊÑæÍ ÊÈÍË Úä ÇáÑÞã ÇáãØÇÈÞ
        public IActionResult EditDmg(int record)
        {
            var dmg = _context.damagedProducts.SingleOrDefault(x => x.Id == record);

            return View(dmg);
        }









        //functions for Product Details table
        public IActionResult ProductDetails()
        {
            ViewBag.Username = HttpContext.Session.GetString("userdata");

        

            var productDetails = _context.productDetails.Join(

                _context.products,

                prodetail => prodetail.ProductId,
                products => products.Id,

                (prodetail, products) => new
                {
					ProductId = prodetail.ProductId,

					id = prodetail.Id,
                    name = products.Name,
                    color = prodetail.Color,
                    price = prodetail.Price,
                    qty = prodetail.Qty,
                    img = prodetail.Images

                }
                ).ToList();

            ViewBag.ProductDetails = productDetails;

            ViewBag.products = _context.products.ToList();

            return View();  

        }


        public IActionResult CreateDetails(ProductDetails productDetails, IFormFile photo)
        {

            if (photo == null || photo.Length == 0)
            {
                //return a message if no file selected
                return Content("File Not Selected");
            }

            // Construct the file path where the photo will be saved
            var path = Path.Combine(_webHostEnvironment.WebRootPath, "img", photo.FileName);

            // Use FileStream to create the file at the specified path
            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                photo.CopyTo(stream); // Copy the content of the photo (IFormFile) to the stream
                stream.Close(); // Close the stream after copying is complete 
            }

            // Assign the file name (just the name, not the full path) to productDetails.Images
            productDetails.Images = photo.FileName;

            _context.Add(productDetails);
            _context.SaveChanges();

            return RedirectToAction("ProductDetails");
        }

		// GET: //Delete Operation for ProductDetails
		public JsonResult DeleteDetails(int id)
		{
			var detailsDel = _context.productDetails.SingleOrDefault(p => p.Id == id);

			if (detailsDel != null)
			{
				_context.productDetails.Remove(detailsDel);
				_context.SaveChanges();
				TempData["details"] = true;
			}
			else
			{
				TempData["details"] = false;
			}

			return Json("ProductDetails");


		}

        // GET: ProductDetails/EditDetails/
        [HttpPost]
		[HttpPost]
		public IActionResult UpdateDetails(ProductDetails productDetails, IFormFile photo)
		{
			if (productDetails == null)
			{
				return BadRequest("Invalid product details");
			}

			var existingProductDetails = _context.productDetails.FirstOrDefault(p => p.ProductId == productDetails.ProductId);

			if (existingProductDetails == null)
			{
				return NotFound("Product details not found");
			}

			// Update product details fields
			existingProductDetails.Price = productDetails.Price;
			existingProductDetails.Qty = productDetails.Qty;
			existingProductDetails.Color = productDetails.Color;

			if (photo != null && photo.Length > 0)
			{
				// Handle photo update
				var path = Path.Combine(_webHostEnvironment.WebRootPath, "img", photo.FileName);

				try
				{
					using (var stream = new FileStream(path, FileMode.Create))
					{
						photo.CopyTo(stream);
					}
					existingProductDetails.Images = photo.FileName;
				}
				catch (Exception ex)
				{
					// Log the exception
					_logger.LogError(ex, "Error saving file");
					TempData["ErrorMessage"] = "Error saving file.";
					return RedirectToAction("ProductDetails");
				}
			}

			try
			{
				_context.productDetails.Update(existingProductDetails);
				_context.SaveChanges();
				TempData["SuccessMessage"] = "Product details updated successfully";
			}
			catch (Exception ex)
			{
				// Log the exception
				_logger.LogError(ex, "Error updating product details");
				TempData["ErrorMessage"] = "Error updating product details.";
			}

			return RedirectToAction("ProductDetails");
		}


		// GET://Edit for Product details table
		public IActionResult EditDetails(int id)
		{
			var productDetails = _context.productDetails.SingleOrDefault(p => p.Id == id);

			if (productDetails == null)
			{
				return NotFound();
			}

			ViewBag.Products = _context.products.ToList(); // Load products for dropdown
			return View(productDetails);
		}



        public JsonResult GetData(int id)
        {
            var product = _context.products.FirstOrDefault(p => p.Id == id);

            if (product != null)
            {
                return Json(product);
            }
            else
            {
                return Json(null);
            }


        }


		public JsonResult GetDetails(int id)
		{
			var productDetails = _context.productDetails.FirstOrDefault(p => p.Id == id);

			if (productDetails != null)
			{
				return Json(productDetails);
			}
			else
			{
				return Json(null);
			}


		}










		public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
