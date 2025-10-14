//dizideki elemanların toplamı

int[] sayilar = { 234,2456,2356,247,8,123456,12345,1234,2345,456,90 };
int toplam = 0;

foreach (int sayi in sayilar)
{
    toplam += sayi;

}

Console.WriteLine("Sayıların toplamı:" + toplam);
Console.ReadLine();

//notasyonu O(n)'dir. çünkü dizide her eleman 1 defa gelir.