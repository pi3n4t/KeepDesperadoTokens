$compress = @{
  Path = "..\icon.png", "..\manifest.json", "..\README.md", ".\bin\Debug\netstandard2.0\KeepDesperadoTokens.dll"
  CompressionLevel = "Fastest"
  DestinationPath = "..\KeepDesperadoTokens.zip"
}
Compress-Archive @compress -Update
exit 0