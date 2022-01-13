using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonCommandHandler.Commands
{
    internal interface ICommandHandler<TCommand>
    {
        public Task HandleAsync(TCommand command);
    }
}
