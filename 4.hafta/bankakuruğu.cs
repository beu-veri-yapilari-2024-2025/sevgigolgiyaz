using System;
using System.Collections.Generic;

namespace bankakuyruğu;

class Program
{
    // --- 1. Dizi Mantığı ile Kuyruk ---
    static void BankaKuyrugu_Dizi()
    {
        Console.WriteLine("=== DİZİ MANTIĞI İLE BANKA KUYRUĞU ===");

        // 3 öncelik grubuna sahip kuyruklar (her biri list olarak tutulur)
        List<string>[] kuyruklar = new List<string>[3];
        for (int i = 0; i < 3; i++)
            kuyruklar[i] = new List<string>();

        // Enqueue işlemleri (müşteri ekleme)
        Enqueue_Dizi(kuyruklar, "İlkin", 1);    // VIP
        Enqueue_Dizi(kuyruklar, "Arzu", 2);   // Ticari
        Enqueue_Dizi(kuyruklar, "Elif", 3); // Bireysel
        Enqueue_Dizi(kuyruklar, "Efe", 1); // VIP

        // Kuyrukları göster
        Yazdir_Dizi(kuyruklar);

        // Dequeue işlemleri (önceliğe göre hizmet)
        Console.WriteLine("\n--- İşlem Başlatılıyor ---");
        Console.WriteLine($"{Dequeue_Dizi(kuyruklar)} hizmet aldı");
        Console.WriteLine($"{Dequeue_Dizi(kuyruklar)} hizmet aldı");
        Console.WriteLine($"{Dequeue_Dizi(kuyruklar)} hizmet aldı");

        // Kalan kuyruklar
        Yazdir_Dizi(kuyruklar);
    }

    static void Enqueue_Dizi(List<string>[] kuyruklar, string musteri, int oncelik)
    {
        kuyruklar[oncelik - 1].Add(musteri);
    }

    static string Dequeue_Dizi(List<string>[] kuyruklar)
    {
        for (int i = 0; i < 3; i++) // 1 en yüksek öncelik
        {
            if (kuyruklar[i].Count > 0)
            {
                string musteri = kuyruklar[i][0];
                kuyruklar[i].RemoveAt(0);
                return musteri;
            }
        }
        return "Kuyrukta kimse yok";
    }

    static void Yazdir_Dizi(List<string>[] kuyruklar)
    {
        Console.WriteLine("\n--- Kuyruk Durumu ---");
        for (int i = 0; i < 3; i++)
        {
            Console.Write($"Grup {i + 1}: ");
            Console.WriteLine(string.Join(", ", kuyruklar[i]));
        }
    }

    // --- 2. Linked List Mantığı ile Kuyruk ---
    static void BankaKuyrugu_LinkedList()
    {
        Console.WriteLine("\n=== LINKED LIST MANTIĞI İLE BANKA KUYRUĞU ===");

        // 3 ayrı kuyruk (öncelik seviyeleri)
        LinkedList<string>[] kuyruklar = new LinkedList<string>[3];
        for (int i = 0; i < 3; i++)
            kuyruklar[i] = new LinkedList<string>();

        // Enqueue işlemleri
        Enqueue_Linked(kuyruklar, "Sinem", 2);
        Enqueue_Linked(kuyruklar, "Sevgi", 1);
        Enqueue_Linked(kuyruklar, "Kağan", 3);
        Enqueue_Linked(kuyruklar, "Hande", 1);

        // Kuyrukları göster
        Yazdir_Linked(kuyruklar);

        // Dequeue işlemleri
        Console.WriteLine("\n--- İşlem Başlatılıyor ---");
        Console.WriteLine($"{Dequeue_Linked(kuyruklar)} hizmet aldı");
        Console.WriteLine($"{Dequeue_Linked(kuyruklar)} hizmet aldı");
        Console.WriteLine($"{Dequeue_Linked(kuyruklar)} hizmet aldı");

        // Kalan kuyruklar
        Yazdir_Linked(kuyruklar);
    }

    static void Enqueue_Linked(LinkedList<string>[] kuyruklar, string musteri, int oncelik)
    {
        kuyruklar[oncelik - 1].AddLast(musteri);
    }

    static string Dequeue_Linked(LinkedList<string>[] kuyruklar)
    {
        for (int i = 0; i < 3; i++)
        {
            if (kuyruklar[i].Count > 0)
            {
                string musteri = kuyruklar[i].First.Value;
                kuyruklar[i].RemoveFirst();
                return musteri;
            }
        }
        return "Kuyrukta kimse yok";
    }

    static void Yazdir_Linked(LinkedList<string>[] kuyruklar)
    {
        Console.WriteLine("\n--- Kuyruk Durumu ---");
        for (int i = 0; i < 3; i++)
        {
            Console.Write($"Grup {i + 1}: ");
            Console.WriteLine(string.Join(", ", kuyruklar[i]));
        }
    }

    // --- Main ---
    static void Main(string[] args)
    {
        BankaKuyrugu_Dizi();
        BankaKuyrugu_LinkedList();

        Console.WriteLine("\nProgram sona erdi...");
    }
}
