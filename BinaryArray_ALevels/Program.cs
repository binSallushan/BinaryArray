using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryArray_ALevels
{
    internal class Program
    {
        internal static void Main()
        {
            const int nullPointer = -1;
            int rootPointer = 0;
            int freePointer = 0;
            var binaryArray = new Node[10];
            
            void InitializeArray()
            {
                rootPointer = nullPointer;
                freePointer = 0;
                for (int i = 0; i < binaryArray.Length - 1; i++)
                {
                    binaryArray[i] = new Node(i + 1, 0, 0);
                }
                binaryArray[binaryArray.Length - 1] = new Node(nullPointer, nullPointer, 0);
            }

            void InsertData(int data)
            {
                if (freePointer == nullPointer)
                    throw new Exception("Array is full");
                var itemAddPointer = freePointer;
                freePointer = binaryArray[freePointer].LeftPointer;
                var itemPointer = rootPointer;
                bool rightBranch = false;
                bool leftBranch = false;
                int pointerToAttachNewNode = nullPointer;
                if (rootPointer == nullPointer)                
                    rootPointer = itemAddPointer;                    
                else
                {
                    do
                    {
                        pointerToAttachNewNode = itemPointer;                        
                        if (data >= binaryArray[pointerToAttachNewNode].Data)
                        {
                            itemPointer = binaryArray[pointerToAttachNewNode].RightPointer;
                            rightBranch = true;
                            leftBranch  = false;
                        }
                        else
                        {
                            itemPointer = binaryArray[pointerToAttachNewNode].LeftPointer;
                            leftBranch = true;
                            rightBranch = false;
                        }
                    }while (itemPointer != nullPointer);

                    if (rightBranch)
                        binaryArray[pointerToAttachNewNode].RightPointer = itemAddPointer;
                    else if (leftBranch)
                        binaryArray[pointerToAttachNewNode].LeftPointer = itemAddPointer;
                }

                binaryArray[itemAddPointer].Data = data;
                binaryArray[itemAddPointer].RightPointer = nullPointer;
                binaryArray[itemAddPointer].LeftPointer = nullPointer;                                                    
            }

            void InsertItems(int[] data)
            {
                foreach(var item in data)
                    InsertData(item);
            }

            InitializeArray();
            InsertItems([10, 20, 5, 15, 2, 30, 25, 35, 28]);
            //InsertItems([10, 20, 5]);                        
                 
            int FindImmediateParentPointer(int childPointer, int parentCheckingStart = 0)
            {
                if (childPointer == nullPointer || childPointer > binaryArray.Length - 1)
                    throw new ArgumentException(nameof(childPointer));
                if (binaryArray[childPointer] == null)
                    throw new Exception("Array is not initialized");
                if (childPointer == rootPointer)
                    throw new ArgumentException(nameof(childPointer));

                var parent = parentCheckingStart;
                var parentFound = false;

                if (binaryArray[parent].LeftPointer == nullPointer && binaryArray[parent].RightPointer == nullPointer)
                    return nullPointer;
                
                if (binaryArray[parent].LeftPointer == childPointer)
                {                    
                    parentFound = true;
                }
                else
                {
                    if (binaryArray[parent].LeftPointer != nullPointer)
                        parent = FindImmediateParentPointer(childPointer, binaryArray[parent].LeftPointer);

                    if (parent != nullPointer)
                        if (binaryArray[parent].LeftPointer == childPointer || binaryArray[parent].RightPointer == childPointer)
                            parentFound = true;
                }
                    
                if (!parentFound)
                {
                    parent = parentCheckingStart;

                    if (binaryArray[parent].RightPointer == childPointer)                   
                        return parent;                  

                    else if(binaryArray[parent].RightPointer == nullPointer)                                            
                        return nullPointer;                    

                    else                    
                        parent = FindImmediateParentPointer(childPointer, binaryArray[parent].RightPointer);                                            
                }

                return parent;
            }

            List<int> FindAllParentPointers(int childPointer)
            {
                if (childPointer == nullPointer || childPointer > binaryArray.Length - 1)
                    throw new ArgumentException(nameof(childPointer));
                if (binaryArray[childPointer] == null)
                    throw new Exception("Array is not initialized");
                if (childPointer == rootPointer)
                    throw new ArgumentException(nameof(childPointer));

                var parentList = new List<int>();
                var parent = FindImmediateParentPointer(childPointer);
                parentList.Add(parent);

                if (parent == rootPointer)
                    return parentList;
                else
                    parentList.AddRange(FindAllParentPointers(parent));

                return parentList;
            }
            List<int> FindLeafPointers(int itemPointer = 0)
            {
                if (itemPointer == nullPointer || itemPointer > binaryArray.Length - 1)
                    throw new ArgumentException(nameof(itemPointer));
                if (binaryArray[itemPointer] == null)
                    throw new Exception("Array is not initialized");
                var leaves = new List<int>();

                if (binaryArray[itemPointer].LeftPointer == nullPointer && binaryArray[itemPointer].RightPointer == nullPointer)
                {
                    leaves.Add(itemPointer);
                    return leaves;
                }

                if (binaryArray[itemPointer].LeftPointer != nullPointer)
                    leaves.AddRange(FindLeafPointers(binaryArray[itemPointer].LeftPointer));
                if (binaryArray[itemPointer].RightPointer != nullPointer)
                    leaves.AddRange(FindLeafPointers(binaryArray[itemPointer].RightPointer));

                return leaves;
            }

            string PrintBinaryTree(string printedTree, int pointer = 0)
            {
                if (pointer == nullPointer || pointer > binaryArray.Length - 1)
                    throw new ArgumentException(nameof(pointer));
                if (binaryArray[pointer] == null)
                    throw new Exception("Array is not initialized");

                var str = "";
                return null;
            }
            //Console.WriteLine(binaryArray[9].Data);
            Console.WriteLine("Immediate parent of pointer 8");
            Console.WriteLine(binaryArray[FindImmediateParentPointer(8)].Data);

            Console.WriteLine("Leaves");
            foreach (var item in FindLeafPointers())
            {
                Console.WriteLine(binaryArray[item].Data);   
            }

            Console.WriteLine("All parents of pointer 8");
            foreach(var item in FindAllParentPointers(8))
            {
                Console.WriteLine(binaryArray[item].Data);
            }

            Console.WriteLine("All parents of all leaves:");
            foreach(var item in FindLeafPointers())
            {
                foreach (var parent in FindAllParentPointers(item))
                {
                    Console.WriteLine(binaryArray[parent].Data);
                }
            }
        }

        class Node
        {
            public int LeftPointer;
            public int RightPointer;
            public int Data;
            public Node() { }

            public Node(int leftPointer, int rightPointer, int data)
            {
                LeftPointer = leftPointer;
                RightPointer = rightPointer;
                Data = data;
            }
        }
    }
}
