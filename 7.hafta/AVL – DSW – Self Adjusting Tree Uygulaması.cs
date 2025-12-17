using System;
using System.Collections.Generic;
using System.Globalization;

namespace AvlDswSelfAdjusting
{
    class Program
    {
        // Türkçe alfabetik karşılaştırma (I/İ, Ç vb. doğru olsun diye)
        static readonly CultureInfo Tr = new CultureInfo("tr-TR");

        static void Main()
        {
            // Verilen karakterler (sırayla eklenecek)
            string[] dizi = { "S", "E", "L", "İ", "M", "K", "A", "Ç", "T", "I" };

            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("=== 1) AVL Ağacını Oluşturma ===");
            var avl = new AvlTree(Tr);
            foreach (var x in dizi)
            {
                avl.Insert(x);
            }

            YazdirTumGezinimler("AVL SON HALİ", avl.Root);

            Console.WriteLine();
            Console.WriteLine("=== 2) DSW (Day–Stout–Warren) ile Yeniden Dengeleme ===");
            Console.WriteLine("DSW'nin amacı: Ağaç yüksekliğini olabilecek minimum seviyeye getirmek");
            Console.WriteLine("DSW'yi geliştirenler: Colin Day, Quentin F. Stout, Bette L. Warren");
            Console.WriteLine("DSW iki temel adım: Right rotation chain (backbone) + Left rotation balancing");
            Console.WriteLine();

            // DSW, BST üzerinde çalışır. AVL zaten BST’dir.
            var dsw = new DswBalancer(Tr);

            // Aynı ağacı DSW ile yeniden düzenleyelim:
            // (DSW rotasyonları parent bağlantılarıyla çalışacağı için root’u dsw üzerinden güncelliyoruz)
            TreeNode dswRoot = avl.Root;
            dsw.ApplyDSW(ref dswRoot);

            YazdirTumGezinimler("DSW SONRASI", dswRoot);

            Console.WriteLine();
            Console.WriteLine("=== 3) Self-Adjusting Tree (Frequency / Priority Rotation) ===");
            // Self-adjust için: erişimde frequency++ ve yüksek frekanslı düğüm root'a yaklaştırılır.
            var sat = new SelfAdjustingTree(Tr);
            sat.Root = dswRoot; // DSW sonrası ağacı devralalım

            // Örnek erişimler (istersen değiştir):
            // En çok erişilenin root’a yaklaşması beklenir.
            string[] erisimler = { "M", "M", "M", "A", "A", "T", "İ", "İ", "İ", "İ" };

            foreach (var key in erisimler)
            {
                Console.WriteLine($"Arama/Erişim: {key}");
                sat.SearchWithFrequency(key);
                YazdirTumGezinimler($"SELF-ADJUST SONRASI (erişilen: {key})", sat.Root);
                Console.WriteLine(new string('-', 60));
            }

            Console.WriteLine("Bitti.");
        }

        static void YazdirTumGezinimler(string baslik, TreeNode root)
        {
            Console.WriteLine($"\n--- {baslik} ---");

            Console.Write("InOrder     : ");
            PrintInOrder(root);
            Console.WriteLine();

            Console.Write("PreOrder    : ");
            PrintPreOrder(root);
            Console.WriteLine();

            Console.Write("PostOrder   : ");
            PrintPostOrder(root);
            Console.WriteLine();

            Console.Write("LevelOrder  : ");
            PrintLevelOrder(root);
            Console.WriteLine();
        }

        // İstenen print fonksiyonları
        static void PrintInOrder(TreeNode n)
        {
            if (n == null) return;
            PrintInOrder(n.Left);
            Console.Write($"{n.Key}({n.Frequency}) ");
            PrintInOrder(n.Right);
        }

        static void PrintPreOrder(TreeNode n)
        {
            if (n == null) return;
            Console.Write($"{n.Key}({n.Frequency}) ");
            PrintPreOrder(n.Left);
            PrintPreOrder(n.Right);
        }

        static void PrintPostOrder(TreeNode n)
        {
            if (n == null) return;
            PrintPostOrder(n.Left);
            PrintPostOrder(n.Right);
            Console.Write($"{n.Key}({n.Frequency}) ");
        }

        static void PrintLevelOrder(TreeNode root)
        {
            if (root == null) return;
            var q = new Queue<TreeNode>();
            q.Enqueue(root);
            while (q.Count > 0)
            {
                var cur = q.Dequeue();
                Console.Write($"{cur.Key}({cur.Frequency}) ");
                if (cur.Left != null) q.Enqueue(cur.Left);
                if (cur.Right != null) q.Enqueue(cur.Right);
            }
        }
    }

    // Düğüm yapısı (frequency + parent dahil)
    public class TreeNode
    {
        public string Key;
        public int Height;           // AVL için
        public int Frequency;        // Self-adjust için

