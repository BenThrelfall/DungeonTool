using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Class for containing the hashing extension method that is used for identifying images / sprites
/// </summary>
public static class ExtensionByteArrayHash {

    /// <summary>
    /// Hashing method for creating hashs for identifing images / sprites
    /// </summary>
    /// <param name="data">Raw byte data for the image</param>
    /// <returns>Hash of the data</returns>
    public static string GetHashSHA1(this byte[] data) {
        using (var sha1 = new System.Security.Cryptography.SHA1CryptoServiceProvider()) {
            return string.Concat(sha1.ComputeHash(data).Select(x => x.ToString("X2")));
        }
    }

}

