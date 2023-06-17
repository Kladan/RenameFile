using System.Text.RegularExpressions;

bool _removeDate = args.Contains("-remove");
bool _repairDate = args.Contains("-repair");

string _sourceFilePath = args[0];

RenameFile();

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
    else if (_repairDate)
    {
        if (Regex.IsMatch(_sourceFilePath, "\\d{8}"))
        {
            string destFileName = Path.GetFileName(_sourceFilePath)
                .Insert(4, "-")
                .Insert(7, "-");

            destFilePath = Path.Combine(Path.GetDirectoryName(_sourceFilePath), destFileName);
        }
    }
    else
    {
        DateTime now = DateTime.Now;
        string date = now.ToString("yyyy-MM-dd");
        destFilePath = Path.Combine(Path.GetDirectoryName(_sourceFilePath),
            date + ' ' + Path.GetFileName(_sourceFilePath));
    }

    return destFilePath;
}
