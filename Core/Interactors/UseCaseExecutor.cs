using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatitoClient.Core.Interactors
{
    public interface UseCaseExecutor
    {
        Task<RX> Execute<RX, I, O>(UseCase<I, O> useCase, I input, Func<O, RX> outputMapper)
            where I : UseCase<I, O>.InputValues
            where O : UseCase<I, O>.OutputValues;
    }
}