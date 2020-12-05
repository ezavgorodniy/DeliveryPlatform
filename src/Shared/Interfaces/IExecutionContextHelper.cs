using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Interfaces
{
    public interface IExecutionContextHelper
    {
        void FillExecutionContextFromJwt(string jwt, IExecutionContext executionContext);
    }
}
