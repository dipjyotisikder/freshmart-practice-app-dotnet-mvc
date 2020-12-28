using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FreshMart.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using FreshMart.Helper;
using Microsoft.AspNetCore.Hosting;
using FreshMart.Database;
using FreshMart.ViewModels;
using FreshMart.Models.Commands;
using MediatR;
using FreshMart.Core;
using FreshMart.ViewModels;

namespace FreshMart.Controllers
{
    [Authorize]
    public class SellersController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IHostingEnvironment _environment;
        private readonly IMediator _mediator;


        public SellersController(AppDbContext context,
            IHostingEnvironment env,
            IMediator mediator)
        {
            _context = context;
            _environment = env;
            _mediator = mediator;
        }

        // GET: Sellers
        [Route("Sellers/{id}")]
        public ViewResult Index(long id, SellerViewModel model)
        {
            var vm = new SellerViewModel
            {
                Seller = model.Seller == null ? _context.Sellers
                .Where(x => x.Id == id)
                .Include(x => x.User).ThenInclude(x => x.District)
                .AsNoTracking().FirstOrDefault() : model.Seller,
                Districts = model.Districts == null || model.Districts.Count == 0 ? _context.Districts.AsNoTracking().ToList() : model.Districts,
                Sellers = model.Sellers == null || model.Sellers.Count == 0 ? _context.Sellers.AsNoTracking().ToList() : model.Sellers
            };
            ViewBag.success = model.Success;
            model.Success = "";
            return View(vm);
        }



