using FreshMart.Core.ViewModels;
using MediatR;
using System;
using System.Collections.Generic;
using FreshMart.Core.Interfaces;
using Microsoft.AspNetCore.Http;

namespace FreshMart.Documents.Commands
{
    public class CreateDocumentCommand : ICommand<IdNameViewModel>
    {
        public IFormFile File { get; set; }
    }
}
