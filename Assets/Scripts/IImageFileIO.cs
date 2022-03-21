public interface IImageFileIO {
    byte[] ReadAllImageBytes(string filePath);
    void SaveGameImage(string hash, byte[] data);   
}