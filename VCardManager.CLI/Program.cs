using VCardManager.CLI;
using VCardManager.Core.Interfaces;
using VCardManager.Core;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();
services.AddTransient<IConsole, SystemConsole>();
services.AddTransient<IFileStore, FileSystemStore>();
services.AddTransient<IVCardRepository, VCardFileRepository>();
services.AddTransient<IVCardService, VCardService>();
services.AddTransient<IMenu, Menu>();

services.BuildServiceProvider()
    .GetRequiredService<IMenu>()
    .Run();


// Pad naar het .vcf-bestand waar alle contacten worden opgeslagen
// const string filePath = "data/contacts.vcf";

// // Maak de concrete implementatie van IConsole aan voor interactie met de gebruiker (via System.Console)
// IConsole console = new SystemConsole();

// // Maak de implementatie van IFileStore aan die werkt met System.IO (echte bestandstoegang)
// IFileStore fileStore = new FileSystemStore();

// // De repository gebruikt het bestandssysteem om contacten te beheren via het opgegeven pad
// IVCardRepository repository = new VCardFileRepository(filePath, fileStore);

// // De service voegt logica toe bovenop de repository en wordt door het menu gebruikt
// IVCardService service = new VCardService(repository);

// // Menu is de console-UI: ontvangt service en console als afhankelijkheden
// var menu = new Menu(service, console);

// // Start de applicatie
// menu.Run();