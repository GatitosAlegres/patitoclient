namespace PatitoClient.Core.Interactors
{
    public class SendTextMessageUseCase : UseCase<SendTextMessageUseCase.InputValuesUC, SendTextMessageUseCase.OutputValuesUC>
    {

        public override OutputValuesUC Execute(InputValuesUC input)
        {

            return new OutputValuesUC();
            
        }

        public record InputValuesUC() : InputValues;


        public record OutputValuesUC() : OutputValues;
    }
}
