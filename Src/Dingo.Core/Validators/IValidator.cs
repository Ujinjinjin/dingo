namespace Dingo.Core.Validators
{
	/// <summary> Validator </summary>
	/// <typeparam name="T">Validated entity type</typeparam>
	internal interface IValidator<in T>
	{
		/// <summary> Validate entity </summary>
		/// <param name="entity"></param>
		/// <returns>True if object is valid; False otherwise</returns>
		bool Validate(T entity);
	}
}
