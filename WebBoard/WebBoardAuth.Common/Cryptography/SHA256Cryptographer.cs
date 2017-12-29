﻿using System;
using System.Security.Cryptography;

namespace WebBoardAuth.Common.Cryptography
{
    public static class SHA256Cryptographer
    {
        public static string GetHash(string input)
        {
            HashAlgorithm hashAlgorithm = SHA256.Create();
            byte[] byteValue = System.Text.Encoding.UTF8.GetBytes(input);
            byte[] byteHash = hashAlgorithm.ComputeHash(byteValue);
            return Convert.ToBase64String(byteHash);
        }
    }
}