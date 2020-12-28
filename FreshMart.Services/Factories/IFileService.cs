using FreshMart.Models;
using FreshMart.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FreshMart.Services
{
    public interface IFileService
    {
        void Upload();
        void Download();
    }
}
