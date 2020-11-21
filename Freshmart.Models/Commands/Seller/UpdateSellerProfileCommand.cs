using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using MediatR;
using FreshMart.Models.ViewModels;

namespace FreshMart.Models.Commands
{
    public class UpdateSellerProfileCommand : IRequest<SellerViewModel>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public DateTime DateOfBirth { get; set; }

        public int DistrictId { get; set; }

        public string CompanyName { get; set; }

        public bool? Approval { get; set; }
    }
}
