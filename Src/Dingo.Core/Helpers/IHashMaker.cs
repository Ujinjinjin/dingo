using Dingo.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dingo.Core.Helpers
{
	/// <summary> Hash maker </summary>
	internal interface IHashMaker
	{
		/// <summary> Get MD5 hash of the file </summary>
		/// <param name="filename">Filename</param>
		/// <returns>String representation of MD5 hash</returns>
		Task<string> GetFileHashAsync(string filename);

		/// <summary> Get migration info of the list of files </summary>
		/// <param name="filePathList">List of file paths to compute info about</param>
		/// <returns>List of <see cref="MigrationInfo"/></returns>
		Task<IList<MigrationInfo>> GetMigrationInfoListAsync(IList<FilePath> filePathList);
	}
}