        // GET: Sellers/Details/5
        [Route("Sellers/Details/{id}")]
        public async Task<IActionResult> Details(long? id)
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
                .Include(s => s.User).ThenInclude(x => x.District).AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);

            if (seller == null)
            {
                return NotFound();
            }

            return View(seller);
        }



        [HttpPost]
        [Route("Sellers/Update/{id}")]
        public async Task<IActionResult> UpdateProfile([FromRoute] int? id, [FromBody] UpdateSellerProfileCommand command)
        {
            command.Id = id ?? command.Id;
            var result = await _mediator.Send(command);
            return Ok(result);
        }



        // GET: Sellers/Create
        [Route("Sellers/Create")]
        public IActionResult Create()
        {
            var sellerChk = _context.Sellers.Where(x => x.Email == User.Identity.Name).AsNoTracking().FirstOrDefault();
            if (sellerChk != null)
            {
                return RedirectToAction("Index", "Sellers", new { id = sellerChk.Id });
            }

            var vm = new SellerViewModel
            {
                Districts = _context.Districts.AsNoTracking().ToList(),
                Sellers = _context.Sellers.AsNoTracking().ToList(),
                Seller = sellerChk
            };

            ViewData["DistrictId"] = new SelectList(_context.Districts, "Id", "Division");
            return View(vm);
        }


        [Authorize]
        [HttpPost]
        [Route("Sellers/request")]
        public async Task<ActionResult> SellerRequest(SellerViewModel model)
        {
            var vm = new SellerViewModel
            {
                Districts = _context.Districts.AsNoTracking().ToList(),
                Sellers = _context.Sellers.AsNoTracking().ToList(),
                Error = ""
            };

            var emailCheck = _context.SellerRequests.Where(c => c.Email == User.Identity.Name);
            if (emailCheck.ToList().Count > 0)
            {
                vm.Error = "You have already requested. You will approved soon!";
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
                vm.Error = "You can not ignore required fields";
                return View("Create", vm);
            }
            else
            {
                var sellerRequest = new SellerRequest
                {
                    SellerName = model.SellerRequest.SellerName,
                    Email = User.Identity.Name,
                    Phone = model.SellerRequest.Phone,
                    //DistrictId = model.SellerRequest.DistrictId,
                    DateOfBirth = model.SellerRequest.DateOfBirth,
                    CompanyName = model.SellerRequest.CompanyName
                };
                await _context.SellerRequests.AddAsync(sellerRequest);
                await _context.SaveChangesAsync();
            }

            vm.Error = "Your request has been sent! Wait for approval.";
            return View("Create", vm);
        }


        [Route("Seller/CreateProduct/{id}")]
        public async Task<ActionResult> CreateProduct(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sellerChk = await _context.Sellers.FindAsync(id);
            if (User.Identity.Name != sellerChk.Email)
            {
                return NotFound();
            }

            var data = await _context.Sellers.Where(c => c.Email == User.Identity.Name).AsNoTracking().SingleOrDefaultAsync();
            if (id == null || data == null)
            {
                NotFound();
            }

            var model = new SellerViewModel
            {
                Products = await _context.Products.Where(c => c.SellerId == id).AsNoTracking().ToListAsync(),
                Districts = await _context.Districts.AsNoTracking().ToListAsync(),
                Categories = await _context.Categories.AsNoTracking().ToListAsync(),
                Sellers = await _context.Sellers.AsNoTracking().ToListAsync(),
                Seller = await _context.Sellers.Where(x => x.Id == id).AsNoTracking().FirstOrDefaultAsync(),

                CreateProductViewModel = new CreateProductViewModel
                {
                    Description = "",
                    Title = "",
                    Price = 0,
                    ItemInStock = 0,
                    Unit = "",
                    OfferPrice = 0,
                    CategoryId = 0
                }
            };
            return View(model);
        }

        [HttpPost]
        [Route("Seller/CreateProduct/{id}")]
        public async Task<ActionResult> CreateProduct(long id, IFormFile file, SellerViewModel request)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var sellerChk = await _context.Sellers.FindAsync(id);
            if (User.Identity.Name != sellerChk.Email)
            {
                return NotFound();
            }

            var data = await _context.Sellers.Where(c => c.Email == User.Identity.Name).FirstOrDefaultAsync();
            if (data == null)
            {
                NotFound();
            }

            if (request.CreateProductViewModel.Title == null)
            {
                ModelState.AddModelError("CreateProductViewModel.Title", "Please enter product name");
            }

            if (request.CreateProductViewModel.Price == 0)
            {
                ModelState.AddModelError("CreateProductViewModel.Price", "Please enter price");
            }

            if (request.CreateProductViewModel.ItemInStock == 0)
            {
                ModelState.AddModelError("CreateProductViewModel.ItemInStock", "Please enter itemInStock");
            }

            if (request.CreateProductViewModel.Unit == null)
            {
                ModelState.AddModelError("CreateProductViewModel.Unit", "Please enter unit");
            }

            if (request.CreateProductViewModel.CategoryId == 0 || request.CreateProductViewModel.CategoryId == null)
            {
                ModelState.AddModelError("CreateProductViewModel.CategoryId", "Please enter category");
            }

            //MODEL WILL BE INVALID IF
            if (request.CreateProductViewModel.Title == null || request.CreateProductViewModel.Price == 0 || request.CreateProductViewModel.ItemInStock == 0 || request.CreateProductViewModel.Unit == null || request.CreateProductViewModel.CategoryId == 0 || request.CreateProductViewModel.CategoryId == null)
            {
                var vmfinal = new SellerViewModel
                {
                    Products = _context.Products.Where(c => c.SellerId == id).AsNoTracking().ToList(),
                    Districts = _context.Districts.AsNoTracking().ToList(),
                    Categories = _context.Categories.AsNoTracking().ToList(),
                    Sellers = _context.Sellers.AsNoTracking().ToList(),
                    Seller = _context.Sellers.Where(x => x.Id == id).AsNoTracking().FirstOrDefault(),
                    Product = request.Product,
                    CreateProductViewModel = request.CreateProductViewModel
                };

                return View(vmfinal);
            }


            var seller = await _context.Sellers.Where(s => s.Email.Contains(User.Identity.Name)).Include(x => x.User).FirstOrDefaultAsync();
            if (seller == null) { return RedirectToAction("request", "Products", new { id = id }); }


            //IMAGE UPLOAD
            ImgUploader img = new ImgUploader(_environment);
            var imgPath = img.ImageUrl(file);
            if (imgPath == null)
            {
                TempData["uploaderr"] = "May be Image is not perfect!";
                return RedirectToAction("CreateProduct", "Sellers", new { id = seller.Id });
            }


            //CREATE PRODUCT MODEL
            var products = new Product
            {
                Id = NumberUtilities.GetUniqueNumber(),
                Title = request.CreateProductViewModel.Title,
                Description = request.CreateProductViewModel.Description,
                Price = request.CreateProductViewModel.Price ?? 0,
                SellerId = seller.Id,
                CategoryId = request.CreateProductViewModel.CategoryId ?? 0,
                DistrictId = seller.User.DistrictId,
                IsPublished = true,
                Unit = request.CreateProductViewModel.Unit,
                ItemInStock = request.CreateProductViewModel.ItemInStock ?? 0,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                OfferExpireDate = DateTime.UtcNow.AddDays(100),
                ImagePath = imgPath,
                OfferPrice = request.CreateProductViewModel.OfferPrice ?? 0     //need to change
            };
            await _context.Products.AddAsync(products);
            await _context.SaveChangesAsync();


            var vms = new SellerViewModel
            {
                Products = await _context.Products.Where(c => c.SellerId == id).ToListAsync(),
                Districts = await _context.Districts.ToListAsync(),
                Categories = await _context.Categories.ToListAsync(),
                Sellers = await _context.Sellers.ToListAsync(),
                Error = "Your Product has been added"
            };


            return View(vms);
        }


        [Route("Sellers/SellerProducts/{id}")]
        public ActionResult SellerProducts(long? id)
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

            var prosList = _context.Products.Where(c => c.SellerId == id).AsNoTracking().ToList();
            var vm = new SellerViewModel
            {
                Products = prosList,
                Sellers = _context.Sellers.AsNoTracking().ToList(),
                Districts = _context.Districts.AsNoTracking().ToList(),
                Categories = _context.Categories.AsNoTracking().ToList(),
                Seller = _context.Sellers.Where(x => x.Id == id).AsNoTracking().FirstOrDefault()
            };

            return View(vm);
        }




        // GET: Sellers/Delete/5
        public async Task<IActionResult> Delete(long? id)
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
                .Include(s => s.User).ThenInclude(s => s.District).AsNoTracking()
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
        public async Task<IActionResult> DeleteConfirmed(long? id)
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

        private bool SellerExists(long id)
        {
            return _context.Sellers.Any(e => e.Id == id);
        }
    }
}
