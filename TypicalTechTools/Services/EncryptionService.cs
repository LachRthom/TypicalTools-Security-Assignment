using System.Security.Cryptography;

namespace TypicalTechTools.Services
{
    public class EncryptionService
    {
        string _secretKey;

        public EncryptionService(IConfiguration config)
        {
            //Request the secret key from appsettings
            _secretKey = config["SecretKey"];
        }

        public byte[] EncryptByteArray(byte[] fileData)
        {
            //Create AES algorithm class which handles generating the Initialization Vector
            using (var aesAlg = Aes.Create())
            {
                //Convert secret key to byte array
                aesAlg.Key = System.Text.Encoding.UTF8.GetBytes(_secretKey);
                //Create an encryptor using the key and initialisation vector to encypt data
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (var memStream = new MemoryStream())
                {
                    //Add the IV to the start of hte byte array 
                    memStream.Write(aesAlg.IV, 0, 16);
                    //Create crypto stream which will pass the data back to the memory stream using the encryptor
                    using (var cryStream = new CryptoStream(memStream, encryptor, CryptoStreamMode.Write))
                    {
                        //Pass the file data through the crypto stream
                        cryStream.Write(fileData, 0, fileData.Length);
                        //Finalize the stream processing
                        cryStream.FlushFinalBlock();
                        //return the mem stream data
                        return memStream.ToArray();
                    }
                }
            }
        }

        public byte[] DecryptByteArray(byte[] encryptedData) 
        {
            //Create an AES algorithm class object
            using (var aesAlg = Aes.Create())
            {
                //Convert secret key to a byte array and store it in the algorithm
                aesAlg.Key = System.Text.Encoding.UTF8.GetBytes(_secretKey);
                //Create byte array to store the IV when we retrieve it from file
                byte[] IV = new byte[16];
                //Copy the first section of the file ot the vyte array to retrieve the IC from the file
                Array.Copy(encryptedData, IV, IV.Length);
                //Create decryptor for the decryption process and hand it the key and IV
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key,IV);

                //Create a memory stream for processing our byte array and holding it after the decryption
                using (var memStream = new MemoryStream())
                {
                    //Create a cryptostream for performaing the decryption using the decryptor..
                    using (var cryStream = new CryptoStream(memStream, decryptor, CryptoStreamMode.Write))
                    {
                        //Pass the file data through the crypto stream, starting after the IV 
                        cryStream.Write(encryptedData,IV.Length, encryptedData.Length - IV.Length);
                        //Finalise the streams processing before moving on
                        cryStream.FlushFinalBlock();
                        //return the final mem stream data
                        return memStream.ToArray();
                    }
                }
            }


        }
    }
}
