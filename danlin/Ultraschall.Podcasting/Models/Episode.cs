using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Ultraschall.Podcasting.Models
{
    public class Episode
    {
        public string Title { get; set; }

        /// <summary>
        /// &lt;guid&gt;
        /// Jedes &lt;item&gt;- Tag sollte über eine statische, eindeutige Kennung ("globally unique identifier") verfügen. 
        /// Wenn Folgen zu einem Feed hinzugefügt werden, werden diese Kennungen ("guids") verglichen (unter Beachtung von Groß- und Kleinschreibung), 
        /// um festzustellen, welche Folgen neu sind. Wird diese Kennung für eine Folge nicht angegeben, wird stattdessen die URL-Adresse 
        /// der Folge verwendet.
        /// </summary>
        public string Guid { get; set; }

        public Link Link { get; set; }

        public List<Link> Links { get; }

        /// <summary>
        /// &lt;pubDate&gt;
        /// Dieses Tag gibt das Datum und die Uhrzeit der Veröffentlichung einer Folge an. Sein Inhalt sollte gemäß RFC 2822 formatiert werden, z. B.:
        ///
        /// Wed, 15 Jun 2005 19:00:00 GMT
        /// </summary>
        public DateTimeOffset PublishedDate { get; set; }

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
        /// &lt;itunes:duration&gt;
        /// Der Inhalt dieses Tags wird in der Spalte "Länge" in iTunes angezeigt. 
        ///
        /// Das Tag kann wie folgt formatiert werden: SS:MM:ss, S:MM:ss, MM:ss oder M:ss (S = Stunden, M = Minuten, s = Sekunden). 
        /// Wird eine Ganzzahl angegeben (ohne Doppelpunkt), wird davon ausgegangen, dass es sich bei dem Wert um eine Sekundenangabe handelt. 
        /// Ist ein Doppelpunkt vorhanden, wird die Zahl links davon als Minuten-, die Zahl rechts davon als Sekundenangabe aufgefasst. 
        /// Bei mehr als zwei Doppelpunkten werden die Zahlen ganz rechts ignoriert.
        /// </summary>
        public TimeSpan Durication { get; set; }

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
        /// &lt;itunes:isClosedCaptioned&gt;
        /// Dieses Tag sollte für Podcasts verwendet werden, die eine Videofolge mit Untertiteln enthalten. Die zwei möglichen Werte für dieses
        /// Tag sind "yes" und "no".
        ///
        /// Dieses Tag wird nur auf &lt;item&gt;-Ebene unterstützt. Wird für dieses Tag "yes" angegeben, erscheint in der Spalte "Name" der jeweiligen
        /// Folge in iTunes ein Symbol für Untertitel ("closed caption"). Ist das Untertitel-Tag vorhanden und weist einen anderen Wert auf (z. B. "no"),
        /// wird kein Hinweis angezeigt.
        /// </summary>
        public bool IsClosedCaptioned { get; set; }

        /// <summary>
        /// &lt;itunes:order&gt;
        /// Dieses Tag kann verwendet werden, um die Standard-Reihenfolge der Folgen im Store aufzuheben.
        ///
        /// Es wird auf &lt;item&gt;-Ebene verwendet, wobei als Wert eine Zahl angegeben wird, die festlegt, 
        /// an welcher Stelle die Folge im Store angezeigt wird. Wenn man beispielsweise möchte, dass eine Folge (&lt;item&gt;) als erste Folge
        /// eines Podcasts angezeigt wird, muss für das &lt;itunes:order&gt;-Tag der Wert "1" festgelegt werden. Wenn Reihenfolgenwerte verschiedener
        /// Folgen in Konflikt zueinander stehen, verwendet der Store die Standard-Reihenfolge (pubDate).
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// &lt;itunes:keywords&gt;
        /// Dieses Tag ermöglicht es Benutzern, eine Suche nach maximal 12 Schlagwörtern durchzuführen. Die einzelnen Schlagwörter werden durch
        /// Kommas voneinander getrennt.
        /// </summary>
        public List<string> Keywords { get; set; }

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

        /// <summary>
        /// &lt;enclosure&gt;
        /// Das Tag &lt;enclosure&gt; hat drei Attribute: "URL", "length" (Länge) und "type" (Art). Ein Enclosure-Tag aus dem obigen Beispiel-Feed:
        ///
        /// &lt;enclosure url="http://beispiel.de/podcasts/alles/EinBisschenVonAllemFolge2.mp3" length="5650889" type="audio/mpeg" /&gt;
        ///
        /// Anhand der Dateierweiterung des URL-Attributs dieses Tags wird festgestellt, ob ein Element im Podcast-Verzeichnis erscheinen soll. Zu den unterstützten Dateiformaten zählen "m4a", "mp3", "mov", "mp4", "m4v", "pdf" und "epub".
        ///
        /// Das "length"-Attribut gibt die Dateigröße in Byte an. Diese Angabe ist in den Dateieigenschaften enthalten (auf dem Mac kann sie über die Option "Informationen" im Kontextmenü abgerufen werden; sie steht in der Zeile "Größe:"). 
        ///
        /// Das "type"-Element hängt von dem Dateityp ab, auf den sich das Enclosure-Tag bezieht. Gängige Dateien und ihre MIME-Typ-Erweiterungen sind in der folgenden Tabelle aufgelistet.
        ///
        /// Datei Art
        /// .mp3 audio/mpeg 
        /// .m4a audio/x-m4a 
        /// .mp4 video/mp4 
        /// .m4v video/x-m4v 
        /// .mov video/quicktime 
        /// .pdf application/pdf 
        /// .epub document/x-epub 
        /// </summary>
        public Enclosure Enclosure { get; set; }

        public Episode()
        {
            Links = new List<Link>();
            Keywords = new List<string>();
        }
    }
}
