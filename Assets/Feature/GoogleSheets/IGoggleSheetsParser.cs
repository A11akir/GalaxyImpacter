namespace Feature.GoogleSheets
{
    public interface IGoggleSheetsParser
    {
        public void Parse(string header, string token);

        void ApplyToSO();

    }
}