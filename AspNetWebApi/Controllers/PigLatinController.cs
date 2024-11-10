using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace AspNetWebApi.Controllers
{
    [Route("api")]
    [ApiController]
    public class PigLatinController(ILogger<PigLatinController> logger) : ControllerBase
    {
        public record InputText(string Value);
        public record PigLatinText(string Value);

        [HttpPost("Function1")]
        public IActionResult Function1([FromBody] InputText inputText)
        {
            logger.LogInformation("ASP.NET Core Web API processed a request.");
            var result = TranslateToPigLatin(inputText.Value);
            return new OkObjectResult(new PigLatinText(result));
        }

        private static string TranslateToPigLatin(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            var words = input.Split(' ');
            StringBuilder pigLatin = new();

            foreach (string word in words)
            {
                if (IsVowel(word[0]))
                {
                    pigLatin.Append(word + "yay ");
                }
                else
                {
                    int vowelIndex = FindFirstVowelIndex(word);
                    if (vowelIndex is -1)
                    {
                        pigLatin.Append(word + "ay ");
                    }
                    else
                    {
                        pigLatin.Append(
                            word.Substring(vowelIndex) + word.Substring(0, vowelIndex) + "ay ");
                    }
                }
            }

            return pigLatin.ToString().Trim();
        }

        private static int FindFirstVowelIndex(string word)
        {
            for (var i = 0; i < word.Length; i++)
            {
                if (IsVowel(word[i]))
                {
                    return i;
                }
            }
            return -1;
        }

        private static bool IsVowel(char c) => char.ToLower(c) is 'a' or 'e' or 'i' or 'o' or 'u';

    }
}
