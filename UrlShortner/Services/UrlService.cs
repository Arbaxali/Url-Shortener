using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UrlShortner.Models;


namespace UrlShortner.Services
{
    public class UrlService
    {

        public static string GenerateShortUrl(string originalUrl) {

            if (string.IsNullOrWhiteSpace(originalUrl))
            {
                throw new ArgumentException("The URL cannot be null or empty.");
            }

            
            string pattern = @"^(https?://)?([a-zA-Z0-9\-]+\.)+[a-zA-Z]{2,}(/.*)?$";
            if (!Regex.IsMatch(originalUrl, pattern))
            {
                throw new ArgumentException("The URL format is invalid.");
            }


            if (Regex.IsMatch(originalUrl, @"^\d+$"))
            {
                throw new ArgumentException("The URL cannot contain only numbers.");
            }



            MongoService mongoService = new MongoService();
            string shorturl = GenerateShortCode();
            


            if (!originalUrl.StartsWith("http://") && !originalUrl.StartsWith("https://"))
            {
                originalUrl = "http://" + originalUrl;
            }


            shortenerModel model = new shortenerModel
            {
                OriginalUrl = originalUrl,
                ShortenedUrl = shorturl,
                CreatedTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time")).ToString("yyyy-MM-dd HH:mm:ss")
,
            };

            
            mongoService.InsertDatatoMongo(model);
            
            
            
            return $"https://localhost:7120/{shorturl}";
        }

        private static string GenerateShortCode()
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            char[] code = new char[6]; // Change the length for shorter/longer codes
            for (int i = 0; i < code.Length; i++)
            {
                code[i] = chars[random.Next(chars.Length)];
            }
            return new string(code);
        }
    }
}
