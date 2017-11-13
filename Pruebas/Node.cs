using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pruebas
{



	//public class TreeWithRoles<TOf>: NodeWithRoles<TOf, int, byte>
	//{

	//}



	//public class Menu : TreeWithRoles<MenuItem>
	//{
	//	public Menu()
	//	{
	//		var ii = new MenuItem();
			
			
			
			
	//	}
	//}

	//public class MenuItem: NodeWithRoles<MenuItem, int, byte>
	//{

	//	public MenuItem()
	//	{
	//		var item = this.Nodes[0];
			
	//	}
	//}


	//public class NodeWithRoles<TOf, TRoles, TPermissions> : NodeOf<TOf>
	//{
	//	public List<TRoles> Roles;
	//	public List<TPermissions> Permissions;

	//	public NodeWithRoles()
	//	{
			
	//	}

	//}


	//public class Menu: MenuItem
	//{

	//}

	//public class MenuItem : ItemWithRoles
	//{
	//	public string Id;
	//	public string Title;

	//}

	//public class ItemWithRoles
	//{
	//	public List<int> Roles;
	//	public List<byte> Permissions;
	//}


	//public class TreeOf<TTree, TNode> : NodeOf<TNode>
	//{

		
	//}


	//public class NodeOf<TNode>
	//{
	//	public List<TNode> Nodes;

	//	//public Tree<TId> Tree;

	//	public Node<TNode> Parent;

	//	public int Level;

	//	public NodeOf()
	//	{
	//		var t = new TreeOf<Menu, MenuItem>();

	//		t.
			
			
	//	}

	//}


	//public class NodeWithRoles<TId>: NodeOf<TId, NodeWithRoles<TId>>
	//{

	//	public NodeWithRoles()
	//	{

	//	}

	//}


	//public class NodeOf<TId, TNode>
	//{
	//	public TId Id;
	//	public string Title;

	//	public List<TNode> Nodes;

	//	//public Tree<TId> Tree;

	//	public Node<TNode> Parent;

	//	public int Level;

	//}


	public class TreeWithRoles<TId>: NodeWithRoles<TId>
	{

	}

	public class NodeWithRoles<TId> : Node<TId>
	{
		public List<int> Roles;
	}

	//public class Menu: TreeOf<string, Menu, MenuItem>
	//{
	//	public Menu()
	//	{
			
			

			
	//	}


	//}

	//public class MenuItem: NodeOf<string, Menu, MenuItem>
	//{
	//	public MenuItem()
	//	{
			
	//	}

	//}

	public class TreeOf<TId, TTree,  TNode>: NodeOf<TId, TTree, TNode> where TNode : NodeOf<TId, TTree, TNode> where TTree : TreeOf<TId, TTree, TNode>
	{
		public TreeOf()
		{
			//this.Nodes[0].

		}
	}


	public class NodeOf<TId, TTree, TNode> where TNode: NodeOf<TId, TTree, TNode> where TTree: TreeOf<TId, TTree, TNode>
	{
		public TId Id;

		public List<TNode> Nodes;

		public TTree Tree;

		public TNode Parent;

		public int Level;


		public void Add(TNode node)
		{
			if (Nodes == null)
			{
				Nodes = new List<TNode>();
			}

			Nodes.Add(node);



			//var nodeOf = (NodeOf<TId, TTree, TNode>)node;


			//node.Parent = (TNode)(NodeOf<TId, TTree, TNode>)this;

			var casted = (TNode)(NodeOf<TId, TTree, TNode>)(TTree)this;



			node.Level = this.Level + 1;

			//Console.WriteLine("type: " + this.GetType().ToString());


			//Tree<TId> tree;

			//if (this.Tree == null)
			//{
			//	//i am the tree
			//	//node.Tree = (Tree<TId>)this;
			//	tree = (Tree<TId>)this;
			//}
			//else
			//{
			//	tree = this.Tree;
			//	//node.Tree = this.Tree;
			//}

			//node.Tree = tree;
			//if (tree.AllocateAllNodes)
			//{
			//	tree.AllNodes.Add(node.Id, node);
			//}
		}
	}




	public class Tree<TId> : Node<TId>
	{

		public bool AllocateAllNodes;

		public Dictionary<TId, Node<TId>> AllNodes;

		public Tree()
		{
			
			

		}

		public Tree(bool allocateAllNodes)
		{
			AllocateAllNodes = allocateAllNodes;
			AllNodes = new Dictionary<TId, Node<TId>>();
		}
	}

	public class Node<TId>
    {
		public TId Id;
		public string Title;

		public List<Node<TId>> Nodes;

		public Tree<TId> Tree;

		public Node<TId> Parent;

		public int Level;

		public void Add(Node<TId> node)
		{
			if(Nodes == null)
			{
				Nodes = new List<Node<TId>>();
			}

			Nodes.Add(node);
			node.Parent = this;
			node.Level = this.Level + 1;


			Tree<TId> tree;

			if (this.Tree == null)
			{
				//i am the tree
				//node.Tree = (Tree<TId>)this;
				tree = (Tree<TId>)this;
			}
			else
			{
				tree = this.Tree;
				//node.Tree = this.Tree;
			}

			node.Tree = tree;
			if(tree.AllocateAllNodes)
			{
				tree.AllNodes.Add(node.Id, node);
			}
		}
    }
}
