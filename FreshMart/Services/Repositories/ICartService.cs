using FreshMart.Helper;
using FreshMart.ViewModels;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using FreshMart.Data;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Linq;

namespace FreshMart.Services
{
    public interface ICartService
    {
        int GetCartCount();
        float GetCartTotalPrice();
    }

}
