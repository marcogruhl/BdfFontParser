namespace BdfFontParser.Models
{
    public struct CharData
    {
        public char Character;
        public string Name;
        public Width ScalableWidth;
        public Width DeviceWidth;
        public BoundingBox BoundingBox;
        public byte[][] Bitmap;
    }
}