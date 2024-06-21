using System.Text;
using Newtonsoft.Json;

public class EsvVerseOfTheDay
{
	public static void _Main()
	{
		foreach (var month in Enumerable.Range(1, 12))
		{
			ProcessMonth(month);
		}
	}

	public static void ProcessMonth(int month)
	{ 
		//var month = 6;
		var startDay = 1;
		var endDay = 99;

		int[] days = { 31, 29, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };

		endDay = int.Min(endDay, days[month - 1]);



		var path = @"C:\personal\sporc-geo\VerseOfTheDay\Extracts\";

		// see https://bolls.life/api/#Get%20a%20translation for instructions on downloading an entire translation
		// this is the link I used to download the ESV:  https://bolls.life/static/translations/ESV.json parsed on the following line
		var o = JsonConvert.DeserializeObject<List<EsvVerse>>(File.ReadAllText(@"C:\personal\sporc-geo\bible_translations\esv.json"));
		var dayRange = Enumerable.Range(startDay, endDay + 1 - startDay);
		var verses = o.Where(v => v.Chapter == month && dayRange.Contains(v.Verse));

		var monthName = new DateTime(2000, month, 1).ToString("MMMM");
		var mainFilepath = Path.Combine(path, monthName, $"{monthName}-all.csv");
		Directory.CreateDirectory(Path.GetDirectoryName(mainFilepath));
		File.WriteAllText(mainFilepath, "", Encoding.UTF8);

		var line = $",{(string.Join(",", dayRange))}\r\n";
		File.AppendAllText(mainFilepath, line, Encoding.UTF8);

		var bookLines = books
			.Select((b, i) => GetBookLine(i, dayRange, verses.Where(v => v.Book == i)))
			.Where(l => !string.IsNullOrWhiteSpace(l));

		File.AppendAllLines(mainFilepath, bookLines, Encoding.UTF8);

		foreach (var day in dayRange)
		{
			var dayFilepath = Path.Combine(Path.GetDirectoryName(mainFilepath), $"{monthName}-{day}.csv");
			File.WriteAllText(dayFilepath, "", Encoding.UTF8);
			var bookLines2 = books
				.Select((b, i) => GetBookLine2(i, day, verses.Where(v => v.Book == i)))
				.Where(l => !string.IsNullOrWhiteSpace(l));
			File.AppendAllLines(dayFilepath, bookLines2, Encoding.UTF8);
		}
	}


	private static string GetBookLine(int book, IEnumerable<int> dayRange, IEnumerable<EsvVerse> verses)
	{
		if (!verses.Any()) return null;
		var builder = new StringBuilder($"{books[book]}");
		foreach (var day in dayRange)
		{
			var verse = verses.SingleOrDefault(v => v.Book == book && v.Verse == day);
			if (verse == null) break;  // chapter has less verses than end of range
			//var text = GetFormattedVerseText(verse.Text);

			builder.Append($",\"{GetFormattedVerseText(verse.Text)}\"");
		}
		return builder.ToString();
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

	private static string GetBookLine2(int book, int day, IEnumerable<EsvVerse> verses)
	{
		if (!verses.Any()) return null;
		var verse = verses.SingleOrDefault(v => v.Book == book && v.Verse == day);
		if (verse == null) return null;  // chapter has less verses than end of range
		var builder = new StringBuilder();
		builder.Append("\"");
		builder.Append($"____ {verse.Chapter}:{day} (ESV): ");
		builder.Append(GetFormattedVerseText(verse.Text));
		builder.Append("\"");
		builder.Append($",\"{books[book]}\",,,,1");
		return builder.ToString();
	}

	public static string[] books = {
		"",  // dummy entry to effectively turn this into a 1-based array
		"Genesis",
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
		"Matthew",
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



	class EsvVerse
	{
		public int Key { get; set; }
		public byte Book { get; set; }
		public string BookName => books[Book];
		public byte Chapter { get; set; }
		public byte Verse { get; set; }
		public string Text { get; set; }
	}

}