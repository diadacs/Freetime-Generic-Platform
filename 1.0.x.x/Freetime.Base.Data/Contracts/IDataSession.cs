﻿using System;
using System.ServiceModel;

namespace Freetime.Base.Data.Contracts
{
    [ServiceContract]
    public interface IDataSession : IDisposable
    {
    }
}