        public TreeNode? Left;
        public TreeNode? Right;
        public TreeNode? Parent;

        public TreeNode(string key)
        {
            Key = key;
            Height = 1;
            Frequency = 0;
        }
    }

    // ========= 1) AVL =========
    public class AvlTree
    {
        public TreeNode? Root { get; set; }
        private readonly CultureInfo _culture;

        public AvlTree(CultureInfo culture) => _culture = culture;

        // İstenen fonksiyon: Insert()
        public void Insert(string key)
        {
            Root = Insert(Root, key, parent: null);
        }

        private TreeNode Insert(TreeNode? node, string key, TreeNode? parent)
        {
            if (node == null)
            {
                var n = new TreeNode(key);
                n.Parent = parent;
                return n;
            }

            int cmp = string.Compare(key, node.Key, ignoreCase: false, _culture);

            if (cmp < 0)
            {
                node.Left = Insert(node.Left, key, node);
            }
            else if (cmp > 0)
            {
                node.Right = Insert(node.Right, key, node);
            }
            else
            {
                // Aynı key gelirse eklemiyoruz (BST’de tekrar yok varsayımı)
                return node;
            }

            // Yükseklik güncelle
            node.Height = 1 + Math.Max(GetHeight(node.Left), GetHeight(node.Right));

            // Denge kontrol
            int balance = GetBalance(node);

            // Dönüşümler (LL, RR, LR, RL)
            // LL: sol-sol ağır
            if (balance > 1 && string.Compare(key, node.Left!.Key, false, _culture) < 0)
                return RotateRight(node);

            // RR: sağ-sağ ağır
            if (balance < -1 && string.Compare(key, node.Right!.Key, false, _culture) > 0)
                return RotateLeft(node);

            // LR: sol-sağ
            if (balance > 1 && string.Compare(key, node.Left!.Key, false, _culture) > 0)
            {
                node.Left = RotateLeft(node.Left);
                if (node.Left != null) node.Left.Parent = node;
                return RotateRight(node);
            }

            // RL: sağ-sol
            if (balance < -1 && string.Compare(key, node.Right!.Key, false, _culture) < 0)
            {
                node.Right = RotateRight(node.Right);
                if (node.Right != null) node.Right.Parent = node;
                return RotateLeft(node);
            }

            return node;
        }

        // İstenen fonksiyonlar: RotateLeft(), RotateRight()
        public TreeNode RotateLeft(TreeNode x)
        {
            TreeNode y = x.Right!;
            TreeNode? T2 = y.Left;

            // Rotasyon
            y.Left = x;
            x.Right = T2;

            // Parent güncelle
            y.Parent = x.Parent;
            x.Parent = y;
            if (T2 != null) T2.Parent = x;

            // Yükseklik güncelle
            x.Height = 1 + Math.Max(GetHeight(x.Left), GetHeight(x.Right));
            y.Height = 1 + Math.Max(GetHeight(y.Left), GetHeight(y.Right));

            return y;
        }

        public TreeNode RotateRight(TreeNode y)
        {
            TreeNode x = y.Left!;
            TreeNode? T2 = x.Right;

            // Rotasyon
            x.Right = y;
            y.Left = T2;

            // Parent güncelle
            x.Parent = y.Parent;
            y.Parent = x;
            if (T2 != null) T2.Parent = y;

            // Yükseklik güncelle
            y.Height = 1 + Math.Max(GetHeight(y.Left), GetHeight(y.Right));
            x.Height = 1 + Math.Max(GetHeight(x.Left), GetHeight(x.Right));

            return x;
        }

        // İstenen fonksiyonlar: GetHeight(), GetBalance()
        public int GetHeight(TreeNode? n) => n?.Height ?? 0;

        public int GetBalance(TreeNode? n)
        {
            if (n == null) return 0;
            return GetHeight(n.Left) - GetHeight(n.Right);
        }
    }

    // ========= 2) DSW (Day–Stout–Warren) =========
    public class DswBalancer
    {
        private readonly CultureInfo _culture;
        public DswBalancer(CultureInfo culture) => _culture = culture;

        // İstenen fonksiyonlar: CreateBackbone(), BalanceBackbone(), ApplyDSW()
        public void ApplyDSW(ref TreeNode? root)
        {
            if (root == null) return;

            // Parent bağlarını toparla (bazı rotasyonlardan sonra güvenli olsun)
            FixParents(root, null);

            int n = CreateBackbone(ref root);
            BalanceBackbone(ref root, n);

            // Son kez parent fix
            FixParents(root, null);
        }

