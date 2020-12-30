using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using MediatR;
using FreshMart.Documents.Commands;
using FreshMart.Core.ViewModels;
using FreshMart.Database;
using FreshMart.Models;
using FreshMart.Core.Options;
using Microsoft.Extensions.Options;
using FreshMart.Services.Factories;
using static FreshMart.Core.Constants.Constants;

namespace FreshMart.Services.CommandHandler
{
    public class DocumentsCommandHandler :
        IRequestHandler<CreateDocumentCommand, IdNameViewModel>,
        IRequestHandler<DeleteDocumentCommand, bool>
    {

        private readonly IEncryptionServices _encryptionService;
        private readonly IMediator _mediator;
        private readonly AppDbContext _context;
        private readonly IFileService _fileService;
        private readonly StorageOptions _storageOption;


        public DocumentsCommandHandler(
            IMediator mediator,
            AppDbContext context,
            IEncryptionServices encryptionService,
            IFileServiceFactory fileFactory,
            IOptionsMonitor<StorageOptions> storageOption
            )
        {
            _mediator = mediator;
            _context = context;
            _encryptionService = encryptionService;
            _storageOption = storageOption.CurrentValue;
            _fileService = fileFactory.Create(_storageOption.DefaultStorage);
        }

        public async Task<IdNameViewModel> Handle(CreateDocumentCommand request, CancellationToken cancellationToken)
        {
            var document = _fileService.Upload(request.File);

            _context.Documents.Add(document);
            var res = await _context.SaveChangesAsync();
            if (res > 0)
            {
                return await Task.FromResult(new IdNameViewModel { Id = document.Id, Name = $"{document.Name}{document.Extension}" });
            }
            return await Task.FromResult(default(IdNameViewModel));
        }




        public async Task<bool> Handle(DeleteDocumentCommand request, CancellationToken cancellationToken)
        {
            //now delete the file from server
            return await _fileService.Delete(request.Id);
        }

    }
}
