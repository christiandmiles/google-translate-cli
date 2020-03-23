# Google Translate CLI

A basic CLI in .NET Core to use the Google Translate API. The default settings are to translate from english to german.

## Usage

There must be a file in the same directory as the script called `service-account.json` which contains the Google translate API service account credentials as exported from their dashboard.

```
./google-translate-cli -i alternate-testimonial.txt
```

## Options

-i, --input              (Default: my-testimonial.txt) Set input file.

-o, --output             (Default: translated-testimonial.txt) Set output file.

-l, --target-language    (Default: de) Set target language.

-s, --source-language    (Default: en) Set language.

--help                   Display this help screen.

--version                Display version information.

## Publishing

A self contained windows 64-bit exe is included in the repo. To generate your own run the following code:

```
dotnet publish -c Release -r win10-x64 /p:PublishSingleFile=true /p:PublishTrimmed=true
```