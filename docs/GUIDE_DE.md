# FlowGuide - Benutzerhandbuch zur Erstellung von Guides

## Inhaltsverzeichnis

1. [Einführung](#einführung)
2. [Vorbereitung auf die Aufzeichnung](#vorbereitung-auf-die-aufzeichnung)
3. [Erstellung eines neuen Guides](#erstellung-eines-neuen-guides)
4. [Bearbeitung eines Guides](#bearbeitung-eines-guides)
5. [Tipps und Best Practices](#tipps-und-best-practices)
6. [Fehlerbehebung](#fehlerbehebung)

---

## Einführung

**FlowGuide** hilft Ihnen, interaktive Schritt-für-Schritt-Anleitungen für beliebige Windows-Anwendungen zu erstellen. Ihre Guides führen Benutzer durch komplexe Prozesse, indem die benötigten Schaltflächen und Felder auf ihrem Bildschirm hervorgehoben werden.

### Was Sie tun können:
* Benutzeraktionen in jeder Anwendung aufzeichnen
* Automatisch Textanweisungen generieren
* Aufgezeichnete Schritte bearbeiten und verfeinern
* Fertige Guides mit Kollegen teilen

### Was Sie benötigen:
* Windows 10 oder 11
* Administratorrechte (für den ersten Start)
* Die Zielanwendung, für die Sie die Anleitung erstellen

---

## Vorbereitung auf die Aufzeichnung

### Schritt 1: Starten Sie den FlowGuide Recorder

1. Öffnen Sie das Verzeichnis `Release\Recorder\`
2. Führen Sie **FlowGuide.Recorder.exe** als Administrator aus
   * Rechtsklick auf die Datei -> "Als Administrator ausführen"
   * Oder: Einmal normal starten und die angeforderten Berechtigungen zulassen

> [Tipp]: Erstellen Sie eine Desktop-Verknüpfung für schnellen Zugriff.

### Schritt 2: Bereiten Sie die Zielanwendung vor

**Vor Beginn der Aufzeichnung:**

1. Öffnen Sie die Anwendung, für die Sie den Guide erstellen möchten (Word, 1C usw.)
2. Richten Sie den Anfangszustand ein (öffnen Sie beispielsweise das gewünschte Startfenster)
3. Schließen Sie nicht benötigte Fenster und Benachrichtigungen
4. Deaktivieren Sie die automatische Speicherung und automatische Updates, falls möglich

> [Warnung]: Der Recorder zeichnet alle Ihre Klicks auf. Bereiten Sie Ihren Arbeitsbereich daher im Voraus vor!

---

## Erstellung eines neuen Guides

### Phase 1: Konfiguration des Guides

1. Füllen Sie im Hauptfenster des FlowGuide Recorders die folgenden Felder aus:

   **Name des Guides** (erforderlich)
   ```
   Beispiel: "Erstellung eines neuen Dokuments in Word"
   ```
   
   **Autor** (erforderlich, standardmäßig Ihr Systembenutzername)
   ```
   Beispiel: "Max Mustermann"
   ```
   
   **Beschreibung** (optional)
   ```
   Beispiel: "Schritt-für-Schritt-Anleitung zur Erstellung eines neuen Dokuments 
   aus einer Vorlage und zum Speichern im angegebenen Ordner"
   ```

2. Klicken Sie auf die Schaltfläche **"Record New Guide"**.

### Phase 2: Start der Aufzeichnung

**Nach dem Klicken auf die Schaltfläche:**

1. Das Hauptfenster des Recorders wird minimiert.
2. Ein schwebendes **Control Panel** (Bedienfeld) erscheint mit folgenden Schaltflächen:
   * **[Start Recording]** - Aufzeichnung starten
   * **[Pause]** - Aufzeichnung pausieren
   * **[Stop]** - Aufzeichnung stoppen und beenden

3. Klicken Sie auf **"Start Recording"**.

**Was als Nächstes passiert:**
* Ein **roter Rahmen** markiert die Elemente unter Ihrem Mauszeiger.
* Dies zeigt an, dass die Aufzeichnung aktiv ist.
* Das Bedienfeld bleibt sichtbar; Sie können es an eine beliebige Stelle auf dem Bildschirm ziehen.

> [Tipp]: Positionieren Sie das Bedienfeld so, dass es die Steuerelemente der Anwendung nicht verdeckt, aber dennoch leicht erreichbar bleibt.

### Phase 3: Aktionen aufzeichnen

**Führen Sie nun die Aktionen aus, die Sie erfassen möchten:**

#### Beispiel: Dokument in Word erstellen

1. **Bewegen Sie den Mauszeiger** über die Registerkarte "Datei".
   * Sie sehen einen roten Rahmen um die Registerkarte.
   
2. **Klicken** Sie auf "Datei".
   * Der Schritt wird aufgezeichnet!
   * Das System generiert automatisch die Anweisung: "Klicken Sie auf die Registerkarte 'Datei'"
   * Ein zugeschnittener Screenshot der Schaltfläche wird erfasst.

3. **Klicken** Sie auf "Neu".
   * Ein weiterer Schritt wird aufgezeichnet!
   * Anweisung: "Klicken Sie auf die Schaltfläche 'Neu'"

4. Fahren Sie mit der Durchführung der erforderlichen Schritte fort...

**Wichtige Richtlinien:**

* Klicken Sie präzise auf die Elemente; das System verlässt sich auf die genauen Steuerelementeigenschaften.
* Machen Sie zwischen den Klicks eine Pause von 1-2 Sekunden, damit das System einen sauberen Screenshot erfassen kann.
* Nehmen Sie sich Zeit; eine langsame und präzise Aufzeichnung liefert bessere Ergebnisse.
* Klicken Sie NICHT auf leere Bereiche, da dies unnötige Schritte erzeugt.

### Phase 4: Pause verwenden

**Wenn Sie eine Pause machen oder Vorbereitungsschritte durchführen müssen:**

1. Klicken Sie auf **[Pause]** im Bedienfeld.
2. Die Aufzeichnung wird unterbrochen.
3. Sie können:
   * Den nächsten Anwendungszustand vorbereiten.
   * Die bisher aufgezeichneten Schritte überprüfen.
   * Eine kurze Pause machen.
4. Klicken Sie auf **"Start Recording"**, um fortzufahren.

> [Tipp] Wann die Pause verwendet werden sollte:
> * Sie müssen komplexe Daten manuell eingeben, ohne dass dies aufgezeichnet wird.
> * Sie warten darauf, dass eine Anwendung oder Daten geladen werden.
> * Sie möchten überprüfen, was bisher aufgezeichnet wurde.

### Phase 5: Aufzeichnung stoppen

**Wenn Sie die Sequenz abgeschlossen haben:**

1. Klicken Sie auf **[Stop]** im Bedienfeld.
2. Das Bedienfeld schließt sich.
3. Das Fenster des **Schritt-Editors** (Step Editor) öffnet sich automatisch.

---

## Bearbeitung eines Guides

### Schritt-Editor-Fenster

Nach dem Stoppen der Aufzeichnung sehen Sie eine Liste aller erfassten Schritte:

```
Step Editor - 8 steps recorded

[1] Klicken Sie auf die Registerkarte "Datei"
Type: LeftClick
[Edit] [Delete]

[2] Klicken Sie auf die Schaltfläche "Neu"
Type: LeftClick
[Edit] [Delete]

[3] Geben Sie Text in das Feld "Name" ein
Type: LeftClick
[Edit] [Delete]

...
```

### Bearbeitung eines einzelnen Schritts

**Um einen Schritt zu ändern:**

1. Klicken Sie auf die Schaltfläche **[Edit]** neben dem Zielschritt.
2. Der Dialog zur Schrittbearbeitung öffnet sich.

**Was Sie anpassen können:**

#### 1. Anweisungstext
```
Vorher: "Klicken Sie auf die Schaltfläche 'Neu'"

Geändert:
"Klicken Sie auf die Schaltfläche 'Neu', um ein neues Dokument aus der Vorlage zu erstellen"
```

> [Tipp]: Formulieren Sie Anweisungen klar, präzise und beschreibend!

#### 2. Aktionsart

**Normal Click**
* Wird für Standardklicks verwendet.
* Der Benutzer muss den Klick während der Wiedergabe manuell ausführen.

**System Action**
* Wird für native Windows-UI-Elemente verwendet.
* Kann vom Player automatisch ausgeführt werden.

**Wann System Actions verwendet werden sollten:**

| Aktion | Typ auswählen |
|---|---|
| Startmenü öffnen | `OpenStartMenu` |
| Windows-Einstellungen öffnen | `OpenSettings` |
| Datei-Explorer öffnen | `OpenFileExplorer` |
| Task-Manager öffnen | `OpenTaskManager` |

#### 3. MS-Settings-Befehl (für System Actions)

Wenn Sie **System Action: OpenSettings** ausgewählt haben, können Sie einen direkten Befehl für die Einstellungskategorie angeben:

```
ms-settings:display          - Anzeigeeinstellungen
ms-settings:network          - Netzwerk und Internet
ms-settings:privacy          - Datenschutzeinstellungen
ms-settings:windowsupdate    - Windows Update
```

#### 4. Auto-Execute-Schaltfläche anzeigen

* Aktiviert (Standard): Dem Benutzer wird während der Wiedergabe eine Schaltfläche "Automatisch ausführen" angezeigt.
* Deaktiviert: Der Benutzer muss die Aktion manuell ausführen.

### Schritte löschen

**Wenn Sie versehentlich eine Aktion aufgezeichnet haben:**

1. Suchen Sie den Schritt in der Liste.
2. Klicken Sie auf **[Delete]**.
3. Bestätigen Sie den Löschvorgang.

> [Warnung]: Das Löschen eines Schritts kann nicht rückgängig gemacht werden! Gehen Sie vorsichtig vor.

### Reihenfolge der Schritte ändern

**Das Ändern der Reihenfolge direkt in der Benutzeroberfläche wird derzeit nicht unterstützt.**

**Workaround:**
1. Schließen Sie den Editor, ohne zu speichern.
2. Zeichnen Sie den Arbeitsablauf in der richtigen Reihenfolge erneut auf.

> [Tipp]: Planen Sie Ihre Aktionen vor der Aufzeichnung sorgfältig!

### Speichern des Guides

**Wenn die Bearbeitung abgeschlossen ist:**

1. Klicken Sie auf die grüne Schaltfläche **"Save Guide"** unten rechts.
2. Der Guide-Datensatz wird gespeichert unter:
   ```
   C:\Users\IhrName\Documents\FlowGuide\GuideName\
   ```
3. Eine Bestätigungsmeldung erscheint: "Guide erfolgreich gespeichert!"

**Gespeicherte Dateien:**
* `guide.json` - Konfigurations- und Schrittbeschreibungs-Metadaten.
* `step_1.png`, `step_2.png`, ... - zugeschnittene Screenshots der Steuerelemente.

### Abbrechen

**Wenn Sie die Aufzeichnung verwwerfen möchten:**

1. Klicken Sie auf die graue Schaltfläche **"Cancel"**.
2. Der Guide wird NICHT gespeichert.
3. Alle aufgezeichneten Schritte werden verworfen.

---

## Bearbeitung eines bestehenden Guides

### Guide zur Bearbeitung laden

1. Klicken Sie im Hauptfenster des FlowGuide Recorders на **"Edit Existing Guide"**.
2. Wählen Sie die Datei **guide.json** aus dem Verzeichnis des Guides aus:
   ```
   C:\Users\IhrName\Documents\FlowGuide\GuideName\guide.json
   ```
3. Der Schritt-Editor öffnet sich mit den geladenen Schritten.

### Verfügbare Aktionen

* Anweisungstext bearbeiten.
* Aktionsart und -einstellungen ändern.
* Schritte löschen.
* Hinweis: Das Hinzufügen neuer Schritte wird nicht unterstützt (erfordert eine neue Aufzeichnung).

### Änderungen speichern

1. Klicken Sie auf **"Save Guide"**.
2. Die Änderungen überschreiben die vorhandenen Dateien.
3. Die vorherige Version wird dauerhaft ersetzt.

> [Tipp]: Sichern Sie den Guide-Ordner, bevor Sie Änderungen vornehmen!

---

## Tipps und Best Practices

### Erstellung hochwertiger Guides

#### 1. Planung

**Entwerfen Sie vor der Aufzeichnung einen Plan:**

```
Beispielplan für "Dokument in Word speichern":

1. Menü Datei öffnen
2. Speichern unter auswählen
3. Auf Durchsuchen klicken, um den Pfad auszuwählen
4. Dateinamen in das entsprechende Feld eingeben
5. Auf Speichern klicken
```

* Schreiben Sie den Plan auf.
* Überprüfen Sie, ob die Schritte logisch sind.
* Stellen Sie sicher, dass keine Schritte fehlen.

#### 2. Vorbereitung des Arbeitsbereichs

**Sorgen Sie für ideale Bedingungen bei der Aufzeichnung:**

* Schließen Sie alle nicht benötigten Fenster.
* Räumen Sie Ihren Desktop auf und blenden Sie persönliche Informationen aus.
* Aktivieren Sie "Nicht stören", um Benachrichtigungen stummzuschalten.
* Bereiten Sie Testdaten im Voraus vor.
* Vergewissern Sie sich, dass die Zielanwendung stabil läuft.

#### 3. Aufzeichnungsprozess

**Goldene Regeln für die Aufzeichnung:**

1. **Langsam und präzise**
   * Machen Sie zwischen den Aktionen eine Pause von 2-3 Sekunden.
   * Geben Sie dem System genügend Zeit, um die Screenshots zu erfassen und zuzuschneiden.

2. **Klicken Sie in die Mitte der Steuerelemente**
   * Dies verbessert die UI-Elementerkennung der UI Automation.
   * Vermeiden Sie Klicks in der Nähe von Rändern.

3. **Standard-Navigationspfade verwenden**
   * Wählen Sie den intuitivsten Weg.
   * Beispiel: "Datei -> Neu" ist besser als "Strg + N".

4. **Eine Aktion pro Schritt**
   * Halten Sie die Schritte einfach.
   * Es ist besser, viele einfache Schritte zu haben als wenige komplizierte.

#### 4. Texte formatieren

**Schreiben Sie klare Anweisungen:**

Schlecht:
```
"Auf die Schaltfläche klicken"
```

Gut:
```
"Klicken Sie auf die Schaltfläche 'Neu' in der oberen linken Ecke des Bildschirms"
```

Besser:
```
"Klicken Sie auf die grüne Schaltfläche 'Neu' in der oberen linken Ecke, um ein neues Dokument aus einer Vorlage zu erstellen"
```

**Fügen Sie hinzu:**
* Detaillierte Beschreibungen.
* Ziele ("um X zu tun").
* Wegbeschreibungen ("in der Ecke").
* Visuelle Details ("grüne Schaltfläche").

### Spezielle Szenarien

#### Formularfelder

**Beim Ausfüllen mehrerer Eingabefelder:**

1. Zeichnen Sie den Klick auf jedes Textfeld einzeln auf.
2. Geben Sie in der Anweisung an, was eingegeben werden soll:
   ```
   "Geben Sie den Namen des Dokuments in das Feld 'Dateiname' ein.
   Beispiel: 'Bericht_2026'"
   ```

#### Dropdowns

1. Klicken Sie auf das Dropdown-Steuerelement.
2. Klicken Sie auf das Zielelement.
3. Definieren Sie den Wert klar:
   ```
   "Wählen Sie 'Microsoft Word' aus der Dropdown-Liste der Programme aus"
   ```

#### Dialogfenster

**Problem:** Pop-up-Fenster können sich an unterschiedlichen Bildschirmkoordinaten öffnen.

**Lösung:**
1. Verlassen Sie sich nicht auf feste Bildschirmpositionen.
2. Referenzieren Sie Schaltflächen mit ihren sichtbaren Namen:
   ```
   Gut: "Klicken Sie auf die Schaltfläche 'OK' im Bestätigungsdialog"
   Schlecht: "Klicken Sie in die untere rechte Ecke"
   ```

#### Wartezeiten

**Wenn ein Schritt Zeit benötigt:**

1. Pausieren Sie die Aufzeichnung.
2. Warten Sie, bis der Vorgang abgeschlossen ist.
3. Setzen Sie die Aufzeichnung fort.
4. Fügen Sie der Anweisung Details zum Warten hinzu:
   ```
   "Warten Sie, bis das Dokument vollständig geladen ist (die Statusanzeige verschwindet)"
   ```

### Optimale Guide-Länge

**Empfehlungen:**

* **Kurze Guides (3-7 Schritte)**: Für grundlegende Aufgaben.
  * Beispiel: "So speichern Sie ein Dokument"
  
* **Mittlere Guides (8-15 Schritte)**: Standardrichtlinien.
  * Beispiel: "Erstellen und Senden einer E-Mail in Outlook"
  
* **Lange Guides (16-30 Schritte)**: Selektiv verwenden.
  * Beispiel: "Erstellung eines komplexen Berichts in der Buchhaltungssoftware"
  
* **Sehr lange Guides (über 30 Schritte)**: In mehrere Tutorials aufteilen.
  * Beispiel: "Teil 1: Einrichtung", "Teil 2: Erstellung"

---

## Fehlerbehebung

### Roter Highlight-Rahmen erscheint nicht

**Ursachen und Lösungen:**

1. **Aufzeichnung ist nicht aktiv**
   * Klicken Sie auf "Start Recording" im Bedienfeld.

2. **Mauszeiger befindet sich über dem Desktop-Hintergrund**
   * Bewegen Sie den Mauszeiger über ein gültiges Steuerelement.

3. **Anwendung wird nicht unterstützt**
   * Überprüfen Sie, ob die Aufzeichnung in anderen Fenstern funktioniert (Word, Datei-Explorer).
   * Beachten Sie, dass einige Anwendungen mit benutzerdefinierten Grafik-Engines keine UI Automation-Eigenschaften freigeben.

### Schritt fehlerhaft erfasst

**Was zu tun ist:**

1. Stoppen Sie die Aufzeichnung NICHT.
2. Drücken Sie **[Pause]**.
3. Planen Sie die korrekte Aktion.
4. Klicken Sie auf **"Start Recording"**, um fortzufahren.
5. Zeichnen Sie den korrekten Schritt auf.
6. Löschen Sie den fehlerhaften Schritt später im Schritt-Editor.

### Screenshot nicht gespeichert

**Ursachen:**
* Klicks wurden zu schnell nacheinander ausgeführt.
* Das System hatte nicht genügend Zeit, um das Bild zu erfassen.

**Lösung:**
* Machen Sie zwischen den Mausaktionen eine Pause von 2-3 Sekunden.
* Vermeiden Sie Hektik.

### "No screenshot available for this step"

Dies ist zu erwarten, wenn:
* Der Schritt eine System Action ist.
* Sich das Anwendungsfenster zu schnell geschlossen oder verändert hat.

Der Guide funktioniert weiterhin korrekt; Screenshots sind für die Wiedergabe nicht zwingend erforderlich.

### Speichern schlägt fehl

**Überprüfen Sie:**

1. Sie haben einen Guide-Namen angegeben.
2. Sie haben den Namen des Autors eingegeben.
3. Ihr Benutzerkonto hat Schreibrechte für den Ordner Dokumente.
4. Es ist genügend Speicherplatz auf dem Laufwerk vorhanden.

### Schritt-Editor öffnet sich nicht

**Lösung:**

1. Schließen Sie den FlowGuide Recorder.
2. Starten Sie ihn erneut als Administrator.
3. Versuchen Sie, die Aufzeichnung über "Edit Existing Guide" zu laden.

### Anwendung hängt sich auf

**Schritte:**

1. Öffnen Sie den Task-Manager (Strg + Umschalt + Esc).
2. Wählen Sie "FlowGuide.Recorder.exe" aus.
3. Klicken Sie auf "Task beenden".
4. Starten Sie das Programm neu.

> [Warnung]: Nicht gespeicherte Aufzeichnungen gehen verloren!

---

## Beispiele für Guides

### Beispiel 1: Einfacher Guide (5 Schritte)

**"So speichern Sie ein Dokument in Word"**

1. Klicken Sie auf die Registerkarte "Datei" in der oberen linken Ecke des Bildschirms.
2. Wählen Sie "Speichern unter" aus dem Menü.
3. Klicken Sie auf "Durchsuchen", um den Speicherort auszuwählen.
4. Geben Sie den Namen des Dokuments in das Feld "Dateiname" ein.
5. Klicken Sie auf die Schaltfläche "Speichern".

### Beispiel 2: Mittlerer Guide (12 Schritte)

**"Erstellung eines Kalenderereignisses in Windows"**

1. Drücken Sie die Windows-Taste (öffnet das Startmenü) - System Action.
2. Suchen Sie nach der Anwendung "Kalender" und klicken Sie darauf.
3. Klicken Sie auf die Schaltfläche "Neues Ereignis" in der oberen linken Ecke.
4. Geben Sie den Titel des Ereignisses in das Feld "Ereignisname" ein.
5. Klicken Sie auf die Auswahl für das Startdatum.
6. Wählen Sie das Datum im Kalender aus.
7. Klicken Sie auf das Feld für die Startzeit.
8. Wählen Sie die Startzeit aus.
9. Klicken Sie auf die Auswahl für das Enddatum.
10. Wählen Sie das Enddatum aus.
11. Fügen Sie eine kurze Beschreibung im Details-Feld hinzu.
12. Klicken Sie auf die Schaltfläche "Speichern".

### Beispiel 3: System Actions

**"Anzeigeeinstellungen in Windows öffnen"**

1. Drücken Sie die Windows-Taste (öffnet das Startmenü) - System Action: OpenStartMenu.
2. Windows-Einstellungen öffnen - System Action: OpenSettings, ms-settings:display.
3. (Die Seite mit den Anzeigeeinstellungen öffnet sich automatisch).

---

## Checkliste zur Qualitätssicherung

Überprüfen Sie vor dem Speichern Ihres Guides folgende Details:

### Vorbereitung
- [ ] Der Arbeitsablaufplan wurde entworfen.
- [ ] Der aktive Arbeitsbereich wurde aufgeräumt.
- [ ] Eingaben und Testparameter sind vorbereitet.
- [ ] Benachrichtigungen sind stummgeschaltet.

### Aufzeichnung
- [ ] Zwischen den Aktionen wurden Pausen von 2-3 Sekunden eingehalten.
- [ ] Es wurden keine redundanten Mausklicks erfasst.
- [ ] Alle kritischen Schritte sind enthalten.
- [ ] Die Reihenfolge ist logisch.

### Bearbeitung
- [ ] Die Anweisungen sind beschreibend und detailliert.
- [ ] Die Ziele der Aktionen sind angegeben ("um X zu tun").
- [ ] Der Text ist frei von Rechtschreibfehlern.
- [ ] Redundante Schritte wurden gelöscht.
- [ ] System Actions sind korrekt konfiguriert (falls vorhanden).

### Überprüfung
- [ ] Der Name des Guides ist beschreibend.
- [ ] Die Beschreibung ist ausgefüllt.
- [ ] Der Name des Autors ist festgelegt.
- [ ] Der Datensatz wurde gespeichert.

### Testing
- [ ] Der Guide wurde im FlowGuide Player geladen und überprüft.
- [ ] Alle Spotlight-Bereiche richten sich korrekt aus.
- [ ] Die Anweisungen sind für Dritte verständlich.

---

## Kontakt und Support

**Benötigen Sie Hilfe?**

1. Lesen Sie den Abschnitt [Fehlerbehebung](#fehlerbehebung).
2. Wenden Sie sich an Ihren Systemadministrator.
3. Öffnen Sie ein Issue im Projekt-Repository.

---

## Fazit

Herzlichen Glückwunsch! Sie sind nun bereit, interaktive Tutorials mit FlowGuide zu erstellen!

**Denken Sie daran:**
* Eine gute Vorbereitung führt zu einem guten Guide.
* Eine präzise und langsame Aufzeichnung sorgt für stabile Screenshots.
* Die Bearbeitung verbessert die Klarheit der Anweisungen.
* Testen Sie Ihren Guide immer, bevor Sie ihn teilen.

**Viel Erfolg!**
