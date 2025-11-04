using System;
using System.Collections.Generic;

namespace IkiYonluLinkedListConsole
{
    // İki yönlü düğüm (node)
    class Dugum
    {
        public int Veri;
        public Dugum Onceki;
        public Dugum Sonraki;

        public Dugum(int veri)
        {
            Veri = veri;
            Onceki = null;
            Sonraki = null;
        }
    }

    // İki yönlü bağlı liste (doubly linked list)
    class IkiYonluLinkedList
    {
        private Dugum head;
        private Dugum tail;
        public IkiYonluLinkedList()
        {
            head = null;
            tail = null;
        }

        // Başa ekleme
        public void BasaEkle(int deger)
        {
            Dugum yeni = new Dugum(deger);
            if (head == null)
            {
                head = tail = yeni;
            }
            else
            {
                yeni.Sonraki = head;
                head.Onceki = yeni;
                head = yeni;
            }
        }

        // Sona ekleme
        public void SonaEkle(int deger)
        {
            Dugum yeni = new Dugum(deger);
            if (tail == null)
            {
                head = tail = yeni;
            }
            else
            {
                tail.Sonraki = yeni;
                yeni.Onceki = tail;
                tail = yeni;
            }
        }

        // Belirli bir veriden sonra ekleme (ilk bulunan hedefin sonrasına)
        public bool HedeftenSonraEkle(int hedef, int deger)
        {
            Dugum cur = head;
            while (cur != null)
            {
                if (cur.Veri == hedef)
                {
                    Dugum yeni = new Dugum(deger);
                    Dugum sonraki = cur.Sonraki;
                    cur.Sonraki = yeni;
                    yeni.Onceki = cur;
                    yeni.Sonraki = sonraki;
                    if (sonraki != null) sonraki.Onceki = yeni;
                    else tail = yeni; // sondaysa tail güncelle
                    return true;
                }
                cur = cur.Sonraki;
            }
            return false; // hedef bulunamadı
        }

        // Belirli bir veriden önce ekleme (ilk bulunan hedefin öncesine)
        public bool HedeftenOnceEkle(int hedef, int deger)
        {
            Dugum cur = head;
            while (cur != null)
            {
                if (cur.Veri == hedef)
                {
                    Dugum yeni = new Dugum(deger);
                    Dugum onceki = cur.Onceki;
                    yeni.Sonraki = cur;
                    cur.Onceki = yeni;
                    yeni.Onceki = onceki;
                    if (onceki != null) onceki.Sonraki = yeni;
                    else head = yeni; // başa eklendiyse head güncelle
                    return true;
                }
                cur = cur.Sonraki;
            }
            return false; // hedef bulunamadı
        }

        // Baştan silme
        public bool BastanSil()
        {
            if (head == null) return false;
            if (head == tail) // tek eleman
            {
                head = tail = null;
                return true;
            }
            head = head.Sonraki;
            head.Onceki = null;
            return true;
        }

        // Sondan silme
        public bool SondanSil()
        {
            if (tail == null) return false;
            if (head == tail)
            {
                head = tail = null;
                return true;
            }
            tail = tail.Onceki;
            tail.Sonraki = null;
            return true;
        }

        // Aradan silme (ilk bulunan değeri sil)
        public bool AradanSil(int deger)
        {
            Dugum cur = head;
            while (cur != null)
            {
                if (cur.Veri == deger)
                {
                    Dugum onceki = cur.Onceki;
                    Dugum sonraki = cur.Sonraki;
                    if (onceki != null) onceki.Sonraki = sonraki;
                    else head = sonraki; // baştan silme
                    if (sonraki != null) sonraki.Onceki = onceki;
                    else tail = onceki; // sondan silme
                    return true;
                }
                cur = cur.Sonraki;
            }
            return false; // bulunamadı
        }

        // Arama (ilk bulunanın 0-tab indexini döndürür; bulunamazsa -1)
        public int IndexOf(int deger)
        {
            Dugum cur = head;
            int idx = 0;
            while (cur != null)
            {
                if (cur.Veri == deger) return idx;
                cur = cur.Sonraki;
                idx++;
            }
            return -1;
        }

        // Tüm listeyi ileri yönde listele
        public void ListeleIleri()
        {
            if (head == null) { Console.WriteLine("Liste boş."); return; }
            Console.Write("İleri: ");
            Dugum cur = head;
            while (cur != null)
            {
                Console.Write(cur.Veri);
                if (cur.Sonraki != null) Console.Write(" <-> ");
                cur = cur.Sonraki;
            }
            Console.WriteLine();
        }

        // Tüm listeyi geri yönde listele
        public void ListeleGeri()
        {
            if (tail == null) { Console.WriteLine("Liste boş."); return; }
            Console.Write("Geri: ");
            Dugum cur = tail;
            while (cur != null)
            {
                Console.Write(cur.Veri);
                if (cur.Onceki != null) Console.Write(" <-> ");
                cur = cur.Onceki;
            }
            Console.WriteLine();
        }

        // Tümünü silme
        public void TumunuSil()
        {
            head = tail = null;
        }

        // Listeyi diziye atma
        public int[] ToArray()
        {
            List<int> temp = new List<int>();
            Dugum cur = head;
            while (cur != null)
            {
                temp.Add(cur.Veri);
                cur = cur.Sonraki;
            }
            return temp.ToArray();
        }

