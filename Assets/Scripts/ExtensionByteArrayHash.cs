using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class ExtensionByteArrayHash {

    public static string GetHashSHA1(this byte[] data) {
        using (var sha1 = new System.Security.Cryptography.SHA1CryptoServiceProvider()) {
            return string.Concat(sha1.ComputeHash(data).Select(x => x.ToString("X2")));
        }
    }

}

