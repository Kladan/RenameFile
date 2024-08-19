using System.Runtime.InteropServices;
using Microsoft.Win32;

if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
{
    AddContextMenuEntries();
    Console.WriteLine("Zum Fortfahren Enter drücken.");
    Console.ReadLine();
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
    string exePathRelative = Path.Combine("RenameFile.exe");
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
