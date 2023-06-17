using System.Text.RegularExpressions;

bool removeDate = args.Contains("-remove");
bool repairDate = args.Contains("-repair");

string sourceFilePath = args[0];
string destFilePath = String.Empty;

if (File.Exists(sourceFilePath))
{
    if (removeDate)
    {
        destFilePath = Regex.Replace(sourceFilePath, "\\d{4}\\-\\d{2}\\-\\d{2} ", "");
        destFilePath = Regex.Replace(destFilePath, "\\d{4}\\-\\d{2}\\-\\d{2}_", "");
    }
    else if (repairDate)
    {
        if (Regex.IsMatch(sourceFilePath, "\\d{8}"))
        {
            string destFileName = Path.GetFileName(sourceFilePath)
                .Insert(4, "-")
                .Insert(7, "-");

            destFilePath = Path.Combine(Path.GetDirectoryName(sourceFilePath), destFileName);
        }
    }
    else
    {
        DateTime now = DateTime.Now;
        string date = now.ToString("yyyy-MM-dd");
        destFilePath = Path.Combine(Path.GetDirectoryName(sourceFilePath),
            date + ' ' + Path.GetFileName(sourceFilePath));
    }

    if (!String.IsNullOrEmpty(destFilePath) && sourceFilePath != destFilePath)
    {
        try
        {
            File.Move(sourceFilePath, destFilePath);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            Console.ReadLine();
            throw;
        }
    }
}
