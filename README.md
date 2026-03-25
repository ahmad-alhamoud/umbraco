# Umbraco Review Platform

## 📋 Inhaltsverzeichnis

- [Einführung](#einführung)
- [Projektstruktur](#projektstruktur)
- [Datenspeicherung](#datenspeicherung)
- [Funktionen](#funktionen)
- [Frontend](#frontend)
- [Architektur](#architektur)
- [Entwicklungsumgebung](#entwicklungsumgebung)
- [Autorin](#autorin)

---

## Einführung

**Umbraco Review Platform** ist eine webbasierte Bewertungsplattform, die es Benutzern ermöglicht, ihre Erfahrungen mit Umbraco CMS zu teilen. Die Anwendung wurde mit dem .NET-basierten CMS Umbraco entwickelt und bietet eine benutzerfreundliche Oberfläche für das Abgeben von Bewertungen sowie ein Admin-Dashboard zur Verwaltung dieser Bewertungen.

### Hauptmerkmale

- ✅ **Benutzerbewertungen** – Besucher können Bewertungen mit 1–5 Sternen und detailliertem Feedback abgeben
- ✅ **Admin-Dashboard** – Administratoren können Bewertungen genehmigen oder ablehnen
- ✅ **Like-Funktion** – Benutzer können hilfreiche Bewertungen liken
- ✅ **Moderation** – Neue Bewertungen müssen vom Admin freigegeben werden
- ✅ **Responsive Design** – Optimierte Darstellung auf Desktop, Tablet und Smartphone

---

## Projektstruktur

Das Projekt folgt einer klaren und wartbaren Struktur, die den MVC-Pattern (Model-View-Controller) von ASP.NET Core und Umbraco folgt.

UmbracoReviewSite/
├── Controllers/
│ ├── BewertungsApiController.cs # API-Endpunkte für Admin-Funktionen
│ ├── BewertungsOberflaechenController.cs # Surface Controller für Formulare
│ └── StartseiteController.cs # Home Controller für einfache Aktionen
├── Models/
│ ├── Bewertung.cs # Datenmodell für Bewertungen
│ ├── IBewertungsRepository.cs # Repository-Interface
│ └── MockBewertungsRepository.cs # In-Memory Repository (keine Datenbank)
├── Views/
│ ├── Startseite.cshtml # Hauptseite mit Bewertungsformular
│ └── AdminDashboard.cshtml # Admin-Dashboard zur Verwaltung
├── umbraco/ # Umbraco CMS Systemdateien
├── appsettings.json # Anwendungskonfiguration
├── Program.cs # Anwendungsstartpunkt
└── UmbracoReviewSite.csproj # Projektdatei mit Abhängigkeiten


---

## Datenspeicherung

### Mock Repository

Das Projekt verwendet **keine echte Datenbank**, sondern ein In-Memory Repository (`MockBewertungsRepository`). Dies ermöglicht eine schnelle Entwicklung ohne Datenbank-Konfiguration. Alle Daten werden im Arbeitsspeicher gehalten und gehen beim Neustart der Anwendung verloren.

**Vorteile:**
- Keine separate Datenbankinstallation erforderlich
- Schnelle Entwicklung und Tests
- Ideal für Präsentationen und Demos

```csharp
public class MockBewertungsRepository : IBewertungsRepository
{
    private static List<Bewertung> _bewertungen = new List<Bewertung>();
    private static int _naechsteId = 1;

    public Task Hinzufuegen(Bewertung bewertung)
    {
        bewertung.Id = _naechsteId++;
        bewertung.ErstellungsDatum = DateTime.Now;
        bewertung.Likes = 0;
        bewertung.IstGenehmigt = false;
        _bewertungen.Add(bewertung);
        return Task.CompletedTask;
    }
}

##Funktionen
Benutzerbewertungen
Besucher können auf der Startseite eine Bewertung abgeben. Das Formular enthält:

Name – Pflichtfeld

E-Mail – Pflichtfeld, wird auf Gültigkeit geprüft

Sternebewertung – 1–5 Sterne als Radio-Buttons

Feedback – Textfeld für detaillierte Rückmeldung

Nach dem Absenden wird die Bewertung im Arbeitsspeicher gespeichert, jedoch noch nicht öffentlich angezeigt. Der Administrator muss sie zuerst genehmigen.

Admin-Dashboard
Das Admin-Dashboard ist unter /admin-dashboard?key=admin123 erreichbar und bietet folgende Funktionen:

Übersicht – Anzahl der ausstehenden Bewertungen

Bewertungsliste – Tabelle mit allen nicht genehmigten Bewertungen

Genehmigen/Ablehnen – Ein-Klick-Aktionen mit Bestätigung

Automatische Aktualisierung – Die Liste aktualisiert sich nach jeder Aktion

Like-Funktion
Benutzer können hilfreiche Bewertungen liken. Die Like-Funktion:

Einfach – Ein Klick auf das Herz-Icon

Echtzeit – Die Anzahl der Likes wird ohne Seitenneuladen aktualisiert

Persönlich – Jeder Benutzer kann eine Bewertung nur einmal liken (via localStorage)

##Frontend
Design-System
Das Frontend verwendet ein konsistentes Design-System mit CSS-Variablen:

:root {
    --primary-red: #e30613;
    --dark-red: #c10510;
    --light-red: #fce4e4;
    --light-gray: #f8fafc;
    --border-gray: #e2e8f0;
    --text-dark: #1e293b;
    --text-light: #64748b;
    --white: #ffffff;
}

## Architektur

### Repository-Pattern

Das Repository-Pattern trennt die Datenzugriffslogik von der Geschäftslogik. Dies ermöglicht:

- **Austauschbarkeit** – Leichter Wechsel zwischen verschiedenen Datenquellen
- **Testbarkeit** – Einfaches Mocken für Unit-Tests
- **Wartbarkeit** – Zentrale Datenzugriffslogik

```csharp
public interface IBewertungsRepository
{
    Task<IEnumerable<Bewertung>> HoleBewertungenFuerSeite(int seitenId, bool nurGenehmigte = true);
    Task<Bewertung?> HoleBewertungNachId(int id);
    Task Hinzufuegen(Bewertung bewertung);
    Task Aktualisieren(Bewertung bewertung);
    Task Loeschen(int id);
    Task<int> LikeBewertung(int bewertungsId);
    Task<IEnumerable<Bewertung>> HoleAusstehendeBewertungen();
}


API Controller
API Controller werden für asynchrone JavaScript-Anfragen verwendet:

Genehmigen/Ablehnen – Admin-Aktionen

Laden von Bewertungen – Automatische Aktualisierung

Like-Funktion – Echtzeit-Updates

[HttpPost]
public IActionResult Genehmigen(int bewertungsId)
{
    try
    {
        var bewertung = _bewertungsRepository.HoleBewertungNachId(bewertungsId).Result;
        if (bewertung == null)
            return NotFound("Bewertung nicht gefunden.");

        bewertung.IstGenehmigt = true;
        _bewertungsRepository.Aktualisieren(bewertung).Wait();

        return Redirect("/admin-dashboard?key=admin123");
    }
    catch (Exception ex)
    {
        return BadRequest($"Fehler: {ex.Message}");
    }
}


##Entwicklungsumgebung
Voraussetzungen
.NET SDK 8.0 oder höher

Umbraco Templates – dotnet new install Umbraco.Templates

Visual Studio 2022 oder VS Code mit C#-Erweiterung

Keine Datenbank erforderlich – Das Projekt verwendet In-Memory-Speicherung

##Autorin
Rita – Entwicklerin der Umbraco Review Platform