        // Yardımcı: liste boş mu?
        public bool BosMu() => head == null;
    }

    class Program
    {
        static void Main(string[] args)
        {
            IkiYonluLinkedList liste = new IkiYonluLinkedList();
            int secim = -1;

            while (secim != 0)
            {
                Console.WriteLine("\n--- İKİ YÖNLÜ LINKED LIST MENÜSÜ ---");
                Console.WriteLine("1  - Başa ekleme");
                Console.WriteLine("2  - Sona ekleme");
                Console.WriteLine("3  - Belirli veriden sonra ekleme");
                Console.WriteLine("4  - Belirli veriden önce ekleme");
                Console.WriteLine("5  - Baştan silme");
                Console.WriteLine("6  - Sondan silme");
                Console.WriteLine("7  - Belirli değeri silme (ilk bulunan)");
                Console.WriteLine("8  - Arama (değerin indexi)");
                Console.WriteLine("9  - İleri yönde listele");
                Console.WriteLine("10 - Geri yönde listele");
                Console.WriteLine("11 - Tümünü silme");
                Console.WriteLine("12 - Listeyi diziye at ve göster");
                Console.WriteLine("0  - Çıkış");
                Console.Write("Seçiminiz: ");

                if (!int.TryParse(Console.ReadLine(), out secim))
                {
                    Console.WriteLine("Geçersiz giriş, lütfen bir sayı girin.");
                    continue;
                }

                switch (secim)
                {
                    case 1:
                        Console.Write("Eklenecek değer: ");
                        if (int.TryParse(Console.ReadLine(), out int v1))
                        {
                            liste.BasaEkle(v1);
                            Console.WriteLine("Başarıyla başa eklendi.");
                        }
                        else Console.WriteLine("Geçersiz sayı.");
                        break;

                    case 2:
                        Console.Write("Eklenecek değer: ");
                        if (int.TryParse(Console.ReadLine(), out int v2))
                        {
                            liste.SonaEkle(v2);
                            Console.WriteLine("Başarıyla sona eklendi.");
                        }
                        else Console.WriteLine("Geçersiz sayı.");
                        break;

                    case 3:
                        Console.Write("Hedef değer (sonrasına ekle): ");
                        if (!int.TryParse(Console.ReadLine(), out int hedef3)) { Console.WriteLine("Geçersiz sayı."); break; }
                        Console.Write("Eklenecek değer: ");
                        if (!int.TryParse(Console.ReadLine(), out int yeni3)) { Console.WriteLine("Geçersiz sayı."); break; }
                        if (liste.HedeftenSonraEkle(hedef3, yeni3)) Console.WriteLine("Hedefin sonrasına eklendi.");
                        else Console.WriteLine("Hedef bulunamadı.");
                        break;

                    case 4:
                        Console.Write("Hedef değer (öncesine ekle): ");
                        if (!int.TryParse(Console.ReadLine(), out int hedef4)) { Console.WriteLine("Geçersiz sayı."); break; }
                        Console.Write("Eklenecek değer: ");
                        if (!int.TryParse(Console.ReadLine(), out int yeni4)) { Console.WriteLine("Geçersiz sayı."); break; }
                        if (liste.HedeftenOnceEkle(hedef4, yeni4)) Console.WriteLine("Hedefin öncesine eklendi.");
                        else Console.WriteLine("Hedef bulunamadı.");
                        break;

                    case 5:
                        if (liste.BastanSil()) Console.WriteLine("Baştan silindi.");
                        else Console.WriteLine("Liste boş, silme yapılamadı.");
                        break;

                    case 6:
                        if (liste.SondanSil()) Console.WriteLine("Sondan silindi.");
                        else Console.WriteLine("Liste boş, silme yapılamadı.");
                        break;

                    case 7:
                        Console.Write("Silinecek değer: ");
                        if (!int.TryParse(Console.ReadLine(), out int sil7)) { Console.WriteLine("Geçersiz sayı."); break; }
                        if (liste.AradanSil(sil7)) Console.WriteLine("Değer silindi (ilk bulunan).");
                        else Console.WriteLine("Değer bulunamadı.");
                        break;

                    case 8:
                        Console.Write("Aranacak değer: ");
                        if (!int.TryParse(Console.ReadLine(), out int ara8)) { Console.WriteLine("Geçersiz sayı."); break; }
                        int idx = liste.IndexOf(ara8);
                        if (idx >= 0) Console.WriteLine($"Değer bulundu. Index (0-tab): {idx}");
                        else Console.WriteLine("Değer bulunamadı.");
                        break;

                    case 9:
                        liste.ListeleIleri();
                        break;

                    case 10:
                        liste.ListeleGeri();
                        break;

                    case 11:
                        liste.TumunuSil();
                        Console.WriteLine("Liste temizlendi.");
                        break;

                    case 12:
                        int[] arr = liste.ToArray();
                        Console.WriteLine("Diziye aktarılan elemanlar:");
                        if (arr.Length == 0) Console.WriteLine("Dizi boş.");
                        else
                        {
                            for (int i = 0; i < arr.Length; i++)
                                Console.WriteLine($"[{i}] = {arr[i]}");
                        }
                        break;

                    case 0:
                        Console.WriteLine("Programdan çıkılıyor...");
                        break;

                    default:
                        Console.WriteLine("Geçersiz seçim.");
                        break;
                }
            }
        }
    }
}
