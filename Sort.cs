/*
 * Created by SharpDevelop.
 * User: mgerdov
 * Date: 10/3/2017
 * Time: 10:12 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Xml;
using System.Xml.Linq;

namespace TaskManager
{
	/// <summary>
	/// Description of Sort.
	/// </summary>
	public class Sort
	{
	    public string Property { get; set; }
	    public string Direction { get; set; }
		

	    public static Sort[] LoadSorts()
	    {
	    	List<Sort> sorts = new List<Sort>();
			
			XmlDocument xDoc1 = new XmlDocument();
			xDoc1.Load(Program.DataDir + "Sorting.xml");
	        // получим корневой элемент
	        XmlElement xRoot1 = xDoc1.DocumentElement;
	        
	        // обход всех узлов в корневом элементе
	        foreach(XmlNode xnode in xRoot1)
	        {
	        	Sort sort = new Sort();
	        	
	        	// получаем атрибут name
/*	            if(xnode.Attributes.Count>0)
	            {
	                XmlNode attr = xnode.Attributes.GetNamedItem("id");
	                if (attr!=null)
	                    Console.WriteLine(attr.Value);
	            }
*/	            // обходим все дочерние узлы элемента user
	            foreach(XmlNode childnode in xnode.ChildNodes)
	            {
	            	// если узел - property
	                if(childnode.Name == "property")
	                {
	                    Console.WriteLine("Property: {0}", childnode.InnerText);
	                    sort.Property = childnode.InnerText;
	                }
	            	
	                // если узел - direction
	                if(childnode.Name == "direction")
	                {
	                    Console.WriteLine("Направление: {0}", childnode.InnerText);
	                    sort.Direction = childnode.InnerText;
	                }
	            }
	            sorts.Add(sort);
	        }
	        return sorts.ToArray();
	    }
	    
		public static IEnumerable<Task> DoSort<Task>(IEnumerable<Task> tasks, Sort[] sort)
		{
		    IOrderedEnumerable<Task> temp = null;
		    foreach (var s in sort)
		    {
		        Func<Task, IComparable> keySelector = GetKeySelector<Task>(s.Property);
		
		        if (temp == null)
		        {
		            temp = s.Direction == "Asc" ?
		                    tasks.OrderBy(keySelector) :
		                    tasks.OrderByDescending(keySelector);
		        }
		        else
		        {
		            temp = s.Direction == "Asc" ?
		                    temp.ThenBy(keySelector) :
		                    temp.ThenByDescending(keySelector);
		        }
		    }
		
		    return temp ?? tasks;
		}
		
		private static Func<Task, IComparable> GetKeySelector<Task>(string property)
		{
		    var param = Expression.Parameter(typeof(Task));
		    var lambda = Expression.Lambda<Func<Task, IComparable>>(
		        Expression.Convert(
		            Expression.Property(param, property),
		            typeof(IComparable)),
		        param);
		    return lambda.Compile();
		}
		
	}
}
