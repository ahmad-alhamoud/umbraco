Umbraco Review Platform

Inhaltsverzeichnis

Einführung\
Projektstruktur\
Datenspeicherung\
Funktionen\
Frontend\
Architektur\
Sicherheit\
Fehlerbehandlung\
Entwicklungsumgebung\
Technologien\
Autorin

* * * * *

Einführung

Umbraco Review Platform ist eine webbasierte Bewertungsplattform, die es Benutzern ermöglicht, ihre Erfahrungen mit Umbraco CMS zu teilen. Die Anwendung wurde mit dem .NET-basierten CMS Umbraco entwickelt und bietet eine benutzerfreundliche Oberfläche für das Abgeben von Bewertungen sowie ein Admin-Dashboard zur Verwaltung dieser Bewertungen.

Hauptmerkmale

Benutzerbewertungen -- Besucher können Bewertungen mit 1 bis 5 Sternen und detailliertem Feedback abgeben\
Admin-Dashboard -- Administratoren können Bewertungen genehmigen oder ablehnen\
Like-Funktion -- Benutzer können hilfreiche Bewertungen liken\
Moderation -- Neue Bewertungen müssen vom Admin freigegeben werden\
Responsive Design -- Optimierte Darstellung auf Desktop, Tablet und Smartphone

* * * * *

Projektstruktur

