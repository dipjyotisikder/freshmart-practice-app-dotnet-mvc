using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FreshMart.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FreshMart.Data;
using FreshMart.Models;
using FreshMart.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using FreshMart.Helper;
using Microsoft.AspNetCore.Hosting;

namespace FreshMart.Controllers
{
    [Authorize]
    public class SellersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _environment;

        public SellersController(ApplicationDbContext context, IHostingEnvironment env)
        {
            _context = context;
            _environment = env;
        }

        // GET: Sellers
        [Route("Sellers/{id}")]
        public IActionResult Index(int id)
        {
            var sellerChk = _context.Sellers.Find(id);

            if (User.Identity.Name != sellerChk.Email)
            {
                return NotFound();
            }

            var vm = new SellerViewModel
            {
                Seller = _context.Sellers.Find(id),
                Districts = _context.Districts.ToList(),
                Sellers = _context.Sellers.ToList()
            };

            return View(vm);
        }



        // GET: Sellers/Details/5
        [Route("Sellers/Details/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var sellerChk = _context.Sellers.Find(id);

            if (User.Identity.Name != sellerChk.Email)
            {
                return NotFound();
            }



            var seller = await _context.Sellers
                .Include(s => s.District)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (seller == null)
            {
                return NotFound();
            }

            return View(seller);
        }





        [HttpPost]
        [Route("Sellers/Update/{id}")]
        public IActionResult Update(int? id, SellerViewModel model)
        {
            if (id == null)
            {
                return NotFound();
            }
            var sellerChk = _context.Sellers.Find(id);

            if (User.Identity.Name != sellerChk.Email)
            {
                return NotFound();
            }


            if (id != model.Seller.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    sellerChk = model.Seller;
                    //_context.Sellers.Update(model.Seller);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SellerExists(model.Seller.Id))
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

            ViewData["DistrictId"] = new SelectList(_context.Districts, "Id", "Division", model.Seller.DistrictId);

            return View(model.Seller);
        }



        // GET: Sellers/Create
        [Route("Sellers/Create")]
        public IActionResult Create()
        {
            var vm = new SellerViewModel
            {
                Districts = _context.Districts.ToList(),
                Sellers = _context.Sellers.ToList()
            };


            ViewData["DistrictId"] = new SelectList(_context.Districts, "Id", "Division");
            return View(vm);
        }

        [Authorize]
        [HttpPost]
        [Route("Sellers/request")]
        public async Task<ActionResult> SellerRequest(SellerViewModel model)
        {
            var emailCheck = _context.SellerRequests.Where(c => c.Email == User.Identity.Name);
            if (emailCheck.ToList().Count > 0)
            {

                ViewBag.err = "You have already requested. You will approved soon!";
                var vm = new SellerViewModel
                {
                    Districts = _context.Districts.ToList(),
                    Sellers = _context.Sellers.ToList(),
                    Error = ViewBag.err
                };
                return View("Create", vm);
            }

            if (model.SellerRequest.SellerName == null)
            {
                ModelState.AddModelError("SellerRequest.SellerName", "Please enter seller name");
            }

            if (model.SellerRequest.DateOfBirth == null)
            {
                ModelState.AddModelError("SellerRequest.DateOfBirth", "Please enter date of birth");
            }


            if (model.SellerRequest.SellerName == null || model.SellerRequest.DateOfBirth == null)
            {
                var vm = new SellerViewModel
                {
                    Districts = _context.Districts.ToList(),
                    Sellers = _context.Sellers.ToList(),
                    Error = "You can not ignore required fields"
                };
                return View("Create", vm);
            }
            else
            {
                var vm = new SellerRequest
                {
                    SellerName = model.SellerRequest.SellerName,
                    Email = User.Identity.Name,
                    Phone = model.SellerRequest.Phone,
                    DistrictId = model.SellerRequest.DistrictId,
                    DateOfBirth = model.SellerRequest.DateOfBirth,
                    CompanyName = model.SellerRequest.CompanyName
                };

                _context.SellerRequests.Add(vm);
                await _context.SaveChangesAsync();
            }
            var vms = new SellerViewModel
            {
                Districts = _context.Districts.ToList(),
                Sellers = _context.Sellers.ToList(),
                Error = "Your request has been sent! Wait for approval."
            };
            return RedirectToAction("Create", "Sellers", vms);
        }



        [Route("Seller/SellProduct/{id}")]
        public ActionResult SellProduct(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var sellerChk = _context.Sellers.Find(id);

            if (User.Identity.Name != sellerChk.Email)
            {
                return NotFound();
            }

            var data = _context.Sellers.Where(c => c.Email == User.Identity.Name);

            if (id == null || data.SingleOrDefault() == null)
            {
                NotFound();
            }
            var vm = new SellerViewModel
            {
                Products = _context.Products.Where(c => c.SellerId == id).ToList(),
                Districts = _context.Districts.ToList(),
                Categories = _context.Categories.ToList(),
                Sellers = _context.Sellers.ToList()
            };
            return View(vm);
        }

        [HttpPost]
        [Route("Seller/SellProduct/{id}")]
        public ActionResult SellProduct(int id, IFormFile file, SellerViewModel vm)
        {
            if (id == 0)
            {
                return NotFound();
            }
            var sellerChk = _context.Sellers.Find(id);

            if (User.Identity.Name != sellerChk.Email)
            {
                return NotFound();
            }

            var data = _context.Sellers.Where(c => c.Email == User.Identity.Name);
            if (data.SingleOrDefault() == null)
            {
                NotFound();
            }

            if (vm.Product.Title == null)
            {
                ModelState.AddModelError("Product.Title", "Please enter product name");
            }
            if (vm.Product.Price == 0)
            {
                ModelState.AddModelError("Product.Price", "Please enter price");
            }
            if (vm.Product.ItemInStock == 0)
            {
                ModelState.AddModelError("Product.ItemInStock", "Please enter itemInStock");
            }
            if (vm.Product.Unit == null)
            {
                ModelState.AddModelError("Product.Unit", "Please enter unit");
            }

            if (vm.Product.Title == null
                || vm.Product.Price == 0
                || vm.Product.ItemInStock == 0
                || vm.Product.Unit == null)
            {
                var vmfinal = new SellerViewModel
                {
                    Products = _context.Products.Where(c => c.SellerId == id).ToList(),
                    Districts = _context.Districts.ToList(),
                    Categories = _context.Categories.ToList(),
                    Sellers = _context.Sellers.ToList()
                };
                return View(vmfinal);
            }


            var Seller = _context.Sellers.Where(s => s.Email.Contains(User.Identity.Name));
            if (Seller.SingleOrDefault() == null)
            {
                return RedirectToAction("request", "Products", new { id = id });
            }


            ImgUploader img = new ImgUploader(_environment);

            var imgPath = img.ImageUrl(file);   //function working here
            if (imgPath == null)
            {
                TempData["uploaderr"] = "May be Image is not perfect!";
                return RedirectToAction("SellProduct", "Sellers", new { id = Seller.Single().Id });
            }


            var products = new Product
            {
                Title = vm.Product.Title,
                Description = vm.Product.Description,
                Price = vm.Product.Price,
                SellerId = Seller.Single().Id,
                CategoryId = vm.Product.CategoryId,
                DistrictId = vm.Product.DistrictId,
                IsPublished = vm.Product.IsPublished,  //manually values
                Unit = vm.Product.Unit,
                ItemInStock = vm.Product.ItemInStock,
                CreatedAt = DateTime.Now,              //manually valuess
                UpdatedAt = DateTime.Now,              //manually valuess
                OfferExpireDate = DateTime.Now,        //manually valuess
                ImagePath = imgPath,
                OfferPrice = 1     //need to change
            };

            _context.Products.Add(products);
            _context.SaveChanges();

            var vms = new SellerViewModel
            {
                Products = _context.Products.Where(c => c.SellerId == id).ToList(),
                Districts = _context.Districts.ToList(),
                Categories = _context.Categories.ToList(),
                Sellers = _context.Sellers.ToList(),
                Error = "Your Product has been added"
            };
            return View(vms);
        }


        [Route("Sellers/SellerProducts/{id}")]
        public ActionResult SellerProducts(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }
            var sellerChk = _context.Sellers.Find(id);

            if (User.Identity.Name != sellerChk.Email)
            {
                return NotFound();
            }

            var prosList = _context.Products.Where(c => c.SellerId == id).ToList();
            var vm = new SellerViewModel
            {
                Products = prosList,
                Sellers = _context.Sellers.ToList(),
                Districts = _context.Districts.ToList(),
                Categories = _context.Categories.ToList()
            };

            return View(vm);
        }




        // GET: Sellers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var sellerChk = _context.Sellers.Find(id);

            if (User.Identity.Name != sellerChk.Email)
            {
                return NotFound();
            }


            var seller = await _context.Sellers
                .Include(s => s.District)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (seller == null)
            {
                return NotFound();
            }

            return View(seller);
        }




        // POST: Sellers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var sellerChk = _context.Sellers.Find(id);

            if (User.Identity.Name != sellerChk.Email)
            {
                return NotFound();
            }

            var seller = await _context.Sellers.SingleOrDefaultAsync(m => m.Id == id);
            _context.Sellers.Remove(seller);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SellerExists(int id)
        {
            return _context.Sellers.Any(e => e.Id == id);
        }
    }
}
