using System.Runtime.InteropServices;
using Microsoft.Win32;

if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
{
    RemoveFromContextMenu();
    Console.WriteLine("Zum Fortfahren Enter drücken.");
    Console.ReadLine();
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
