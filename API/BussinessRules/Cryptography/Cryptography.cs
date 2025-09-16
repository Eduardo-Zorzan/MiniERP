using System.Security.Cryptography;

namespace API.BussinessRules.Cryptography
{
	public class Cryptography
	{
		public static async Task<Entities.CriptographyReturn> Encrypt(string input)
		{
			if	(string.IsNullOrEmpty(input))
				throw new ArgumentException("Input cannot be null or empty.", nameof(input));

			byte[] key;
			byte[] iv;


			using (TripleDES tripleDes = TripleDES.Create())
			{
				key = tripleDes.Key;
				iv = tripleDes.IV;
			}

			Entities.CriptographyReturn criptographyReturn = new Entities.CriptographyReturn
			{
				Key = key,
				Iv = iv,
				Output = await Encrypt(input, key, iv)
			};

			return criptographyReturn;
		}

		public static async Task<string> Decrypt(Entities.CriptographyReturn criptography)
		{

			return await DecryptTextFromFile(criptography.Output, criptography.Key, criptography.Iv);
		}

		public static async Task<string> Encrypt(string input, byte[] key, byte[] iv)
		{
			try
			{
				using TripleDES tripleDes = TripleDES.Create();
				tripleDes.Key = key;
				tripleDes.IV = iv;

				using MemoryStream memoryStream = new MemoryStream();
				using ICryptoTransform encryptor = tripleDes.CreateEncryptor();
				using CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
				using StreamWriter writer = new StreamWriter(cryptoStream);

				await writer.WriteAsync(input);
				await writer.FlushAsync();
				await cryptoStream.FlushFinalBlockAsync();

				return Convert.ToBase64String(memoryStream.ToArray());
			}
			catch (CryptographicException e)
			{
				Console.WriteLine("A Cryptographic error occurred: {0}", e.Message);
				throw;
			}
		}

		public static async Task<string> DecryptTextFromFile(string input, byte[] key, byte[] iv)
		{
			try
			{
				TripleDES tripleDes = TripleDES.Create();
				tripleDes.Key = key;
				tripleDes.IV = iv;

				byte[] encryptedBytes = Convert.FromBase64String(input);
				using MemoryStream memoryStream = new MemoryStream(encryptedBytes);
				using ICryptoTransform decryptor = tripleDes.CreateDecryptor();
				using CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
				using StreamReader reader = new StreamReader(cryptoStream);

				return await reader.ReadToEndAsync();
			}
			catch (CryptographicException e)
			{
				Console.WriteLine("A Cryptographic error occurred: {0}", e.Message);
				throw;
			}
		}
	}
}
