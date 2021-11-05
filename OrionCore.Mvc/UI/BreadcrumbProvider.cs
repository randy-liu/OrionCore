using OrionCore.API.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.IO;
using System.Resources;
using System.Text.RegularExpressions;

namespace OrionCore.Mvc.UI
{

	/// <summary>麵包屑提供者</summary>
	public interface IBreadcrumbProvider
	{
		/// <summary></summary>
		List<IBreadcrumb> GetPathList(string currentUrl);
	}






	/*===========================================*/

	/// <summary>麵包屑提供者</summary>
	public class BreadcrumbProvider : IBreadcrumbProvider
	{
			   
		private List<BreadcrumbNode> _nodeList;
		private BreadcrumbNode _root;
		private ResourceManager _resource;

			   
		/// <summary></summary>
		public BreadcrumbProvider(string configPath) : this(configPath, null) { }

		/// <summary></summary>
		public BreadcrumbProvider(string configPath, ResourceManager resource)
		{
			using (var stream = File.OpenRead(configPath))
			{ init(stream, resource); }
		}

		/// <summary></summary>
		public BreadcrumbProvider(Stream stream) : this(stream, null) { }

		/// <summary></summary>
		public BreadcrumbProvider(Stream stream, ResourceManager resource)
		{
			init(stream, resource);
		}





		private void init(Stream stream, ResourceManager resource)
		{
			var doc = XDocument.Load(stream);
			_nodeList = new List<BreadcrumbNode>();
			_root = mappingToNode(doc.Root);
			_resource = resource;
		}



		private string attr(XElement el, XName name)
		{
			string value = (string)el.Attribute(name);
			if (value.NoText()) { return null; }

			if(name != "url") { return value; }
			if(value[0] != '~') { return value; }
			return value; 
		}




		private BreadcrumbNode mappingToNode(XElement el, BreadcrumbNode parent = null)
		{
			var node = new BreadcrumbNode
			{
				Name    = attr(el,"name"),
				Url     = attr(el, "url"),
				Icon    = attr(el, "icon"),
				Pattern = attr(el, "pattern"),
				Parent  = parent,
			};
			node.ChildNodes = el.Elements("node").Select(x => mappingToNode(x, node)).ToList();

			_nodeList.Add(node);
			return node;
		}



		private BreadcrumbNode findNode(string basePath)
		{
			if (!basePath.HasText()) { return _root; }
			basePath = basePath.ToLower();

			foreach (var node in _nodeList)
			{
				if (node.Pattern.HasText())
				{
					if (basePath.IsMatch(node.Pattern)) { return node; }
				}
				else if (node.Url.HasText())
				{
					string url = node.Url.TrimStart('~').ToLower();
					if (basePath.Contains(url)) { return node; }
				}
			}
			return _root;
		}



		/// <summary></summary>
		public List<IBreadcrumb> GetPathList(string currentUrl)
		{
			var node = findNode(currentUrl);
			var result = new List<IBreadcrumb>();

			do
			{
				result.Add(node);
				node = node.Parent;
			} while (node != null);
			
			result.Reverse();
			if(_resource == null) { return result; }


			/* 多語轉換 */
			return result.Select(x => new BreadcrumbNode
			{
				Name = _resource.GetString(x.Name) ?? x.Name,
				Url = x.Url,
				Icon = x.Icon,
			}).ToList(x => (IBreadcrumb)x);
		}

	}



}
