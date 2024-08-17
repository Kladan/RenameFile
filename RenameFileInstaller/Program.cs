using System.Runtime.InteropServices;
using Microsoft.Win32;

bool _installTool = args.Contains("-install");
bool _uninstallTool = args.Contains("-uninstall");
bool _showHelp = NeedsHelp(args);

if (_showHelp)
{
    Console.WriteLine("------------------------------------------------------------------");
    Console.WriteLine(" Die folgenden Parameter stehen zur Verfügung:");
    Console.WriteLine(" -install - Einträge dem Kontextmenü hinzufügen");
    Console.WriteLine(" -uninstall - Einträge aus dem Kontextmenü entfernen");
    Console.WriteLine("------------------------------------------------------------------");
    Console.ReadLine();
}

if (_installTool)
{
    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
    {
        AddContextMenuEntries();
        Console.WriteLine("Zum Fortfahren Enter drücken.");
        Console.ReadLine();
    }
}

if (_uninstallTool)
{
    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
    {
        RemoveFromContextMenu();
        Console.WriteLine("Zum Fortfahren Enter drücken.");
        Console.ReadLine();
    }
}

bool NeedsHelp(string[] args)
{
    return
        args.Contains("-h") ||
        args.Contains("-help") ||
        args.Contains("--h") ||
        args.Contains("--help") ||
        args.Contains("/?");
}

void AddContextMenuEntries()
{
    try
    {
        AddContextMenuEntry("FNDateAddToday", "Heutiges Datum vorne anfügen", "-addDateToday");
        Console.WriteLine("\"Heutiges Datum vorne anfügen\" dem Kontextmenü hinzugefügt.");

        AddContextMenuEntry("FNDateAddLastChange", "Änderungsdatum vorne anfügen", "-addDateLastChange");
        Console.WriteLine("\"Änderungsdatum vorne anfügen\" dem Kontextmenü hinzugefügt.");

        AddContextMenuEntry("FNDateRemove", "Datum vorne entfernen", "-removeDate");
        Console.WriteLine("\"Datum vorne entfernen\" dem Kontextmenü hinzugefügt.");

        AddContextMenuEntry("FNDateRepair", "Datum vorne reparieren", "-repairDate");
        Console.WriteLine("\"Datum vorne reparieren\" dem Kontextmenü hinzugefügt.");
    }
    catch (UnauthorizedAccessException e)
    {
        Console.WriteLine("Hierfür weden Administrator-Rechte benötigt.");
    }
    catch (FileNotFoundException e)
    {
        Console.WriteLine($"{e.Message} {e.FileName}");
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
    }
}

static void AddContextMenuEntry(string entryName, string entryText, string parameter)
{
    string exePathRelative = Path.Combine("..\\..\\..\\..\\RenameFile\\bin\\Debug\\net6.0\\RenameFile.exe");
    string exePathAbsolute = Path.GetFullPath(exePathRelative);

    if (!File.Exists(exePathAbsolute))
    {
        throw new FileNotFoundException("Die auszuführende Datei wurde nicht gefunden.", exePathAbsolute);
    }
    
    RegistryKey root = Registry.ClassesRoot;
    RegistryKey shell = root.OpenSubKey(@"*\shell", true);

    RegistryKey filenameDateAdd = shell.CreateSubKey(entryName, true);
    filenameDateAdd.SetValue("", entryText);

    RegistryKey command = filenameDateAdd.CreateSubKey("command", true);
    command.SetValue("", $"\"{exePathAbsolute}\" \"%1\" {parameter}");
}

void RemoveFromContextMenu()
{
    try
    {
        RegistryKey root = Registry.ClassesRoot;
        RegistryKey shell = root.OpenSubKey(@"*\shell", true);

        shell.DeleteSubKeyTree("FNDateAddToday", false);
        Console.WriteLine("\"Heutiges Datum vorne anfügen\" aus dem Kontextmenü entfernt.");

        shell.DeleteSubKeyTree("FNDateAddLastChange", false);
        Console.WriteLine("\"Änderungsdatum vorne anfügen\" aus dem Kontextmenü entfernt.");

        shell.DeleteSubKeyTree("FNDateRemove", false);
        Console.WriteLine("\"Datum vorne entfernen\" aus dem Kontextmenü entfernt.");

        shell.DeleteSubKeyTree("FNDateRepair", false);
        Console.WriteLine("\"Heutiges Datum vorne reparieren\" aus dem Kontextmenü entfernt.");
    }
    catch (UnauthorizedAccessException e)
    {
        Console.WriteLine("Hierfür werden Administrator-Rechte benötigt.");
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
    }
}
