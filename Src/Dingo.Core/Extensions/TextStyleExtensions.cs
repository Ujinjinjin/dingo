using Dingo.Core.Models;
using System.Drawing;

namespace Dingo.Core.Extensions
{
	/// <summary> Collection of extensions for <see cref="TextStyle"/> </summary>
	public static class TextStyleExtensions
	{
		/// <summary> Convert text style to color </summary>
		/// <param name="source">Text style</param>
		/// <returns>Color</returns>
		public static Color ToColor(this TextStyle source)
		{
			return source switch
			{
				TextStyle.Info => Color.FromArgb(27, 150, 236),
				TextStyle.Warning => Color.FromArgb(250, 166, 39),
				TextStyle.Error => Color.FromArgb(233, 53, 25),
				TextStyle.Success => Color.FromArgb(47, 170, 79),
				TextStyle.Plain => Color.Empty,
				TextStyle.Unknown => Color.Empty,
				_ => Color.Empty
			};
		}
	}
}
