namespace Web.Infrastructure.Validation
{
	public class ValidatedInputModel<T> where T : class 
	{
		public ValidatedInputModel(T inputModel)
		{
			InputModel = inputModel;
		}

		public T InputModel { get; private set; }

		public bool FailedValidation { get; set; }
	}
}