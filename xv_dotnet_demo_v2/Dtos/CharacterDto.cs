namespace xv_dotnet_demo.Dtos
{
    public class CharacterDto
    {
        public int id { get; set; }

        public string name { get; set; }

        public string status { get; set; }

        public string species { get; set; }

        public string type { get; set; }

        public string gender { get; set; }

        public string image { get; set; }

        public string url { get; set; }

        public string createdAt { get; set; }

        public CharacterDataDto origin { get; set; }

        public CharacterDataDto location { get; set; }

        public List<string> episode { get; set; } = new List<string>();
    }
}
