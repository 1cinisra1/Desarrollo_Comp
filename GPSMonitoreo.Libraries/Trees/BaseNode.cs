using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Libraries.Trees
{
	public class BaseNode<TId, TTree, TNode> where TNode : BaseNode<TId, TTree, TNode>, new() where TTree : BaseNode<TId, TTree, TNode>
	{

		public TId Id;
		public string Description;

		public List<TNode> Children;

		public TNode Parent;

		public TTree Tree;

		public int Level;


		public virtual void Add(TNode node)
		{
			if (Children == null)
			{
				Children = new List<TNode>();
			}

			Children.Add(node);

			node.Parent = this as TNode;

			//((BaseNode<TId, TTree, TNode>)node).Parent = this;

			//if(this.Level == 0)
			//{
			//	node.Tree = this as TTree;
			//}
			//else
			//{
			//	node.Tree = this.Tree;
			//}

			node.Tree = this.Tree;


			

			//Nodes.Add(node);
			//node.Parent = this;
			node.Level = this.Level + 1;

		}
		public void Populate(System.Data.DataTable dataTable, string idFieldName, string descriptionFieldName, string levelFieldName, Action<TNode, System.Data.DataRow> childAdded = null)
		{
			TNode currentNode;
			TNode childNode;


			currentNode = this as TNode;

			int level = 0;
			int nextLevel;

			int x = 0;

			System.Data.DataRow next;

			foreach (System.Data.DataRow row in dataTable.Rows)
			{
				level = Convert.ToInt32(row[levelFieldName]);


				childNode = new TNode()
				{
					Id = (TId)row[idFieldName],
					Description = (string)row[descriptionFieldName]
				};

				currentNode.Add(childNode);

				if(childAdded != null)
				{
					childAdded(childNode, row);
				}

				var rows = dataTable.Rows;

				if ((x + 1) < rows.Count)
				{
					next = rows[x + 1];

					nextLevel = Convert.ToInt32(next[levelFieldName]); ;

					if (nextLevel > level)
					{
						currentNode = childNode;
					}
					else if (nextLevel < level)
					{
						int diff = level - nextLevel;
						while (diff > 0)
						{
							currentNode = currentNode.Parent;
							diff--;
						}
					}
				}
				x++;
			}
		}
	}
}
