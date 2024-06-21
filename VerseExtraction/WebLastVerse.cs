using System.Text;

public class WebLastVerse
{
	public static void Main()
	{
		const int lastCount = 1;
		var path = @"C:\personal\sporc-geo\MiscBible\";

		// see https://bolls.life/api/#Get%20a%20translation for instructions on downloading an entire translation
		// this is the link I used to download the ESV:  https://bolls.life/static/translations/ESV.json parsed on the following line
		var verses = GetVerses(@"C:\personal\sporc-geo\bible_translations\engwebu_readaloud", lastCount);

		var mainFilepath = Path.Combine(path, $"web-last{lastCount}.csv");
		Directory.CreateDirectory(Path.GetDirectoryName(mainFilepath));
		File.WriteAllText(mainFilepath, "", Encoding.UTF8);

		var bookVerses = verses
			.GroupBy(v => v.Book)
			.Select(g => new
			{
				BookNumber = g.Key,
				Text = string.Join(' ', g.TakeLast(lastCount).Select(v => v.Text))
			})
			.Select(e => $"\"{GetFormattedVerseText(e.Text)}\",{books[e.BookNumber]},");

		File.AppendAllLines(mainFilepath, bookVerses, Encoding.UTF8);

	}

	private static List<WebVerse> GetVerses(string path, int lastCount)
	{
		var verses = new List<WebVerse>();
		for (int bookNumber = 0; bookNumber < books.Length; ++bookNumber)
		{
			if (string.IsNullOrWhiteSpace(books[bookNumber])) continue;
			var filespec = $"engwebu_{bookNumber:D3}_???_*_read.txt";
			var files = Directory.EnumerateFiles(path, filespec, SearchOption.TopDirectoryOnly);
			foreach (var file in files)
			{
				var filename = Path.GetFileNameWithoutExtension(file);
				var chapterNumber = byte.Parse(filename.Substring(16, 3).TrimEnd('_'));
				var lines = File.ReadAllLines(file);
				byte verseNumber = 0;
				foreach (var line in lines.Where(l => !string.IsNullOrWhiteSpace(l)).TakeLast(lastCount))
				{
					var verse = new WebVerse
					{
						Book = (byte)bookNumber,
						Chapter = chapterNumber,
						Verse = ++verseNumber,
						Text = line
					};
					verses.Add(verse);
				}
			}

		}
		return verses;
	}

	private static string GetFormattedVerseText(string verseText)
	{
		return verseText
			.Replace("\n", " ")
			.Replace("\r", "")
			.Replace("  ", " ")
			.Replace("  ", " ")
			.Replace("  ", " ")
			.Replace("\"", "\"\"")
			.Trim();
	}

	// placeholder strings exist in the array, so that the book numbers assigned
	// in the input data align with the array index.
	public static string[] books = {
		"",
		"",
		"Genesis",    // Genesis is book 002 for some reason
		"Exodus",
		"Leviticus",
		"Numbers",
		"Deuteronomy",
		"Joshua",
		"Judges",
		"Ruth",
		"1 Samuel",
		"2 Samuel",
		"1 Kings",
		"2 Kings",
		"1 Chronicles",
		"2 Chronicles",
		"Ezra",
		"Nehemiah",
		"Esther",
		"Job",
		"Psalms",
		"Proverbs",
		"Ecclesiastes",
		"Song of Solomon",
		"Isaiah",
		"Jeremiah",
		"Lamentations",
		"Ezekiel",
		"Daniel",
		"Hosea",
		"Joel",
		"Amos",
		"Obadiah",
		"Jonah",
		"Micah",
		"Nahum",
		"Habakkuk",
		"Zephaniah",
		"Haggai",
		"Zechariah",
		"Malachi",
		"",			// books 041-069 are apocrypha or unassigned
		"",
		"",
		"",
		"",
		"",
		"",
		"",
		"",
		"",
		"",
		"",
		"",
		"",
		"",
		"",
		"",
		"",
		"",
		"",
		"",
		"",
		"",
		"",
		"",
		"",				
		"",
		"",
		"",
		"Matthew",		// Matthew is book 070
		"Mark",
		"Luke",
		"John",
		"Acts",
		"Romans",
		"1 Corinthians",
		"2 Corinthians",
		"Galatians",
		"Ephesians",
		"Philippians",
		"Colossians",
		"1 Thessalonians",
		"2 Thessalonians",
		"1 Timothy",
		"2 Timothy",
		"Titus",
		"Philemon",
		"Hebrews",
		"James",
		"1 Peter",
		"2 Peter",
		"1 John",
		"2 John",
		"3 John",
		"Jude",
		"Revelation",
	};



	class WebVerse
	{
		public int Key { get; set; }
		public byte Book { get; set; }
		public string BookName => books[Book];
		public byte Chapter { get; set; }
		public byte Verse { get; set; }
		public string Text { get; set; }
	}

}