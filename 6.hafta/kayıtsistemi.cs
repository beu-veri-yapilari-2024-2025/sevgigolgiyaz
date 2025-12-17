using System;
using System.Collections.Generic;

class OyuncuNode
{
    public int FormaNo;
    public string Ad;
    public string Soyad;

    public OyuncuNode Sol;
    public OyuncuNode Sag;

    public OyuncuNode(int formaNo, string ad, string soyad)
    {
        FormaNo = formaNo;
        Ad = ad;
        Soyad = soyad;
        Sol = Sag = null;
    }
}

class OyuncuBST
{
    public OyuncuNode Kok;

    // Oyuncu ekleme (BST kuralına göre)
    public void Ekle(int formaNo, string ad, string soyad)
    {
        Kok = EkleRec(Kok, formaNo, ad, soyad);
    }

    private OyuncuNode EkleRec(OyuncuNode node, int formaNo, string ad, string soyad)
    {
        if (node == null)
            return new OyuncuNode(formaNo, ad, soyad);

        if (formaNo < node.FormaNo)
            node.Sol = EkleRec(node.Sol, formaNo, ad, soyad);
        else if (formaNo > node.FormaNo)
            node.Sag = EkleRec(node.Sag, formaNo, ad, soyad);

        return node;
    }

    // Oyuncu arama
    public OyuncuNode Ara(int formaNo)
    {
        return AraRec(Kok, formaNo);
    }

    private OyuncuNode AraRec(OyuncuNode node, int formaNo)
    {
        if (node == null || node.FormaNo == formaNo)
            return node;

        if (formaNo < node.FormaNo)
            return AraRec(node.Sol, formaNo);

        return AraRec(node.Sag, formaNo);
    }

    // Oyuncu silme
    public void Sil(int formaNo)
    {
        Kok = SilRec(Kok, formaNo);
    }

    private OyuncuNode SilRec(OyuncuNode root, int formaNo)
    {
        if (root == null) return null;

        if (formaNo < root.FormaNo)
            root.Sol = SilRec(root.Sol, formaNo);
        else if (formaNo > root.FormaNo)
            root.Sag = SilRec(root.Sag, formaNo);
        else
        {
            // 1) Yaprak düğüm ise
            if (root.Sol == null && root.Sag == null)
                return null;

            // 2) Tek çocuk varsa
            if (root.Sol == null)
                return root.Sag;
            if (root.Sag == null)
                return root.Sol;

            // 3) İki çocuk varsa → sağ alttaki en küçük değer ile değiştir
            OyuncuNode min = MinBul(root.Sag);

            root.FormaNo = min.FormaNo;
            root.Ad = min.Ad;
            root.Soyad = min.Soyad;

            root.Sag = SilRec(root.Sag, min.FormaNo);
        }

        return root;
    }

    // Preorder dolaşma
    public void Preorder(OyuncuNode node)
    {
        if (node == null) return;

        Console.WriteLine($"{node.FormaNo} - {node.Ad} {node.Soyad}");
        Preorder(node.Sol);
        Preorder(node.Sag);
    }

    // Inorder dolaşma
    public void Inorder(OyuncuNode node)
    {
        if (node == null) return;

        Inorder(node.Sol);
        Console.WriteLine($"{node.FormaNo} - {node.Ad} {node.Soyad}");
        Inorder(node.Sag);
    }

    // Postorder dolaşma
    public void Postorder(OyuncuNode node)
    {
        if (node == null) return;

        Postorder(node.Sol);
        Postorder(node.Sag);
        Console.WriteLine($"{node.FormaNo} - {node.Ad} {node.Soyad}");
    }

    // Level Order (Genişlik Öncelikli)
    public void LevelOrder(OyuncuNode node)
    {
        if (node == null) return;

        Queue<OyuncuNode> kuyruk = new Queue<OyuncuNode>();
        kuyruk.Enqueue(node);

        while (kuyruk.Count > 0)
        {
            var temp = kuyruk.Dequeue();
            Console.WriteLine($"{temp.FormaNo} - {temp.Ad} {temp.Soyad}");

            if (temp.Sol != null) kuyruk.Enqueue(temp.Sol);
            if (temp.Sag != null) kuyruk.Enqueue(temp.Sag);
        }
    }

    // En küçük forma
    public OyuncuNode MinBul(OyuncuNode node)
    {
        while (node.Sol != null)
            node = node.Sol;
        return node;
    }

    // En büyük forma
    public OyuncuNode MaxBul(OyuncuNode node)
    {
        while (node.Sag != null)
            node = node.Sag;
        return node;
    }
}

class Program
{
    static void Main(string[] args)
    {
        OyuncuBST bst = new OyuncuBST();

        Console.WriteLine("İlk eklediğiniz oyuncu SMAÇÖR olacak ve ağacın kökü olacaktır.\n");

        Console.Write("Kaç oyuncu ekleyeceksiniz? : ");
        int n = int.Parse(Console.ReadLine());

        for (int i = 0; i < n; i++)
        {
            Console.WriteLine($"\n{i + 1}. Oyuncu Bilgileri:");

            Console.Write("Forma Numarası: ");
            int forma = int.Parse(Console.ReadLine());

            Console.Write("Ad: ");
            string ad = Console.ReadLine();

            Console.Write("Soyad: ");
            string soyad = Console.ReadLine();

            bst.Ekle(forma, ad, soyad);
        }

        Console.WriteLine("\n--- PREORDER ---");
        bst.Preorder(bst.Kok);

        Console.WriteLine("\n--- INORDER ---");
        bst.Inorder(bst.Kok);

        Console.WriteLine("\n--- POSTORDER ---");
        bst.Postorder(bst.Kok);

        Console.WriteLine("\n--- LEVEL ORDER ---");
        bst.LevelOrder(bst.Kok);

        Console.WriteLine("\nEn küçük forma numarası:");
        var min = bst.MinBul(bst.Kok);
        Console.WriteLine($"{min.FormaNo} - {min.Ad} {min.Soyad}");

        Console.WriteLine("\nEn büyük forma numarası:");
        var max = bst.MaxBul(bst.Kok);
        Console.WriteLine($"{max.FormaNo} - {max.Ad} {max.Soyad}");
    }
}
