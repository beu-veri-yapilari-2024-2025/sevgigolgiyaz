using System;
using System.IO;
using System.Text;

namespace HanoiKuleleri
{
    class Program
    {
        // Tüm hamleleri biriktirmek için (dosyaya da yazacağız)
        static StringBuilder kayit = new StringBuilder();
        static int hamleSayisi = 0;

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("🧠 Hanoi Kuleleri Problemi - C# Konsol Uygulaması");
            Console.WriteLine("-----------------------------------------------");
            Console.Write("Disk sayısını giriniz (örnek: 3): ");

            if (!int.TryParse(Console.ReadLine(), out int diskSayisi) || diskSayisi <= 0)
            {
                Console.WriteLine("Hatalı giriş! Pozitif bir tam sayı giriniz.");
                return;
            }

            kayit.AppendLine($"Hanoi Kuleleri Problemi - Disk Sayısı: {diskSayisi}");
            kayit.AppendLine($"Tarih: {DateTime.Now}");
            kayit.AppendLine("-----------------------------------------------");

            Console.WriteLine("\n💡 Çözüm adımları:\n");
            // Kaynak: A, Hedef: C, Yardımcı: B
            Hanoi(diskSayisi, 'A', 'C', 'B');

            string ozet = $"\nToplam hamle sayısı: {hamleSayisi}";
            Console.WriteLine(ozet);
            kayit.AppendLine(ozet);

            // Sonuç dosyasını kaydet
            string dosyaAdi = "HanoiCikti.txt";
            File.WriteAllText(dosyaAdi, kayit.ToString(), Encoding.UTF8);

            Console.WriteLine($"\n📁 Hamleler \"{dosyaAdi}\" dosyasına kaydedildi.");
            Console.WriteLine("\nProgram tamamlandı. Çıkmak için bir tuşa basınız...");
            Console.ReadKey();
        }

        /// <summary>
        /// Hanoi algoritması - Rekürsif çözüm
        /// </summary>
        static void Hanoi(int n, char kaynak, char hedef, char yardimci)
        {
            if (n == 1)
            {
                hamleSayisi++;
                string satir = $"{hamleSayisi}. hamle: {kaynak} çubuğundaki 1 numaralı diski {hedef} çubuğuna taşı.";
                Console.WriteLine(satir);
                kayit.AppendLine(satir);
                return;
            }

            // Adım 1: n-1 diski yardımcıya taşı
            Hanoi(n - 1, kaynak, yardimci, hedef);

            // Adım 2: en büyük diski hedefe taşı
            hamleSayisi++;
            string satir2 = $"{hamleSayisi}. hamle: {kaynak} çubuğundaki {n}. diski {hedef} çubuğuna taşı.";
            Console.WriteLine(satir2);
            kayit.AppendLine(satir2);

            // Adım 3: n-1 diski yardımcıdan hedefe taşı
            Hanoi(n - 1, yardimci, hedef, kaynak);
        }
    }
}
