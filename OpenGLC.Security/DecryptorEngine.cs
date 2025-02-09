﻿using OpenGLC.Models.Security;
using OpenGLC.MVC.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace OpenGLC.Security
{
    public class DecryptorEngine
	{

		private readonly ISecurityKeys _securityKeysValues;
		public DecryptorEngine(ISecurityKeys securityKeysValues)
		{
			_securityKeysValues = securityKeysValues;
		}

		public async Task<DecryptorResultModel> Decrypt(string hashedPassword, string salt)
		{
			try
			{
				//string secretKey = UtilService.GetAppSettingsConfiguration("security", "passwordPrivateKey");
				string secretKey = _securityKeysValues.passwordPrivateKey;
				var saltBuffer = Encoding.UTF8.GetBytes(salt);
				hashedPassword = hashedPassword.Replace(" ", "+");
				string password = string.Empty;
				byte[] cipherBytes = Convert.FromBase64String(hashedPassword);
				using (Aes encryptor = Aes.Create())
				{
					Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(secretKey, saltBuffer, 1000, HashAlgorithmName.SHA256);
					encryptor.Key = pdb.GetBytes(32);
					encryptor.IV = pdb.GetBytes(16);
					using (MemoryStream ms = new MemoryStream())
					{
						using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
						{
							await cs.WriteAsync(cipherBytes, 0, cipherBytes.Length);
							cs.Close();
						}
						password = Encoding.Unicode.GetString(ms.ToArray());
					}
				}
				return new DecryptorResultModel { PlainPassword = password, IsError = false, ErrorMessage = "", TechnicalMessage = "" };
			}
			catch (Exception ex)
			{
				return new DecryptorResultModel { PlainPassword = "", IsError = true, ErrorMessage = "Cannot decrypt given password", TechnicalMessage = ex.Message };
			}
		}

	}
}
