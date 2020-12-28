using FreshMart.Database;
using FreshMart.Models.Commands;
using FreshMart.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FreshMart.Services.CommandHandler
{
    public class SellerCommandHandler : IRequestHandler<UpdateSellerProfileCommand, SellerViewModel>
    {
        private readonly IProductService _productService;
        private readonly AppDbContext _context;

        public SellerCommandHandler(IProductService productService,
            AppDbContext context)
        {
            _productService = productService;
            _context = context;
        }

        public async Task<SellerViewModel> Handle(UpdateSellerProfileCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var seller = _context.Sellers.Where(x => x.Id == request.Id)
                    .Include(x => x.User)
                    .ThenInclude(x => x.District)
                    .FirstOrDefault();

                if (seller != null)
                {
                    seller.Id = request.Id;
                    seller.Name = request.Name;
                    seller.Email = request.Email;
                    seller.CompanyName = request.CompanyName;
                    seller.DateOfBirth = request.DateOfBirth;
                    seller.Phone = request.Phone;

                    //navigated value updated
                    seller.User.DistrictId = request.DistrictId;
                }
                var res = await _context.SaveChangesAsync();
                var vm = new SellerViewModel
                {
                    Seller = seller,
                    Districts = _context.Districts.AsNoTracking().ToList(),
                    Sellers = _context.Sellers.AsNoTracking().ToList(),
                    Success = res > 0 ? "Updated successfully" : ""
                };
                return await Task.FromResult(vm);
            }
            catch (DbUpdateConcurrencyException) { }

            return default;
        }
    }
}
