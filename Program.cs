using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Microsoft.Win32;

bool _addDateToday = args.Contains("-addDateToday");
bool _addDateLastChange = args.Contains("-addDateLastChange");
bool _removeDate = args.Contains("-removeDate");
bool _repairDate = args.Contains("-repairDate");
bool _installTool = args.Contains("-install");
bool _uninstallTool = args.Contains("-uninstall");
bool _help = ShouldHelp(args);

string _sourceFilePath = args[0];

if (_help)
{
    Console.WriteLine("------------------------------------------------------------------");
    Console.WriteLine(" Die folgenden Parameter stehen zur Verfügung:");
    Console.WriteLine(" -addDateToday - Heutiges Datum vorne an den Dateinamen anfügen");
    Console.WriteLine(" -addDateLastChange - Änderungsdatum vorne an den Dateinamen anfügen");
    Console.WriteLine(" -removeDate - Datum vorne entfernen");
    Console.WriteLine(" -repairDate - Heutiges Datum vorne reparieren");
    Console.WriteLine(" -install - Einträge dem Kontextmenü hinzufügen");
    Console.WriteLine(" -uninstall - Einträge aus dem Kontextmenü entfernen");
    Console.WriteLine(" Gib außerdem als ersten Parameter den vollständigen Dateipfad an");
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

if (_addDateToday || _addDateLastChange || _removeDate || _repairDate)
{
    RenameFile();
    Console.WriteLine("Zum Fortfahren Enter drücken.");
    Console.ReadLine();
}

bool ShouldHelp(string[] args)
{
    return
        args.Contains("-h") ||
        args.Contains("-help") ||
        args.Contains("--h") ||
        args.Contains("--help") ||
        args.Contains("/?");
}

void RenameFile()
{
    if (File.Exists(_sourceFilePath))
    {
        string destFilePath = ModifyFilename();

        if (!String.IsNullOrEmpty(destFilePath) && _sourceFilePath != destFilePath)
        {
            try
            {
                File.Move(_sourceFilePath, destFilePath);
                Console.WriteLine("Datei umbenannt.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.ReadLine();
                throw;
            }
        }
    }
}

string ModifyFilename()
{
    string destFilePath = String.Empty;

    if (_removeDate)
    {
        destFilePath = Regex.Replace(_sourceFilePath, "\\d{4}\\-\\d{2}\\-\\d{2} ", "");
        destFilePath = Regex.Replace(destFilePath, "\\d{4}\\-\\d{2}\\-\\d{2}_", "");
    }
    
    if (_repairDate)
    {
        if (Regex.IsMatch(_sourceFilePath, "\\d{8}"))
        {
            string destFileName = Path.GetFileName(_sourceFilePath)
                .Insert(4, "-")
                .Insert(7, "-");

            destFilePath = Path.Combine(Path.GetDirectoryName(_sourceFilePath), destFileName);
        }
    }
    
    if (_addDateToday)
    {
        DateTime now = DateTime.Now;
        string date = now.ToString("yyyy-MM-dd");
        destFilePath = Path.Combine(Path.GetDirectoryName(_sourceFilePath),
            $"{date} {Path.GetFileName(_sourceFilePath)}");
    }

    if (_addDateLastChange)
    {
        FileInfo fileInfo = new FileInfo(_sourceFilePath);
        if (fileInfo.Exists)
        {
            string date = fileInfo.LastWriteTime.ToString("yyyy-MM-dd");
            destFilePath = Path.Combine(Path.GetDirectoryName(_sourceFilePath),
                $"{date} {Path.GetFileName(_sourceFilePath)}");
        }
    }

    return destFilePath;
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
    catch (Exception e)
    {
        Console.WriteLine(e);
    }
}

static void AddContextMenuEntry(string entryName, string entryText, string parameter)
{
    string exePath = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;

    RegistryKey root = Registry.ClassesRoot;
    RegistryKey shell = root.OpenSubKey(@"*\shell", true);

    RegistryKey filenameDateAdd = shell.CreateSubKey(entryName, true);
    filenameDateAdd.SetValue("", entryText);

    RegistryKey command = filenameDateAdd.CreateSubKey("command", true);
    command.SetValue("", $"\"{exePath}\" \"%1\" {parameter}");
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
        Console.WriteLine("Hierfür weden Administrator-Rechte benötigt.");
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
    }
}
