using OrionCore.API.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Resources;
using System.Text.RegularExpressions;

namespace OrionCore.Mvc.UI
{

	/// <summary>選單提供者</summary>
	public interface IMenuProvider
	{
		/// <summary></summary>
		string AreaName { get; }

		/// <summary></summary>
		List<MenuItem> GetList(IPrincipal user, string currentUrl);

		/// <summary></summary>
		List<MenuItem> GetAllowList(IPrincipal user, string currentUrl);
	}


	/// <summary>選單提供者</summary>
	public class MenuProvider : IMenuProvider
	{
		private List<MenuItem> _menuList;
		private ResourceManager _resource;

		/// <summary></summary>
		public string AreaName { get; private set; }



		/// <summary></summary>
		public MenuProvider(string configPath) : this(null, configPath, null) { }

		/// <summary></summary>
		public MenuProvider(string configPath, ResourceManager resource) : this(null, configPath, resource) { }

		/// <summary></summary>
		public MenuProvider(string areaName, string configPath) : this(areaName, configPath, null) { }

		/// <summary></summary>
		public MenuProvider(string areaName, string configPath, ResourceManager resource)
		{
			using (var stream = File.OpenRead(configPath))
			{ init(areaName, stream, resource); }
		}

		/// <summary></summary>
		public MenuProvider(Stream stream) : this(null, stream, null) { }

		/// <summary></summary>
		public MenuProvider(Stream stream, ResourceManager resource) : this(null, stream, resource) { }

		/// <summary></summary>
		public MenuProvider(string areaName, Stream stream) : this(areaName, stream, null) { }

		/// <summary></summary>
		public MenuProvider(string areaName, Stream stream, ResourceManager resource)
		{
			init(areaName, stream, resource);
		}


		/// <summary></summary>
		private void init(string areaName, Stream stream, ResourceManager resource)
		{
			AreaName = areaName;
			_resource = resource;

			var doc = XDocument.Load(stream);


			/* 用 XSD 驗證 */
			Stream xsdStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("OrionCore.Mvc.UI.menus.xsd");
			var schemas = new XmlSchemaSet();
			schemas.Add("", XmlReader.Create(xsdStream));
			doc.Validate(schemas, (o, e) => { throw new XmlException(e.Message); });

            _menuList = getMenuList(doc.Root);
		}


        private List<MenuItem> getMenuList(XElement root)
        {
            List<MenuItem> list = root
                .Elements("menu")
                .Concat(root.Elements("sub"))
                .Select(menu => new MenuItem
                {
                    Name = attr(menu, "name"),
                    Url = attr(menu, "url"),
                    Target = attr(menu, "target"),
                    ACT = attr(menu, "act"),
                    Icon = attr(menu, "icon"),
                    Pattern = attr(menu, "pattern"),

                    UseFirstSubUrl = "true".Equals(attr(menu, "useFirstSubUrl")),
                    SubItems = getMenuList(menu),
                })
                .ToList();

            return list;
        }



        private string attr(XElement el, XName name) 
		{
			string value = (string)el.Attribute(name);
			if (value.NoText()) { return null; }

			if (name != "url") { return value; }
			if (value[0] != '~') { return value; }
			if (AreaName.NoText()) { return value; }

			value = Regex.Replace(value, "^~/", "~/" + AreaName + "/");
			return value;
		}



		private bool isContainsPath(string basePath, MenuItem item)
		{
			if (!item.Url.HasText() || !basePath.HasText()) { return false; }

			if (item.Pattern.HasText()) { return basePath.IsMatch(item.Pattern); }

			string path = item.Url.TrimStart('~').ToLower();
			return basePath.ToLower().Contains(path); 
		}




		/// <summary></summary>
		public List<MenuItem> GetList(IPrincipal user, string currentUrl)
		{
			var result = new List<MenuItem>();
            result = cloneListByRole(_menuList, user, currentUrl);


			if (_resource == null) { return result; }

            /* 多語轉換 */
            foreach (var item in result.Traverse(x => x.SubItems))
            {
                item.Name = _resource.GetString(item.Name) ?? item.Name;
            }

            return result;
		}

