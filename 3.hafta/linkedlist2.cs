using System;
using System.Collections.Generic;

namespace linkedlist2
{
  
    class Node
    {
        public int OgrenciNo { get; set; }
        public string DersKodu { get; set; }
        public string HarfNotu { get; set; }

        public Node SonrakiOgrenci;   // Aynı dersteki diğer öğrenci
        public Node SonrakiDers;      // Aynı öğrencinin diğer dersi
    }

    class Program
    {
        // Öğrencilerin derslerini tutmak için
        static Dictionary<int, Node> Ogrenciler = new Dictionary<int, Node>();

        // Derslerdeki öğrencileri tutmak için
        static Dictionary<string, Node> Dersler = new Dictionary<string, Node>();

        static void Main(string[] args)
        {
            int secim = -1;

            while (secim != 0)
            {
                Console.WriteLine("\n--- BAĞLI LİSTE DERS – ÖĞRENCİ SİSTEMİ ---");
                Console.WriteLine("1 - Bir öğrenciye yeni ders ekle");
                Console.WriteLine("2 - Bir derse yeni öğrenci ekle");
                Console.WriteLine("3 - Bir öğrencinin bir dersini sil");
                Console.WriteLine("4 - Bir dersteki bir öğrenciyi sil");
                Console.WriteLine("5 - Bir dersteki tüm öğrencileri listele (numaraya göre)");
                Console.WriteLine("6 - Bir öğrencinin tüm derslerini listele (ders koduna göre)");
                Console.WriteLine("0 - Çıkış");
                Console.Write("Seçiminiz: ");

                if (!int.TryParse(Console.ReadLine(), out secim))
                {
                    Console.WriteLine("Lütfen geçerli bir sayı girin.");
                    continue;
                }

                switch (secim)
                {
                    case 1:
                        OgrenciyeDersEkle();
                        break;
                    case 2:
                        DerseOgrenciEkle();
                        break;
                    case 3:
                        OgrenciDersSil();
                        break;
                    case 4:
                        DerstenOgrenciSil();
                        break;
                    case 5:
                        DerstekiOgrencileriListele();
                        break;
                    case 6:
                        OgrencininDersleriniListele();
                        break;
                    case 0:
                        Console.WriteLine("Program sonlandırılıyor...");
                        break;
                    default:
                        Console.WriteLine("Geçersiz seçim!");
                        break;
                }
            }
        }

        static void OgrenciyeDersEkle()
        {
            Console.Write("Öğrenci numarası: ");
            int ogrNo = int.Parse(Console.ReadLine());
            Console.Write("Ders kodu: ");
            string ders = Console.ReadLine();
            Console.Write("Harf notu: ");
            string harf = Console.ReadLine();

            Node yeni = new Node { OgrenciNo = ogrNo, DersKodu = ders, HarfNotu = harf };

            // Öğrencinin ders listesine ekle
            if (!Ogrenciler.ContainsKey(ogrNo))
                Ogrenciler[ogrNo] = yeni;
            else
            {
                Node temp = Ogrenciler[ogrNo];
                while (temp.SonrakiDers != null)
                    temp.SonrakiDers = temp.SonrakiDers.SonrakiDers;
                temp.SonrakiDers = yeni;
            }

            // Dersin öğrenci listesine ekle
            if (!Dersler.ContainsKey(ders))
                Dersler[ders] = yeni;
            else
            {
                Node temp = Dersler[ders];
                while (temp.SonrakiOgrenci != null)
                    temp = temp.SonrakiOgrenci;
                temp.SonrakiOgrenci = yeni;
            }

            Console.WriteLine("Ders başarıyla eklendi!");
        }

        static void DerseOgrenciEkle()
        {
            Console.Write("Ders kodu: ");
            string ders = Console.ReadLine();
            Console.Write("Öğrenci numarası: ");
            int ogrNo = int.Parse(Console.ReadLine());
            Console.Write("Harf notu: ");
            string harf = Console.ReadLine();

            // Aslında işlem aynı
            OgrenciyeDersEkle();
        }

        static void OgrenciDersSil()
        {
            Console.Write("Silinecek öğrencinin numarası: ");
            int ogrNo = int.Parse(Console.ReadLine());
            Console.Write("Silinecek ders kodu: ");
            string ders = Console.ReadLine();

            if (Ogrenciler.ContainsKey(ogrNo))
            {
                Node temp = Ogrenciler[ogrNo];
                Node onceki = null;

                while (temp != null)
                {
                    if (temp.DersKodu == ders)
                    {
                        if (onceki == null)
                            Ogrenciler[ogrNo] = temp.SonrakiDers;
                        else
                            onceki.SonrakiDers = temp.SonrakiDers;
                        break;
                    }
                    onceki = temp;
                    temp = temp.SonrakiDers;
                }

                Console.WriteLine("Ders silindi!");
            }
            else
                Console.WriteLine("Bu öğrenci bulunamadı!");
        }

        static void DerstenOgrenciSil()
        {
            Console.Write("Ders kodu: ");
            string ders = Console.ReadLine();
            Console.Write("Silinecek öğrenci no: ");
            int ogrNo = int.Parse(Console.ReadLine());

            if (Dersler.ContainsKey(ders))
            {
                Node temp = Dersler[ders];
                Node onceki = null;

                while (temp != null)
                {
                    if (temp.OgrenciNo == ogrNo)
                    {
                        if (onceki == null)
                            Dersler[ders] = temp.SonrakiOgrenci;
                        else
                            onceki.SonrakiOgrenci = temp.SonrakiOgrenci;
                        break;
                    }
                    onceki = temp;
                    temp = temp.SonrakiOgrenci;
                }

                Console.WriteLine("Öğrenci silindi!");
            }
            else
                Console.WriteLine("Bu ders bulunamadı!");
        }

        static void DerstekiOgrencileriListele()
        {
            Console.Write("Ders kodu: ");
            string ders = Console.ReadLine();

            if (!Dersler.ContainsKey(ders))
            {
                Console.WriteLine("Bu ders bulunamadı!");
                return;
            }

            List<Node> liste = new List<Node>();
            Node temp = Dersler[ders];
            while (temp != null)
            {
                liste.Add(temp);
                temp = temp.SonrakiOgrenci;
            }

            liste.Sort((a, b) => a.OgrenciNo.CompareTo(b.OgrenciNo));

            Console.WriteLine($"\n{ders} dersindeki öğrenciler:");
            foreach (var n in liste)
                Console.WriteLine($"Öğrenci No: {n.OgrenciNo}, Harf Notu: {n.HarfNotu}");
        }

        static void OgrencininDersleriniListele()
        {
            Console.Write("Öğrenci numarası: ");
            int ogrNo = int.Parse(Console.ReadLine());

            if (!Ogrenciler.ContainsKey(ogrNo))
            {
                Console.WriteLine("Bu öğrenci bulunamadı!");
                return;
            }

            List<Node> liste = new List<Node>();
            Node temp = Ogrenciler[ogrNo];
            while (temp != null)
            {
                liste.Add(temp);
                temp = temp.SonrakiDers;
            }

            liste.Sort((a, b) => a.DersKodu.CompareTo(b.DersKodu));

            Console.WriteLine($"\n{ogrNo} numaralı öğrencinin dersleri:");
            foreach (var n in liste)
                Console.WriteLine($"Ders: {n.DersKodu}, Harf Notu: {n.HarfNotu}");
        }
    }
}
