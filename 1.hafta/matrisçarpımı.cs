//matris çarpımı

using System;

class Program
{
    static void Main()
    {
        int[,] matris1 = new int[3, 4] {
            { 1, 4, 8, 5 },
            { 3, 3, 4, 5 },
            { 1, 4, 7, 2 }
        };

        // matris1 sütun sayısı 4 olduğu için matris2 satır sayısı 4 olmalı
        int[,] matris2 = new int[4, 2] {
            { 3, 7 },
            { 6, 4 },
            { 8, 8 },
            { 1, 2 }
        };

        int m = matris1.GetLength(0); // satır sayısı (3)
        int k = matris1.GetLength(1); // matris1 sütun sayısı (4)
        int k2 = matris2.GetLength(0); // matris2 satır sayısı (4)
        int n = matris2.GetLength(1); // matris2 sütun sayısı (2)

        if (k != k2)
        {
            Console.WriteLine("Matris çarpılamaz: matris1 sütun sayısı ile matris2 satır sayısı eşit olmalı.");
            return;
        }

        int[,] sonuc = new int[m, n];

        for (int i = 0; i < m; i++)
        {
            for (int j = 0; j < n; j++)
            {
                int toplam = 0;
                for (int t = 0; t < k; t++)
                {
                    toplam += matris1[i, t] * matris2[t, j];
                }
                sonuc[i, j] = toplam;
            }
        }

        Console.WriteLine("Sonuç Matrisi: ");
        for (int i = 0; i < m; i++)
        {
            for (int j = 0; j < n; j++)
            {
                Console.Write(sonuc[i, j] + "\t");
            }
            Console.WriteLine();
        }

        Console.ReadLine();
    }
}


// notasyon O(n3)'tür.


