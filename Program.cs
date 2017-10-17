/*
 * Created by SharpDevelop.
 * User: User
 * Date: 03.05.2017
 * Time: 17:37
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
// Manual at https://metanit.com/sharp/tutorial/16.4.php

using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Xml;
using System.Xml.Linq;

namespace TaskManager
{
	internal sealed class Program
	{
		public static string DataDir = "../../data/";
		// for running from IDE only. Replace with "." for release!
		[STAThread]
		private static void Main(string[] args)
		{
			// Start GUI application Frame
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			MainForm form = new MainForm();
			Application.Run(form);
		}
	}
}