namespace System_Programing_Critical_Section_Homework4;
public class Car
{
    public string? Model {  get; set; }
    public string? Vendor {  get; set; }
    public int Year {  get; set; }
    public string? ImagePath {  get; set; }
    public Car() 
    {
    
    }
    public Car(string? vendor, string? model, int year, string? ımagePath)
    {
        Vendor = vendor;
        Model = model;
        Year = year;
        ImagePath = ımagePath;
    }
}
