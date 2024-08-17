using System.Text.RegularExpressions;

bool _addDateToday = args.Contains("-addDateToday");
bool _addDateLastChange = args.Contains("-addDateLastChange");
bool _removeDate = args.Contains("-removeDate");
bool _repairDate = args.Contains("-repairDate");
bool _showHelp = NeedsHelp(args);

string _sourceFilePath = args[0];

if (_showHelp)
{
    Console.WriteLine("------------------------------------------------------------------");
    Console.WriteLine(" Die folgenden Parameter stehen zur Verfügung:");
    Console.WriteLine(" -addDateToday - Heutiges Datum vorne an den Dateinamen anfügen");
    Console.WriteLine(" -addDateLastChange - Änderungsdatum vorne an den Dateinamen anfügen");
    Console.WriteLine(" -removeDate - Datum vorne entfernen");
    Console.WriteLine(" -repairDate - Heutiges Datum vorne reparieren");
    Console.WriteLine(" Gib außerdem als ersten Parameter den vollständigen Dateipfad an");
    Console.WriteLine("------------------------------------------------------------------");
    Console.ReadLine();
}

if (_addDateToday || _addDateLastChange || _removeDate || _repairDate)
{
    RenameFile();
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