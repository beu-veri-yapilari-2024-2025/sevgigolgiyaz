using System;

namespace LinkedListStudent
{
    // Öğrenci veri yapısı
    class Student
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Number { get; set; }

        public Student(string first, string last, string number)
        {
            FirstName = first;
            LastName = last;
            Number = number;
        }

        public override string ToString()
        {
            return $"{FirstName} {LastName} ({Number})";
        }
    }

    // Tek bağlı liste için node
    class Node
    {
        public Student Data;
        public Node Next;

        public Node(Student data)
        {
            Data = data;
            Next = null;
        }
    }

    // Linked list sınıfı
    class LinkedList
    {
        private Node head;

        public LinkedList()
        {
            head = null;
        }

        // Başa ekle
        public void AddFirst(Student s)
        {
            Node n = new Node(s);
            n.Next = head;
            head = n;
        }

        // Sona ekle
        public void AddLast(Student s)
        {
            Node n = new Node(s);
            if (head == null)
            {
                head = n;
                return;
            }
            Node cur = head;
            while (cur.Next != null) cur = cur.Next;
            cur.Next = n;
        }

        // Belirli numaradan sonra ekle (arama numarasına göre)
        public bool InsertAfter(string targetNumber, Student s)
        {
            Node cur = head;
            while (cur != null)
            {
                if (cur.Data.Number == targetNumber)
                {
                    Node n = new Node(s);
                    n.Next = cur.Next;
                    cur.Next = n;
                    return true;
                }
                cur = cur.Next;
            }
            return false;
        }

        // Belirli numaradan önce ekle
        public bool InsertBefore(string targetNumber, Student s)
        {
            if (head == null) return false;
            if (head.Data.Number == targetNumber)
            {
                AddFirst(s);
                return true;
            }
            Node prev = null;
            Node cur = head;
            while (cur != null && cur.Data.Number != targetNumber)
            {
                prev = cur;
                cur = cur.Next;
            }
            if (cur == null) return false;
            Node n = new Node(s);
            prev.Next = n;
            n.Next = cur;
            return true;
        }

        // Belirli numaralı düğümü sil
        public bool Delete(string targetNumber)
        {
            if (head == null) return false;
            if (head.Data.Number == targetNumber)
            {
                head = head.Next;
                return true;
            }
            Node prev = null;
            Node cur = head;
            while (cur != null && cur.Data.Number != targetNumber)
            {
                prev = cur;
                cur = cur.Next;
            }
            if (cur == null) return false;
            prev.Next = cur.Next;
            return true;
        }

        // Hedef düğümün öncesini sil (hedefin kendisi kalır)
        public bool DeleteBefore(string targetNumber)
        {
            if (head == null || head.Next == null) return false;
            if (head.Data.Number == targetNumber) return false; // baştan önce yok
            // eğer ikinci düğüm hedefse, başı sil
            if (head.Next.Data.Number == targetNumber)
            {
                head = head.Next;
                return true;
            }
            Node prevPrev = null;
            Node prev = head;
            Node cur = head.Next;
            while (cur != null && cur.Data.Number != targetNumber)
            {
                prevPrev = prev;
                prev = cur;
                cur = cur.Next;
            }
            if (cur == null) return false; // bulunamadı
            // prev is the node before cur (target). Remove prev
            if (prevPrev == null) // shouldn't happen due to checks
            {
                head = cur;
            }
            else
            {
                prevPrev.Next = cur;
            }
            return true;
        }

        // Hedef düğümün sonrasını sil (hedef kalır)
        public bool DeleteAfter(string targetNumber)
        {
            Node cur = head;
            while (cur != null && cur.Data.Number != targetNumber) cur = cur.Next;
            if (cur == null || cur.Next == null) return false;
            cur.Next = cur.Next.Next;
            return true;
        }

        // Arama: numaraya göre
        public Node SearchByNumber(string number)
        {
            Node cur = head;
            while (cur != null)
            {
                if (cur.Data.Number == number) return cur;
                cur = cur.Next;
            }
            return null;
        }

        // Listele
        public void PrintAll()
        {
            if (head == null)
            {
                Console.WriteLine("Liste boş.");
                return;
            }
            Node cur = head;
            int i = 1;
            while (cur != null)
            {
                Console.WriteLine($"{i}. {cur.Data}");
                cur = cur.Next;
                i++;
            }
        }
    }

    class Program
    {
        static void Main()
        {
            LinkedList list = new LinkedList();
            bool running = true;
            while (running)
            {
                Console.WriteLine("\n--- Öğrenci LinkedList Menüsü ---");
                Console.WriteLine("1. Başa ekle");
                Console.WriteLine("2. Sona ekle");
                Console.WriteLine("3. Bir numaradan sonra ekle");
                Console.WriteLine("4. Bir numaradan önce ekle");
                Console.WriteLine("5. Numara ile sil");
                Console.WriteLine("6. Bir numaranın öncesini sil");
                Console.WriteLine("7. Bir numaranın sonrasını sil");
                Console.WriteLine("8. Numara ile ara");
                Console.WriteLine("9. Listele");
                Console.WriteLine("0. Çıkış");
                Console.Write("Seçiminiz: ");
                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        list.AddFirst(InputStudent());
                        break;
                    case "2":
                        list.AddLast(InputStudent());
                        break;
                    case "3":
                        Console.Write("Hedef numara (sonrasına ekle): ");
                        string t1 = Console.ReadLine();
                        Console.WriteLine(list.InsertAfter(t1, InputStudent()) ? "Eklendi." : "Hedef bulunamadı.");
                        break;
                    case "4":
                        Console.Write("Hedef numara (öncesine ekle): ");
                        string t2 = Console.ReadLine();
                        Console.WriteLine(list.InsertBefore(t2, InputStudent()) ? "Eklendi." : "Hedef bulunamadı.");
                        break;
                    case "5":
                        Console.Write("Silinecek numara: ");
                        string del = Console.ReadLine();
                        Console.WriteLine(list.Delete(del) ? "Silindi." : "Bulunamadı.");
                        break;
                    case "6":
                        Console.Write("Öncesi silinecek hedef numara: ");
                        string db = Console.ReadLine();
                        Console.WriteLine(list.DeleteBefore(db) ? "Öncesi silindi." : "Silme başarısız/bulunamadı.");
                        break;
                    case "7":
                        Console.Write("Sonrası silinecek hedef numara: ");
                        string da = Console.ReadLine();
                        Console.WriteLine(list.DeleteAfter(da) ? "Sonrası silindi." : "Silme başarısız/bulunamadı.");
                        break;
                    case "8":
                        Console.Write("Aranacak numara: ");
                        string s = Console.ReadLine();
                        var node = list.SearchByNumber(s);
                        Console.WriteLine(node != null ? $"Bulundu: {node.Data}" : "Bulunamadı.");
                        break;
                    case "9":
                        list.PrintAll();
                        break;
                    case "0":
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Geçersiz seçim.");
                        break;
                }
            }
            Console.WriteLine("Program sonlandı.");
        }

        static Student InputStudent()
        {
            Console.Write("Ad: ");
            string ad = Console.ReadLine();
            Console.Write("Soyad: ");
            string soyad = Console.ReadLine();
            Console.Write("Numara: ");
            string num = Console.ReadLine();
            return new Student(ad, soyad, num);
        }
    }
}
