using System.Text;
using Newtonsoft.Json;

public class ReinaValeraLastVerse
{
	public static void _Main()
	{
		const int firstCount = 2;
		var path = @"C:\personal\sporc-geo\MiscBible\";

		// see https://bolls.life/api/#Get%20a%20translation for instructions on downloading an entire translation
		// this is the link I used to download the RV1960:  https://bolls.life/static/translations/RV1960.json parsed on the following line
		var verses = JsonConvert.DeserializeObject<List<RvVerse>>(File.ReadAllText(@"C:\personal\sporc-geo\bible_translations\rv1960.json"));

		var mainFilepath = Path.Combine(path, $"rv1960-first{2}.csv");
		Directory.CreateDirectory(Path.GetDirectoryName(mainFilepath));
		File.WriteAllText(mainFilepath, "", Encoding.UTF8);

		var bookVerses = verses
			.GroupBy(v => v.Book)
			.Select(g => new
			{
				BookNumber = g.Key,
				Text = string.Join(' ', g.Take(firstCount).Select(v => v.Text))
			})
			.Select(e => $"\"{GetFormattedVerseText(e.Text)}\",{books[e.BookNumber]},");

		File.AppendAllLines(mainFilepath, bookVerses, Encoding.UTF8);

	}

	private static string GetFormattedVerseText(string verseText)
	{
		return verseText
			.Replace("<br>", " ")
			.Replace("<br/>", " ")
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



	class RvVerse
	{
		public int Key { get; set; }
		public byte Book { get; set; }
		public string BookName => books[Book];
		public byte Chapter { get; set; }
		public byte Verse { get; set; }
		public string Text { get; set; }
	}

}