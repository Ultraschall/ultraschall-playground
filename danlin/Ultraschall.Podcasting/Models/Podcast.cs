using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Ultraschall.Podcasting.Models
{
    public class Podcast
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public List<Link> Links { get; }


        /// <summary>
        /// &lt;language&gt;
        /// Da der iTS weltweit verfügbar ist, ist es wichtig, die Sprache eines Podcasts anzugeben. Akzeptiert werden die Werte in
        /// der ISO 639-1 Alpha-2 Liste (Sprachcodes aus zwei Buchstaben, einige davon mit möglichen Modifikatoren wie "en-us" oder "de-de").
        /// </summary>
        public string Language { get; set; }

        public DateTimeOffset LastUpdatedTime { get; set; }


        /// <summary>
        /// &lt;itunes:author&gt; 
        /// Der Inhalt dieses Tags wird in der Spalte "Interpret" in iTunes angezeigt. Ist das Tag nicht vorhanden, 
        /// verwendet iTunes den Inhalt des Tags &lt;author&gt;. Falls das &lt;itunes:author&gt;- Tag auf Feed-Ebene nicht vorhanden ist, 
        /// verwendet iTunes den Inhalt des &lt;managingEditor&gt;-Tags.
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// &lt;itunes:block&gt;
        /// Dieses Tag wird innerhalb eines &lt;channel&gt;- Elements verwendet, um zu verhindern, 
        /// dass der gesamte Podcast im Podcast-Verzeichnis von iTunes angezeigt wird. 
        /// Wird es innerhalb eines &lt;item&gt;- Elements verwendet, verhindert es, dass die jeweilige 
        /// Folge im Podcast-Verzeichnis von iTunes angezeigt wird. Dies empfiehlt sich beispielsweise, 
        /// wenn eine bestimmte Folge von iTunes ausgeschlossen werden soll, da ihr Inhalt dazu führen könnte, 
        /// dass der Feed aus iTunes entfernt wird.
        ///
        /// Ist dieses Tag vorhanden und auf den Wert "yes" (ohne Beachtung von Groß- und Kleinschreibung) gesetzt, 
        /// heißt das, dass der Feed oder die Folge gesperrt werden soll. Ein beliebiger anderer Wert für dieses Tag, 
        /// z. B. eine leere Zeichenfolge, wird als Anweisung interpretiert, den Feed oder die Folge freizugeben. 
        /// Auf Feed-Ebene bleibt der Sperrstatus des Feeds unverändert, wenn kein "block"-Tag vorhanden ist. 
        /// Ist auf Folgenebene kein "block"-Tag vorhanden, ist dies gleichbedeutend mit einer nicht aktivierten Sperre ("block=no").
        /// </summary>
        public bool Block { get; set; }

        /// <summary>
        /// &lt;itunes:category&gt;
        /// Für die Suche in den Podcast-Kategorien in iTunes gibt es zwei Möglichkeiten: über die "Übersicht" im Bereich 
        /// "Alles auf einen Klick" oder durch Auswahl einer Kategorie im Bereich "Kategorien". Der Weg über die Übersicht führt 
        /// zu einer textbasierten Tabelle, der Weg über die Kategorien zu einer Seite mit Podcast-Bildern.
        ///
        /// Eine Liste aller Kategorien und Unterkategorien, die der iTunes Store unterstützt, befindet sich am Ende dieses Dokuments. 
        /// Für eine Platzierung im älteren, textbasierten Übersichtssystem können Podcast-Feeds bis zu drei Angaben zur 
        /// Kategorie/Unterkategorie enthalten. (So zählt beispielsweise "Music" (Musik) als eine Angabe und 
        /// "Business &gt; Careers" (Geschäft &gt; Karriere) ebenso.) Für eine Platzierung im neueren, auf Kategorie-Links basierenden 
        /// System und in den Listen "Top-Charts – Podcasts" und "Top-Charts – Folgen" in der rechten Spalte der meisten Podcast-Seiten 
        /// wird nur die erste im Feed angegebene Kategorie verwendet.
        ///
        /// Kategorien und Unterkategorien können folgendermaßen festgelegt werden. Anhand eines &lt;itunes:category&gt;- Tags auf oberster Ebene 
        /// wird die Suchkategorie festgelegt, während ein eingebettetes &lt;itunes:category&gt;- Tag die Unterkategorie angibt. Dabei kann aus den 
        /// verfügbaren Kategorien und Unterkategorien in iTunes gewählt werden. Das &-Zeichen muss dabei in jedem Fall durch & ersetzt werden. 
        /// Eine vollständige Liste der Kategorien findet sich am Ende dieses Dokuments.
        /// </summary>
        public List<Category> Categories { get; set; }

        /// <summary>
        /// &lt;itunes:image&gt;
        /// Dieses Tag gibt die Grafik für einen Podcast an. Die URL-Adresse für das Bild muss in das "href"-Attribut eingefügt werden. 
        /// iTunes bevorzugt quadratische Bilder im JPG-Format mit einer Größe von mindestens 1400 x 1400 Pixeln und weicht damit von den 
        /// Angaben für das standardmäßige RSS-"image"-Tag ab. Damit ein Podcast für eine Vorstellung im iTS in Frage kommt, 
        /// muss das zugehörige Bild mindestens 1400 x 1400 Pixel groß sein.
        ///
        /// iTunes unterstützt RGB-Bilddateien im JPG- und PNG-Format (CMYK wird nicht unterstützt). Die URL muss auf ".jpg" oder ".png" enden. 
        /// Ist das  Tag &lt;itunes:image&gt; nicht vorhanden, verwendet iTunes den Inhalt des RSS-Tags "image".
        ///
        /// Wird das Bild eines Podcasts geändert, sollte auch der Name der Datei geändert werden. iTunes ändert beim Auslesen des Feeds das Bild
        /// unter Umständen nicht, wenn die URL-Adresse dieselbe ist.
        ///
        /// Es lohnt sich, etwas Zeit in die Erstellung eines ansprechenden, originellen Bildes zu investieren, das den Podcast gut darstellt. 
        /// Potenzielle Abonnenten werden dieses Bild auf der Seite des Podcasts sehen. Eine kleinere Version des Bildes wird in den Suchergebnissen 
        /// und bei einer Vorstellung des Podcasts angezeigt. Das Design sollte in beiden Größen effektiv sein.
        ///
        /// Das &lt;itunes:image&gt;-Tag wird auch auf Folgenebene unterstützt. Die besten Ergebnisse lassen sich durch Verwendung des &lt;itunes:image&gt;-Tags auf
        /// "item"-Ebene und Einbeziehung desselben Bildes in die Metadaten der Mediendatei erzielen. Um in iTunes Bilder in die Metadaten einer 
        /// Folge einzubeziehen, wähle die Folge aus und wähle dann "Informationen" aus dem Menü "Ablage". Klicke auf den Bereich "Cover". 
        /// Klicke dann auf "Hinzufügen", wähle die gewünschte Bilddatei aus und klicke auf "Auswählen".
        /// </summary>
        public Uri Image { get; set; }

        /// <summary>
        /// &lt;itunes:explicit&gt;
        /// Dieses Tag wird verwendet, um anzugeben, ob ein Podcast anstößiges Material enthält. Die drei Werte für das Tag sind "yes", "no" und "clean".
        /// 
        /// Hat dieses Tag den Wert "yes", wird neben dem Podcast-Bild im iTunes Store und in der Spalte "Name" in iTunes der Schriftzug "Explicit" als
        /// Warnung für Erziehungsberechtigte angezeigt. Ist als Wert "clean" festgelegt, wird die Grafik "Clean" angezeigt, was darauf hinweist, 
        /// dass die Folgen keine anstößige Sprache oder nur für Erwachsene geeigneten Inhalte enthalten. Ist dieses Tag vorhanden und weist einen 
        /// anderen Wert auf (z. B. "no"), wird kein Hinweis angezeigt – standardmäßig bleibt dieses Feld leer.
        /// </summary>
        public Explicit Explicit { get; set; }

        /// <summary>
        /// &lt;itunes:complete&gt;
        /// Dieses Tag kann verwendet werden, um anzuzeigen, dass ein Podcast abgeschlossen ist.
        ///
        /// Es wird nur auf &lt;channel&gt;-Ebene unterstützt. Wird für dieses Tag der Wert "yes" festgelegt, zeigt dies an, dass keine weiteren Folgen
        /// zu diesem Podcast hinzugefügt werden. Ist das &lt;itunes:complete&gt;-Tag vorhanden und weist einen anderen Wert auf (z. B. "no"), 
        /// hat dies keinerlei Auswirkungen auf den Podcast
        /// </summary>
        public bool Complete { get; set; }

        /// <summary>
        /// &lt;itunes:keywords&gt;
        /// Dieses Tag ermöglicht es Benutzern, eine Suche nach maximal 12 Schlagwörtern durchzuführen. Die einzelnen Schlagwörter werden durch
        /// Kommas voneinander getrennt.
        /// </summary>
        public List<string> Keywords { get; set; }

        /// <summary>
        /// &lt;itunes:new-feed-url&gt;
        /// Mit diesem Tag lässt sich die URL-Adresse ändern, unter der sich der Podcast-Feed befindet. Es wird auf &lt;channel&gt;- Ebene hinzugefügt.
        /// Das Format des Feeds lautet wie folgt:
        /// &lt;itunes:new-feed-url&gt;http://neuerort.de/beispiel.rss&lt;/itunes:new-feed-url&gt; 
        /// Nachdem das Tag zum alten Feed hinzugefügt wurde, sollte dieser noch für etwa 48 Stunden aktiv bleiben, bevor er zurückgezogen wird.
        /// Dann wird iTunes das Verzeichnis mit der neuen URL-Adresse des Feeds aktualisiert haben. Im Abschnitt "Ändern der URL-Adresse eines Feeds"
        /// (siehe oben) finden sich weitere Informationen dazu.
        /// </summary>
        public Uri NewFeedUrl { get; set; }

        /// <summary>
        /// &lt;itunes:owner&gt;
        /// Dieses Tag enthält Informationen, die für die Kontaktaufnahme mit dem Eigentümer des Podcasts verwendet werden, 
        /// falls es Fragen zu dem Podcast gibt. Es wird nicht öffentlich angezeigt.
        ///
        /// Die E-Mail Adresse des Eigentümers ist in einem eingebetteten &lt;itunes:email&gt;- Element anzugeben.
        ///
        /// Der Name des Eigentümers ist in einem eingebetteten &lt;itunes:name&gt;- Element anzugeben.
        /// </summary>
        public string Owner { get; set; }

        /// <summary>
        /// &lt;itunes:subtitle&gt;
        /// Der Inhalt dieses Tags wird in iTunes in der Spalte "Beschreibung" angezeigt. Die Anzeige des Untertitels ist am besten möglich, 
        /// wenn er nur wenige Wörter umfasst.
        /// </summary>
        public string Subtitle { get; set; }

        /// <summary>
        /// &lt;itunes:summary&gt;
        /// Der Inhalt dieses Tags wird in einem separaten Fenster angezeigt. Dieses wird eingeblendet, wenn ein Benutzer auf das Symbol "i" in
        /// der Spalte "Beschreibung" klickt. Die Informationen werden auch auf der iTunes Seite für den Podcast angezeigt. 
        /// Dieses Feld kann bis zu 4.000 Zeichen enthalten. Ist &lt;itunes:summary&gt; nicht vorhanden, wird der Inhalt des &lt;description&gt;- Tags verwendet.
        /// </summary>
        public string Summary { get; set; }


        // addons
        public string Copyright { get; set; }

        public List<Episode> Episodes { get; }

        public Podcast()
        {
            Links = new List<Link>();
            Categories = new List<Category>();
            Episodes = new List<Episode>();
            Keywords = new List<string>();
        }
    }
}
