using FreshMart.Database;
using FreshMart.Models.Commands;
using FreshMart.Models.ViewModels;
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
        private readonly ApplicationDbContext _context;

        public SellerCommandHandler(IProductService productService,
            ApplicationDbContext context)
        {
            _productService = productService;
            _context = context;
        }


        public async Task<SellerViewModel> Handle(UpdateSellerProfileCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var sellerChk = _context.Sellers.Find(request.Id);

                sellerChk.Id = request.Id;
                sellerChk.Name = request.Name;
                sellerChk.Email = request.Email;
                sellerChk.DistrictId = request.DistrictId;
                sellerChk.Approval = request.Approval;
                sellerChk.CompanyName = request.CompanyName;
                sellerChk.DateOfBirth = request.DateOfBirth;
                sellerChk.Phone = request.Phone;
                var res = await _context.SaveChangesAsync();



                var vm = new SellerViewModel
                {
                    Seller = sellerChk,
                    Districts = _context.Districts.AsNoTracking().ToList(),
                    Sellers = _context.Sellers.AsNoTracking().ToList(),
                    Success = res > 0 ? "Updated successfully" : ""
                };



                return await Task.FromResult(vm);
            }
            catch (DbUpdateConcurrencyException)
            {
            }

            return default;
        }
    }
}
