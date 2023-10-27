namespace PatitoClient.Core.Interactors
{
    public class ReceiveTextMessageUseCase : UseCase<ReceiveTextMessageUseCase.InputValuesUC, ReceiveTextMessageUseCase.OutputValuesUC>
    {
        public override OutputValuesUC Execute(InputValuesUC input)
        {
            return new OutputValuesUC();
        }
        

        public record InputValuesUC() : InputValues;

        public record OutputValuesUC() : OutputValues;
    }
}