        // 1) Backbone (vine) oluşturma: sağ omurga elde etmek için right rotation zinciri
        public int CreateBackbone(ref TreeNode? root)
        {
            int count = 0;
            TreeNode? grandParent = null;
            TreeNode? parent = root;

            while (parent != null)
            {
                if (parent.Left != null)
                {
                    // Right rotate parent
                    TreeNode left = parent.Left;
                    parent.Left = left.Right;
                    if (left.Right != null) left.Right.Parent = parent;

                    left.Right = parent;
                    left.Parent = grandParent;
                    parent.Parent = left;

                    if (grandParent == null)
                        root = left;
                    else
                        grandParent.Right = left;

                    // parent değişmez, aynı seviyede devam
                    parent = left;
                }
                else
                {
                    count++;
                    grandParent = parent;
                    parent = parent.Right;
                }
            }
            return count;
        }

        // 2) Backbone’u dengeleme: belirli sayıda left rotation ile yüksekliği küçültme
        public void BalanceBackbone(ref TreeNode? root, int n)
        {
            if (root == null) return;

            int m = GreatestPowerOf2LessThanNPlus1(n + 1) - 1;

            // İlk sıkıştırma
            Compress(ref root, n - m);

            // Sonraki sıkıştırmalar
            while (m > 1)
            {
                m /= 2;
                Compress(ref root, m);
            }
        }

        // Left rotation sıkıştırma
        private void Compress(ref TreeNode? root, int count)
        {
            TreeNode? grandParent = null;
            TreeNode? parent = root;
            TreeNode? child = root?.Right;

            for (int i = 0; i < count; i++)
            {
                if (parent == null || child == null) break;

                // Left rotate parent
                parent.Right = child.Left;
                if (child.Left != null) child.Left.Parent = parent;

                child.Left = parent;
                child.Parent = grandParent;
                parent.Parent = child;

                if (grandParent == null)
                    root = child;
                else
                    grandParent.Right = child;

                // Bir sonraki ikiliye ilerle
                grandParent = child;
                parent = grandParent.Right;
                child = parent?.Right;
            }
        }

        private int GreatestPowerOf2LessThanNPlus1(int x)
        {
            int p = 1;
            while (p <= x) p <<= 1;
            return p >> 1;
        }

        private void FixParents(TreeNode? node, TreeNode? parent)
        {
            if (node == null) return;
            node.Parent = parent;
            FixParents(node.Left, node);
            FixParents(node.Right, node);
        }
    }

    // ========= 3) Self-Adjusting (Frequency / Priority) =========
    public class SelfAdjustingTree
    {
        public TreeNode? Root { get; set; }
        private readonly CultureInfo _culture;

        public SelfAdjustingTree(CultureInfo culture) => _culture = culture;

        // İstenen fonksiyonlar:
        // SearchWithFrequency(), PriorityRotate(), AdjustTowardsRoot()

        public TreeNode? SearchWithFrequency(string key)
        {
            TreeNode? cur = Root;
            while (cur != null)
            {
                int cmp = string.Compare(key, cur.Key, false, _culture);
                if (cmp == 0)
                {
                    // erişim sayısı artar
                    cur.Frequency++;

                    // priority mantığıyla yukarı taşı
                    AdjustTowardsRoot(cur);
                    return cur;
                }
                cur = (cmp < 0) ? cur.Left : cur.Right;
            }
            return null;
        }

        // Düğümü, parent'ından daha yüksek frequency’ye sahipse yukarı doğru döndürür.
        public void AdjustTowardsRoot(TreeNode node)
        {
            while (node.Parent != null)
            {
                TreeNode parent = node.Parent;

                // Öncelik kuralı: frequency büyükse yukarı çıksın
                if (node.Frequency > parent.Frequency)
                {
                    PriorityRotate(node);
                }
                else
                {
                    // frekans eşit/az ise dur (istersen eşitte de yukarı taşıyabilirsin)
                    break;
                }
            }

            // Root’u güncelle
            while (Root?.Parent != null) Root = Root.Parent;
            if (node.Parent == null) Root = node;
        }

        // Node, parent'ının solundaysa sağa; sağındaysa sola rotasyonla bir seviye yükselir.
        public void PriorityRotate(TreeNode node)
        {
            TreeNode parent = node.Parent!;
            TreeNode? grand = parent.Parent;

            if (parent.Left == node)
            {
                // Right rotation (node yukarı)
                parent.Left = node.Right;
                if (node.Right != null) node.Right.Parent = parent;

                node.Right = parent;
                parent.Parent = node;
            }
            else
            {
                // Left rotation (node yukarı)
                parent.Right = node.Left;
                if (node.Left != null) node.Left.Parent = parent;

                node.Left = parent;
                parent.Parent = node;
            }

            node.Parent = grand;

            if (grand == null)
            {
                Root = node;
            }
            else
            {
                if (grand.Left == parent) grand.Left = node;
                else grand.Right = node;
            }
        }
    }
}