Das Projekt folgt einer klaren und wartbaren Struktur, die den MVC-Pattern von [ASP.NET](https://asp.net/) Core und Umbraco folgt.

UmbracoReviewSite\
Controllers\
BewertungsApiController.cs - API-Endpunkte für Admin-Funktionen\
BewertungsOberflaechenController.cs - Surface Controller für Formulare\
StartseiteController.cs - Home Controller für einfache Aktionen\
Models\
Bewertung.cs - Datenmodell für Bewertungen\
IBewertungsRepository.cs - Repository-Interface\
MockBewertungsRepository.cs - In-Memory Repository keine Datenbank\
Views\
Startseite.cshtml - Hauptseite mit Bewertungsformular\
AdminDashboard.cshtml - Admin-Dashboard zur Verwaltung\
umbraco - Umbraco CMS Systemdateien\
appsettings.json - Anwendungskonfiguration\
Program.cs - Anwendungsstartpunkt\
UmbracoReviewSite.csproj - Projektdatei mit Abhängigkeiten

* * * * *

Datenspeicherung

Mock Repository

Das Projekt verwendet keine echte Datenbank, sondern ein In-Memory Repository MockBewertungsRepository. Dies ermöglicht eine schnelle Entwicklung ohne Datenbank-Konfiguration. Alle Daten werden im Arbeitsspeicher gehalten und gehen beim Neustart der Anwendung verloren.

Vorteile\
Keine separate Datenbankinstallation erforderlich\
Schnelle Entwicklung und Tests\
Ideal für Präsentationen und Demos

* * * * *

Funktionen

Benutzerbewertungen

Besucher können auf der Startseite eine Bewertung abgeben. Das Formular enthält:\
Name -- Pflichtfeld\
E-Mail -- Pflichtfeld, wird auf Gültigkeit geprüft\
Sternebewertung -- 1 bis 5 Sterne als Radio-Buttons\
Feedback -- Textfeld für detaillierte Rückmeldung

Nach dem Absenden wird die Bewertung im Arbeitsspeicher gespeichert, jedoch noch nicht öffentlich angezeigt. Der Administrator muss sie zuerst genehmigen.

Admin-Dashboard

Das Admin-Dashboard ist unter admin-dashboard?key=admin123 erreichbar und bietet folgende Funktionen:\
Übersicht -- Anzahl der ausstehenden Bewertungen\
Bewertungsliste -- Tabelle mit allen nicht genehmigten Bewertungen\
Genehmigen Ablehnen -- Ein-Klick-Aktionen mit Bestätigung\
Automatische Aktualisierung -- Die Liste aktualisiert sich nach jeder Aktion

Like-Funktion

Benutzer können hilfreiche Bewertungen liken. Die Like-Funktion:\
Einfach -- Ein Klick auf das Herz-Icon\
Echtzeit -- Die Anzahl der Likes wird ohne Seitenneuladen aktualisiert\
Persönlich -- Jeder Benutzer kann eine Bewertung nur einmal liken via localStorage

* * * * *

Frontend

Design-System

Das Frontend verwendet ein konsistentes Design-System mit CSS-Variablen.

primary-red e30613\
dark-red c10510\
light-red fce4e4\
light-gray f8fafc\
border-gray e2e8f0\
text-dark 1e293b\
text-light 64748b\
white ffffff

Lange Kommentare

Kommentare über 150 Zeichen werden automatisch gekürzt und erhalten einen Mehr lesen-Button.

* * * * *

Architektur

Repository-Pattern

Das Repository-Pattern trennt die Datenzugriffslogik von der Geschäftslogik. Dies ermöglicht:\
Austauschbarkeit -- Leichter Wechsel zwischen verschiedenen Datenquellen\
Testbarkeit -- Einfaches Mocken für Unit-Tests\
Wartbarkeit -- Zentrale Datenzugriffslogik

Surface Controller

Surface Controller werden für Formulare und Benutzerinteraktionen verwendet. Sie bieten:\
Anti-Forgery-Token -- Schutz gegen CSRF-Angriffe\
TempData -- Temporäre Datenspeicherung zum Beispiel Erfolgsmeldungen\
Redirect -- Umleitung nach Formularabsendung

API Controller

API Controller werden für asynchrone JavaScript-Anfragen verwendet:\
Genehmigen Ablehnen -- Admin-Aktionen\
Laden von Bewertungen -- Automatische Aktualisierung\
Like-Funktion -- Echtzeit-Updates

* * * * *

Sicherheit

Anti-Forgery Token

Alle Formulare sind mit Anti-Forgery-Token geschützt.

XSS-Schutz

Benutzereingaben werden beim Anzeigen escaped, um Cross-Site-Scripting XSS zu verhindern.

Admin-Zugang

Der Admin-Bereich ist mit einem einfachen Passwortschutz gesichert key=admin123. Bei Bedarf kann dies durch ein robustes Authentifizierungssystem ersetzt werden.

* * * * *

Fehlerbehandlung

Fehler werden benutzerfreundlich angezeigt:\
Formularvalidierung -- Alle Felder werden serverseitig geprüft\
Repository-Fehler -- Fehlermeldungen werden in TempData gespeichert\
JavaScript-Fehler -- Konsolenausgaben für Entwickler, Benutzer erhalten freundliche Meldungen

* * * * *

Entwicklungsumgebung

Voraussetzungen

NET SDK 8.0 oder höher\
Umbraco Templates -- dotnet new install Umbraco.Templates\
Visual Studio 2022 oder VS Code mit C-Erweiterung\
Keine Datenbank erforderlich -- Das Projekt verwendet In-Memory-Speicherung

Projekt ausführen

Neues Projekt erstellen\
dotnet new umbraco --name UmbracoReviewSite\
cd UmbracoReviewSite

SQLite-Paket hinzufügen für Umbraco\
dotnet add package Microsoft.Data.Sqlite

Projekt ausführen\
dotnet run

Nach dem Start ist die Anwendung unter https://localhost:xxxxx erreichbar. Der Admin-Bereich ist unter admin-dashboard?key=admin123 verfügbar.

Entwicklung mit VS Code

Empfohlene Erweiterungen:\
C von Microsoft\
Razor für Syntax-Highlighting

* * * * *

Technologien

Technologie Beschreibung\
NET 8.0 Plattform für die Anwendung\
Umbraco 13 Content Management System\
SQLite Datenbank für Umbraco\
Mock Repository In-Memory-Speicherung für Bewertungen keine Datenbank\
Razor Templating-Engine für Views\
CSS Grid Flexbox Responsive Layout\
Font Awesome Icons für Like-Buttons\
Git Versionskontrolle

* * * * *

Autorin

Rita -- Entwicklerin der Umbraco Review Platform