using System;
using System.Collections.Generic;

class Node
{
    public int Value;
    public Node Left;
    public Node Right;

    public Node(int value)
    {
        Value = value;
        Left = Right = null;
    }
}

class BinarySearchTree
{
    public Node Root;

    public void Insert(int value)
    {
        Root = InsertRec(Root, value);
    }

    private Node InsertRec(Node root, int value)
    {
        if (root == null)
        {
            root = new Node(value);
            return root;
        }

        if (value < root.Value)
            root.Left = InsertRec(root.Left, value);
        else
            root.Right = InsertRec(root.Right, value);

        return root;
    }

    // Preorder (Root - Left - Right)
    public void Preorder(Node root)
    {
        if (root == null) return;

        Console.Write(root.Value + " ");
        Preorder(root.Left);
        Preorder(root.Right);
    }

    // Inorder (Left - Root - Right)
    public void Inorder(Node root)
    {
        if (root == null) return;

        Inorder(root.Left);
        Console.Write(root.Value + " ");
        Inorder(root.Right);
    }

    // Postorder (Left - Right - Root)
    public void Postorder(Node root)
    {
        if (root == null) return;

        Postorder(root.Left);
        Postorder(root.Right);
        Console.Write(root.Value + " ");
    }

    // Level-order (BFS)
    public void LevelOrder(Node root)
    {
        if (root == null) return;

        Queue<Node> queue = new Queue<Node>();
        queue.Enqueue(root);

        while (queue.Count > 0)
        {
            Node temp = queue.Dequeue();
            Console.Write(temp.Value + " ");

            if (temp.Left != null)
                queue.Enqueue(temp.Left);

            if (temp.Right != null)
                queue.Enqueue(temp.Right);
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        BinarySearchTree bst = new BinarySearchTree();

        Console.Write("Kaç tane değer gireceksiniz? : ");
        int n = int.Parse(Console.ReadLine());

        Console.WriteLine("Değerleri giriniz:");

        for (int i = 0; i < n; i++)
        {
            int value = int.Parse(Console.ReadLine());
            bst.Insert(value);
        }

        Console.Write("Preorder: ");
        bst.Preorder(bst.Root);
        Console.WriteLine();

        Console.Write("Inorder: ");
        bst.Inorder(bst.Root);
        Console.WriteLine();

        Console.Write("Postorder: ");
        bst.Postorder(bst.Root);
        Console.WriteLine();

        Console.Write("Level-order: ");
        bst.LevelOrder(bst.Root);
        Console.WriteLine();
    }
}
