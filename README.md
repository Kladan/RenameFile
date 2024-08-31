# RenameFile

![Rename File by adding a date](assets/cover.png)

RenameFile bietet verschiedene Optionen zum Umbenennen von Dateien basierend auf Datumsangaben.

## Übersicht
Das `RenameFile` Repository enthält eine Lösung (`RenameFile.sln`) mit drei Projekten:
- `RenameFile.csproj`
- `install.csproj`
- `uninstall.csproj`

## Projekte

### RenameFile
Dieses Projekt bietet verschiedene Optionen zum Umbenennen von Dateien basierend auf Datumsangaben.

#### Verfügbare Parameter:
- `-addDateToday`: Heutiges Datum vorne an den Dateinamen anfügen
- `-addDateLastChange`: Änderungsdatum vorne an den Dateinamen anfügen
- `-removeDate`: Datum vorne entfernen
- `-repairDate`: Heutiges Datum vorne reparieren
- `-h`, `-help`, `--h`, `--help`, `/?`: Hilfe anzeigen

#### Beispiel:
```sh
RenameFile.exe "C:\Pfad\zur\Datei.txt" -addDateToday
```

### install
Die Ausführung der install.exe fügt Kontextmenüeinträge für die verschiedenen Umbenennungsoptionen hinzu.

#### Kontextmenüeinträge:
- Heutiges Datum vorne anfügen
- Änderungsdatum vorne anfügen
- Datum vorne entfernen
- Datum vorne reparieren

Im Hintergrund sind das Aufrufe der RenameFile.exe.
Daher müssen install.exe und RenameFile.exe im selben Verzeichnis liegen.
Werden diese verschoben, aktualisiert eine erneute Ausführung der install.exe die Einträge. 

### uninstall
Die Ausführung der uninstall.exe entfernt die zuvor hinzugefügten Kontextmenüeinträge.

## Veröffentlichung
- Das `install` Projekt hat Referenzen auf die beiden anderen Projekte, wodurch beim Publish von `install` alle drei Projekte gebaut werden.
- Stelle sicher, dass alle Projekte in dasselbe Verzeichnis veröffentlicht werden.
