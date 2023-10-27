using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatitoClient.Core.Interactors
{
    public abstract class UseCase<I, O>
    where I : UseCase<I, O>.InputValues
    where O : UseCase<I, O>.OutputValues
    {
        public abstract O Execute(I input);

        public interface InputValues
        {
        }

        public interface OutputValues
        {
        }
    }
}
