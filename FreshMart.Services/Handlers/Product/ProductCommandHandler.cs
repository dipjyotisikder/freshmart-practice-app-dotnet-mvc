using FreshMart.Models.Commands;
using FreshMart.ViewModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FreshMart.Services.CommandHandler
{
    public class ProductCommandHandler : IRequestHandler<CreateProductCommand, ProductViewModel>
    {
        private readonly IProductService _productService;

        public ProductCommandHandler(IProductService productService)
        {
            _productService = productService;
        }


        public Task<ProductViewModel> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {


            throw new NotImplementedException();
        }
    }
}
