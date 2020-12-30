using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace FreshMart.Core.Interfaces
{
    public interface IQuery<TResponse> : IRequest<TResponse>
    {
    }
}
