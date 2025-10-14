//binary search'in notasyonu 0(logn)'dir.



int[] sayilar = { 1,2,3,4,5,6,7,8,9,10,11,12};

//aranacak sayının girilmesi
Console.WriteLine("Aradığınız sayıyı giriniz");
int aranansayi = int.Parse(Console.ReadLine());

int sonuc = Array.BinarySearch(sayilar, aranansayi);



if (sonuc >= 0)
{
    Console.WriteLine($"Aradığınız sayı dizide mevcut. Index numarası: {sonuc} 'dir.");
}
else
{
    Console.WriteLine("Aradığınız sayı dizide mecvut değildir.");
}


Console.ReadLine();