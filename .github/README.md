# LightNovelSniffer
[![FOSSA Status](https://app.fossa.io/api/projects/git%2Bgithub.com%2FLightNovelSniffer%2FLightNovelSniffer.svg?type=shield)](https://app.fossa.io/projects/git%2Bgithub.com%2FLightNovelSniffer%2FLightNovelSniffer?ref=badge_shield)

C# .Net 4.5 Web crawler to create PDF and Epub version of online light novels.

This is a core library. It will need a proper interface to work. For now, there is only a [CLI available](https://github.com/JoshuaArus/LightNovelSniffer-CLI). A [GUI](https://github.com/JoshuaArus/LightNovelSniffer-GUI) is planned but haven't been started yet (feel free to join the adventure !)

## Instructions
### Entry Point
You program need to call the `ConfigTools.InitConf` function with the Config.xml filename to load the default parameters, and initialize global variables. You can call the function multiple times with different files to override values.
When it's done, call `ConfigTools.InitLightNovels` function with the LightNovels.xml filename to load the LN list. You can call this function multiple times with different files. By default it will add LNs to a global list. If you call the function with `true` as the second parameter, the list will be cleared before file import.

Then, use the `WebCrawler` class as entry point to download chapters (either defined in the `LightNovels.xml` or `LightNovels_user.xml` file, or dynamically instantiated in your program)

### Communication
To allow the library to interact with the user or an external programm (ask information, or output some progress), classes implementing the `IInput` and `IOutput` interface are required for the `WebCrawler` constructor.

### Parser
New parsers (to handle new websites) can be added either to this project in the `Web/Parser` folder with a pull request, or dynamically loaded in the `ParserFactory`.
In any case, classes need to implement the `IParser` interface.

### Translations
Translations can be added to the project by adding a `Strings.**Code**.resx` file to Resource folder (where **Code** is a CultureInfo code like "en-US" or "fr-FR"...). Feel free to submit your corrections, suggestions or addition in a PR.

## Dependencies
### Web parser
Web parser use [HtmlAgilityPack](https://www.nuget.org/packages/HtmlAgilityPack/1.4.9.5) to parse string to HtmlNodes.

### Epub
The library Ionic.Zip.dll is part of the [DotNetZip library](https://www.nuget.org/packages/DotNetZip/) which have been cloned in this repo because of some needed modification

### PDF
PDF files are generated by the [iTextSharp library](https://www.nuget.org/packages/iTextSharp/5.5.10)


## License
[![FOSSA Status](https://app.fossa.io/api/projects/git%2Bgithub.com%2FLightNovelSniffer%2FLightNovelSniffer.svg?type=large)](https://app.fossa.io/projects/git%2Bgithub.com%2FLightNovelSniffer%2FLightNovelSniffer?ref=badge_large)