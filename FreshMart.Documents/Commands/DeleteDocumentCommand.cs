using FreshMart.Core.Interfaces;
using FreshMart.Core.ViewModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace FreshMart.Documents.Commands
{
    public class DeleteDocumentCommand : ICommand<bool>
    {
        public long Id { get; set; }
    }
}