        /// <summary>複製清單</summary>
        private List<MenuItem> cloneListByRole(List<MenuItem> menuList, IPrincipal user, string currentUrl)
        {
            if(menuList == null || menuList.Count == 0) { return new List<MenuItem>(); }

            var result = new List<MenuItem>();

            foreach (var item in menuList)
            {
                var main = item.Clone();
                main.SubItems = cloneListByRole(item.SubItems, user, currentUrl);
                main.CanAccess = (main.ACT == null || user.IsInRole(main.ACT));
                main.IsActive = isContainsPath(currentUrl, main);

                if (main.HasSubItems)
                {
                    main.IsActive |= main.SubItems.Any(x => x.IsActive);
                }

                if (main.HasSubItems && item.UseFirstSubUrl)
                {
                    var sub = main.SubItems.First();
                    main.Url = sub.Url;
                    main.Pattern = sub.Pattern;
                    main.CanAccess = sub.CanAccess;
                }

                result.Add(main);
            }

            return result;
        }




        /// <summary></summary>
        public List<MenuItem> GetAllowList(IPrincipal user, string currentUrl)
		{
            List<MenuItem> result = GetList(user, currentUrl);
            result = filterAllowList(result);

            return result;
		}


        private List<MenuItem> filterAllowList(List<MenuItem> list)
        {
            List<MenuItem> result = list
                .Where(x => x.CanAccess)
                .Each(x => x.SubItems = filterAllowList(x.SubItems))
                .Where(x => x.Url.HasText() || x.SubItems.Count > 0)
                .ToList();

            return result;
        }


    }



	/// <summary>選單提供者</summary>
	public interface IMenuManager : IMenuProvider
	{
		/// <summary></summary>
		List<string> GetAreas();

		/// <summary></summary>
		List<MenuItem> GetListByArea(string areaName, IPrincipal user, string currentUrl);

		/// <summary></summary>
		List<MenuItem> GetAllowListByArea(string areaName, IPrincipal user, string currentUrl);

	}


	/// <summary>選單管理者</summary>
	public class MenuManager : IMenuManager
	{
		private static MenuManager _instance;

		/// <summary>單例實體</summary>
		public static MenuManager Instance
		{
			get
			{
				if (object.ReferenceEquals(_instance, null))
				{
					var temp = new MenuManager();
					_instance = temp;
				}
				return _instance;
			}
		}

		/// <summary></summary>
		public static void Register(IMenuProvider provider)
		{
			Instance.Add(provider);
		}




		/*=======================================*/

		private Dictionary<string, IMenuProvider> _providers = new Dictionary<string, IMenuProvider>();


		/// <summary></summary>
		public string AreaName { get { return null; } }

		/// <summary></summary>
		public void Add(IMenuProvider provider)
		{
			if (provider.AreaName.NoText()) { throw new ArgumentNullException(nameof(provider.AreaName)); }
			_providers[provider.AreaName] = provider;
		}


		/// <summary></summary>
		public List<string> GetAreas()
		{
			return _providers.Keys.ToList();
		}

		/// <summary></summary>
		public List<MenuItem> GetList(IPrincipal user, string currentUrl)
		{
			return _providers.Values.SelectMany(x => x.GetList(user, currentUrl)).ToList();
		}

		/// <summary></summary>
		public List<MenuItem> GetAllowList(IPrincipal user, string currentUrl)
		{
			return _providers.Values.SelectMany(x => x.GetAllowList(user, currentUrl)).ToList();
		}


		/// <summary></summary>
		public List<MenuItem> GetListByArea(string areaName, IPrincipal user, string currentUrl)
		{
			if (!_providers.ContainsKey(areaName)) { return new List<MenuItem>(); }
			return _providers[areaName].GetList(user, currentUrl);
		}

		/// <summary></summary>
		public List<MenuItem> GetAllowListByArea(string areaName, IPrincipal user, string currentUrl)
		{
			if (!_providers.ContainsKey(areaName)) { return new List<MenuItem>(); }
			return _providers[areaName].GetAllowList(user, currentUrl);
		}


	}

}
