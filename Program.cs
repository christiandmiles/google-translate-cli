using System;
using System.IO;
using System.Linq;
using CommandLine;
using Google.Api.Gax.ResourceNames;
using Google.Cloud.Translate.V3;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace google_translate_cli
{
  class Program
  {
    public class Options
    {
      [Option('i', "input", Required = false, HelpText = "Set input file.", Default = "my-testimonial.txt")]
      public string Input { get; set; }

      [Option('o', "output", Required = false, HelpText = "Set output file.", Default = "translated-testimonial.txt")]
      public string Output { get; set; }

      [Option('l', "target-language", Required = false, HelpText = "Set target language.", Default = "de")]
      public string TargetLanguage { get; set; }

      [Option('s', "source-language", Required = false, HelpText = "Set language.", Default = "en")]
      public string SourceLanguage { get; set; }
    }

    static void Main(string[] args)
    {
      Parser.Default.ParseArguments<Options>(args)
        .WithParsed<Options>(o =>
        {
          var text = File.ReadAllText(o.Input);
          Console.WriteLine(text);
          var result = Translate(text, o.TargetLanguage, o.SourceLanguage);
          Console.WriteLine(result);
          File.WriteAllText(o.Output, result);
        });      
    }

    public static string Translate(string text, string targetLanguage, string sourceLanguage) {
      var credentialsPath = Path.Combine(Directory.GetCurrentDirectory(), "service-account.json");
      Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", credentialsPath);
      var projectId = JObject.Parse(File.ReadAllText(credentialsPath)).SelectToken("project_id").ToString();

      TranslationServiceClient translationServiceClient = TranslationServiceClient.Create();
      TranslateTextRequest request = new TranslateTextRequest
      {
        Contents = { text },
        TargetLanguageCode = targetLanguage,
        SourceLanguageCode = sourceLanguage,
        ParentAsLocationName = new LocationName(projectId, "global")
      };
      TranslateTextResponse response = translationServiceClient.TranslateText(request);
      return response.Translations.FirstOrDefault()?.TranslatedText;
    }
  }
}
